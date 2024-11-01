namespace CodeAnalysis.TestTools.References;

/// <summary>The latest version of a NuGet package and the time it was checked.</summary>
public sealed record NuGetLatestVersionCheck()
{
    public string? Version { get; init; }

    public required DateTime Checked { get; init; }
}
