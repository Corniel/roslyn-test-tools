namespace CodeAnalysis.TestTools.References;

public partial class NuGetPackage
{
    /// <summary>Resolves the folders of a package.</summary>
    [Pure]
    public static async Task<IReadOnlyCollection<Folder>> ResolveFoldersAsync(string packageId, string filter = "*.dll", string? version = Latest, string? runtime = null)
    {
        var package = new NuGetPackage(packageId, await GetVersionAsync(packageId, version), runtime);
        if (!package.CacheDirectory.Exists) await NuGetRepository.DownloadAsync(package);

        return package.CacheDirectory
            .GetFiles(filter, SearchOption.AllDirectories)
            .GroupBy(file => file.Directory?.Name.Split('+')[0])
            .Select(group => new Folder(Path.GetFileName(group.Key)!, [.. group]))
            .ToArray();
    }

    /// <summary>Represents a folder in a NuGet Package.</summary>
    [DebuggerDisplay("Name = {Name}, Count = {Files.Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    public sealed record Folder(string name, IReadOnlyCollection<FileInfo> files) : IReadOnlyCollection<FileInfo>
    {
        /// <summary>The name of the package.</summary>
        public string Name { get; } = name;

        /// <inheritdoc />
        public int Count => Files.Count;

        /// <summary>The files in the folder.</summary>
        public IReadOnlyCollection<FileInfo> Files { get; } = files;

        /// <inheritdoc />
        [Pure]
        public IEnumerator<FileInfo> GetEnumerator() => Files.GetEnumerator();

        /// <inheritdoc />
        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
