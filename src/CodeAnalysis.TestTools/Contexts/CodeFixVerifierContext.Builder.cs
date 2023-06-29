namespace CodeAnalysis.TestTools.Contexts;

public partial record CodeFixVerifierContext<TContext>
    where TContext : AnalyzerVerifyContext<TContext>
{
    /// <summary>Adds a (code) snippet.</summary>
    [Pure]
    public CodeFixVerifierContext<TContext> AddSnippet(string code)
        => this with { Sources = Sources.Add(Code.Snippet(code, Language)) };

    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public CodeFixVerifierContext<TContext> AddSource(string path)
        => this with { Sources = Sources.Add(Code.FromFile(new FileInfo(path))) };
}
