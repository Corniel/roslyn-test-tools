namespace CodeAnalysis.TestTools.Text;

/// <summary>A lightweight representation of line number and text.</summary>
[DebuggerDisplay("{LineNumber}: {Text}")]
public readonly struct Line
{
    /// <summary>Initializes a new instance of the <see cref="Line"/> struct.</summary>
    internal Line(int number, string text)
    {
        LineNumber = number;
        Text = text;
    }

    /// <summary>Gets the number of the line.</summary>
    public readonly int LineNumber;

    /// <summary>Gets the text of the line.</summary>
    public readonly string Text;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Text;
}
