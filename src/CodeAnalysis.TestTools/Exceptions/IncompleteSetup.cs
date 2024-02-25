namespace CodeAnalysis.TestTools;

/// <summary>Throws when the setup is not completed yet.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class IncompleteSetup : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup() { }

    /// <summary>Initializes a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup(string message)
        : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup(string message, Exception innerException)
        : base(message, innerException) { }

#if NET8_0_OR_GREATER
#else
    /// <summary>Initializes a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    protected IncompleteSetup(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
#endif
    /// <summary>Initializes a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    [Pure]
    public static IncompleteSetup New(string message, params object[] args)
        => new(string.Format(message, args));
}
