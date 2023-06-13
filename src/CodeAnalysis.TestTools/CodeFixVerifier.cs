using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeAnalysis.TestTools;

/// <summary>Extensions to allow <see cref="CodeFixProvider"/> verification.</summary>
public static class CodeFixVerifier
{
    /// <summary>Creates code fix verifier context.</summary>
    public static CodeFixVerifierContext ForCodeFix<TCodeFix>(this AnalyzerVerifyContext analyzerContext)
        where TCodeFix : CodeFixProvider, new()
        => analyzerContext.ForCodeFix(new TCodeFix());

    /// <summary>Creates code fix verifier context.</summary>
    public static CodeFixVerifierContext ForCodeFix(this AnalyzerVerifyContext analyzerContext, CodeFixProvider codeFix)
        => new(analyzerContext, codeFix);
}
