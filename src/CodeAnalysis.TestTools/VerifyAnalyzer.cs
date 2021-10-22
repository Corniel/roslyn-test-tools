using Microsoft.CodeAnalysis.Diagnostics;
using CodeAnalysis.TestTools.Contexts;
using CodeAnalysis.TestTools.Diagnostics;

namespace CodeAnalysis.TestTools
{
    /// <summary>Helper class to create <see cref="AnalyzerVerifyContext"/>'s.</summary>
    public static class VerifyAnalyzer
    {
        /// <summary>Creates a C# analyzer verify context.</summary>
        public static CSharpAnalyzerVerifyContext ForCS(this DiagnosticAnalyzer analyzer, CSharpAnalyzerVerifyContext defaults = null)
        {
            var context = defaults ?? new CSharpAnalyzerVerifyContext();
            return context.Add(analyzer);
        }

        /// <summary>Creates a Visual Basic analyzer verify context.</summary>
        public static VisualBasicAnalyzerVerifyContext ForVB(this DiagnosticAnalyzer analyzer, VisualBasicAnalyzerVerifyContext defaults = null)
        {
            var context = defaults ?? new VisualBasicAnalyzerVerifyContext();
            return context.Add(analyzer);
        }

        /// <summary>Verifies that the reported issues only have expected issues.</summary>
        public static void Verify(this AnalyzerVerifyContext context)
            => Guard.NotNull(context, nameof(context))
            .ReportIssues()
            .ShouldHaveExpectedIssuesOnly();
    }
}
