namespace CodeAnalysis.TestTools.Contexts;

/// <summary>The <see cref="AnalyzerVerifyContext{TContext}"/>
/// contains builder methods that allow extend the context using a fluent syntax.
/// </summary>
public abstract record AnalyzerVerifyContext<TContext> : AnalyzerVerifyContext
    where TContext : AnalyzerVerifyContext<TContext>
{
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
            Sources = Sources.AddRange(Guard.HasAny(paths, nameof(paths)).Select(path => Code.FromFile(new FileInfo(path))))
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
            References = References.AddRange(Guard.HasAny(references, nameof(references)))
        };

    /// <summary>Adds NuGet packages.</summary>
    [Pure]
    public TContext AddPackages(params NuGetPackage[] packages)
        => self with
        {
            References = References.AddRange(Guard.HasAny(packages, nameof(packages)).Cast<MetadataReference>())
        };
   

    /// <summary>Defines the output kind. (Default <see cref="OutputKind.DynamicallyLinkedLibrary"/>)</summary>
    [Pure]
    public TContext WithOutputKind(OutputKind outputKind)
        => self with 
        {
            OutputKind = Guard.DefinedEnum(outputKind, nameof(outputKind)) 
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
            IgnoredDiagnosics = DiagnosticIds.Empty.AddRange(diagnosticIds)
        };

    /// <remarks>Syntactic sugar.</remarks>
    private TContext self => (TContext)this;
}
