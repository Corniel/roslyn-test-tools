namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>Represents a verification issue.</summary>
public abstract record Issue : IComparable<Issue>
{
    /// <summary>Creates a new instance of the <see cref="Issue"/> record.</summary>
    protected Issue(string diagnosticId, IssueType type, string message, IssueLocation location)
    {
        DiagnosticId = diagnosticId ?? string.Empty;
        Type = type;
        Message = message ?? string.Empty;
        Location = location ?? IssueLocation.None;
    }

    /// <summary>Gets the diagnostic ID of the issue.</summary>
    public string DiagnosticId { get; }

    /// <summary>Gets the type of the issue.</summary>
    public IssueType Type { get; }

    /// <summary>Gets the message of the issue.</summary>
    public string Message { get; }

    /// <summary>Gets the location of the issue.</summary>
    public IssueLocation Location { get; }

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Issue other)
    {
        if (other is null) return 1;
        else return Location.CompareTo(other.Location);
    }

    /// <summary>gives the relevant info to report.</summary>
    [Pure]
    public virtual string ReportInfo()
        => $"{DiagnosticId}: {Location.ReportInfo()} {Message}{(Type == IssueType.Error ? " (ERROR)" : "")}";
}
