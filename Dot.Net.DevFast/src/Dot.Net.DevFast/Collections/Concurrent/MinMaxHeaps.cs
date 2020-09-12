using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Collections.Concurrent
{
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
    /// Lock based Binary Max Heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap type which also implements <seealso cref="IComparable{T}"/></typeparam>
    public sealed class ConcurrentMaxHeap<T> : LockBasedConcurrentHeap<T>
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