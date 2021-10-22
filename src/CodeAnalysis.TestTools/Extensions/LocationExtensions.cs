using CodeAnalysis.TestTools;

namespace Microsoft.CodeAnalysis
{
    /// <summary>Extensions on <see cref="Location"/>.</summary>
    public static class LocationExtensions
    {
        /// <summary>Gets the line number of the location.</summary>
        public static int LineNumber(this Location location)
             => Guard.NotNull(location, nameof(location)).GetLineSpan().StartLinePosition.Line + 1;
    }
}
