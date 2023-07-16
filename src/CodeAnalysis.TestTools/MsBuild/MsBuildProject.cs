using System.Xml.Linq;
using System.Xml.XPath;

namespace CodeAnalysis.TestTools.MsBuild;

internal sealed class MsBuildProject
{
    internal MsBuildProject(XElement element, FileInfo path)
    {
        Imports = element
            .XPathSelectElements($"//{nameof(Import)}")
            .Select(e => new Import(e, path))
            .ToArray();

        PackageReferences = element
            .XPathSelectElements($"//{nameof(PackageReference)}")
            .Select(e => new PackageReference(e))
            .ToArray();

        Path = path;
    }

    public FileInfo Path { get; }

    public IReadOnlyCollection<Import> Imports { get; }
    public IReadOnlyCollection<PackageReference> PackageReferences { get; private set; }

    public MetadataReferences MetadataReferences()
    {
        var lookup = new Dictionary<string, string?>();

        foreach (var p in ImportsAndSelf())
        {
            if ((p.Include ?? p.Update) is { Length: > 0 } key)
            {
                lookup[key] = p.Version;
            }
        }

        var references = new MetadataReferences();

        foreach (var kvp in lookup)
        {
            references = references.AddRange(NuGetPackage.Resolve(kvp.Key, kvp.Value));
        }
        return references;
    }

    private IEnumerable<PackageReference> ImportsAndSelf()
    {
        foreach (var import in Imports
            .Select(i => i.Project).OfType<MsBuildProject>()
            .SelectMany(i => i.ImportsAndSelf()))
        {
            yield return import;
        }
        foreach (var p in PackageReferences)
        {
            yield return p;
        }
    }

    public static MsBuildProject Load(FileInfo path)
    {
        var document = XDocument.Load(path.FullName);
        return new(document.Root!, path);
    }
}
