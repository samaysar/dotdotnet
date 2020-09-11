namespace Dot.Net.DevFast.Collections.Interfaces
{
    /// <summary>
    /// Interface to declare compaction contract.
    /// </summary>
    public interface ICompactAbleHeap
    {
        /// <summary>
        /// Internally allocated storage will be compacted to match the current usage.
        /// <para>
        /// CAREFUL: Compaction can induce some latency. Do NOT call if memory gain is insignificant.
        /// </para>
        /// </summary>
        void Compact();
    }
}