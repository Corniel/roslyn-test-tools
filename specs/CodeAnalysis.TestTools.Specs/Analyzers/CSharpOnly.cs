using Microsoft.CodeAnalysis.CSharp;

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class CSharpOnly : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new(nameof(CSharpOnly), "C# only", "Specs", string.Empty, DiagnosticSeverity.Warning, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(
            c => { },
            SyntaxKind.ClassDeclaration);
    }
}
