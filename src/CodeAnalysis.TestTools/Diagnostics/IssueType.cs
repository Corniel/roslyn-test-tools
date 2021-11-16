namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary>The type of the issue.</summary>
public enum IssueType
{
    /// <summary>Non-compliant issue.</summary>
    Noncompliant = 0,

    /// <summary>Error.</summary>
    Error = 9,
}

/// <summary>Extensions on <see cref="IssueType"/></summary>
public static class IssueTypeExtensions
{
    /// <summary>Returns true if the issue type matches the diagnostic severity.</summary>
    public static bool Matches(this IssueType type, DiagnosticSeverity severity)
        => type switch
        {
            IssueType.Noncompliant => severity == DiagnosticSeverity.Warning,
            IssueType.Error => severity == DiagnosticSeverity.Error,
            _ => false,
        };
}
