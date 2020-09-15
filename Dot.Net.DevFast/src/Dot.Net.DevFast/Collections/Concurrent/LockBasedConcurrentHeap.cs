using System;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections.Concurrent
{
    /// <summary>
    /// Lock based implementation of concurrent heap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockBasedConcurrentHeap<T> : IResizableHeap<T>
    {
        private readonly object _syncRoot;
        private readonly IResizableHeap<T> _heap;

        // for testing purpose
        internal LockBasedConcurrentHeap(IResizableHeap<T> heap,
            object syncRoot)
        {
            _heap = heap;
            _syncRoot = syncRoot;
        }

        /// <summary>
        /// Ctor with heap instance.
        /// </summary>
        /// <param name="heap">Instance of the heap secure with lock</param>
        /// <exception cref="DdnDfException">When provided instance is null.</exception>
        public LockBasedConcurrentHeap(IResizableHeap<T> heap) :
            this(heap.ThrowIfNull($"{nameof(heap)} instance not provided."),
                new object())
        {
        }

        /// <inheritdoc />
        public bool IsEmpty
        { 
            get
            {
                lock (_syncRoot)
                {
                    return _heap.IsEmpty;
                }
            }
        }

        /// <inheritdoc />
        public bool IsFull
        {
            get
            {
                lock (_syncRoot)
                {
                    return _heap.IsFull;
                }
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _heap.Count;
                }
            }
        }

        /// <inheritdoc />
        public int Capacity
        {
            get
            {
                lock (_syncRoot)
                {
                    return _heap.Capacity;
                }
            }
        }

        /// <inheritdoc />
        public bool CanResize
        {
            get
            {
                lock (_syncRoot)
                {
                    return _heap.CanResize;
                }
            }
        }

        /// <summary>
        /// Returns the first element of the heap without removing it from the heap.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When the heap is empty.</exception>
        public T Peek()
        {
            lock (_syncRoot)
            {
                return _heap.Peek();
            }
        }

        /// <inheritdoc />
        public bool TryPeek(out T item)
        {
            lock (_syncRoot)
            {
                return _heap.TryPeek(out item);
            }
        }

        /// <summary>
        /// Removes and returns the first element from the heap.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When the heap is empty.</exception>
        public T Pop()
        {
            lock (_syncRoot)
            {
                return _heap.Pop();
            }
        }

        /// <inheritdoc />
        public bool TryPop(out T item)
        {
            lock (_syncRoot)
            {
                return _heap.TryPop(out item);
            }
        }

        /// <summary>
        /// Adds given element to the heap.
        /// </summary>
        /// <param name="item">Element to add</param>
        /// <exception cref="DdnDfException">When element cannot be added.</exception>
        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _heap.Add(item);
            }
        }

        /// <inheritdoc />
        public bool TryAdd(T item)
        {
            lock (_syncRoot)
            {
                return _heap.TryAdd(item);
            }
        }

        /// <inheritdoc />
        public void Compact()
        {
            lock (_syncRoot)
            {
                _heap.Compact();
            }
        }

        /// <inheritdoc />
        public void FreezeCapacity(bool compact)
        {
            lock (_syncRoot)
            {
                _heap.FreezeCapacity(compact);
            }
        }
    }
}