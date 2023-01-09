namespace CodeAnalysis.TestTools.References;

/// <summary>Represents a collection of <see cref="MetadataReference"/>'s.</summary>
public sealed class MetadataReferences : GuardedCollection<MetadataReference, MetadataReferences>
{
    /// <summary>Gets an empty set of medata references.</summary>
    public static readonly MetadataReferences Empty = new(Array.Empty<MetadataReference>());

    /// <summary>Creates a new instance of the <see cref="DiagnosticIds"/> class.</summary>
    internal MetadataReferences(params MetadataReference[] references) : base(references) { }

    /// <inheritdoc />
    protected override bool Equals(MetadataReference item1, MetadataReference item2)
        => item1.Display.ToUpperInvariant() == item2.Display.ToUpperInvariant();

    /// <inheritdoc />
    protected override MetadataReference Guards(MetadataReference item) => item;

    /// <inheritdoc />
    [Pure]
    protected override MetadataReferences New(IEnumerable<MetadataReference> items) => new(items.ToArray());
}
