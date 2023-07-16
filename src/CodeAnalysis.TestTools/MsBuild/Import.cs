using System.Xml.Linq;

namespace CodeAnalysis.TestTools.MsBuild;

internal sealed class Import
{
    internal Import(XElement element, FileInfo project)
    {
        if (element.Attribute(nameof(Project))?.Value is { } path
            && new FileInfo(Path.Combine(project.Directory!.FullName, path)) is { Exists: true } location)
        {
            Project = MsBuildProject.Load(location);
        }
    }

    public MsBuildProject? Project { get; }
}
