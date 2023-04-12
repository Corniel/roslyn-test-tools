namespace CodeAnalysis.TestTools;

/// <summary>Thrown when a parse error occurs.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class ParseError : FormatException
{
    /// <summary>Creates a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError() { }

    /// <summary>Creates a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError(string message)
        : base(message) { }

    /// <summary>Creates a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>Creates a new instance of the <see cref="ParseError"/> class.</summary>
    protected ParseError(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    [Pure]
    internal static ParseError New(string message, params object[] args)
        => new(string.Format(message, args));
}
