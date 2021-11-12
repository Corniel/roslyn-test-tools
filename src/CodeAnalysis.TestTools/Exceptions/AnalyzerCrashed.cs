namespace CodeAnalysis.TestTools;

/// <summary>Thrown when a <see cref="DiagnosticAnalyzer"/> under test crashes.</summary>
[Serializable]
public class AnalyzerCrashed : InvalidOperationException
{
    /// <summary>Creates a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed()
        : base(Messages.AnalyzerCrashed) { }

    /// <summary>Creates a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed(string message)
        : base(message) { }

    /// <summary>Creates a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    public AnalyzerCrashed(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>Creates a new instance of the <see cref="AnalyzerCrashed"/> class.</summary>
    protected AnalyzerCrashed(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
