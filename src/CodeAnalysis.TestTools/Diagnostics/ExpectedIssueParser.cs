using System.Text.RegularExpressions;

namespace CodeAnalysis.TestTools.Diagnostics;

internal static partial class ExpectedIssueParser
{
    private const int PreciseOffSet = -1;

    [Pure]
    public static IReadOnlyCollection<ExpectedIssue> Parse(IEnumerable<Line> lines)
    {
        var regulars = new ExpectedIssues();
        var precises = new ExpectedIssues();

        foreach (var line in lines)
        {
            if (Pattern.Precise.Match(line.Text) is { Success: true } precise)
            {
                precises.Add(IssueFromMatch(
                    precise
                        .SinglePosition(line)
                        .NoRemainingCurlyBrace(line),
                    line.LineNumber + PreciseOffSet));
            }
            else if (Pattern.Regular.Match(line.Text) is { Success: true } regular)
            {
                regulars.Add(IssueFromMatch(regular.NoRemainingCurlyBrace(line), line.LineNumber));
            }
            else if (Pattern.Unprecise.IsMatch(line.Text) || Pattern.Irregular.IsMatch(line.Text))
            {
                throw ParseError.New(Messages.ParseError_InvalidPattern, line.LineNumber);
            }
        }
        return regulars.Merge(precises);
    }

    [Pure]
    private static ExpectedIssue IssueFromMatch(Match match, int lineNumber)
        => new(
            diagnosticId: match.DiagnosticId(),
            type: match.IssueType(),
            message: match.Message(),
            location: new IssueLocation(
                lineNumber: lineNumber + match.Offset(),
                start: match.Start() ?? match.ColumnStart(),
                spanSize: match.Length() ?? match.ColumnLength()));

    [Pure]
    private static Match NoRemainingCurlyBrace(this Match match, Line line)
    {
        var offset = match.Index + match.Length;
        var remaining = line.Text[offset..];

        var index_open = remaining.IndexOf('{');
        var index_clos = remaining.IndexOf('}');

        if (index_open > 0)
        {
            throw ParseError.New(Messages.ParseError_RemainingCurlyBrace, '{', line.LineNumber, offset + index_open + 1);
        }
        else if (index_clos > 0)
        {
            throw ParseError.New(Messages.ParseError_RemainingCurlyBrace, '}', line.LineNumber, offset + index_clos + 1);
        }
        else
        {
            return match;
        }
    }

    [FluentSyntax]
    private static Match SinglePosition(this Match match, Line line)
    {
        if (match.Groups["invalid"] is not { Success: true } invalid)
        {
            return match;
        }
        else
        {
            int pos = invalid.Index + 1;
            throw ParseError.New(Messages.Parse_Error_RepeatingPreciseLocation, line.LineNumber, pos);
        }
    }
}
