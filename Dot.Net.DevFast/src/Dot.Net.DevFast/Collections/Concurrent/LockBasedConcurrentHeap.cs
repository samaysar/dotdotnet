using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections.Concurrent
{
    /// <summary>
    /// Lock based implementation of concurrent heap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockBasedConcurrentHeap<T> : IResizableHeap<T>
    {
        private readonly object _syncRoot;
        private readonly IResizableHeap<T> _heap;

        // for testing purpose
        internal LockBasedConcurrentHeap(IResizableHeap<T> heap,
            object syncRoot)
        {
            _heap = heap;
            _syncRoot = syncRoot;
        }

        /// <summary>
        /// Ctor with heap instance.
        /// </summary>
        /// <param name="heap">Instance of the heap secure with lock</param>
        /// <exception cref="DdnDfException">When provided instance is null.</exception>
        public LockBasedConcurrentHeap(IResizableHeap<T> heap) :
            this(heap.ThrowIfNull($"{nameof(heap)} instance not provided."),
                new object())
        {
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

        /// <summary>
        /// Pops all the items (without using snapshot of the current state), i.e.
        /// during enumeration, all other permutations will be reflected.
        /// Though it is safe to call this method several times concurrently, each enumeration
        /// will only receive the part of the state (dependents on the exact state of the heap at runtime). Hence,
        /// order is guaranteed, in absence of concurrent adding, otherwise not.
        /// In effect, if only single call of this method is running without any other permutation
        /// to its state (like no other add, pop is called concurrently),
        /// then the output will be the enumeration of all the elements in the correct order.
        /// Finally, if only concurrent pops are running (without any concurrent adding), then order
        /// is still guaranteed but not the sequence pattern.
        /// </summary>
        public IEnumerable<T> PopAll()
        {
            bool hasItem;
            T next;
            lock (_syncRoot)
            {
                hasItem = _heap.TryPop(out next);
            }

            while (hasItem)
            {
                yield return next;
                lock (_syncRoot)
                {
                    hasItem = _heap.TryPop(out next);
                }
            }
        }

        /// <summary>
        /// Complementary to <see cref="PopAll"/>. It extracts all the elements based on
        /// current state of the heap.
        /// <para>
        /// CAREFUL: As the method returns <seealso cref="List{T}"/> (instead of <seealso cref="IEnumerable{T}"/>),
        /// it will allocate memory. For large heap, this can lead to latency and memory consumption.
        /// </para>
        /// </summary>
        public List<T> PopAllConsistent()
        {
            lock (_syncRoot)
            {
                return _heap is AbstractBinaryHeap<T> abstractHeap
                    ? abstractHeap.PopAllConsistent()
                    : _heap.PopAll().ToList();
            }
        }

        /// <summary>
        /// Adds all the items (using snapshot of the current state) to the heap, i.e.
        /// during enumeration, all elements will be added based on the state that was captured.
        /// And, it is also safe to call this method several times concurrently, each enumeration
        /// will add elements to heap based on its runtime state. Nonetheless, for the whole duration of the enumeration,
        /// the underlying head wont be usable and all other operations will block. If the supplied enumeration is slow
        /// to execute and/or time consuming otherwise, AVOID this method and instead use <see cref="Add"/>.
        /// Finally, order and elements' sequencing, both, are guaranteed, in absence of concurrent pops, otherwise not.
        /// </summary>
        public int AddAll(IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                return _heap.AddAll(items);
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
                _heap.FreezeCapacity(compact);
            }
        }

        /// <inheritdoc />
        public T[] All()
        {
            lock (_syncRoot)
            {
                return _heap.All();
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return All().AsEnumerable().GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}