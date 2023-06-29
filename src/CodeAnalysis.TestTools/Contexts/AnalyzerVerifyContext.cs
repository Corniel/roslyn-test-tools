namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents the context to verify <see cref="DiagnosticAnalyzer"/> behavior.
/// </summary>
public abstract record AnalyzerVerifyContext
{
    /// <summary>Initializes a new instance of the <see cref="AnalyzerVerifyContext"/> class.</summary>
    protected AnalyzerVerifyContext()
    {
        Analyzers = new Analyzers(Language);
    }

    /// <summary>Gets the language (of the options sources etc.).</summary>
    public abstract Language Language { get; }

    /// <summary>Gets the analyzer(s) to verify for.</summary>
    public Analyzers Analyzers { get; init; }

    /// <summary>Gets the diagnostic ID's toe ignore.</summary>
    public DiagnosticIds IgnoredDiagnostics { get; init; } = DiagnosticIds.Empty;

    /// <summary>Gets if the compiler warnings should be ignored.</summary>
    public bool IgnoreCompilerWarnings { get; init; } = true;

    /// <summary>Gets the compilation.</summary>
    [Pure]
    public abstract Task<Compilation> GetCompilationAsync();

    /// <summary>Gets the diagnostics.</summary>
    [Pure]
    public async Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync()
        => await (await GetCompilationAsync()).GetDiagnosticsAsync(Analyzers);

    /// <summary>Reports (both expected, unexpected, and not reported) issues for the analyzer verify context.</summary>
    [Pure]
    [DebuggerStepThrough]
    public IEnumerable<Issue> ReportIssues()
        => Run.Sync(() => ReportIssuesAsync());

    /// <summary>Reports (both expected, unexpected, and not reported) issues for the analyzer verify context.</summary>
    [Pure]
    public async Task<IEnumerable<Issue>> ReportIssuesAsync()
    {
        var compilation = await GetCompilationAsync();
        var diagnostics = await compilation.GetDiagnosticsAsync(Analyzers);
        var expected = compilation.GetExpectedIssues();

        return IgnoreCompilerWarnings
            ? IssueComparer.Compare(diagnostics.Where(diagnostic => !diagnostic.IsCompilerWarning()), expected, IgnoredDiagnostics)
            : IssueComparer.Compare(diagnostics, expected, IgnoredDiagnostics);
    }

    /// <summary>Gets the assembly name of the compilation.</summary>
    protected virtual string AssemblyName => $"{Analyzers.First().GetType().Name}.Verify";
}
