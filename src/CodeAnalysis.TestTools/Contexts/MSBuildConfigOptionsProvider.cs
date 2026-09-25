namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Provides (global) analyzer config options to the analyzer, simulating
/// the compiler visible MSBuild properties that are passed in a build.
/// </summary>
internal sealed class MSBuildConfigOptionsProvider(IReadOnlyDictionary<string, string> properties) : AnalyzerConfigOptionsProvider
{
    private readonly AnalyzerConfigOptions Options = new MSBuildOptions(Guard.NotNull(properties));

    /// <inheritdoc />
    public override AnalyzerConfigOptions GlobalOptions => Options;

    /// <inheritdoc />
    [Pure]
    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => Options;

    /// <inheritdoc />
    [Pure]
    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => Options;

    /// <summary>
    /// Reads the compiler visible MSBuild properties
    /// from the (global) analyzer config options.
    /// </summary>
    private sealed class MSBuildOptions(IReadOnlyDictionary<string, string> properties) : AnalyzerConfigOptions
    {
        private const string Prefix = "build_property.";

        /// <inheritdoc />
        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            value = default;
            return key.StartsWith(Prefix, StringComparison.Ordinal)
                && properties.TryGetValue(key[Prefix.Length..], out value);
        }
    }
}
