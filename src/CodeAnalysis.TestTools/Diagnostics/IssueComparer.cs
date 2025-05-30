namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Compares reported issues, with expected issues.</summary>
public static class IssueComparer
{
    /// <summary>Compares reported issues, with expected issues.</summary>
    /// <returns>
    /// A collection of expected, unexpected, and not reported issues.
    /// </returns>
    [Pure]
    public static IEnumerable<Issue> Compare(
        IEnumerable<Diagnostic> actual,
        IEnumerable<ExpectedIssue> expected,
        IEnumerable<string> ignoredDiagnostics)
    {
        var lookup = expected.ToHashSet();
        var updated = new List<Issue>();
        ignoredDiagnostics ??= [];

        foreach (var diagnostic in actual.Where(diagnostic => !ignoredDiagnostics.Contains(diagnostic.Id)))
        {
            if (lookup.FirstOrDefault(issue => issue.Matches(diagnostic)) is ExpectedIssue issue)
            {
                lookup.Remove(issue);
                updated.Add(ExpectedIssue.New(diagnostic));
            }
            else
            {
                updated.Add(UnexpectedIssue.FromDiagnostic(diagnostic));
            }
        }
        foreach (var issue in lookup)
        {
            updated.Add(issue.NotReported());
        }

        return updated.OrderBy(issue => issue.Location);
    }
}
