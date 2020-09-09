using System;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Interface exposing sizing strategy for the binary heap.
    /// </summary>
    public interface IHeapResizing
    {
        /// <summary>
        /// Calculates the new size of the heap, based on the given value of current size.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        int NewSize(int currentSize);
    }

    /// <inheritdoc />
    /// <summary>
    /// Simply throws exception saying heap initially created supposed to have fixed capacity.
    /// </summary>
    public sealed class HeapNoResizing : IHeapResizing
    {
        /// <summary>
        /// Calling this method will throw an exception as no resizing can be done.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        /// <exception cref="InvalidOperationException">Resizing heap is not possible.</exception>
        public int NewSize(int currentSize)
        {
            throw new InvalidOperationException("Heap has a fixed capacity that cannot be increased.");
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Increases size of the heap in a fixed size steps.
    /// </summary>
    public sealed class StepHeapResizing : IHeapResizing
    {
        private readonly int _stepSize;

        /// <summary>
        /// Ctor with step size.
        /// </summary>
        /// <param name="stepSize">Step size for the increments.</param>
        public StepHeapResizing(int stepSize)
        {
            _stepSize = stepSize.ThrowIfLess(1, $"{nameof(stepSize)} cannot be zero (0) or negative.");
        }

        /// <summary>
        /// New size is simply the sum of the initial fixed step size and current capacity.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        public int NewSize(int currentSize)
        {
            return _stepSize + currentSize;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Increases size of the heap by given percentage.
    /// </summary>
    public sealed class PercentHeapResizing : IHeapResizing
    {
        private readonly double _multiplier;

        /// <summary>
        /// Ctor with increment percentage.
        /// <para>
        /// Value 100 means new value will be increased by 100 percent (i.e. double).
        /// Similarly 50 means value will be multiplied by 1.5 (cast to int).
        /// </para>
        /// <para>
        /// IMPORTANT: If the percent increase (after int cast) yield no increase, then value
        /// will be increased by 1.
        /// </para>
        /// </summary>
        /// <param name="incrementPercentage">Percentage to use.</param>
        public PercentHeapResizing(int incrementPercentage)
        {
            _multiplier =
                (1 + incrementPercentage.ThrowIfLess(1,
                     $"{nameof(incrementPercentage)} cannot be zero (0) or negative.") / 100.0);
        }

        /// <summary>
        /// New size is multiplier increased (int cast), with lower bound to <paramref name="currentSize"/>+1.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        public int NewSize(int currentSize)
        {
            var newSize = (int) (currentSize * _multiplier);
            return newSize.Equals(currentSize) ? (currentSize + 1) : newSize;
        }
    }

    /// <summary>
    /// Abstract binary heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap type which also implements <seealso cref="IComparable{T}"/></typeparam>
    public abstract class BinaryHeap<T>
        where T : IComparable<T>
    {
        private T[] _dataCollection;
        private int _count;

        /// <summary>
        /// Ctor with initial heap capacity.
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        protected BinaryHeap(int initialCapacity)
        {
            _dataCollection =
                new T[initialCapacity.ThrowIfNegative($"{nameof(initialCapacity)} cannot be negative.").Equals(0)
                    ? StdLookUps.DefaultHeapSize
                    : initialCapacity];
            _count = 0;
        }

        /// <summary>
        /// Add an item to the heap.
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            EnsureCapacity();
            _dataCollection[_count] = item;
            BubbleUp(_count++);
        }

        private static int GetLeftChildIndex(int elementIndex) => (elementIndex << 1) + 1;
        private static int GetRightChildIndex(int elementIndex) => (elementIndex + 1) << 1;
        private static int GetParentIndex(int elementIndex) => (elementIndex - 1) >> 1;

        private void BubbleUp(int current)
        {
            while (!current.Equals(0) &&
                   ChildGoesUp(_dataCollection[current], _dataCollection[GetParentIndex(current)]))
            {

            }
        }

        private void EnsureCapacity()
        {
            if (!_count.Equals(_dataCollection.Length)) return;
            var newCollection = new T[NewSize(_count)
                .ThrowIfLess(_count + 1, "New heap size cannot be less than current heap size.")];
            Array.Copy(_dataCollection, newCollection, _count);
            _dataCollection = newCollection;
        }

        private void Swap(int firstIndex, int secondIndex)
        {
            var temp = _dataCollection[firstIndex];
            _dataCollection[firstIndex] = _dataCollection[secondIndex];
            _dataCollection[secondIndex] = temp;
        }

        /// <summary>
        /// Computes the new heap size based on its current size.
        /// </summary>
        /// <param name="currentSize">Current size of the heap.</param>
        protected abstract int NewSize(int currentSize);

        /// <summary>
        /// Returns the truth value whether given <paramref name="child"/> should be above in binary tree hierarchy
        /// compared to its given <paramref name="parent"/>.
        /// </summary>
        /// <param name="child">Child item</param>
        /// <param name="parent">Parent item</param>
        protected abstract bool ChildGoesUp(T child, T parent);
    }

    /// <summary>
    /// Min binary heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap type which also implements <seealso cref="IComparable{T}"/></typeparam>
    public sealed class MinHeap<T> : BinaryHeap<T>
        where T : IComparable<T>
    {
        private readonly IHeapResizing _heapResizing;

        /// <summary>
        /// Ctor with initial capacity and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="heapResizing">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        public MinHeap(int initialCapacity,
            IHeapResizing heapResizing = null) : base(initialCapacity)
        {
            _heapResizing = heapResizing ?? new HeapNoResizing();
        }

        /// <summary>
        /// Computes the new heap size based on its current size.
        /// </summary>
        /// <param name="currentSize">Current size of the heap.</param>
        protected override int NewSize(int currentSize)
        {
            return _heapResizing.NewSize(currentSize);
        }

        /// <summary>
        /// Return true when child is smaller compared to its parent.
        /// </summary>
        /// <param name="child">Child item</param>
        /// <param name="parent">Parent item</param>
        protected override bool ChildGoesUp(T child, T parent)
        {
            return child.CompareTo(parent) < 0;
        }
    }
}