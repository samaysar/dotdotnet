namespace Dot.Net.DevFast.Collections.Interfaces
{
    /// <summary>
    /// Interface to declare compaction contract.
    /// </summary>
    public interface ICompactAbleHeap
    {
        /// <summary>
        /// Internally allocated storage will be compacted to match the current usage.
        /// </summary>
        void Compact();
    }
}