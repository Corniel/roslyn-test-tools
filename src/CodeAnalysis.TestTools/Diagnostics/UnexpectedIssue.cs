namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Represents an unexpected verification issue.</summary>
public sealed record UnexpectedIssue : Issue
{
    /// <summary>Creates a new instance of the <see cref="UnexpectedIssue"/> record.</summary>
    public UnexpectedIssue(string diagnosticId, IssueType type, string message, IssueLocation location)
        : base(diagnosticId, type, message, location) { }

    /// <inheritdoc />
    public override string ReportInfo()
        => $"[+] {base.ReportInfo()}";

    /// <summary>Creates an unexpected verification issue based on a diagnostic.</summary>
    public static UnexpectedIssue FromDiagnostic(Diagnostic diagnostic)
        => new(
            diagnosticId: Guard.NotNull(diagnostic, nameof(diagnostic)).Id,
            type: diagnostic.GetIssueType(),
            message: diagnostic.GetMessage(),
            location: IssueLocation.FromLocation(diagnostic.Location));
}
