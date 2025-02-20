using Microsoft.CodeAnalysis.CSharp;

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class ReportError : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new DiagnosticDescriptor(nameof(ReportError), "Reports Error", "Do not use classes", string.Empty, DiagnosticSeverity.Error, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(
            c => c.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], c.Node.GetLocation())),
            SyntaxKind.ClassDeclaration);
    }
}
