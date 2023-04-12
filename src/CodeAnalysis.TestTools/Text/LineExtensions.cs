namespace CodeAnalysis.TestTools.Text;

/// <summary>Extensions on <see cref="Line"/>.</summary>
public static class LineExtensions
{
    /// <summary>Creates lines based on <see cref="TextLine"/>'s.</summary>
    [Pure]
    public static IEnumerable<Line> Lines(this IEnumerable<TextLine> textLines)
        => Guard.NotNull(textLines, nameof(textLines))
        .Select(textLine => new Line(textLine.LineNumber + 1, textLine.ToString()));

    /// <summary>Creates lines from a string.</summary>
    [Pure]
    public static IEnumerable<Line> Lines(this string str)
        => Guard.NotNull(str, nameof(str))
        .Split(new[] { LineEnd.Windows, LineEnd.Unix }, StringSplitOptions.None)
        .Select((line, index) => new Line(index + 1, line));
}
