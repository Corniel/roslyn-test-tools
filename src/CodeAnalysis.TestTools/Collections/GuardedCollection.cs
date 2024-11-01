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
public abstract class GuardedCollection<TElement, TCollection> : IReadOnlyCollection<TElement>
    where TElement : class
    where TCollection : GuardedCollection<TElement, TCollection>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly TElement[] Collection;

    /// <summary>Initializes a new instance of the <see cref="GuardedCollection{TElement, TCollection}"/> class.</summary>
    protected GuardedCollection(TElement[] items) => Collection = items;

    /// <inheritdoc />
    public int Count => Collection.Length;

    /// <summary>Initializes a new instance of the <see cref="GuardedCollection{TElement, TCollection}"/> class.</summary>
    [Pure]
    protected abstract TCollection New(IEnumerable<TElement> items);

    /// <summary>Adds an item to the collection.</summary>
    [Pure]
    public TCollection Add(TElement item) => AddRange(Enumerable.Repeat(item, 1));

    /// <summary>Adds items to the collection.</summary>
    [Pure]
    public TCollection AddRange(params TElement[] items)
        => AddRange(Guard.NotNull(items).AsEnumerable());

    /// <summary>Adds items to the collection.</summary>
    [Pure]
    public TCollection AddRange(IEnumerable<TElement> items)
    {
        Guard.NotNull(items);
        return New(Collection.Concat(items.Select(Guards)));
    }

    /// <summary>
    /// Determines if the collection contains the requested item or not.
    /// </summary>
    [Pure]
    public bool Contains(TElement item) => Array.Exists(Collection, existing => Equals(item, existing));

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
