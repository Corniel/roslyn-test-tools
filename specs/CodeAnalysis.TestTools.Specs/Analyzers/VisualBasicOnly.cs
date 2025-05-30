using Microsoft.CodeAnalysis.VisualBasic;

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.VisualBasic)]

internal sealed class VisualBasicOnly : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new(nameof(VisualBasicOnly), "VB.NET only", "Specs", string.Empty, DiagnosticSeverity.Warning, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(
            c => { },
            SyntaxKind.ClassBlock);
    }
}
