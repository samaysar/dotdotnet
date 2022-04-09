using System.Collections.Generic;

namespace Dot.Net.DevFast.Collections.Interfaces
{
    /// <summary>
    /// Heap data structure interface.
    /// </summary>
    /// <typeparam name="T">Heap element type</typeparam>
    public interface IHeap<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the truth value whether the heap is empty or not.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the truth value whether the heap is full or not.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Current count of the elements in the heap.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Current capacity of the heap.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Returns the first element of the heap without removing it from the heap.
        /// </summary>
        T Peek();

        /// <summary>
        /// Returns the truth value whether heap contains at least one (1) item and outs the first element of the heap
        /// without removing it from the heap.
        /// </summary>
        /// <param name="item">out element</param>
        bool TryPeek(out T item);

        /// <summary>
        /// Removes and returns the first element from the heap.
        /// </summary>
        T Pop();

        /// <summary>
        /// Returns the truth value whether the first heap element was successfully removed
        /// and outs that element.
        /// </summary>
        /// <param name="item">out element</param>
        bool TryPop(out T item);

        /// <summary>
        /// Adds given element to the heap.
        /// </summary>
        /// <param name="item">Element to add</param>
        void Add(T item);

        /// <summary>
        /// Tries adding given element in the heap.
        /// Returns the truth value whether it was successfully added or not.
        /// </summary>
        /// <param name="item">Element to add.</param>
        bool TryAdd(T item);

        /// <summary>
        /// Removes all the elements from the heap.
        /// </summary>
        IEnumerable<T> PopAll();

        /// <summary>
        /// Adds all elements of the given enumeration to the heap.
        /// Returns the count of the elements that were successfully added.
        /// </summary>
        /// <param name="items">Enumeration of the Elements to add.</param>
        int AddAll(IEnumerable<T> items);

        /// <summary>
        /// Returns a copy of the internal collection without removing elements from it.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> All();
    }
}