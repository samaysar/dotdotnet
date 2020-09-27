using System.Collections.Generic;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Binary heap based limited element abstract heap, i.e. k-heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain heap state on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only HIGHEST/LOWEST k
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

        /// <summary>
        /// It ALWAYS returns true. Prefer using the overload <see cref="TryAdd(T,out T)"/>.
        /// </summary>
        /// <param name="item">Item to add</param>
        public sealed override bool TryAdd(T item)
        {
            TryAdd(item, out _);
            return true;
        }

        /// <summary>
        /// Returns false, if item has been added without replacing existing item
        /// or the given item cannot be added (not part of k-heap).
        /// When returns true, also outs the item that was popped (removed) from heap to
        /// keep the given item.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="popped">Popped item, if any, that added item has been replaced with</param>
        public bool TryAdd(T item, out T popped)
        {
            popped = default;
            if (base.TryAdd(item) || !LeftPrecedes(GetFirstUnsafe(), item)) return false;
            popped = Pop();
            return base.TryAdd(item);
        }
    }

    /// <summary>
    /// Binary heap based limited element min heap, i.e. k-min heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain heap state on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only LOWEST k
    /// elements, this abstract class is the right choice.
    /// </example>
    /// <para>
    /// NOTE: During pop, order is NOT guaranteed, however, it should NOT matter.
    /// </para>
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
    /// It will maintain heap state on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only HIGHEST k
    /// elements, this class is the right choice.
    /// </example>
    /// <para>
    /// NOTE: During pop, order is NOT guaranteed, however, it should NOT matter.
    /// </para>
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