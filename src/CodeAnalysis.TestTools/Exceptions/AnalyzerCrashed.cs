namespace CodeAnalysis.TestTools;

/// <summary>Thrown when a <see cref="DiagnosticAnalyzer"/> under test crashes.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class AnalyzerCrashed : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed()
        : base(Messages.AnalyzerCrashed) { }

    /// <summary>Initializes a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed(string message)
        : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed(string message, Exception innerException)
        : base(message, innerException) { }

#if NET8_0_OR_GREATER
#else
    /// <summary>Initializes a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    protected AnalyzerCrashed(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
#endif
}
