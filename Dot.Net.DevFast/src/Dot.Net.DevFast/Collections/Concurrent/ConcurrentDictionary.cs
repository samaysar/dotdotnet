using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<TKey, TValue>[] _data;
        private volatile int _count;
        private readonly int _concurrencyLevel;

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
            _count = 0;
            _concurrencyLevel = Math.Max(2, Math.Min(Math.Max(2, concurrencyLevel), Environment.ProcessorCount));
            _data = new Dictionary<TKey, TValue>[_concurrencyLevel];
            initialCapacity = Math.Max(0, (int)Math.Ceiling(((double)initialCapacity) / _concurrencyLevel));
            for (var i = 0; i < _concurrencyLevel; i++)
            {
                _data[i] = new Dictionary<TKey, TValue>(initialCapacity, comparer);
            }
        }

        /// <inheritdoc />
        public int Count => _count;

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
            //We do not want to take all locks together
            //this call will provide best-effort clearing on whole collection.
            foreach (var d in _data)
            {
                var lockTaken = false;
                try
                {
                    Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                    d.Clear();
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
            valueComparer ??= EqualityComparer<TValue>.Default;
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
                          valueComparer.Equals(v, item.Value) &&
                          d.Remove(item.Key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }

            if (removed)
            {
                Interlocked.Decrement(ref _count);
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

            if (removed)
            {
                Interlocked.Decrement(ref _count);
            }

            return removed;
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

            Interlocked.Increment(ref _count);
        }

        /// <inheritdoc />
        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            var d = GetPartition(key);
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                return d.ContainsKey(key);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }
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
            try
            {
                Monitor.TryEnter(d, Timeout.Infinite, ref lockTaken);
                return d.TryGetValue(key, out value);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(d);
            }
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public TValue this[TKey key]
        {
            get => TryGetValue(key, out var v) ? v : throw new KeyNotFoundException();
            set => Add(key, value);
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => ((IReadOnlyDictionary<TKey, TValue>)this).Keys.ToList();

        /// <inheritdoc />
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => new ConcurrentDictionaryKeyEnumerable(this);

        /// <inheritdoc />
        public ICollection<TValue> Values => ((IReadOnlyDictionary<TKey, TValue>)this).Values.ToList();

        /// <inheritdoc />
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => new ConcurrentDictionaryValueEnumerable(this);

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