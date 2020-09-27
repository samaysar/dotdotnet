using System.Collections.Generic;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Binary heap based limited element abstract heap, i.e. k-heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain the order on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only TOP/BOTTOM k
    /// elements, this abstract class is the right choice.
    /// </example>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public abstract class AbstractLimitHeap<T>: AbstractBinaryHeap<T>
    {
        /// <summary>
        /// Ctor with heap size, i.e. total number of elements to keep.
        /// </summary>
        /// <param name="heapSize">Total top elements to keep</param>
        /// <exception cref="DdnDfException">When supplied size is less than 1</exception>
        protected AbstractLimitHeap(int heapSize) 
            : base(heapSize.ThrowIfLess(1, $"{nameof(heapSize)} cannot be less than 1"))
        {
        }

        /// <inheritdoc />
        public override bool TryAdd(T item)
        {
            if (base.TryAdd(item)) return true;
            if (!LeftPrecedes(GetFirstUnsafe(), item)) return false;
            Pop();
            return base.TryAdd(item);
        }
    }

    /// <summary>
    /// Binary heap based limited element min heap, i.e. k-min heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain the order on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only MINIMUM k
    /// elements, this class is the right choice.
    /// </example>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public sealed class MinLimitHeap<T> : AbstractLimitHeap<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with heap size, i.e. total number of elements to keep, and comparer instance.
        /// </summary>
        /// <param name="heapSize">Total top elements to keep</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <exception cref="DdnDfException">When supplied size is less than 1</exception>
        public MinLimitHeap(int heapSize,
            IComparer<T> comparer = null) : base(heapSize)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <inheritdoc />
        protected override bool LeftPrecedes(T left, T right) => _comparer.Compare(left, right) > 0;
    }

    /// <summary>
    /// Binary heap based limited element min heap, i.e. k-max heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain the order on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only MAXIMUM k
    /// elements, this class is the right choice.
    /// </example>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public sealed class MaxLimitHeap<T> : AbstractLimitHeap<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with heap size, i.e. total number of elements to keep, and comparer instance.
        /// </summary>
        /// <param name="heapSize">Total top elements to keep</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <exception cref="DdnDfException">When supplied size is less than 1</exception>
        public MaxLimitHeap(int heapSize,
            IComparer<T> comparer = null) : base(heapSize)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <inheritdoc />
        protected override bool LeftPrecedes(T left, T right) => _comparer.Compare(left, right) < 0;
    }
}