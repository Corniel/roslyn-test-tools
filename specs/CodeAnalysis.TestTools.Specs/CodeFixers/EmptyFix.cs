using Microsoft.CodeAnalysis.CodeFixes;

namespace Specs.CodeFixers;

internal sealed class EmptyFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => Array.Empty<string>().ToImmutableArray();

    public override Task RegisterCodeFixesAsync(CodeFixContext context) => Task.CompletedTask;

    public override FixAllProvider GetFixAllProvider() => null;
}
