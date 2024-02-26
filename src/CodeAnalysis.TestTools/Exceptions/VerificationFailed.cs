namespace CodeAnalysis.TestTools;

/// <summary>Thrown when the expected issue verification fails.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class VerificationFailed : Exception
{
    /// <summary>Initializes a new instance of the <see cref="VerificationFailed"/> class.</summary>
    public VerificationFailed() { }

    /// <summary>Initializes a new instance of the <see cref="VerificationFailed"/> class.</summary>
    public VerificationFailed(string message)
        : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="VerificationFailed"/> class.</summary>
    public VerificationFailed(string message, Exception innerException)
        : base(message, innerException) { }
}
