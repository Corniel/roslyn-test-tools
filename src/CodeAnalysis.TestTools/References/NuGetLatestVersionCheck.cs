namespace CodeAnalysis.TestTools.References;

/// <summary>The latest version of a NuGet package and the time it was checked.</summary>
public sealed record NuGetLatestVersionCheck(string? Version, DateTime Checked);
