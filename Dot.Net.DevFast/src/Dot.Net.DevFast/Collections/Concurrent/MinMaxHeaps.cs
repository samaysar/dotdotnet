using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Collections.Concurrent
{
    /// <summary>
    /// Lock based implementation of concurrent heap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockBasedConcurrentHeap<T> : IHeap<T>, ICompactAbleHeap, IResizableHeap
    {
        private readonly object _syncRoot = new object();
        private readonly AbstractSizableBinaryHeap<T> _heap;

        /// <inheritdoc />
        public LockBasedConcurrentHeap(AbstractSizableBinaryHeap<T> heap)
        {
            _heap = heap;
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
                _heap.Compact();
            }
        }
    }

    /// <summary>
    /// Lock based Binary Min Heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap type</typeparam>
    public sealed class ConcurrentMinHeap<T> : LockBasedConcurrentHeap<T>
    {
        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="resizeStrategy">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        public ConcurrentMinHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IResizeStrategy resizeStrategy = null) : base(new MinHeap<T>(initialCapacity, comparer, resizeStrategy))
        {
        }
    }

    /// <summary>
    /// Implementation of Binary Max Heap.
    /// </summary>
    /// <typeparam name="T">Heap type which also implements <seealso cref="IComparable{T}"/></typeparam>
    public class ConcurrentMaxHeap<T> : LockBasedConcurrentHeap<T>
    {
        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="resizeStrategy">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        public ConcurrentMaxHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IResizeStrategy resizeStrategy = null) : base(new MaxHeap<T>(initialCapacity, comparer, resizeStrategy))
        {
        }
    }
}