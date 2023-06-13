namespace CodeAnalysis.TestTools.Contexts;

public partial record CodeFixVerifierContext
{
    /// <summary>Adds a (code) snippet.</summary>
    [Pure]
    public CodeFixVerifierContext AddSnippet(string code)
        => this with { Sources = Sources.Add(Code.Snippet(code, Language)) };

    /// <summary>Adds a (code) source file.</summary>
    [Pure]
    public CodeFixVerifierContext AddSource(string path)
        => this with { Sources = Sources.Add(Code.FromFile(new FileInfo(path))) };
}
