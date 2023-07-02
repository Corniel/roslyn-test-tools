namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="Location"/>.</summary>
public static class LocationExtensions
{
    /// <summary>Gets the line number of the location.</summary>
    [Pure]
    public static int LineNumber(this Location location)
         => Guard.NotNull(location).GetLineSpan().StartLinePosition.Line + 1;
}
