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

    public override string ToString()
    {
        var sb = new StringBuilder("<PackageReference ");
        if(Include is { })
        {
            sb.Append($@"Include=""{Include}"" ");
        }
        if (Update is { })
        {
            sb.Append($@"Update=""{Update}"" ");
        }
        if (Version is { })
        {
            sb.Append($@"Version=""{Version}"" ");
        }
        sb.Append("/>");
        return sb.ToString();
    }
}
