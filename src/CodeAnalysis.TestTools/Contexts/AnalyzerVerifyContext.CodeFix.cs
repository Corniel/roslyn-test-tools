using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeAnalysis.TestTools.Contexts;

public partial record AnalyzerVerifyContext<TContext> : AnalyzerVerifyContext
	where TContext : AnalyzerVerifyContext<TContext>
{
	/// <summary>Creates code fix verifier context.</summary>
	public CodeFixVerifierContext<TContext> ForCodeFix<TCodeFix>()
		where TCodeFix : CodeFixProvider, new()
		=> ForCodeFix(new TCodeFix());

	/// <summary>Creates code fix verifier context.</summary>
	public CodeFixVerifierContext<TContext> ForCodeFix(CodeFixProvider codeFix)
		=> new(self, codeFix);
}
