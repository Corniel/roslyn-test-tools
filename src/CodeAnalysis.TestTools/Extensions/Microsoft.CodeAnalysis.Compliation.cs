namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="Compilation"/>.</summary>
public static class CompliationExtensions
{
    /// <summary>Gets the diagnostics for the specified analyzers.</summary>
    public static async Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync(
        this Compilation compilation,
        Analyzers analyzers,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(compilation, nameof(compilation));
        Guard.HasAny(analyzers, nameof(analyzers));

        var options = compilation.Options.WithSpecificDiagnosticOptions(analyzers.DiagnosticsToReport);

        var diagnostics = (await compilation
            .WithOptions(options)
            .WithAnalyzers(analyzers.ToImmutableArray(), cancellationToken: cancellationToken)
            .GetAllDiagnosticsAsync(cancellationToken));

        return cancellationToken.IsCancellationRequested
            ? diagnostics
            : diagnostics.ThrowOnAnalyzerCrashed();
    }
    
    /// <summary>Gets the expected issues.</summary>
    public static IReadOnlyCollection<ExpectedIssue> GetExpectedIssues(this Compilation compilation)
    {
        Guard.NotNull(compilation, nameof(compilation));
        return compilation.SyntaxTrees
            .SelectMany(tree => ExpectedIssue.Parse(tree.GetText().Lines.Lines())
                .Select(issue => issue.WithFilePath(tree.FilePath)))
            .ToImmutableArray();
    }

    private static IReadOnlyCollection<Diagnostic> ThrowOnAnalyzerCrashed(this IReadOnlyCollection<Diagnostic> diagnostics)
        => diagnostics.FirstOrDefault(d => d.IsAnalyzerCrashed()) is { } diagnostic
            ? throw new AnalyzerCrashed(diagnostic.GetMessage())
            : diagnostics;

}
