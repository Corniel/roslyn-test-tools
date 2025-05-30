// <copyright file = "CollectionDebugView.cs">
// Copyright (c) 2018-current, Corniel Nobel.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace CodeAnalysis.TestTools.Collections;

/// <summary>Allows the debugger to display collections.</summary>
internal sealed class CollectionDebugView(IEnumerable enumeration)
{
    /// <summary>A reference to the enumeration to display.</summary>
    private readonly IEnumerable enumeration = enumeration;

    /// <summary>The array that is shown by the debugger.</summary>
    /// <remarks>
    /// Every time the enumeration is shown in the debugger, a new array is created.
    /// By doing this, it is always in sync with the current state of the enumeration.
    /// </remarks>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public object[] Items => [.. enumeration.Cast<object>()];
}
