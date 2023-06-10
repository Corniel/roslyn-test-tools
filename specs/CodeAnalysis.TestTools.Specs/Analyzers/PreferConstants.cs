#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Specs.Analyzers;

/// <summary>
/// Inspired by: https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class PreferConstants : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => new[]
        {
                new DiagnosticDescriptor(nameof(PreferConstants), "Prefer constants", "'{0}' can be a constant.", string.Empty, DiagnosticSeverity.Warning, true),
        }.ToImmutableArray();

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is LocalDeclarationStatementSyntax localDeclaration
            && !localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
        {
            // Perform data flow analysis on the local declaration.
            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

            // Retrieve the local symbol for each variable in the local declaration
            // and ensure that it is not written outside of the data flow analysis region.
            var variable = localDeclaration.Declaration.Variables.Single();
            var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable, context.CancellationToken);
            if (!dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], context.Node.GetLocation(), localDeclaration.Declaration.Variables.First().Identifier.ValueText));
            }
        }
    }
}
