namespace CodeAnalysis.TestTools;

/// <summary>Thrown when a language conflict occurs.</summary>
[ExcludeFromCodeCoverage]
public class LanguageConflict : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="LanguageConflict"/> class.</summary>
    public LanguageConflict()
        : base(Messages.LanguageConflict) { }

    /// <summary>Initializes a new instance of the <see cref="LanguageConflict"/> class.</summary>
    public LanguageConflict(string message)
        : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="LanguageConflict"/> class.</summary>
    public LanguageConflict(string message, Exception innerException)
        : base(message, innerException) { }
}
