using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents a VB.NET specific context to verify <see cref="DiagnosticAnalyzer"/> behavior.
/// </summary>
public record VisualBasicAnalyzerVerifyContext : AnalyzerVerifyContext<VisualBasicAnalyzerVerifyContext>
{
    /// <summary>Creates a new instance of the <see cref="VisualBasicAnalyzerVerifyContext"/> class.</summary>
    public VisualBasicAnalyzerVerifyContext()
    {
        Options = new VisualBasicParseOptions(LanguageVersion.VisualBasic16_9);
        References.Add(Reference.FromType<Microsoft.VisualBasic.DateAndTime>());
        IgnoredDiagnosics.Add(DiagnosticId.BC36716);
    }

    /// <inheritdoc />
    public override Language Language => Language.VisualBasic;

    /// <summary>Gets the global defined imports.</summary>
    public IReadOnlyCollection<GlobalImport> GlobalImports { get; private set; } = GlobalImport.Parse(
        "Microsoft.VisualBasic",
        "System",
        "System.Collections",
        "System.Collections.Generic",
        "System.Data",
        "System.Diagnostics",
        "System.Linq",
        "System.Threading.Tasks").ToImmutableArray();

    /// <summary>Sets the global defined imports.</summary>
    [Pure]
    public VisualBasicAnalyzerVerifyContext WithGlobalImports(IEnumerable<GlobalImport> imports)
    {
        GlobalImports = imports?.ToImmutableArray() ?? GlobalImports;
        return this;
    }

    /// <summary>Sets the VB.NET language version to parse with (default VB.NET).</summary>
    [Pure]
    public VisualBasicAnalyzerVerifyContext WithLanguageVersion(LanguageVersion version)
        => WithOptions(new(version));

    /// <summary>Sets the VB.NET options to parse with (default VB.NET 16.9).</summary>
    [Pure]
    public VisualBasicAnalyzerVerifyContext WithOptions(VisualBasicParseOptions options)
        => this with { Options = Guard.NotNull(options, nameof(options)) };

    /// <inheritdoc />
    protected override CompilationOptions Update(CompilationOptions options)
    {
        var vb = (options as VisualBasicCompilationOptions) ?? new VisualBasicCompilationOptions(OutputKind);
        return vb.WithOutputKind(OutputKind).WithGlobalImports(GlobalImports);
    }
}
