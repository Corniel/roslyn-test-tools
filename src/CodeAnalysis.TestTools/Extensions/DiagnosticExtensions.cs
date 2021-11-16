using CodeAnalysis.TestTools;
using CodeAnalysis.TestTools.Diagnostics;

namespace Microsoft.CodeAnalysis.Diagnostics
{
    /// <summary>Extensions on <see cref="Diagnostic"/>.</summary>
    public static class DiagnosticExtensions
    {
        /// <summary>Gets the issue type.</summary>
        public static IssueType GetIssueType(this Diagnostic diagnostic)
            => Guard.NotNull(diagnostic, nameof(diagnostic)).Severity switch
            {
                DiagnosticSeverity.Error => IssueType.Error,
                _ => IssueType.Noncompliant,
            };

        /// <summary>Returns true it the diagnostic is warning from the compiler.</summary>
        public static bool IsAnalyzerCrashed(this Diagnostic diagnostic)
            => Guard.NotNull(diagnostic, nameof(diagnostic)).Id == DiagnosticId.AD0001;

        /// <summary>Returns true it the diagnostic is warning from the compiler.</summary>
        public static bool IsCompilerWarning(this Diagnostic diagnostic)
            => Guard.NotNull(diagnostic, nameof(diagnostic)).Severity != DiagnosticSeverity.Error
            && (diagnostic.Id.StartsWith("CS") || diagnostic.Id.StartsWith("BC"));
    }
}
