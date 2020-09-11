using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Sizable binary heap abstract implementation.
    /// </summary>
    /// <typeparam name="T">Heap type</typeparam>
    public abstract class AbstractSizableBinaryHeap<T> : AbstractBinaryHeap<T>
    {
        private IResizeStrategy _heapResizing;

        /// <summary>
        /// Ctor with initial capacity. <seealso cref="HeapNoResizing"/> is used as sizing strategy.
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        protected AbstractSizableBinaryHeap(int initialCapacity) : this(initialCapacity, new HeapNoResizing())
        {
        }

        /// <summary>
        /// Ctor with initial capacity and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="resizeStrategy">Heap resizing strategy.</param>
        /// <exception cref="DdnDfException">When given capacity is negative or resizing strategy is not provided.</exception>
        protected AbstractSizableBinaryHeap(int initialCapacity, IResizeStrategy resizeStrategy) : base(initialCapacity)
        {
            _heapResizing = resizeStrategy.ThrowIfNull($"{nameof(resizeStrategy)} is not provided.");
        }

        /// <summary>
        /// Gets the current truth value whether resizing is possible or not.
        /// <para>
        /// NOTE: After calling <see cref="FreezeCapacity"/>, it will always return false.
        /// </para>
        /// </summary>
        public bool CanResize => _heapResizing.CanResize;

        /// <summary>
        /// Calling this method will freeze the capacity (i.e. heap will not resize upon add).
        /// Also, runs compaction on the internally allocated storage based on <paramref name="compact"/> flag.
        /// </summary>
        /// <param name="compact">If true, internally allocated storage will be compacted. Careful it can induce some latency.</param>
        public void FreezeCapacity(bool compact = false)
        {
            _heapResizing = new HeapNoResizing();
            if (compact) Compact();
        }

        /// <summary>
        /// Apply resizing strategy when heap is full.
        /// </summary>
        protected sealed override bool EnsureCapacity()
        {
            if (!IsFull) return true;
            if (!_heapResizing.TryComputeNewSize(Count, out var newSize)) return false;
            InternalCopyData(newSize);
            return true;
        }
    }
}