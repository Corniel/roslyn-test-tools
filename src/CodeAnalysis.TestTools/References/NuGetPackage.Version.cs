using NuGet.Versioning;

namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    /// <summary>Gets the version of the package.</summary>
    [Pure]
    public static async Task<NuGetVersion> GetVersionAsync(string packageId, string? version)
        => string.IsNullOrEmpty(version) || version == Latest
            ? await NuGetRepository.GetLatestVersionAsync(packageId)
            : new NuGetVersion(version);
}
