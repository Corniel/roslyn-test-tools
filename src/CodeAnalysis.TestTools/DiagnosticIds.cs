namespace CodeAnalysis.TestTools;

/// <summary>Contains a collection of diagnostic ID's.</summary>
public sealed class DiagnosticIds : GuardedCollection<string>
{
    /// <inheritdoc />
    protected override bool Equals(string item1, string item2)
        => item1.ToUpperInvariant() == item2.ToUpperInvariant();

    /// <inheritdoc />
    protected override string Guards(string item)
        => Guard.NotNullOrEmpty(item, nameof(item));
}
