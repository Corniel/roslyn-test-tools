using System.Text.RegularExpressions;

namespace CodeAnalysis.TestTools.Diagnostics;

internal static class Pattern
{
#pragma warning disable SA1303 // Const field names should begin with upper-case letter
    private const string comment = @"(?<comment>//|'|<!--|/\*)";
    private const string position_precise = @"\s*(?<position>\^+)(\s+(?<invalid>\^+))*";
    private const string position_regular = @"(?<!\s*\^+\s)";
    private const string issue_type = @"\s*(?<issueType>Noncompliant|Secondary|Error)";
    private const string offset = @"(\s*@(?<offset>[+-]?\d+))?";
    private const string start_length = @"(\s*\^(?<start>\d+)#(?<length>\d+))?";
    private const string diagnostic_id = @"(\s*\[(?<diagnosticId>.+)\])?";
    private const string message = @"(\s*\{\{(?<message>.+)\}\})?";
#pragma warning restore SA1303 // Const field names should begin with upper-case letter

    public static readonly Regex Regular = Rx(comment, position_regular, issue_type, offset, start_length, diagnostic_id, message);
    public static readonly Regex Precise = Rx(@"^\s*", comment, position_precise, issue_type, "?", offset, diagnostic_id, message, @"\s*(-->|\*/)?$");
    public static readonly Regex Irregular = Rx(comment, ".*", issue_type);
    public static readonly Regex Unprecise = Rx(@"^\s*", comment, ".*", position_precise);

    [Pure]
    public static int Offset(this Match match)
       => match.Groups["offset"] is { Success: true } m
       ? int.Parse(m.Value) : 0;

    [Pure]
    public static string DiagnosticId(this Match match)
        => match.Groups["diagnosticId"] is { Success: true } ids
        ? ids.Value : string.Empty;

    [Pure]
    public static IssueType IssueType(this Match match)
        => match.Groups["issueType"] is { Success: true } issueType
        && Enum.TryParse<IssueType>(issueType.Value, ignoreCase: true, out var type)
        ? type : default;

    [Pure]
    public static string Message(this Match match)
        => match.Groups["message"] is { Success: true } mes
        ? mes.Value : string.Empty;

    [Pure]
    public static int? Start(this Match match)
        => match.Groups["position"] is { Success: true } position
        ? (int?)position.Index : null;

    [Pure]
    public static int? Length(this Match match)
        => match.Groups["position"] is { Success: true } position
        ? (int?)position.Length : null;

    [Pure]
    public static int? ColumnStart(this Match match)
        => match.Groups["start"] is { Success: true } columnStart
        ? (int?)int.Parse(columnStart.Value) : null;

    [Pure]
    public static int? ColumnLength(this Match match)
        => match.Groups["length"] is { Success: true } length
        ? (int?)int.Parse(length.Value) : null;

    [Pure]
    private static Regex Rx(params string[] components) => new(string.Concat(components), RegexOptions.Compiled);
}
