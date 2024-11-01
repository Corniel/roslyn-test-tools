using NuGet.Versioning;

namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    [Pure]
    private static async Task<NuGetPackage> ResolveAsync(string packageId, string? version, string? runtime)
    {
        if (packageId == Microsoft_Build_NoTargets.Id) return Microsoft_Build_NoTargets;
        else
        {
            var version_ = string.IsNullOrEmpty(version) || version == Latest
                ? await NuGetRepository.GetLatestVersionAsync(packageId)
                : new NuGetVersion(version);

            var package = new NuGetPackage(packageId, version_, runtime);
            if (!package.CacheDirectory.Exists) await NuGetRepository.DownloadAsync(package);

            var dllsPerDirectory = package.CacheDirectory
                .GetFiles("*.dll", SearchOption.AllDirectories)
                .GroupBy(file => file.Directory?.Name.Split('+')[0])
                .Select(group => (dir: Path.GetFileName(group.Key), dlls: group.AsEnumerable()))
                .ToArray();

            foreach (var allowed in SortedAllowedDirectories)
            {
                // dllsPerDirectory can contain the same <directory> from \lib\<directory> and \ref\<directory>. We don't care who wins.
                if (dllsPerDirectory.Where(info => info.dir == allowed)
                    .Select(x => x.dlls).FirstOrDefault() is { } paths)
                {
                    package.Runtime ??= allowed;
                    package.references.AddRange(paths.Select(path => MetadataReference.CreateFromFile(path.FullName)));
                    return package;
                }
            }
            throw package.IncompletSetup();
        }
    }

    private static DirectoryInfo PackagesDirectory
        => new(Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? @"..\..\..\..\..\packages");

    private static readonly string[] SortedAllowedDirectories =
    [
        "net",
        "net5.0",
        "netstandard2.1",
        "netstandard2.0",
        "net47",
        "net461",
        "netstandard1.6",
        "netstandard1.3",
        "netstandard1.1",
        "netstandard1.0",
        "net451",
        "net45",
        "net40",
        "net20",
        "portable-net45",
        "lib", // This has to be last, some packages have DLLs directly in "lib" directory
    ];
}
