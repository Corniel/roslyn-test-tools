namespace CodeAnalysis.TestTools.Contexts;

/// <summary>Extensions on <see cref="AnalyzerVerifyContext"/>.</summary>
public static class AnalyzerVerifyContextExtensions
{
    /// <summary>Adds an (optional) extra analyzer.</summary>
    [Pure]
    public static TContext Add<TContext>(this TContext context, DiagnosticAnalyzer analyzer)
        where TContext : AnalyzerVerifyContext
        => context with
        {
            Analyzers = context.Analyzers.Add(analyzer),
        };

    /// <summary>Adds (optional) extra analyzers.</summary>
    [Pure]
    public static TContext Add<TContext>(this TContext context, params DiagnosticAnalyzer[] analyzers)
        where TContext : AnalyzerVerifyContext
        => context with
        {
            Analyzers = context.Analyzers.AddRange(analyzers),
        };
}
