using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <inheritdoc cref="Dictionary{TKey,TValue}"/>
    public sealed class OneToUniqueManyDictionary<TKey, TValue> : Dictionary<TKey, HashSet<TValue>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public OneToUniqueManyDictionary() : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToUniqueManyDictionary(IEqualityComparer<TKey> comparer)
            : base(0, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.OneToManyDictionary`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToUniqueManyDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <param name="valueComparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the value.</param>
        public OneToUniqueManyDictionary(IEnumerable<Tuple<TKey, HashSet<TValue>>> collection,
            IEqualityComparer<TKey> comparer = null,
            IEqualityComparer<TValue> valueComparer = null) : this(comparer)
        {
            collection.ForEach(x => x.Item2.ForEach(y => Add(x.Item1, y, valueComparer)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <param name="valueComparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the value.</param>
        public OneToUniqueManyDictionary(IEnumerable<KeyValuePair<TKey, HashSet<TValue>>> collection,
            IEqualityComparer<TKey> comparer = null,
            IEqualityComparer<TValue> valueComparer = null) : this(comparer)
        {
            collection.ForEach(x => x.Value.ForEach(y => Add(x.Key, y, valueComparer)));
        }

        /// <summary>
        /// Add the provide <paramref name="value"/> to the associated <see cref="HashSet{T}"/> when <paramref name="key"/> is found;
        /// otherwise, creates a new <see cref="HashSet{T}"/> with <paramref name="value"/> in it and adds the key-value pair.
        /// </summary>
        /// <param name="key">Key instance</param>
        /// <param name="value">Value instance</param>
        /// <param name="valueComparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the value.</param>
        public bool Add(TKey key, TValue value,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (TryGetValue(key, out var l))
            {
                return l.Add(value);
            }

            Add(key, new HashSet<TValue>(valueComparer) { value });
            return true;
        }

        /// <summary>
        /// Checks if the given <paramref name="key"/> exists and <paramref name="value"/> is part of associated key set.
        /// </summary>
        /// <param name="key">Key instance</param>
        /// <param name="value">Value instance</param>
        public bool Contains(TKey key, TValue value)
        {
            return TryGetValue(key, out var l) && l.Contains(value);
        }

        /// <summary>
        /// Remove the <paramref name="value"/> if the <paramref name="key"/> exists and <paramref name="value"/> is inside the associated <see cref="HashSet{T}"/>.
        /// It also removes the <paramref name="key"/>, if the resultant set is empty.
        /// </summary>
        /// <param name="key">Key instance</param>
        /// <param name="value">Value instance</param>
        public bool Remove(TKey key, TValue value)
        {
            if (!TryGetValue(key, out var l)) return false;
            if (!l.Remove(value)) return false;
            if (l.Count == 0)
            {
                Remove(key);
            }

            return true;
        }
    }
}