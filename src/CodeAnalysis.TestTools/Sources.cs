namespace CodeAnalysis.TestTools;

/// <summary>Represents a collection of sources (code).</summary>
public sealed class Sources : GuardedCollection<Code, Sources>
{
    /// <summary>Initializes a new instance of the <see cref="Sources"/> class.</summary>
    public Sources(Language language) : this(language, []) { }

    /// <summary>Initializes a new instance of the <see cref="Sources"/> class.</summary>
    private Sources(Language language, Code[] code) : base(code) => Language = language;

    /// <summary>Gets the language of the sources.</summary>
    public Language Language { get; }

    /// <inheritdoc />
    [Pure]
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
        => new(Language, [.. Guard.NotNull(items)]);
}
