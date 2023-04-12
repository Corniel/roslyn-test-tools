namespace CodeAnalysis.TestTools;

/// <summary>Contains a collection of diagnostic ID's.</summary>
public sealed class DiagnosticIds : GuardedCollection<string, DiagnosticIds>
{
    /// <summary>Gets an empty set of diagnostic ID's.</summary>
    public static readonly DiagnosticIds Empty = new(Array.Empty<string>());

    /// <summary>Creates a new instance of the <see cref="DiagnosticIds"/> class.</summary>
    private DiagnosticIds(string[] items) : base(items) { }

    /// <inheritdoc />
    [Pure]
    protected override bool Equals(string item1, string item2)
        => item1.ToUpperInvariant() == item2.ToUpperInvariant();

    /// <inheritdoc />
    [Impure]
    protected override string Guards(string item)
        => Guard.NotNullOrEmpty(item, nameof(item));

    /// <inheritdoc />
    [Pure]
    protected override DiagnosticIds New(IEnumerable<string> items) => new(Guard.NotNull(items, nameof(items)).ToArray());
}
