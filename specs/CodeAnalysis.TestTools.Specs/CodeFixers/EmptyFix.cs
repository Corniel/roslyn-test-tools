using Microsoft.CodeAnalysis.CodeFixes;
using System.Diagnostics.Contracts;

namespace Specs.CodeFixers;

internal sealed class EmptyFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [];

    public override Task RegisterCodeFixesAsync(CodeFixContext context) => Task.CompletedTask;

    [Pure]
    public override FixAllProvider? GetFixAllProvider() => null;
}
