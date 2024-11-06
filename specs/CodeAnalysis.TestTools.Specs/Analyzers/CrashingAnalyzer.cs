using Microsoft.CodeAnalysis.CSharp;

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class CrashingAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new DiagnosticDescriptor(nameof(CrashingAnalyzer), "Crash!", "Specs", string.Empty, DiagnosticSeverity.Warning, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(
            c => throw new NotSupportedException(),
            SyntaxKind.ClassDeclaration);
    }
}
