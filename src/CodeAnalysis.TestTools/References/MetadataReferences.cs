namespace CodeAnalysis.TestTools.References;

/// <summary>Represents a collection of <see cref="MetadataReference"/>'s.</summary>
public sealed class MetadataReferences : GuardedCollection<MetadataReference>
{
    /// <summary>Creates a new instance of the <see cref="MetadataReferences"/> class.</summary>
    public MetadataReferences() { }

    /// <summary>Creates a new instance of the <see cref="MetadataReferences"/> class.</summary>
    public MetadataReferences(params MetadataReference[] references)
    {
        AddRange(references);
    }

    /// <inheritdoc />
    protected override bool Equals(MetadataReference item1, MetadataReference item2)
        => item1.Display.ToUpperInvariant() == item2.Display.ToUpperInvariant();

    /// <inheritdoc />
    protected override MetadataReference Guards(MetadataReference item) => item;
}
