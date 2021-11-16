using NuGet.Versioning;

namespace CodeAnalysis.TestTools.References;

/// <summary>Represents a NuGet package containing one or more <see cref="MetadataReference"/>'s.</summary>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public sealed partial class NuGetPackage : IReadOnlyCollection<MetadataReference>
{
    /// <summary>The latest (stable, pre-releases are excluded) version of a package.</summary>
    public const string Latest = nameof(Latest);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<MetadataReference> references = new();

    /// <summary>Creates a new instance of the <see cref="NuGetPackage"/> class.</summary>
    private NuGetPackage(string packageId, NuGetVersion version, string runtime)
    {
        Id = packageId;
        Version = version;
        Runtime = runtime;
    }

    /// <summary>Gets the ID of the package.</summary>
    public string Id { get; }

    /// <summary>Gets the version of the package.</summary>
    public NuGetVersion Version { get; }

    /// <summary>Gets the runtime of the package.</summary>
    public string Runtime { get; private set; }

    /// <summary>Gets the amount of assemblies contained.</summary>
    public int Count => references.Count;

    /// <summary>Gets the location of the cached assemblies.</summary>
    public DirectoryInfo CacheDirectory
        => new(Path.Combine(PackagesDirectory.FullName, Id, $"cached.{Version}", Runtime == null ? string.Empty : $@"runtimes\{Runtime}\"));

    /// <inheritdoc />
    public override string ToString()
        => Runtime is null
        ? $"{Id} v{Version}, Assemblies: {Count}"
        : $"{Id} v{Version} ({Runtime}), Assemblies: {Count}";

    /// <inheritdoc />
    public IEnumerator<MetadataReference> GetEnumerator() => references.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal IncompleteSetup IncompletSetup()
        => Runtime is null
        ? IncompleteSetup.New(Messages.Nuget_NoDlls, Id, Version)
        : IncompleteSetup.New(Messages.Nuget_NoDllsWithRuntime, Id, Version, Runtime);
}
