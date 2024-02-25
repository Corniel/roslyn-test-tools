using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents the context to verify <see cref="CodeFixProvider"/> behavior.
/// </summary>
[Inheritable]
public partial record CodeFixVerifierContext<TContext>
    where TContext : AnalyzerVerifyContext<TContext>
{
    /// <summary>Initializes a new instance of the <see cref="CodeFixVerifierContext{TContext}"/> class.</summary>
    public CodeFixVerifierContext(TContext analyzerContext, CodeFixProvider codeFix)
    {
        AnalyzerContext = Guard.NotNull(analyzerContext);
        if (AnalyzerContext.Sources.Count > 1)
        {
            throw new NotSupportedException(Messages.NotSupported_MultipleSources);
        }
        CodeFix = Guard.NotNull(codeFix);
        Sources = new Sources(Language);
    }

    /// <summary>Gets the language (of the options sources etc.).</summary>
    public Language Language => AnalyzerContext.Language;

    /// <summary>Gets the analyzer context.</summary>
    public TContext AnalyzerContext { get; }

    /// <summary>Gets the code fix provider under test.</summary>
    public CodeFixProvider CodeFix { get; }

    /// <summary>Gets the sources (snippets, files) to verify with.</summary>
    public Sources Sources { get; init; }

    /// <summary>Verifies the code fix provider iteratively.</summary>
    public void Verify() => Run.Sync(() => VerifyAsync());

    /// <summary>Verifies the code fix provider iteratively.</summary>
    public async Task VerifyAsync()
    {
        if (!Sources.Any()) throw new IncompleteSetup(Messages.IncompleteSetup_NoExpectedSources);
        if (Sources.Count != 1) throw new NotSupportedException(Messages.NotSupported_MultipleExpectedSources);

        var current = AnalyzerContext;

        while ((await ApplyCodeAction(current)) is { } updated
            && !AreEqual(current.Sources, updated.Sources))
        {
            current = updated;
        }

        if (!AreEqual(Sources, current.Sources))
        {
            var sb = new StringBuilder()
                .AppendLine("The code fix had a different outcome than expected.")
                .AppendLine("Actual code after fix:")
                .AppendLine(current.Sources.Single().ToString())
                .AppendLine()
                .AppendLine("Expected code:")
                .AppendLine(Sources.Single().ToString());

            throw new VerificationFailed(sb.ToString());
        }
    }

    [Pure]
    private static bool AreEqual(Sources l, Sources r)
        => l.Single().HaveSameLines(r.Single());

    [Pure]
    private async Task<TContext> ApplyCodeAction(TContext context)
    {
        var document = context.GetDocument();
        var diagnostics = await context.GetDiagnosticsAsync();

        if (diagnostics
            .Where(IsFixable)
            .SelectMany(d => GetCodeActions(document, d))
            .FirstOrDefault() is not { } action)
        {
            return context;
        }
        else
        {
            var operations = await action.GetOperationsAsync(default);
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            var changed = solution.GetDocument(document.Id)!;
            var sources = new Sources(Language).Add(await Code.FromDocumentAsync(changed));

            return context with { Sources = sources };
        }
    }

    [Pure]
    private bool IsFixable(Diagnostic diagnostic) => CodeFix.FixableDiagnosticIds.Contains(diagnostic.Id);

    [Pure]
    private IReadOnlyCollection<CodeAction> GetCodeActions(Document document, Diagnostic diagnostic)
    {
        var actions = new List<CodeAction>();
        var context = new CodeFixContext(document, diagnostic, (action, _) => actions.Add(action), default);
        Run.Sync(() => CodeFix.RegisterCodeFixesAsync(context));
        return actions;
    }
}
