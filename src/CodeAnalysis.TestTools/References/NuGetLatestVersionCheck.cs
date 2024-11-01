namespace CodeAnalysis.TestTools.References;

/// <summary>The latest version of a NuGet package and the time it was checked.</summary>
public sealed record NuGetLatestVersionCheck()
{
    /// <summary>The version of the NuGet package.</summary>
    public string? Version { get; init; }

    /// <summary>The last time the version have been checked.</summary>
    public required DateTime Checked { get; init; }
}
