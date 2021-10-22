using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics.Contracts;

namespace CodeAnalysis.TestTools.Contexts
{
    /// <summary>
    /// Represents a C# specific context to verify <see cref="DiagnosticAnalyzer"/> behavior.
    /// </summary>
    public class CSharpAnalyzerVerifyContext
        : AnalyzerVerifyContext<CSharpAnalyzerVerifyContext>
    {
        /// <summary>Creates a new instance of the <see cref="CSharpAnalyzerVerifyContext"/> class.</summary>
        public CSharpAnalyzerVerifyContext()
        {
            Options = new CSharpParseOptions(LanguageVersion.CSharp9);
        }

        /// <inheritdoc />
        public override Language Language => Language.CSharp;

        /// <summary>Gets if unsafe code is allowed. (default: false)</summary>
        public bool AllowUnsafe { get; private set; }

        /// <summary>Sets the C# language version to parse with (default C# 9.0).</summary>
        [Pure]
        public CSharpAnalyzerVerifyContext WithLanguageVersion(LanguageVersion version)
            => WithOptions(new(version));

        /// <summary>Sets the C# parse options to parse with (default C# 9.0).</summary>
        [Pure]
        public CSharpAnalyzerVerifyContext WithOptions(CSharpParseOptions options)
        {
            Options = Guard.NotNull(options, nameof(options));
            return this;
        }

        /// <summary>Allow unsafe code (false by default).</summary>
        [Pure]
        public CSharpAnalyzerVerifyContext WithUnsafeCode(bool enable)
        {
            AllowUnsafe = enable;
            return this;
        }

        /// <inheritdoc />
        protected override CompilationOptions Update(CompilationOptions options)
        {
            var cs = (options as CSharpCompilationOptions) ?? new CSharpCompilationOptions(OutputKind);
            return cs.WithOutputKind(OutputKind).WithAllowUnsafe(AllowUnsafe);
        }
    }
}
