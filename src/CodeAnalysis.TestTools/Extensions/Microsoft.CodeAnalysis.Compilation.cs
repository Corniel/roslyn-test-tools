namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="Compilation"/>.</summary>
public static class CompilationExtensions
{
    extension(Compilation compilation)
    {
        /// <summary>Gets the diagnostics for the specified analyzers.</summary>
        [Pure]
        public Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync(
            Analyzers analyzers,
            CancellationToken cancellationToken = default)
            => compilation.GetDiagnosticsAsync(analyzers, [], cancellationToken);

        /// <summary>Gets the diagnostics for the specified analyzers.</summary>
        [Pure]
        public async Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync(
            Analyzers analyzers,
            IEnumerable<AdditionalText> texts,
            CancellationToken cancellationToken = default)
        {
            Guard.HasAny(analyzers);

            var options = compilation.Options.WithSpecificDiagnosticOptions(analyzers.DiagnosticsToReport);
            var analyzerOptions = new AnalyzerOptions([.. texts], new EmptyAnalyzerConfigOptionsProvider());

            var diagnostics = await compilation
                .WithOptions(options)
                .WithAnalyzers([.. analyzers], analyzerOptions)
                .GetAllDiagnosticsAsync(cancellationToken);

            return cancellationToken.IsCancellationRequested
                ? diagnostics
                : diagnostics.ThrowOnAnalyzerCrashed();
        }

        /// <summary>Gets the expected issues.</summary>
        [Pure]
        public IReadOnlyCollection<ExpectedIssue> GetExpectedIssues() =>
        [
            .. compilation.SyntaxTrees
                .SelectMany(tree => ExpectedIssue.Parse(tree.GetText().Lines.Lines())
                .Select(issue => issue.WithFilePath(tree.FilePath)))
        ];
    }

    [FluentSyntax]
    private static IReadOnlyCollection<Diagnostic> ThrowOnAnalyzerCrashed(this IReadOnlyCollection<Diagnostic> diagnostics)
        => diagnostics.FirstOrDefault(d => d.HasAnalyzerCrashed()) is { } diagnostic
            ? throw new AnalyzerCrashed(diagnostic.GetMessage())
            : diagnostics;

    private sealed class EmptyAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        private static readonly NoAnalyzerConfigOptions None = new();

        public override AnalyzerConfigOptions GlobalOptions => None;

        [Pure]
        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => None;

        [Pure]
        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => None;
    }

    private sealed class NoAnalyzerConfigOptions : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            value = default;
            return false;
        }
    }
}
