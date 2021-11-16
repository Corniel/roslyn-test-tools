namespace CodeAnalysis.TestTools;

/// <summary>Throws when the setup is not completed yet.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class IncompleteSetup : InvalidOperationException
{
    /// <summary>Creates a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup() { }

    /// <summary>Creates a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup(string message)
        : base(message) { }

    /// <summary>Creates a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public IncompleteSetup(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>Creates a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    protected IncompleteSetup(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>Creates a new instance of the <see cref="IncompleteSetup"/> class.</summary>
    public static IncompleteSetup New(string message, params object[] args)
        => new(string.Format(message, args));
}
