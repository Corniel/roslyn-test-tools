namespace CodeAnalysis.TestTools;

/// <summary>Contains a collection of diagnostic ID's.</summary>
public sealed class DiagnosticIds : GuardedCollection<string, DiagnosticIds>
{
    /// <summary>Gets an empty set of diagnostic ID's.</summary>
    public static readonly DiagnosticIds Empty = new([]);

    /// <summary>Initializes a new instance of the <see cref="DiagnosticIds"/> class.</summary>
    private DiagnosticIds(string[] items) : base(items) { }

    /// <inheritdoc />
    [Pure]
    protected override bool Equals(string item1, string item2)
        => item1.Equals(item2, StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc />
    [Impure]
    protected override string Guards(string item)
        => Guard.NotNullOrEmpty(item);

    /// <inheritdoc />
    [Pure]
    protected override DiagnosticIds New(IEnumerable<string> items) => new([.. Guard.NotNull(items)]);
}
