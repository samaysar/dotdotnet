namespace Dot.Net.DevFast.Collections.Interfaces
{
    /// <summary>
    /// Interface exposing sizing strategy for the binary heap.
    /// </summary>
    public interface IResizeStrategy
    {
        /// <summary>
        /// Calculates the new size of the heap, based on the given value of current size.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        /// <param name="newSize">New size</param>
        bool TryComputeNewSize(int currentSize, out int newSize);

        /// <summary>
        /// Gets the truth value whether new size can be computed.
        /// </summary>
        bool CanResize { get; }
    }
}