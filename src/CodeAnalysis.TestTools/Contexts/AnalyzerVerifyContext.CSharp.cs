using Microsoft.CodeAnalysis.CSharp;

namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents a C# specific context to verify <see cref="DiagnosticAnalyzer"/> behavior.
/// </summary>
[Inheritable]
public record CSharpAnalyzerVerifyContext
    : AnalyzerVerifyContext<CSharpAnalyzerVerifyContext>
{
    /// <summary>Creates a new instance of the <see cref="CSharpAnalyzerVerifyContext"/> class.</summary>
    public CSharpAnalyzerVerifyContext()
    {
        Options = new CSharpParseOptions(LanguageVersion.CSharp12);
    }

    /// <inheritdoc />
    public override Language Language => Language.CSharp;

    /// <summary>Gets if unsafe code is allowed. (default: false)</summary>
    public bool AllowUnsafe { get; init; }

    /// <summary>Sets the C# language version to parse with (default C# 12.0).</summary>
    [Pure]
    public CSharpAnalyzerVerifyContext WithLanguageVersion(LanguageVersion version)
        => WithOptions(new(version));

    /// <summary>Sets the C# parse options to parse with (default C# 12.0).</summary>
    [Pure]
    public CSharpAnalyzerVerifyContext WithOptions(CSharpParseOptions options)
        => this with { Options = Guard.NotNull(options) };

    /// <summary>Allow unsafe code (false by default).</summary>
    [Pure]
    public CSharpAnalyzerVerifyContext WithUnsafeCode(bool enable)
        => this with { AllowUnsafe = enable };

    /// <inheritdoc />
    [Pure]
    protected override CompilationOptions Update(CompilationOptions? options)
    {
        var cs = (options as CSharpCompilationOptions) ?? new CSharpCompilationOptions(OutputKind);
        return cs.WithOutputKind(OutputKind).WithAllowUnsafe(AllowUnsafe);
    }
}
