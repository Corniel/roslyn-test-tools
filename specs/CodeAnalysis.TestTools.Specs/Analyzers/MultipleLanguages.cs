#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting

namespace Specs.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
internal sealed class MultipleLanguages : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [new DiagnosticDescriptor(nameof(MultipleLanguages), "multple languages", "Specs", string.Empty, DiagnosticSeverity.Warning, true)];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(
            c => { },
            Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.ClassBlock);
        context.RegisterSyntaxNodeAction(
            c => { },
            Microsoft.CodeAnalysis.CSharp.SyntaxKind.ClassDeclaration);
    }
}
