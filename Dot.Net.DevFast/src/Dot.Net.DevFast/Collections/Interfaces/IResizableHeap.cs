namespace Dot.Net.DevFast.Collections.Interfaces
{
    /// <summary>
    /// Interface to declare resizing operations contract.
    /// </summary>
    public interface IResizableHeap<T> : IHeap<T>, ICompactAbleHeap
    {
        /// <summary>
        /// Gets the current truth value whether resizing is possible or not.
        /// <para>
        /// NOTE: After calling <see cref="FreezeCapacity"/>, it will always return false.
        /// </para>
        /// </summary>
        bool CanResize { get; }

        /// <summary>
        /// Calling this method will freeze the capacity (i.e. heap will not resize upon add).
        /// Also, runs compaction on the internally allocated storage based on <paramref name="compact"/> flag.
        /// </summary>
        /// <param name="compact">If true, internally allocated storage will be compacted.</param>
        void FreezeCapacity(bool compact);
    }
}