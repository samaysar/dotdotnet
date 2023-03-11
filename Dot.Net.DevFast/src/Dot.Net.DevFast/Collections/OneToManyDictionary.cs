using System;
using System.Collections;
using System.Collections.Generic;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
    public class OneToManyDictionary<TKey, TValue> : IDictionary<TKey, List<TValue>>,
        IDictionary,
        IReadOnlyDictionary<TKey, List<TValue>>
    {
        private readonly Dictionary<TKey, List<TValue>> _innerDico;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public OneToManyDictionary() : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToManyDictionary(IEqualityComparer<TKey> comparer)
          : this(0, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToManyDictionary(IEnumerable<Tuple<TKey, List<TValue>>> collection, IEqualityComparer<TKey> comparer = null) : this(comparer)
        {
            collection.ForEach(x => x.Item2.ForEach(y => Add(x.Item1, y)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToManyDictionary(IEnumerable<KeyValuePair<TKey, List<TValue>>> collection, IEqualityComparer<TKey> comparer = null) : this(comparer)
        {
            collection.ForEach(x => x.Value.ForEach(y => Add(x.Key, y)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneToManyDictionary{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.OneToManyDictionary`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public OneToManyDictionary(int capacity, IEqualityComparer<TKey> comparer = null)
        {
            _innerDico = new Dictionary<TKey, List<TValue>>(capacity, comparer);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
        public void Clear()
        {
            ((IDictionary)_innerDico).Clear();
        }

        /// <inheritdoc />
        public object SyncRoot => ((ICollection)_innerDico).SyncRoot;

        /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
        public int Count => _innerDico.Count;

        bool ICollection<KeyValuePair<TKey, List<TValue>>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, List<TValue>>>)_innerDico).IsReadOnly;

        bool IDictionary.IsReadOnly => ((IDictionary)_innerDico).IsReadOnly;

        /// <inheritdoc />
        public bool IsFixedSize => ((IDictionary)_innerDico).IsFixedSize;

        /// <inheritdoc />
        public bool IsSynchronized => ((ICollection)_innerDico).IsSynchronized;

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return _innerDico.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            return _innerDico.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(object key, object value)
        {
            ((IDictionary)_innerDico).Add(key, value);
        }

        /// <inheritdoc />
        public void Add(TKey key, List<TValue> value)
        {
            _innerDico.Add(key, value);
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, List<TValue>> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Add the provide <paramref name="value"/> to the associated <see cref="List{T}"/> when <paramref name="key"/> is found;
        /// otherwise, creates a new <see cref="List{T}"/> with <paramref name="value"/> in it and adds the key-value pair.
        /// </summary>
        /// <param name="key">Key instance</param>
        /// <param name="value">Value instance</param>
        public void Add(TKey key, TValue value)
        {
            if (TryGetValue(key, out var l))
            {
                l.Add(value);
            }
            else
            {
                Add(key, new List<TValue> { value });
            }
        }

        /// <inheritdoc />
        public bool Contains(object key)
        {
            if (key is TKey k)
            {
                return ContainsKey(k);
            }
            if (key is KeyValuePair<TKey, List<TValue>> v)
            {
                return Contains(v);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, List<TValue>> item)
        {
            return _innerDico.TryGetValue(item.Key, out var l) && l.EqualsItemWise(item.Value);
        }

        /// <inheritdoc />
        public void Remove(object key)
        {
            if (key is TKey k)
            {
                Remove(k);
            }
            if (key is KeyValuePair<TKey, List<TValue>> v)
            {
                Remove(v);
            }
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            return _innerDico.Remove(key);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
        {
            return Contains(item) && Remove(item.Key);
        }

        /// <summary>
        /// Remove the <paramref name="item"/> value if the <paramref name="item"/> key exists and value is inside the associated <see cref="List{T}"/>.
        /// </summary>
        /// <param name="item">Key-Value Pair.</param>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key, item.Value);
        }

        /// <summary>
        /// Remove the <paramref name="value"/> if the <paramref name="key"/> exists and <paramref name="value"/> is inside the associated <see cref="List{T}"/>.
        /// </summary>
        /// <param name="key">Key instance</param>
        /// <param name="value">Value instance</param>
        public bool Remove(TKey key, TValue value)
        {
            return TryGetValue(key, out var l) && l.Remove(value);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, List<TValue>>>)_innerDico).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            ((ICollection)_innerDico).CopyTo(array, index);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
        public bool ContainsKey(TKey key)
        {
            return _innerDico.ContainsKey(key);
        }
        
        /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
        public bool TryGetValue(TKey key, out List<TValue> value)
        {
            return _innerDico.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public object this[object key]
        {
            get => this[(TKey)key];
            set => this[(TKey)key] = (List<TValue>)value;
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
        public List<TValue> this[TKey key]
        {
            get => _innerDico[key];
            set => _innerDico[key] = value;
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => _innerDico.Keys;

        ICollection IDictionary.Keys => ((IDictionary)_innerDico).Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, List<TValue>>.Keys => Keys;

        /// <inheritdoc />
        public ICollection<List<TValue>> Values => _innerDico.Values;

        ICollection IDictionary.Values => ((IDictionary)_innerDico).Values;

        IEnumerable<List<TValue>> IReadOnlyDictionary<TKey, List<TValue>>.Values => Values;
    }
}
