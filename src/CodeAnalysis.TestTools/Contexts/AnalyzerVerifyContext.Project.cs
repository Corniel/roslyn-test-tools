namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents a project file based context to verify <see cref="DiagnosticAnalyzer"/> behavior.
/// </summary>
[Inheritable]
public record ProjectAnalyzerVerifyContext : AnalyzerVerifyContext
{
    /// <summary>Initializes a new instance of the <see cref="ProjectAnalyzerVerifyContext"/> class.</summary>
    public ProjectAnalyzerVerifyContext(Project project)
    {
        Project = Guard.NotNull(project);
        Analyzers = new Analyzers(Language);
    }

    /// <summary>Gets the project.</summary>
    public Project Project { get; }

    /// <inheritdoc />
    public override Language Language => Language.Parse(Project?.Language);

    /// <summary>Gets the compilation.</summary>
    [Pure]
    public override Task<Compilation> GetCompilationAsync()
        => Project.GetCompilationAsync()!;

    /// <summary>Adds an (optional) extra analyzer.</summary>
    [Pure]
    public ProjectAnalyzerVerifyContext Add(DiagnosticAnalyzer analyzer)
        => this with { Analyzers = Analyzers.Add(analyzer) };

    /// <inheritdoc cref="Project.AssemblyName" />
    protected override string AssemblyName => Project.AssemblyName;

    /// <inheritdoc />
    [Pure]
    internal override IEnumerable<AdditionalText> GetAdditionalText()
        => Project.AdditionalDocuments.Select(d => d.ToAdditionalText());
}
