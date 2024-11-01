namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Represents an expected verification issue.</summary>
public sealed record ExpectedIssue : Issue
{
    /// <summary>Initializes a new instance of the <see cref="ExpectedIssue"/> class.</summary>
    public ExpectedIssue(string diagnosticId, IssueType type, string message, IssueLocation location)
        : base(diagnosticId, type, message, location) { }

    /// <inheritdoc />
    [Pure]
    public override string ReportInfo() => $"[ ] {base.ReportInfo()}";

    /// <summary>Returns true if issue is a location only (specified as // ^^^^^) issue.</summary>
    [Pure]
    public bool IsLocationOnly()
        => string.IsNullOrEmpty(Message)
        && Type == IssueType.Noncompliant
        && Location is { }
        && Location.Start.HasValue
        && Location.SpanSize.HasValue;

    /// <summary>Returns true if the issue matches the specified diagnostic.</summary>
    [Pure]
    public bool Matches(Diagnostic diagnostic)
       => diagnostic is { }
       && Location.Matches(diagnostic.Location)
       && Type.Matches(diagnostic.Severity)
       && (string.IsNullOrEmpty(Message) || Message == diagnostic.GetMessage());

    /// <summary>Creates a not reported issue based on the expected issue.</summary>
    [Pure]
    public NotReportedIssue NotReported() => new(DiagnosticId, Type, Message, Location);

    /// <summary>Updates the location.</summary>
    [Pure]
    public ExpectedIssue Update(IssueLocation location)
    {
        Guard.NotNull(location);
        return new(
            diagnosticId: DiagnosticId,
            type: Type,
            message: Message,
            location: location);
    }

    /// <summary>Updates the file path of the issue.</summary>
    [Pure]
    public ExpectedIssue WithFilePath(string filePath)
        => new(DiagnosticId, Type, Message, Location.WithFilePath(filePath));

    /// <summary>Gets an updated issue enriched with the diagnostic.</summary>
    [Pure]
    public static ExpectedIssue New(Diagnostic diagnostic)
    {
        Guard.NotNull(diagnostic);
        return new(
            diagnosticId: diagnostic.Descriptor.Id,
            type: diagnostic.GetIssueType(),
            message: diagnostic.GetMessage(),
            location: IssueLocation.FromLocation(diagnostic.Location));
    }

    /// <summary>Parses the lines for expected issues.</summary>
    [Pure]
    public static IReadOnlyCollection<ExpectedIssue> Parse(IEnumerable<Line> lines)
        => ExpectedIssueParser.Parse(lines);
}
