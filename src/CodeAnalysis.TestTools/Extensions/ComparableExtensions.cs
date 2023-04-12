namespace System;

internal static class ComparableExtensions
{
    /// <summary>Returns null for zero, otherwise the compare value.</summary>
    /// <remarks>
    /// Allows:
    /// <code>
    /// return string.Compare(this, other).Compare()
    /// ?? etc..
    /// </code>
    /// </remarks>
    [Pure]
    public static int? Compare(this int compare) => compare == 0 ? null : compare;
}
