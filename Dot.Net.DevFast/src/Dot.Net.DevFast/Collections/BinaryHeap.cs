using System;
using System.Collections.Generic;
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
        /// Ctor with <seealso cref="StdLookUps.DefaultHeapResizeStep"/> as default step value.
        /// </summary>
        public StepHeapResizing() : this(StdLookUps.DefaultHeapResizeStep)
        {
        }

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
    /// <typeparam name="T">Heap type</typeparam>
    public abstract class BinaryHeap<T>
    {
        private T[] _dataCollection;

        /// <summary>
        /// Ctor with initial heap capacity.
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap.</param>
        protected BinaryHeap(int initialCapacity)
        {
            _dataCollection = new T[initialCapacity.ThrowIfNegative($"{nameof(initialCapacity)} cannot be negative")];
            Count = 0;
        }

        /// <summary>
        /// Returns the first element of the heap.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When the heap is empty.</exception>
        public T Peek()
        {
            if (TryPeek(out var item)) return item;
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Returns the truth value whether heap contains at least one (1) item and outs the first element of the heap.
        /// </summary>
        public bool TryPeek(out T item)
        {
            item = _dataCollection[0];
            return !IsEmpty;
        }

        /// <summary>
        /// Removes and returns the first element from the heap.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When the heap is empty.</exception>
        public T Pop()
        {
            if (TryPop(out var item)) return item;
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Returns the truth value whether the first heap element was successfully removed
        /// and outs that element.
        /// </summary>
        public bool TryPop(out T item)
        {
            item = _dataCollection[0];
            if (IsEmpty) return false;
            _dataCollection[0] = _dataCollection[--Count];
            PushDown();
            return true;
        }

        /// <summary>
        /// Adds given element to the heap.
        /// </summary>
        /// <param name="item">Element to add</param>
        public void Add(T item)
        {
            EnsureCapacity();
            _dataCollection[Count] = item;
            BubbleUp(Count++);
        }

        /// <summary>
        /// Gets the truth value whether the heap is empty or not.
        /// </summary>
        public bool IsEmpty => Count.Equals(0);

        /// <summary>
        /// Gets the truth value whether the heap is full or not.
        /// </summary>
        public bool IsFull => Count.Equals(Capacity);

        /// <summary>
        /// Current count of the elements in the heap.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Current capacity of the heap.
        /// </summary>
        public int Capacity => _dataCollection.Length;

        private static int LeftChildIndex(int elementIndex) => (elementIndex << 1) + 1;
        private static int ParentIndex(int elementIndex) => (elementIndex - 1) >> 1;

        /// <summary>
        /// Internally allocated storage will be compacted to match the current <see cref="Count"/>.
        /// <para>
        /// CAREFUL: Compaction can induce some latency. Do NOT call if memory gain is insignificant.
        /// </para>
        /// </summary>
        public void Compact()
        {
            InternalCopyData(Count);
        }

        private void BubbleUp(int current)
        {
            while (!current.Equals(0))
            {
                var parentIndex = ParentIndex(current);
                if (!LeftPrecedes(_dataCollection[current], _dataCollection[parentIndex])) return;
                current = Swap(current, parentIndex);
            }
        }

        private void PushDown()
        {
            var current = 0;
            var leftIndex = 1;
            while (leftIndex < Count)
            {
                var swapWith = current;
                var rightIndex = leftIndex + 1;
                if (rightIndex < Count &&
                    LeftPrecedes(_dataCollection[rightIndex], _dataCollection[leftIndex]))
                {
                    swapWith = rightIndex;
                }
                else
                {
                    if (LeftPrecedes(_dataCollection[leftIndex], _dataCollection[current]))
                    {
                        swapWith = leftIndex;
                    }
                }

                if (current.Equals(swapWith)) return;
                current = Swap(current, swapWith);
                leftIndex = LeftChildIndex(current);
            }
        }

        private void EnsureCapacity()
        {
            if (!IsFull) return;
            InternalCopyData(NewSize(Count)
                .ThrowIfLess(Count + 1, "New heap size cannot be less than current heap size."));
        }

        private void InternalCopyData(int newSize)
        {
            var newCollection = new T[newSize];
            Array.Copy(_dataCollection, newCollection, Count);
            _dataCollection = newCollection;
        }

        private int Swap(int firstIndex, int secondIndex)
        {
            var temp = _dataCollection[firstIndex];
            _dataCollection[firstIndex] = _dataCollection[secondIndex];
            _dataCollection[secondIndex] = temp;
            return secondIndex;
        }

        /// <summary>
        /// Computes the new heap size based on its current size.
        /// </summary>
        /// <param name="currentSize">Current size of the heap.</param>
        protected abstract int NewSize(int currentSize);

        /// <summary>
        /// Returns the truth value whether given <paramref name="left"/> element precedes
        /// compared to given <paramref name="right"/> element, in assumed sorted order (i.e. if
        /// we start popping out elements from heap, whether element provided as <paramref name="left"/> must
        /// be popped out before the element provided as <paramref name="right"/>).
        /// </summary>
        /// <param name="left">Left element</param>
        /// <param name="right">Right element</param>
        protected abstract bool LeftPrecedes(T left, T right);
    }

    /// <summary>
    /// Sizable binary heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap type</typeparam>
    public abstract class SizableBinaryHeap<T> : BinaryHeap<T>
    {
        private IHeapResizing _heapResizing;

        /// <summary>
        /// Ctor with initial capacity and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="heapResizing">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        protected SizableBinaryHeap(int initialCapacity,
            IHeapResizing heapResizing = null) : base(initialCapacity)
        {
            _heapResizing = heapResizing ?? new HeapNoResizing();
        }

        /// <summary>
        /// Computes the new heap size based on supplied resizing strategy.
        /// </summary>
        /// <param name="currentSize">Current size of the heap.</param>
        protected sealed override int NewSize(int currentSize) => _heapResizing.NewSize(currentSize);

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
    }

    /// <summary>
    /// Implementation of Binary Min Heap.
    /// </summary>
    /// <typeparam name="T">Heap type</typeparam>
    public class MinHeap<T> : SizableBinaryHeap<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="heapResizing">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        public MinHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IHeapResizing heapResizing = null) : base(initialCapacity, heapResizing ?? new HeapNoResizing())
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
    public class MaxHeap<T> : SizableBinaryHeap<T>
        where T : IComparable<T>
    {
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Ctor with initial capacity, comparer and resizing strategy.
        /// Different in-built resizing strategy are available (<seealso cref="HeapNoResizing"/>, <seealso cref="StepHeapResizing"/>
        /// and <seealso cref="PercentHeapResizing"/>).
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="heapResizing">Heap resizing strategy. If not provided, then <seealso cref="HeapNoResizing"/> will be internally used.</param>
        public MaxHeap(int initialCapacity,
            IComparer<T> comparer = null,
            IHeapResizing heapResizing = null) : base(initialCapacity, heapResizing ?? new HeapNoResizing())
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