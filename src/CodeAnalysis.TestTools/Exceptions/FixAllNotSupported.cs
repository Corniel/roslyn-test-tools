namespace CodeAnalysis.TestTools;

public class FixAllNotSupported : NotSupportedException
{
    /// <summary>Creates a new instance of the <see cref="FixAllNotSupported"/> class.</summary>
    public FixAllNotSupported()
        : base(Messages.FixAllNotSupported) { }

    /// <summary>Creates a new instance of the <see cref="FixAllNotSupported"/> class.</summary>
    public FixAllNotSupported(string message)
        : base(message) { }

    /// <summary>Creates a new instance of the <see cref="FixAllNotSupported"/> class.</summary>
    public FixAllNotSupported(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>Creates a new instance of the <see cref="FixAllNotSupported"/> class.</summary>
    protected FixAllNotSupported(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
