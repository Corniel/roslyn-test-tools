namespace CodeAnalysis.TestTools;

/// <summary>Represents a collection of sources (code).</summary>
public sealed class Sources : GuardedCollection<Code, Sources>
{
    /// <summary>Creates a new instance of the <see cref="Sources"/> class.</summary>
    public Sources(Language language) : this(language, Array.Empty<Code>()) { }

    /// <summary>Creates a new instance of the <see cref="Sources"/> class.</summary>
    private Sources(Language language, Code[] code) : base(code) => Language = language;

    /// <summary>Gets the language of the sources.</summary>
    public Language Language { get; }

    /// <inheritdoc />
    protected override bool Equals(Code item1, Code item2)
        => item1.FilePath == item2.FilePath;

    /// <inheritdoc />
    protected override Code Guards(Code item)
      => item.Language == Language
      ? item
      : throw new LanguageConflict();

    /// <inheritdoc />
    [Pure]
    protected override Sources New(IEnumerable<Code> items) 
        => new(Language, Guard.NotNull(items, nameof(items)).ToArray());
}
