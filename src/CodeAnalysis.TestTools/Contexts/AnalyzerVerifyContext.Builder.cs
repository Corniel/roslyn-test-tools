namespace CodeAnalysis.TestTools.Contexts;

/// <summary>The <see cref="AnalyzerVerifyContext{TContext}"/>
/// contains builder methods that allow extend the context using a fluent syntax.
/// </summary>
public abstract partial record AnalyzerVerifyContext<TContext> : AnalyzerVerifyContext
    where TContext : AnalyzerVerifyContext<TContext>
{
    /// <summary>Initializes a new instance of the <see cref="AnalyzerVerifyContext{TContext}"/> class.</summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // handled by implementations.
    protected AnalyzerVerifyContext
        ()
    {
        Sources = new Sources(Language);
        References = Reference.Defaults;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>Gets the parse options to compile with.</summary>
    public virtual ParseOptions Options { get; init; }

    /// <summary>Gets the output kind of the compilation.</summary>
    public OutputKind OutputKind { get; init; } = OutputKind.DynamicallyLinkedLibrary;

    /// <summary>Gets the sources (snippets, files) to verify with.</summary>
    public Sources Sources { get; init; }

    /// <summary>Gets the (external) references to compile with.</summary>
    public MetadataReferences References { get; init; } = MetadataReferences.Empty;

    /// <summary>Adds an (optional) extra analyzer.</summary>
    [Pure]
    public TContext Add(DiagnosticAnalyzer analyzer)
        => self with { Analyzers = Analyzers.Add(analyzer) };

    /// <summary>Adds a (code) snippet.</summary>
    [Pure]
    public TContext AddSnippet(string code)
        => self with { Sources = Sources.Add(Code.Snippet(code, Language)) };
    
    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public TContext AddSource(string path)
        => self with { Sources = Sources.Add(Code.FromFile(new FileInfo(path))) };

    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public TContext AddSources(params string[] paths)
        => self with
        {
            Sources = Sources.AddRange(Guard.HasAny(paths).Select(path => Code.FromFile(new FileInfo(path))))
        };

    /// <summary>Adds a reference to the assembly of the <typeparamref name="TContainingType"/>.</summary>
    [Pure]
    public TContext AddReference<TContainingType>()
        => AddReferences(Reference.FromType<TContainingType>());

    /// <summary>Adds references.</summary>
    [Pure]
    public TContext AddReferences(params MetadataReference[] references)
        => self with
        {
            References = References.AddRange(Guard.HasAny(references))
        };

    /// <summary>Adds NuGet packages.</summary>
    [Pure]
    public TContext AddPackages(params NuGetPackage[] packages)
        => self with
        {
            References = References.AddRange(Guard.HasAny(packages).SelectMany(p => p))
        };
   
    /// <summary>Defines the output kind. (Default <see cref="OutputKind.DynamicallyLinkedLibrary"/>)</summary>
    [Pure]
    public TContext WithOutputKind(OutputKind outputKind)
        => self with 
        {
            OutputKind = Guard.DefinedEnum(outputKind) 
        };

    /// <summary>Sets if compiler warnings should be enabled or not (disabled by default).</summary>
    [Pure]
    public TContext WithCompilerWarnings(bool enable)
        => self with 
        {
            IgnoreCompilerWarnings = !enable 
        };

    /// <summary>Sets the diagnostic ID's to ignore.</summary>
    [Pure]
    public TContext WithIgnoredDiagnostics(params string[] diagnosticIds)
        => self with
        {
            IgnoredDiagnostics = DiagnosticIds.Empty.AddRange(diagnosticIds)
        };

    /// <remarks>Syntactic sugar.</remarks>
    private TContext self => (TContext)this;

    /// <summary>
    /// Gets the compilation based on the context's:
    /// * sources
    /// * meta-data references
    /// * parse options
    /// * compiler options
    /// </summary>
    [Pure]
    public sealed override Task<Compilation> GetCompilationAsync()
        => GetProject()
        .WithParseOptions(Options)
        .GetCompilationAsync()!;


    /// <summary>Updates the compilations options before applying the diagnostics.</summary>
    [Pure]
    protected abstract CompilationOptions Update(CompilationOptions? options);

    [Pure]
    internal virtual Project GetProject()
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
    internal Document GetDocument() => GetProject().Documents.Single();
}
