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
    public const string DiagnosticId = nameof(PreferConstants);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => new[]
        {
                new DiagnosticDescriptor(DiagnosticId, "Prefer constants", "'{0}' can be a constant.", string.Empty, DiagnosticSeverity.Warning, true),
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
            && !localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword)
            // Perform data flow analysis on the local declaration.
            && context.SemanticModel.AnalyzeDataFlow(localDeclaration) is { } dataFlowAnalysis)
        {
            // Retrieve the local symbol for each variable in the local declaration
            // and ensure that it is not written outside of the data flow analysis region.
            var variable = localDeclaration.Declaration.Variables.Single();
            if (context.SemanticModel.GetDeclaredSymbol(variable, context.CancellationToken) is { } variableSymbol
                && !dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], context.Node.GetLocation(), localDeclaration.Declaration.Variables.First().Identifier.ValueText));
            }
        }
    }
}
