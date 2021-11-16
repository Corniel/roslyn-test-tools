namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Represents the line location of an <see cref="Issue"/>.</summary>
public sealed record IssueLocation : IComparable<IssueLocation>
{
    /// <summary>A unknown/none-existing issue location (raised on assembly level).</summary>
    public static readonly IssueLocation None = new(default, default, default);

    /// <summary>Creates a new instance of the <see cref="IssueLocation"/> record.</summary>
    public IssueLocation(string filePath, int lineNumber, int? start, int? spanSize)
    {
        FilePath = filePath ?? string.Empty;
        LineNumber = lineNumber;
        Start = start;
        SpanSize = spanSize;
    }

    /// <summary>Creates a new instance of the <see cref="IssueLocation"/> record.</summary>
    public IssueLocation(int lineNumber, int? start, int? spanSize)
        : this(default, lineNumber, start, spanSize) { }

    /// <summary>Creates a new instance of the <see cref="IssueLocation"/> record.</summary>
    public IssueLocation(int lineNumber)
        : this(default, lineNumber, default, default) { }

    /// <summary>Gets the file path of the issue.</summary>
    public string FilePath { get; }

    /// <summary>Gets the (1-based) line number of the issue.</summary>
    public int LineNumber { get; }

    /// <summary>Gets the start (char) of the issue.</summary>
    public int? Start { get; }

    /// <summary>Gets the span size of the issue.</summary>
    public int? SpanSize { get; }

    /// <summary>Gets an updated version with a set file path.</summary>
    public IssueLocation WithFilePath(string filePath) => new(filePath, LineNumber, Start, SpanSize);

    /// <summary>Returns true if the locations match.</summary>
    public bool Matches(Location location)
        => location is { }
        && location.SourceTree.FilePath == FilePath
        && location.LineNumber() == LineNumber
        && (!Start.HasValue || Start == location.GetLineSpan().StartLinePosition.Character)
        && (!SpanSize.HasValue || SpanSize == location.SourceSpan.Length);

    /// <summary>gives the relevant info to report.</summary>
    public string ReportInfo()
        => IsPrecise()
        ? $"@{LineNumber:00}[{Start,2}, {Start + SpanSize,2}]"
        : $"@{LineNumber:00}[.., ..]";

    /// <summary>Return true if the location is defined precisely (with start and span size).</summary>
    public bool IsPrecise()
        => Start.HasValue
        && SpanSize.HasValue;

    /// <inheritdoc />
    public int CompareTo(IssueLocation other)
    {
        if (other is null) return 1;
        else return string.CompareOrdinal(FilePath, other.FilePath).Compare()
            ?? LineNumber.CompareTo(other.LineNumber).Compare()
            ?? Nullable.Compare(Start, other.Start).Compare()
            ?? Nullable.Compare(SpanSize, other.SpanSize);
    }

    /// <inheritdoc />
    public override string ToString()
        => $"{FilePath}{ReportInfo()}";

    /// <summary>Creates an <see cref="IssueLocation"/> from a <see cref="Location"/></summary>
    public static IssueLocation FromLocation(Location location)
        => location is null || location == Location.None
        ? None
        : new(
            filePath: Guard.NotNull(location, nameof(location)).SourceTree.FilePath,
            lineNumber: location.LineNumber(),
            start: location.GetLineSpan().StartLinePosition.Character,
            spanSize: location.SourceSpan.Length);
}
