using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Implementation of Binary Min Heap.
    /// </summary>
    /// <typeparam name="T">Heap type</typeparam>
    public class MinHeap<T> : AbstractSizableBinaryHeap<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="resizeStrategy">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        public MinHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IResizeStrategy resizeStrategy = null) : base(initialCapacity, resizeStrategy ?? new HeapNoResizing())
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Return true when left is less than compared to its right.
        /// </summary>
        /// <param name="left">Left hand element</param>
        /// <param name="right">Right hand element</param>
        protected sealed override bool LeftPrecedes(T left, T right) => _comparer.Compare(left, right) < 0;
    }

    /// <summary>
    /// Implementation of Binary Max Heap.
    /// </summary>
    /// <typeparam name="T">Heap type which also implements <seealso cref="IComparable{T}"/></typeparam>
    public class MaxHeap<T> : AbstractSizableBinaryHeap<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="resizeStrategy">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        public MaxHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IResizeStrategy resizeStrategy = null) : base(initialCapacity, resizeStrategy ?? new HeapNoResizing())
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Return true when left is greater than compared to its right.
        /// </summary>
        /// <param name="left">Left hand element</param>
        /// <param name="right">Right hand element</param>
        protected sealed override bool LeftPrecedes(T left, T right) => _comparer.Compare(left, right) > 0;
    }
}