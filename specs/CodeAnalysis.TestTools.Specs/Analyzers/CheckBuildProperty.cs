namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
internal sealed class CheckBuildProperty(string property) : DiagnosticAnalyzer
{
    public const string DiagnosticId = nameof(CheckBuildProperty);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new(DiagnosticId, "Check MSBuild property", "{0} = '{1}'", string.Empty, DiagnosticSeverity.Warning, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterCompilationAction(Check);
    }

    private void Check(CompilationAnalysisContext context)
    {
        if (context.Options.AnalyzerConfigOptionsProvider.GlobalOptions.TryGetValue($"build_property.{property}", out var value))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                SupportedDiagnostics[0],
                Location.None,
                property, value));
        }
    }
}
