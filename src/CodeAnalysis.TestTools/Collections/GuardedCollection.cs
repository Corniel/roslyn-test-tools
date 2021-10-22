using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeAnalysis.TestTools.Collections
{
    /// <summary>An implementation of an <see cref="ICollection{T}"/>,
    /// that has a built-in guard for adding elements.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements.
    /// </typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    public abstract class GuardedCollection<T> : ICollection<T>
        where T : class
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<T> items = new();

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>Adds an item to the collection.</summary>
        public void Add(T item)
        {
            Guard.NotNull(item, nameof(item));
            Guards(item);
            if (!Contains(item)) items.Add(item);
        }
        /// <summary>Adds items to the collection.</summary>
        public void AddRange(params T[] items) => AddRange(items.AsEnumerable());

        /// <summary>Adds items to the collection.</summary>
        public void AddRange(IEnumerable<T> items)
        {
            Guard.NotNull(items, nameof(items));
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <inheritdoc/>
        public bool Contains(T item) => items.Any(existing => Equals(item, existing));

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public void Clear() => items.Clear();

        /// <inheritdoc/>
        public bool Remove(T item) => items.Remove(item);

        /// <summary>Returns true if the two items are equal.</summary>
        abstract protected bool Equals(T item1, T item2);

        /// <summary>Guards items that can be added, throws otherwise.</summary>
        abstract protected T Guards(T item);
                
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
