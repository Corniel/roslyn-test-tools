namespace CodeAnalysis.TestTools;

/// <summary>Contains a collection of <see cref="DiagnosticAnalyzer"/>'s
/// all supporting the required language.
/// </summary>
public sealed class Analyzers : GuardedCollection<DiagnosticAnalyzer, Analyzers>
{
    /// <summary>Creates a new instance of the <see cref="Analyzers"/> class.</summary>
    private Analyzers(Language language, DiagnosticAnalyzer[] analyzers) : base(analyzers) => Language = language;

    /// <summary>Creates a new instance of the <see cref="Analyzers"/> class.</summary>
    public Analyzers(Language language) : this(language, Array.Empty<DiagnosticAnalyzer>()) { }

    /// <summary>The language that the analyzers support.</summary>
    public Language Language { get; }

    /// <summary>Gets the diagnostics to report.</summary>
    public IEnumerable<KeyValuePair<string, ReportDiagnostic>> DiagnosticsToReport
        => DiagnosticIds
        .Select(id => KeyValuePair.Create(id, ReportDiagnostic.Warn))
        .Concat(new[] { KeyValuePair.Create(DiagnosticId.AD0001, ReportDiagnostic.Error) });

    /// <summary>Gets all (supported) diagnostic ID's.</summary>
    public ISet<string> DiagnosticIds
        => this.SelectMany(analyzer => analyzer.SupportedDiagnostics)
        .Select(diagnostic => diagnostic.Id)
        .ToHashSet();

    /// <inheritdoc />
    protected override bool Equals(DiagnosticAnalyzer item1, DiagnosticAnalyzer item2)
        => item1.GetType() == item2.GetType();

    /// <summary>Guards that the analyzer supports the language.</summary>
    protected override DiagnosticAnalyzer Guards(DiagnosticAnalyzer item)
        => item.SupportedLanguages().Any(supported => supported == Language)
        ? item
        : throw new LanguageConflict(string.Format(
            Messages.LanguageConflict_Analyzer,
            item.GetType().Name,
            Language));

    /// <inheritdoc />
    [Pure]
    protected override Analyzers New(IEnumerable<DiagnosticAnalyzer> items) => new(Language, items.ToArray());
}
