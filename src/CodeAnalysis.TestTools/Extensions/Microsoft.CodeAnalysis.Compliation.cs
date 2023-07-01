using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="Compilation"/>.</summary>
public static class CompliationExtensions
{
    /// <summary>Gets the diagnostics for the specified analyzers.</summary>
    [Pure]
    public static Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync(
        this Compilation compilation,
        Analyzers analyzers,
        CancellationToken cancellationToken = default)
        => compilation.GetDiagnosticsAsync(analyzers, Array.Empty<TextDocument>(), cancellationToken);

    /// <summary>Gets the diagnostics for the specified analyzers.</summary>
    [Pure]
    public static async Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync(
        this Compilation compilation,
        Analyzers analyzers,
        IEnumerable<TextDocument> documents,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(compilation);
        Guard.HasAny(analyzers);

        var options = compilation.Options.WithSpecificDiagnosticOptions(analyzers.DiagnosticsToReport);

        var texts = documents.Select(d => new AdditionalTextDocument(d)).Cast<AdditionalText>().ToImmutableArray();

        var analyzerOptions = new AnalyzerOptions(texts, new EmptyAnalyzerConfigOptionsProvider());

        var diagnostics = (await compilation
            .WithOptions(options)
            .WithAnalyzers(analyzers.ToImmutableArray(), analyzerOptions, cancellationToken: cancellationToken)
            .GetAllDiagnosticsAsync(cancellationToken));

        return cancellationToken.IsCancellationRequested
            ? diagnostics
            : diagnostics.ThrowOnAnalyzerCrashed();
    }

    /// <summary>Gets the expected issues.</summary>
    [Pure]
    public static IReadOnlyCollection<ExpectedIssue> GetExpectedIssues(this Compilation compilation)
    {
        Guard.NotNull(compilation);
        return compilation.SyntaxTrees
            .SelectMany(tree => ExpectedIssue.Parse(tree.GetText().Lines.Lines())
                .Select(issue => issue.WithFilePath(tree.FilePath)))
            .ToImmutableArray();
    }

    [FluentSyntax]
    private static IReadOnlyCollection<Diagnostic> ThrowOnAnalyzerCrashed(this IReadOnlyCollection<Diagnostic> diagnostics)
        => diagnostics.FirstOrDefault(d => d.IsAnalyzerCrashed()) is { } diagnostic
            ? throw new AnalyzerCrashed(diagnostic.GetMessage())
            : diagnostics;


    private sealed class EmptyAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        private static readonly NoAnalyzerConfigOptions None = new();

        public override AnalyzerConfigOptions GlobalOptions => None;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => None;

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
