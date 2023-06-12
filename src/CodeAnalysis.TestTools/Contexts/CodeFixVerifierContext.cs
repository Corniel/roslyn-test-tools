using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeAnalysis.TestTools.Contexts;

/// <summary>
/// Represents the context to verify <see cref="CodeFixProvider"/> behavior.
/// </summary>
[Inheritable]
public partial record CodeFixVerifierContext
{
    /// <summary>Creates a new instance of the <see cref="CodeFixVerifierContext"/> class.</summary>
    public CodeFixVerifierContext(AnalyzerVerifyContext analyzerContext, CodeFixProvider codeFix)
    {
        AnalyzerContext = Guard.NotNull(analyzerContext, nameof(analyzerContext));
        CodeFix = Guard.NotNull(codeFix, nameof(codeFix));
        Sources = new Sources(Language);
    }

    /// <summary>Gets the language (of the options sources etc.).</summary>
    public Language Language => AnalyzerContext.Language;

    /// <summary>Gets the analyzer context.</summary>
    public AnalyzerVerifyContext AnalyzerContext { get; }

    /// <summary>Gets the code fix provider under test.</summary>
    public CodeFixProvider CodeFix { get; }

    /// <summary>Gets the sources (snippets, files) to verify with.</summary>
    public Sources Sources { get; init; }

    /// <summary>Verifies the code fix provider.</summary>
    /// <param name="kind">
    /// The code fix kind to apply.
    /// </param>
    public void Verify(CodeFixKind kind)
    {
        if (Guard.DefinedEnum(kind, nameof(kind)) == CodeFixKind.Iterative)
        {
            Verify();
        }
        else
        {
            VerifyFixAll();
        }
    }

    /// <summary>Verifies the code fix provider (iterative).</summary>
    public void Verify()
    {
        Run.Sync(()=> VerifyAsync());
    }

    private async Task VerifyAsync()
    {
        if (Sources.Count != 1) throw new NotSupportedException("Single fix");

        var current = AnalyzerContext;

        while ((await ApplyCodeAction(current)) is { } updated
            && !AreEqual(current.Sources, updated.Sources))
        {
            current = updated;
        }

        if(!AreEqual(Sources, current.Sources))
        {
            throw new VerificationFailed(current.Sources.Single().ToString());
        }
    }

    [Pure]
    private static bool AreEqual(Sources l, Sources r)
    {
        var ls = l.Single().ToString();
        var rs = r.Single().ToString();
        return ls == rs;
    }

    private async Task<AnalyzerVerifyContext> ApplyCodeAction(AnalyzerVerifyContext context)
    {
        if((await context
            .GetDiagnosticsAsync())
            .FirstOrDefault(d => CodeFix.FixableDiagnosticIds.Contains(d.Id)) is not { } diagnostic)
        {
            return context;
        }

        var document = context.GetDocument();

        if (GetCodeActions(document, diagnostic) is not { Count: > 0 } actions)
        {
            return context;
        }
        else
        {
            var operations = await actions.First().GetOperationsAsync(default);
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            var changed = solution.GetDocument(document.Id)!;

            return context with
            {
                Sources = new Sources(Language).Add(await Code.FromDocumentAsync(changed))
            };
        }
    }

    /// <summary>Verifies the code fix provider (all fixes at once).</summary>
    public void VerifyFixAll()
    {
        if (CodeFix.GetFixAllProvider() is not { } provider)
        {
            throw new FixAllNotSupported();
        }
        
       
    }

    private IReadOnlyCollection<CodeAction> GetCodeActions(Document document, Diagnostic diagnostic)
    {
        var actions = new List<CodeAction>();
        var context = new CodeFixContext(document, diagnostic, (action, _) => actions.Add(action), default);
        Run.Sync(() => CodeFix.RegisterCodeFixesAsync(context));
        return actions;
    }
}
