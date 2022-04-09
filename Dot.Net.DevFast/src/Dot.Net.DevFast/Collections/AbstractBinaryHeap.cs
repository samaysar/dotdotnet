using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private T[] _heapData;

        /// <summary>
        /// Ctor with initial heap capacity.
        /// </summary>
        /// <param name="initialCapacity">Initial capacity of the heap.</param>
        /// <exception cref="DdnDfException">When given capacity is negative.</exception>
        protected AbstractBinaryHeap(int initialCapacity)
        {
            _heapData = new T[initialCapacity.ThrowIfNegative($"{nameof(initialCapacity)} cannot be negative")];
            Count = 0;
        }

        /// <inheritdoc />
        public bool IsEmpty => Count.Equals(0);

        /// <inheritdoc />
        public bool IsFull => Count.Equals(Capacity);

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public int Capacity => _heapData.Length;

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
        public virtual bool TryPeek(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }
            item = _heapData[0];
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
        public virtual bool TryPop(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }
            item = _heapData[0];
            _heapData[0] = _heapData[--Count];
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
        public virtual bool TryAdd(T item)
        {
            if (!EnsureCapacity()) return false;
            _heapData[Count] = item;
            BubbleUp(Count++);
            return true;
        }

        /// <inheritdoc />
        public IEnumerable<T> PopAll()
        {
            while (TryPop(out var item))
            {
                yield return item;
            }
        }

        /// <inheritdoc />
        public int AddAll(IEnumerable<T> items)
        {
            var count = 0;
            foreach (var item in items)
            {
                if (!TryAdd(item)) return count;
                count++;
            }

            return count;
        }

        /// <inheritdoc />
        public void Compact()
        {
            InternalCopyData(Count);
        }

        /// <summary>
        /// Its an unsafe getter. !!! Does not check for the presence of the element
        /// at 0th index. !!!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T GetFirstUnsafe()
        {
            return _heapData[0];
        }

        internal List<T> PopAllConsistent()
        {
            var results = new List<T>(Count);
            results.AddRange(PopAll());
            return results;
        }

        internal IEnumerable<T> InternalStateAsEnumerable()
        {
            return new ArraySegment<T>(_heapData, 0, Count);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BubbleUp(int current)
        {
            while (!current.Equals(0))
            {
                var parentIndex = ParentIndex(current);
                if (!LeftPrecedes(_heapData[current], _heapData[parentIndex])) return;
                current = SwapNReturnLastParam(current, parentIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushDown()
        {
            var current = 0;
            var leftIndex = 1;
            while (leftIndex < Count)
            {
                var swapWith = leftIndex;
                var rightIndex = leftIndex + 1;
                if (rightIndex < Count &&
                    LeftPrecedes(_heapData[rightIndex], _heapData[leftIndex]))
                {
                    swapWith = rightIndex;
                }
                if (!LeftPrecedes(_heapData[swapWith], _heapData[current]))
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool EnsureCapacity()  => !IsFull;

        /// <summary>
        /// Replaces the internal array with a new array of a given <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of the new array.</param>
        /// <exception cref="DdnDfException">When the given size is less than current count.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void InternalCopyData(int size)
        {
            size.ThrowIfLess(Count, $"{nameof(size)} is less than current {nameof(Count)}. Cannot resize.");
            var newCollection = new T[size];
            Array.Copy(_heapData, newCollection, Count);
            _heapData = newCollection;
        }

        private int SwapNReturnLastParam(int firstIndex, int secondIndex)
        {
            var temp = _heapData[firstIndex];
            _heapData[firstIndex] = _heapData[secondIndex];
            _heapData[secondIndex] = temp;
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

        /// <inheritdoc/>
        public IEnumerable<T> All()
        {
            var newCollection = new T[Count];
            Array.Copy(_heapData, newCollection, Count);
            return newCollection;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return InternalStateAsEnumerable().GetEnumerator();
        }
    }
}