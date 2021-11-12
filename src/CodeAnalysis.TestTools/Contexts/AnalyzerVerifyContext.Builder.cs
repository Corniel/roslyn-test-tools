namespace CodeAnalysis.TestTools.Contexts;

/// <summary>The <see cref="AnalyzerVerifyContext{TContext}"/>
/// contains builder methods that allow extend the context using a fluent syntax.
/// </summary>
public abstract class AnalyzerVerifyContext<TContext> : AnalyzerVerifyContext
    where TContext : AnalyzerVerifyContext<TContext>
{
    /// <summary>Adds an (optional) extra analyzer.</summary>
    [Pure]
    public TContext Add(DiagnosticAnalyzer analyzer)
    {
        Analyzers.Add(analyzer);
        return self;
    }

    /// <summary>Adds a (code) snippet.</summary>
    [Pure]
    public TContext AddSnippet(string code)
    {
        Sources.Add(Code.Snippet(code, Language));
        return self;
    }

    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public TContext AddSource(string path)
    {
        Sources.Add(Code.FromFile(new FileInfo(path)));
        return self;
    }

    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public TContext AddSources(params string[] paths)
    {
        Guard.HasAny(paths, nameof(paths));
        Sources.AddRange(paths.Select(path => Code.FromFile(new FileInfo(path))));
        return self;
    }

    /// <summary>Adds a reference to the assembly of the <typeparamref name="TContainingType"/>.</summary>
    [Pure]
    public TContext AddReference<TContainingType>()
        => AddReferences(Reference.FromType<TContainingType>());

    /// <summary>Adds references.</summary>
    [Pure]
    public TContext AddReferences(params MetadataReference[] references)
    {
        Guard.HasAny(references, nameof(references));
        References.AddRange(references);
        return self;
    }

    /// <summary>Adds NuGet packages.</summary>
    [Pure]
    public TContext AddPackages(params NuGetPackage[] packages)
    {
        Guard.HasAny(packages, nameof(packages));
        References.AddRange(packages.SelectMany(package => package));
        return self;
    }

    /// <summary>Defines the output kind. (Default <see cref="OutputKind.DynamicallyLinkedLibrary"/>)</summary>
    [Pure]
    public TContext WithOutputKind(OutputKind outputKind)
    {
        OutputKind = Guard.DefinedEnum(outputKind, nameof(outputKind));
        return self;
    }

    /// <summary>Sets if compiler warnings should be enabled or not (disabled by default).</summary>
    [Pure]
    public TContext WithCompilerWarnings(bool enable)
    {
        IgnoreCompilerWarnings = !enable;
        return self;
    }

    /// <summary>Sets the diagnostic ID's to ignore.</summary>
    [Pure]
    public TContext WithIgnoredDiagnostics(params string[] diagnosticIds)
    {
        Guard.NotNull(diagnosticIds, nameof(diagnosticIds));
        IgnoredDiagnosics.Clear();
        IgnoredDiagnosics.AddRange(diagnosticIds);
        return self;
    }

    /// <remarks>Syntactic sugar.</remarks>
    private TContext self => (TContext)this;
}
