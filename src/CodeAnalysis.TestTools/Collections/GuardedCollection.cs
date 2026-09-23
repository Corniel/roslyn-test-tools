namespace CodeAnalysis.TestTools.Collections;

/// <summary>An implementation of an <see cref="ICollection{T}"/>,
/// that has a built-in guard for adding elements.
/// </summary>
/// <typeparam name="TElement">
/// The type of elements.
/// </typeparam>
/// /// <typeparam name="TCollection">
/// The type of the collection.
/// </typeparam>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public abstract class GuardedCollection<TElement, TCollection>(ImmutableArray<TElement> items) : IReadOnlyCollection<TElement>
    where TElement : class
    where TCollection : GuardedCollection<TElement, TCollection>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ImmutableArray<TElement> Collection = items;

    /// <inheritdoc />
    public int Count => Collection.Length;

    /// <summary>Initializes a new instance of the <see cref="GuardedCollection{TElement, TCollection}"/> class.</summary>
    [Pure]
    protected abstract TCollection New(IEnumerable<TElement> items);

    /// <summary>Adds an item to the collection.</summary>
    [Pure]
    public TCollection Add(TElement item) => AddRange(item);

    /// <summary>Adds items to the collection.</summary>
    [Pure]
    public TCollection AddRange(params IEnumerable<TElement> items)
    {
        Guard.NotNull(items);
        return New([..Collection, ..items.Select(Guards)]);
    }

    /// <summary>
    /// Determines if the collection contains the requested item or not.
    /// </summary>
    [Pure]
    public bool Contains(TElement item) => Collection.Contains(item);

    /// <summary>Returns true if the two items are equal.</summary>
    [Pure]
    protected abstract bool Equals(TElement item1, TElement item2);

    /// <summary>Guards items that can be added, throws otherwise.</summary>
    protected abstract TElement Guards(TElement item);

    /// <inheritdoc />
    [Pure]
    public IEnumerator<TElement> GetEnumerator() => Collection.AsEnumerable().GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
