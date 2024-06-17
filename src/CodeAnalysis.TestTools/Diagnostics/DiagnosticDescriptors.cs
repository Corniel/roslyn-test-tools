namespace CodeAnalysis.TestTools.Diagnostics;

/// <summary><see cref="DiagnosticDescriptor"/> tooling.</summary>
public static class DiagnosticDescriptors
{
    /// <summary>
    /// Equality comparer for <see cref="DiagnosticDescriptor"/> based on
    /// <see cref="DiagnosticDescriptor.Id"/>.
    /// </summary>
    public static readonly IEqualityComparer<DiagnosticDescriptor> ById = new EqualById();

    private sealed class EqualById : IEqualityComparer<DiagnosticDescriptor>
    {
        /// <inheritdoc />
        [Pure]
        public bool Equals(DiagnosticDescriptor? x, DiagnosticDescriptor? y) => Equals(x?.Id, y?.Id);

        /// <inheritdoc />
        [Pure]
        public int GetHashCode([DisallowNull] DiagnosticDescriptor obj) => obj.Id.GetHashCode();
    }
}
