namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Verifies if only expected issues occured.</summary>
public static class IssueVerifier
{
    /// <summary>
    /// Throws a <see cref="VerificationFailed"/> if there are any
    /// unexpected or not reported issues.
    /// </summary>
    [FluentSyntax]
    public static IEnumerable<Issue> ShouldHaveExpectedIssuesOnly(this IEnumerable<Issue> issues)
    {
        Guard.NotNull(issues, nameof(issues)).Log();

        if (issues.Unexpected().Any() || issues.NotReported().Any())
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            issues.Log(writer);
            throw new VerificationFailed(Encoding.UTF8.GetString(stream.ToArray()));
        }
        return issues;
    }

    /// <summary>Logs all issues, both, expected, unexpected and not reported issues.</summary>
    [FluentSyntax]
    public static IEnumerable<Issue> Log(this IEnumerable<Issue> issues, TextWriter? writer = null)
    {
        Guard.NotNull(issues, nameof(issues));
        writer ??= Console.Out;

        writer.WriteLine(issues.GetReportInfoHeader());

        var filePath = string.Empty;

        foreach (var issue in issues)
        {
            if (issue.Location.FilePath != filePath)
            {
                filePath = issue.Location.FilePath;
                writer.WriteLine($"File: {filePath}");
            }
            writer.WriteLine(issue.ReportInfo());
        }

        return issues;
    }

    /// <summary>Represents the report info of the issues.</summary>
    [Pure]
    public static string GetReportInfoHeader(this IEnumerable<Issue> issues)
    {
        Guard.NotNull(issues, nameof(issues));
        var actual_issues = issues.Reported().IssueCount();
        var actual_errors = issues.Reported().ErrorCount();
        var unexpected_issues = issues.Unexpected().IssueCount();
        var unexpected_errors = issues.Unexpected().ErrorCount();
        var not_reported_issues = issues.NotReported().IssueCount();
        var not_reported_errors = issues.NotReported().ErrorCount();

        return new StringBuilder()
            .Append("Verification ")
            .AppendLine(issues.Unexpected().Any() || issues.NotReported().Any() ? "failed." : "succeeded.")
            .Append(Report("Reported", actual_issues, actual_errors, force: true))
            .Append(Report("Unexpectedly reported", unexpected_issues, unexpected_errors))
            .Append(Report("Did not report", not_reported_issues, not_reported_errors))
            .ToString();

        static string Report(string prefix, int issues, int errors, bool force = false)
        {
            var components = new[] { $"{issues} issues", $"{errors} errors" };
            if (issues == 1) components[0] = components[0][..^1];
            if (errors == 1) components[1] = components[1][..^1];
            if (issues > 0 || errors > 0) return $"* {prefix} {string.Join(", and ", components.Where(c => c[0] != '0'))}.{Environment.NewLine}";
            else if (force) return $"* {prefix} 0 issues, and 0 errors.{Environment.NewLine}";
            else return string.Empty;
        }
    }

    [Pure]
    private static IEnumerable<Issue> Reported(this IEnumerable<Issue> issues)
        => issues.Where(issue => issue is not NotReportedIssue);

    [Pure]
    private static IEnumerable<Issue> Unexpected(this IEnumerable<Issue> issues)
       => issues.OfType<UnexpectedIssue>();

    [Pure]
    private static IEnumerable<Issue> NotReported(this IEnumerable<Issue> issues)
         => issues.OfType<NotReportedIssue>();

    [Pure]
    private static int ErrorCount(this IEnumerable<Issue> issues)
        => issues.Count(issue => issue.Type == IssueType.Error);

    [Pure]
    private static int IssueCount(this IEnumerable<Issue> issues)
        => issues.Count(issue => issue.Type != IssueType.Error);
}
