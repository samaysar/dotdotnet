using System;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Abstract binary heap implementation.
    /// </summary>
    /// <typeparam name="T">Heap element type</typeparam>
    public abstract class AbstractBinaryHeap<T> : IHeap<T>, ICompactAbleHeap
    {
        private T[] _dataCollection;

        /// <summary>
        /// Ctor with initial heap capacity.
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        protected AbstractBinaryHeap(int initialCapacity)
        {
            _dataCollection = new T[initialCapacity.ThrowIfNegative($"{nameof(initialCapacity)} cannot be negative")];
            Count = 0;
        }

        /// <inheritdoc />
        public bool IsEmpty => Count.Equals(0);

        /// <inheritdoc />
        public bool IsFull => Count.Equals(Capacity);

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public int Capacity => _dataCollection.Length;

        private static int LeftChildIndex(int elementIndex) => (elementIndex << 1) + 1;
        private static int ParentIndex(int elementIndex) => (elementIndex - 1) >> 1;

        /// <summary>
        /// Returns the first element of the heap without removing it from the heap.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When the heap is empty.</exception>
        public T Peek()
        {
            if (TryPeek(out var item)) return item;
            throw new IndexOutOfRangeException();
        }

        /// <inheritdoc />
        public bool TryPeek(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }
            item = _dataCollection[0];
            return true;
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

        /// <inheritdoc />
        public bool TryPop(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }
            item = _dataCollection[0];
            _dataCollection[0] = _dataCollection[--Count];
            PushDown();
            return true;
        }

        /// <summary>
        /// Adds given element to the heap.
        /// </summary>
        /// <param name="item">Element to add</param>
        /// <exception cref="DdnDfException">When element cannot be added.</exception>
        public void Add(T item)
        {
            if (!TryAdd(item))
            {
                throw new DdnDfException(DdnDfErrorCode.DemandUnfulfilled,
                    "Unable to add element in the heap.");
            }
        }

        /// <inheritdoc />
        public bool TryAdd(T item)
        {
            if (!EnsureCapacity()) return false;
            _dataCollection[Count] = item;
            BubbleUp(Count++);
            return true;
        }

        /// <inheritdoc />
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
                current = SwapNReturnLastParam(current, parentIndex);
            }
        }

        private void PushDown()
        {
            var current = 0;
            var leftIndex = 1;
            while (leftIndex < Count)
            {
                var swapWith = leftIndex;
                var rightIndex = leftIndex + 1;
                if (rightIndex < Count &&
                    LeftPrecedes(_dataCollection[rightIndex], _dataCollection[leftIndex]))
                {
                    swapWith = rightIndex;
                }
                if (!LeftPrecedes(_dataCollection[swapWith], _dataCollection[current]))
                {
                    return;
                }
                current = SwapNReturnLastParam(current, swapWith);
                leftIndex = LeftChildIndex(current);
            }
        }

        /// <summary>
        /// Ensures that there is a capacity to add an element.
        /// </summary>
        protected virtual bool EnsureCapacity()  => !IsFull;

        /// <summary>
        /// Replaces the internal array with a new array of a given <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of the new array.</param>
        /// <exception cref="DdnDfException">When the given size is less than current count.</exception>
        protected void InternalCopyData(int size)
        {
            size.ThrowIfLess(Count, $"{nameof(size)} is less than current {nameof(Count)}. Cannot resize.");
            var newCollection = new T[size];
            Array.Copy(_dataCollection, newCollection, Count);
            _dataCollection = newCollection;
        }

        private int SwapNReturnLastParam(int firstIndex, int secondIndex)
        {
            var temp = _dataCollection[firstIndex];
            _dataCollection[firstIndex] = _dataCollection[secondIndex];
            _dataCollection[secondIndex] = temp;
            return secondIndex;
        }

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
}