namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Represents an unexpected verification issue.</summary>
public sealed record UnexpectedIssue : Issue
{
    /// <summary>Initializes a new instance of the <see cref="UnexpectedIssue"/> class.</summary>
    public UnexpectedIssue(string diagnosticId, IssueType type, string message, IssueLocation location)
        : base(diagnosticId, type, message, location) { }

    /// <inheritdoc />
    [Pure]
    public override string ReportInfo() => $"[+] {base.ReportInfo()}";

    /// <summary>Creates an unexpected verification issue based on a diagnostic.</summary>
    [Pure]
    public static UnexpectedIssue FromDiagnostic(Diagnostic diagnostic)
        => new(
            diagnosticId: Guard.NotNull(diagnostic).Id,
            type: diagnostic.GetIssueType(),
            message: diagnostic.GetMessage(),
            location: IssueLocation.FromLocation(diagnostic.Location));
}
