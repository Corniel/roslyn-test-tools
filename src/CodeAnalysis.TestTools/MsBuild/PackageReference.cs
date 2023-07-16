using System.Xml.Linq;

namespace CodeAnalysis.TestTools.MsBuild;

internal sealed class PackageReference
{
    internal PackageReference(XElement element)
    {
        Include = element.Attribute(nameof(Include))?.Value;
        Update = element.Attribute(nameof(Update))?.Value;
        Version = element.Attribute(nameof(Version))?.Value;
    }

    public string? Include { get; }
    public string? Update { get; }
    public string? Version { get; }
}
