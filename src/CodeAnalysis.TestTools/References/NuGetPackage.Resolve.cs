namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    /// <summary>Resolves a package of choice.</summary>
    [Pure]
    public static NuGetPackage Resolve(string packageId, string? version, string? runtime = null)
        => Run.Sync(() => ResolveAsync(packageId, version, runtime));

    /// <inheritdoc cref="Resolve(string, string?, string?)" />
    [Pure]
    public static async Task<NuGetPackage> ResolveAsync(string packageId, string? version, string? runtime)
    {
        if (packageId == Microsoft_Build_NoTargets.Id) return Microsoft_Build_NoTargets;
        else
        {
            var package = new NuGetPackage(packageId, await GetVersionAsync(packageId, version), runtime);

            var folders = await ResolveFoldersAsync(packageId, version: version, runtime: runtime);

            foreach (var allowed in SortedAllowedDirectories)
            {
                // dllsPerDirectory can contain the same <directory> from \lib\<directory> and \ref\<directory>. We don't care who wins.
                if (folders.FirstOrDefault(info => info.Name == allowed) is { } folder)
                {
                    package.Runtime ??= allowed;
                    package.references.AddRange(folder.Select(path => MetadataReference.CreateFromFile(path.FullName)));
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
