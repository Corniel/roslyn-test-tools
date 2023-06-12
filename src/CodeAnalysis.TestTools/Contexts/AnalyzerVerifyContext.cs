namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents the context to verify <see cref="DiagnosticAnalyzer"/> behavior.
/// </summary>
public abstract record AnalyzerVerifyContext
{
    /// <summary>Creates a new instance of the <see cref="AnalyzerVerifyContext"/> class.</summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // handled by implementations.
    protected AnalyzerVerifyContext()
    {
        Analyzers = new Analyzers(Language);
        Sources = new Sources(Language);
        References = Reference.Defaults;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>Gets the language (of the options sources etc.).</summary>
    public abstract Language Language { get; }

    /// <summary>Gets the analyzer(s) to verify for.</summary>
    public Analyzers Analyzers { get; init; }

    /// <summary>Gets the parse options to compile with.</summary>
    public ParseOptions Options { get; init; }

    /// <summary>Gets the sources (snippets, files) to verify with.</summary>
    public Sources Sources { get; init; }

    /// <summary>Gets the diagnostic ID's toe ignore.</summary>
    public DiagnosticIds IgnoredDiagnostics { get; init; } = DiagnosticIds.Empty;

    /// <summary>Gets the (external) references to compile with.</summary>
    public MetadataReferences References { get; init; } = MetadataReferences.Empty;

    /// <summary>Gets the output kind of the compilation.</summary>
    public OutputKind OutputKind { get; init; } = OutputKind.DynamicallyLinkedLibrary;

    /// <summary>Gets if the compiler warnings should be ignored.</summary>
    public bool IgnoreCompilerWarnings { get; init; } = true;

    /// <summary>
    /// Gets the compilation based on the context's:
    /// * sources
    /// * meta-data references
    /// * parse options
    /// * compiler options
    /// </summary>
    [Pure]
    public Task<Compilation?> GetCompilationAsync()
        => GetProject()
        .WithParseOptions(Options)
        .GetCompilationAsync();

    /// <summary>Gets the diagnostics.</summary>
    [Pure]
    public async Task<IReadOnlyCollection<Diagnostic>> GetDiagnosticsAsync()
        => (await GetCompilationAsync()) is { } compliation
        ? await compliation.GetDiagnosticsAsync(Analyzers)
        : Array.Empty<Diagnostic>();

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
        var diagnostics = await compilation!.GetDiagnosticsAsync(Analyzers);
        var expected = compilation!.GetExpectedIssues();

        return IgnoreCompilerWarnings
            ? IssueComparer.Compare(diagnostics.Where(diagnostic => !diagnostic.IsCompilerWarning()), expected, IgnoredDiagnostics)
            : IssueComparer.Compare(diagnostics, expected, IgnoredDiagnostics);
    }

    /// <summary>Updates the compilations options before applying the diagnostics.</summary>
    [Pure]
    protected abstract CompilationOptions Update(CompilationOptions? options);

    /// <summary>Gets the assembly name of the compilation.</summary>
    protected virtual string AssemblyName => $"{Analyzers.First().GetType().Name}.Verify";

    [Pure]
    internal Project GetProject()
    {
        if (!Sources.Any()) throw new IncompleteSetup(Messages.IncompleteSetup_NoSources);
        else
        {
            using var workspace = new AdhocWorkspace();
            var solution = workspace.CurrentSolution;

            var project = solution
                .AddProject(AssemblyName, AssemblyName, Language.Name)
                .AddMetadataReferences(References)
                .AddSources(Sources);

            return project.WithCompilationOptions(Update(project.CompilationOptions));
        }
    }

    [Pure]
    internal Document GetDocument() 
        => GetProject().Documents.ToArray() is { Length: 1 } documents
            ? documents[0]
            : throw new NotSupportedException("Single Source");
}
