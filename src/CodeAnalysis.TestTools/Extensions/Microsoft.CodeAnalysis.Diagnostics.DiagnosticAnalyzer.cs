using System.Reflection;

namespace Microsoft.CodeAnalysis.Diagnostics;

/// <summary>Extensions on <see cref="DiagnosticAnalyzer"/>.</summary>
public static class DiagnosticAnalyzerExtensions
{
    /// <summary>Gets the supported languages of the analyzer.</summary>
    [Pure]
    public static IReadOnlyCollection<Language> SupportedLanguages(this DiagnosticAnalyzer analyzer)
        => Guard.NotNull(analyzer)
            .GetType()
            .GetCustomAttributes<DiagnosticAnalyzerAttribute>(inherit: true)
            .SelectMany(attr => attr.Languages)
            .Select(lang => Language.Parse(lang))
            .Where(lang => lang != Language.None)
            .Distinct()
            .ToImmutableArray();
}
