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

    /// <summary>Adds an MSBuild property exposed to the analyzers.</summary>
    [Pure]
    public ProjectAnalyzerVerifyContext WithBuildProperty(string name, string value) => this with
    {
        MSBuildProperties = MSBuildProperties.SetItem(name, value),
    };

    /// <summary>Adds MSBuild properties exposed to the analyzers.</summary>
    [Pure]
    public ProjectAnalyzerVerifyContext WithBuildProperties(params (string Name, string Value)[] properties) => this with
    {
        MSBuildProperties = MSBuildProperties.SetItems(properties.Select(p => new KeyValuePair<string, string>(p.Name, p.Value))),
    };

    /// <inheritdoc cref="Project.AssemblyName" />
    protected override string AssemblyName => Project.AssemblyName;

    /// <inheritdoc />
    [Pure]
    internal override IEnumerable<AdditionalText> GetAdditionalText()
        => Project.AdditionalDocuments.Select(d => d.ToAdditionalText());
}
