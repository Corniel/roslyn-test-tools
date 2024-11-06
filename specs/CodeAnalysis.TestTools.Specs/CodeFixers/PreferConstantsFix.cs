using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics.Contracts;

namespace Specs.CodeFixers;

internal sealed class PreferConstantsFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = [PreferConstants.DiagnosticId];

    [Pure]
    public override FixAllProvider? GetFixAllProvider() => null;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = (await context.Document.GetSyntaxRootAsync(context.CancellationToken))!;
        var declaration = root.FindToken(diagnosticSpan.Start).Parent!.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();

        context.RegisterCodeFix(
            CodeAction.Create(
                "Make constant",
                c => ChangeDocument(context.Document, declaration, c)),
            diagnostic);
    }

    private static async Task<Document> ChangeDocument(Document document, LocalDeclarationStatementSyntax localDeclaration, CancellationToken cancellationToken)
    {
        // Remove the leading trivia from the local declaration.
        var firstToken = localDeclaration.GetFirstToken();
        var leadingTrivia = firstToken.LeadingTrivia;
        var trimmedLocal = localDeclaration.ReplaceToken(
            firstToken, firstToken.WithLeadingTrivia(SyntaxTriviaList.Empty));

        // Create a const token with the leading trivia.
        var constToken = SyntaxFactory.Token(leadingTrivia, SyntaxKind.ConstKeyword, SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker));

        // Insert the const token into the modifiers list, creating a new modifiers list.
        var newModifiers = trimmedLocal.Modifiers.Insert(0, constToken);

        // Produce the new local declaration.
        var newLocal = trimmedLocal
            .WithModifiers(newModifiers)
            .WithDeclaration(localDeclaration.Declaration);

        // Add an annotation to format the new local declaration.
        var formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

        // Replace the old local declaration with the new local declaration.
        var oldRoot = (await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;
        var newRoot = oldRoot.ReplaceNode(localDeclaration, formattedLocal);

        // Return document with transformed tree.
        return document.WithSyntaxRoot(newRoot);
    }
}
