#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting

using System.IO;

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
internal sealed class CheckAdditionalFiles : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => new[]
        {
            new DiagnosticDescriptor(nameof(CheckAdditionalFiles), "Check additional files", "Contains {0}: '{1}'", string.Empty, DiagnosticSeverity.Warning, true),
        }
        .ToImmutableArray();

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterCompilationAction(Check);
    }

    private void Check(CompilationAnalysisContext context)
    {
        foreach (var file in context.Options.AdditionalFiles)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                SupportedDiagnostics[0],
                Location.None,
                Path.GetFileName(file.Path), RemoveNewLines(file)));
        }
    }

    private static string RemoveNewLines(AdditionalText file)
        => file
            .ToString()!
            .Replace("\r\n", string.Empty)
            .Replace("\n", string.Empty);
}
