using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace CodeAnalysis.TestTools.References;

internal static class NuGetRepository
{
    private static readonly Uri NuGetV3 = new("https://api.nuget.org/v3/index.json");

    public static async Task DownloadAsync(NuGetPackage package)
    {
        var resource = await Repo();
        using var stream = new MemoryStream();
        await resource.CopyNupkgToStreamAsync(package.Id, package.Version, stream, new SourceCacheContext(), NullLogger.Instance, default);
        using var packageReader = new PackageArchiveReader(stream);
        var dllFiles = packageReader.GetFiles().Where(IsDll).ToArray();

        if (dllFiles.Any())
        {
            foreach (var dllFile in dllFiles)
            {
                packageReader.ExtractFile(dllFile, Path.Combine(package.CacheDirectory.FullName, dllFile), NullLogger.Instance);
            }
        }
        else throw package.IncompletSetup();

        static bool IsDll(string file) => file.ToUpperInvariant().EndsWith(".DLL");
    }

    /// <summary>Gets the latest version of a NuGet package.</summary>
    [Pure]
    public static async Task<NuGetVersion> GetLatestVersionAsync(string packageId)
    {
        var cache = await GetLatestVersionsCacheAsync();
        if (cache.TryGetValue(packageId, out var cached)
            && cached.Version is { Length: > 0 }
            && cached.Checked.AddDays(5) >= DateTime.UtcNow)
        {
            return new NuGetVersion(cached.Version);
        }
        else
        {
            var repo = await Repo();
            var all = await repo.GetAllVersionsAsync(packageId, new SourceCacheContext(), NullLogger.Instance, default);
            var latest = all.OrderByDescending(v => v.Version).First(version => !version.IsPrerelease);
            cache[packageId] = new NuGetLatestVersionCheck(latest.OriginalVersion, DateTime.UtcNow);

            if (LatestVersionsFile.Directory is { Exists: false } directory)
            {
                directory.Create();
            }
            await cache.SaveAsync(LatestVersionsFile);
            return latest;
        }
    }

    public static DirectoryInfo LocalDirectory
        => new(Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? @"..\..\..\..\..\packages");

    private static FileInfo LatestVersionsFile =>
        new(Path.Combine(LocalDirectory.FullName, "latest-versions.json"));

    [Pure]
    private static Task<FindPackageByIdResource> Repo()
        => Repository.Factory.GetCoreV3(NuGetV3.AbsoluteUri).GetResourceAsync<FindPackageByIdResource>();

    [Pure]
    private static async Task<NuGetLatestVersions> GetLatestVersionsCacheAsync()
    {
        latestVersions ??= await NuGetLatestVersions.LoadAsync(LatestVersionsFile);
        return latestVersions;
    }

    private static NuGetLatestVersions? latestVersions;
}
