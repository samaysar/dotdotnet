﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Dot.Net.DevFast.Collections.Concurrent
{
    /// <summary>
    /// Represents a thread-safe collection of key-value pairs that can be accessed by multiple threads concurrently.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public sealed class ConcurrentDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly Dictionary<TKey, TValue>[] _data;
        private readonly int _concurrencyLevel;
        private readonly int _initialCapacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDictionary{TKey, TValue}" /> class that is empty and
        /// has the default initial capacity, has default concurrency level,
        /// and uses the default comparer for the key type.
        /// </summary>
        public ConcurrentDictionary() : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDictionary{TKey, TValue}" /> class that is empty and
        /// has the given initial capacity, has default concurrency level,
        /// and uses the default comparer (if provided else default) for the key type.
        /// </summary>
        public ConcurrentDictionary(int initialCapacity,
            IEqualityComparer<TKey> comparer = null) : this(initialCapacity,
            Environment.ProcessorCount,
            comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDictionary{TKey, TValue}" /> class that
        /// contains all the items of the provide <paramref name="collection"/> and
        /// has default concurrency level,
        /// and uses the default comparer (if provided else default) for the key type.
        /// </summary>
        public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection,
            IEqualityComparer<TKey> comparer = null) : this(0,
            Environment.ProcessorCount,
            comparer)
        {
            foreach (var pair in collection)
            {
                Add(pair);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDictionary{TKey, TValue}" /> class that is empty
        /// and has the given initial capacity, has given concurrency level
        /// and uses the comparer (if provided else default) for the key type.
        /// <para>
        /// NOTE: <paramref name="concurrencyLevel"/> has internal lower bound=2 and upper bound=<see cref="Environment.ProcessorCount"/>.
        /// </para>
        /// </summary>
        /// <param name="initialCapacity">Initial estimated capacity</param>
        /// <param name="concurrencyLevel">Concurrency level</param>
        /// <param name="comparer">Key comparer</param>
        public ConcurrentDictionary(int initialCapacity, 
            int concurrencyLevel, 
            IEqualityComparer<TKey> comparer = null)
        {
            _comparer = comparer;
            _concurrencyLevel = Math.Max(2, Math.Min(Math.Max(2, concurrencyLevel), Environment.ProcessorCount));
            _data = new Dictionary<TKey, TValue>[_concurrencyLevel];
            _initialCapacity = Math.Max(0, (int)Math.Ceiling(((double)initialCapacity) / _concurrencyLevel));
            for (var i = 0; i < _concurrencyLevel; i++)
            {
                _data[i] = new Dictionary<TKey, TValue>(_initialCapacity, _comparer);
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                var count = 0;
                for (var i = 0; i < _data.Length; i++)
                {
                    var d = _data[i];
                    var lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                        count += d.Count;
                    }
                    finally
                    {
                        if (lockTaken) Monitor.Exit(d);
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Truth value whether collection is empty or not.
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <inheritdoc />
        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new ConcurrentDictionaryEnumerator(this);
        }
        
        /// <inheritdoc />
        public void Clear()
        {
            Clear(true);
        }

        /// <summary>
        /// Clear items in the all the partitions.
        /// </summary>
        /// <param name="releaseMemory">If <see langword="true"/>, partitions are recreated to release previously allocated memory.</param>
        public void Clear(bool releaseMemory)
        {
            //We do not want to take all locks together
            //this call will provide best-effort clearing on whole collection.
            for (var i = 0; i < _data.Length; i++)
            {
                var d = _data[i];
                var lockTaken = false;
                try
                {
                    Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                    if (releaseMemory)
                    {
                        Interlocked.CompareExchange(ref _data[i],
                            new Dictionary<TKey, TValue>(_initialCapacity, _comparer),
                            d);
                    }
                    else
                    {
                        d.Clear();
                    }
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(d);
                }
            }
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item, null);
        }

        /// <summary>
        /// Checks whether given key/value pair is part of current collection using provided <paramref name="valueComparer"/>.
        /// If <paramref name="valueComparer"/> is <see langword="null" />, then <see cref="EqualityComparer{TValue}.Default"/>
        /// will be used.
        /// </summary>
        /// <param name="item">Key value pair to check</param>
        /// <param name="valueComparer">Equality comparer for the value.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item, IEqualityComparer<TValue> valueComparer)
        {
            var d = GetPartition(item.Key);
            var lockTaken = false;
            bool foundValue;
            TValue v;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                foundValue = d.TryGetValue(item.Key, out v);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            //it is safe to compare value outside of try block
            //this way, we will release the lock quicker!
            return foundValue && (valueComparer ?? EqualityComparer<TValue>.Default).Equals(v, item.Value);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var pair in this)
            {
                array[arrayIndex++] = pair;
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item, null);
        }

        /// <summary>
        /// Removes the given key/value pair from the collection using provided <paramref name="valueComparer"/>.
        /// If <paramref name="valueComparer"/> is <see langword="null" />, then <see cref="EqualityComparer{TValue}.Default"/>
        /// will be used.
        /// </summary>
        /// <param name="item">Key value pair to be removed.</param>
        /// <param name="valueComparer">Equality comparer for the value.</param>
        public bool Remove(KeyValuePair<TKey, TValue> item, IEqualityComparer<TValue> valueComparer)
        {
            var d = GetPartition(item.Key);
            var lockTaken = false;
            bool removed;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                //We do not want to take Remove logic out of this
                //try block, though it is possible (with almost no probability)
                //that by the time we retake the lock on partition,
                //another thread removed the key and re-added with another value!!!
                removed = d.TryGetValue(item.Key, out var v) &&
                          (valueComparer ?? EqualityComparer<TValue>.Default).Equals(v, item.Value) &&
                          d.Remove(item.Key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return removed;
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            bool removed;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                removed = d.Remove(key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return removed;
        }

        /// <summary>
        /// Attempts to remove and return the value that has the specified key.
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">When this method returns, contains the object removed from the collection, or the default value of the <see langword="TValue" /> type if <paramref name="key" /> does not exist.</param>
        /// <returns><see langword="true" /> if the object was removed successfully; otherwise, <see langword="false" />.</returns>
        public bool TryRemove(TKey key, out TValue value)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            bool removed;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                removed = d.TryGetValue(key, out value) && d.Remove(key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return removed;
        }

        /// <summary>
        /// Adds a key/value pair to the collection by using the specified function
        /// if the key does not already exist.
        /// Returns the new value, or the existing value if the key exists.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return TryGetValue(key, out var v) ? v : GetOrAdd(key, valueFactory(key));
        }

        /// <summary>
        /// Adds a key/value pair to the collection if the key does not already exist.
        /// Returns the new value, or the existing value if the key exists.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value.</param>
        /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        public TValue GetOrAdd(TKey key, TValue value)
        {
            return TryAddCore(key, value, out var existingValue) ? value : existingValue;
        }

        /// <summary>
        /// Adds <paramref name="key"/>/<paramref name="addValue"/> pair to the collection
        /// if the <paramref name="key"/> does not already exist,
        /// or updates <paramref name="key"/>/value pair by using <paramref name="updateValueFactory"/> lambda
        /// if the <paramref name="key"/> already exists.
        /// </summary>
        /// <param name="key">The key to be added or updated</param>
        /// <param name="addValue">The value to be added</param>
        /// <param name="updateValueFactory">Value generating lambda for an existing key and value</param>
        /// <param name="comparer">Value comparer. If not provided then default implementation will be used.</param>
        /// <returns>The new value for the key. This will be either be <paramref name="addValue" /> (if the key was absent) or the result of <paramref name="updateValueFactory" /> (if the key was present).</returns>
        public TValue AddOrUpdate(TKey key,
            TValue addValue,
            Func<TKey, TValue, TValue> updateValueFactory,
            IEqualityComparer<TValue> comparer = null)
        {
            while (!TryAddCore(key, addValue, out var existing))
            {
                var newValue = updateValueFactory(key, existing);
                if (TryUpdate(key, newValue, existing, comparer)) return newValue;
            }

            return addValue;
        }

        /// <summary>
        /// Updates the value associated with <paramref name="key" /> to <paramref name="newValue" />
        /// if the existing value with <paramref name="key" /> is equal to <paramref name="comparisonValue" />.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="newValue">Replacement value.</param>
        /// <param name="comparisonValue">Value to compare with the existing key value.</param>
        /// <param name="comparer">Value comparer. If not provided then default implementation will be used.</param>
        /// <returns><see langword="true" /> if the value with <paramref name="key" /> was equal to <paramref name="comparisonValue" /> and was replaced with <paramref name="newValue" />; otherwise, <see langword="false" />.</returns>
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue, IEqualityComparer<TValue> comparer = null)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            var updated = false;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                if (d.TryGetValue(key, out var v) &&
                    (comparer ?? EqualityComparer<TValue>.Default).Equals(v, comparisonValue))
                {
                    d[key] = newValue;
                    updated = true;
                }
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return updated;
        }

        /// <summary>
        /// Attempts to add the specified key and value to the collection.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <returns> <see langword="true" /> if the key/value pair was added successfully; <see langword="false" /> if the key already exists.</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            bool added;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
#if NET5_0_OR_GREATER
                added = d.TryAdd(key, value);
#else
                added = !d.ContainsKey(key);
                if (added)
                {
                    d.Add(key, value);
                }
#endif
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return added;
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                d.Add(key, value);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }
        }

        /// <inheritdoc />
        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            bool reply;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                reply = d.ContainsKey(key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return reply;
        }

        /// <inheritdoc />
        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            bool reply;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                reply = d.TryGetValue(key, out value);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return reply;
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public TValue this[TKey key]
        {
            get => TryGetValue(key, out var v) ? v : throw new KeyNotFoundException();
            set
            {
                var d = GetPartition(key);
                var lockTaken = false;
                try
                {
                    Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                    d[key] = value;
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(d);
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => ((IReadOnlyDictionary<TKey, TValue>)this).Keys.ToList();

        /// <inheritdoc />
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => new ConcurrentDictionaryKeyEnumerable(this);

        /// <inheritdoc />
        public ICollection<TValue> Values => ((IReadOnlyDictionary<TKey, TValue>)this).Values.ToList();

        /// <inheritdoc />
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => new ConcurrentDictionaryValueEnumerable(this);

        private bool TryAddCore(TKey key, 
            TValue newValue, 
            out TValue existingValue)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            var added = false;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                if (!d.TryGetValue(key, out existingValue))
                {
                    d.Add(key, newValue);
                    added = true;
                }
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            return added;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Dictionary<TKey, TValue> GetPartition(TKey key)
        {
            unchecked
            {
                return _data[(uint)key.GetHashCode() % _concurrencyLevel];
            }
        }

        private bool TryGetPartition(int position, out Dictionary<TKey, TValue> partition)
        {
            partition = default;
            if (position < 0) return false;
            if (position >= _concurrencyLevel) return false;
            partition = _data[position];
            return true;
        }

        private sealed class ConcurrentDictionaryEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly ConcurrentDictionary<TKey, TValue> _instance;
            private int _currentPosition;
            private bool _acquiredPartition;
            private bool _lockTaken;
            private Dictionary<TKey, TValue> _currentPartition;
            private IEnumerator<KeyValuePair<TKey, TValue>> _currentEnumerator;

            public ConcurrentDictionaryEnumerator(ConcurrentDictionary<TKey, TValue> instance)
            {
                _instance = instance;
                _lockTaken = false;
                _acquiredPartition = false;
                Reset();
            }

            ~ConcurrentDictionaryEnumerator()
            {
                Dispose(false);
            }

            public bool MoveNext()
            {
                Current = default;
                if (!_acquiredPartition) return false;
                TakeLock();
                if (!_currentEnumerator.MoveNext())
                {
                    while (MoveNextPartition())
                    {
                        TakeLock();
                        if (!_currentEnumerator.MoveNext()) continue;
                        Current = _currentEnumerator.Current;
                        return true;
                    }

                    Current = default;
                    return false;
                }

                Current = _currentEnumerator.Current;
                return true;
            }

            public void Reset()
            {
                ReleaseLock();
                _currentPosition = 0;
                AcquirePartition();
            }

            public KeyValuePair<TKey, TValue> Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool _)
            {
                ReleaseLock();
            }

            private bool MoveNextPartition()
            {
                ReleaseLock();
                return AcquirePartition();
            }

            private void TakeLock()
            {
                if (!_lockTaken)
                {
                    Monitor.TryEnter(_currentPartition, Timeout.Infinite, ref _lockTaken);
                    _currentEnumerator = _currentPartition.GetEnumerator();
                }
            }

            private void ReleaseLock()
            {
                if (_lockTaken)
                {
                    try
                    {
                        _currentEnumerator.Dispose();
                    }
                    finally
                    {
                        Monitor.Exit(_currentPartition);
                        _lockTaken = false;
                    }
                }
            }

            private bool AcquirePartition()
            {
                _acquiredPartition = _instance.TryGetPartition(_currentPosition++, out _currentPartition);
                return _acquiredPartition;
            }
        }

        private class ConcurrentDictionaryKeyEnumerable : IEnumerable<TKey>
        {
            private readonly IEnumerable<KeyValuePair<TKey, TValue>> _instance;

            public ConcurrentDictionaryKeyEnumerable(IEnumerable<KeyValuePair<TKey, TValue>> instance)
            {
                _instance = instance;
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new ConcurrentDictionaryKeyEnumerator(_instance.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ConcurrentDictionaryKeyEnumerator : IEnumerator<TKey>
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> _instance;

            public ConcurrentDictionaryKeyEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> instance)
            {
                _instance = instance;
            }

            public bool MoveNext()
            {
                if (_instance.MoveNext())
                {
                    Current = _instance.Current.Key;
                    return true;
                }

                Current = default;
                return false;
            }

            public void Reset()
            {
                _instance.Reset();
            }

            public TKey Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _instance.Dispose();
            }
        }

        private class ConcurrentDictionaryValueEnumerable : IEnumerable<TValue>
        {
            private readonly IEnumerable<KeyValuePair<TKey, TValue>> _instance;

            public ConcurrentDictionaryValueEnumerable(IEnumerable<KeyValuePair<TKey, TValue>> instance)
            {
                _instance = instance;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new ConcurrentDictionaryValueEnumerator(_instance.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ConcurrentDictionaryValueEnumerator : IEnumerator<TValue>
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> _instance;

            public ConcurrentDictionaryValueEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> instance)
            {
                _instance = instance;
            }

            public bool MoveNext()
            {
                if (_instance.MoveNext())
                {
                    Current = _instance.Current.Value;
                    return true;
                }

                Current = default;
                return false;
            }

            public void Reset()
            {
                _instance.Reset();
            }

            public TValue Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _instance.Dispose();
            }
        }
    }
}