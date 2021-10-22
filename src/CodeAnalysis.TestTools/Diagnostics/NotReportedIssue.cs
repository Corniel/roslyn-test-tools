namespace CodeAnalysis.TestTools.Diagnostics
{
    /// <summary>Represents a not reported verification issue.</summary>
    public sealed record NotReportedIssue : Issue
    {
        /// <summary>Creates a new instance of the <see cref="NotReportedIssue"/> record.</summary>
        public NotReportedIssue(string diagnosticId, IssueType type, string message, IssueLocation location)
            : base(diagnosticId, type, message, location) { }

        /// <inheritdoc />
        public override string ReportInfo()
            => $"[-] {base.ReportInfo()}";
    }
}
