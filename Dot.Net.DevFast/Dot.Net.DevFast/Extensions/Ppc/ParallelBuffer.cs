using System.Collections.Concurrent;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Parallel buffer related standard values and methods.
    /// </summary>
    public static class ParallelBuffer
    {
        /// <summary>
        /// Standard buffer size.
        /// </summary>
        public const int StandardSize = 256;

        /// <summary>
        /// Minimum buffer size.
        /// </summary>
        public const int MinSize = 1;

        /// <summary>
        /// Unbounded buffer size.
        /// </summary>
        public const int Unbounded = 0;

        /// <summary>
        /// Created blocking collection with given buffer size.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="bufferSize">Size of buffer.</param>
        public static BlockingCollection<T> CreateBuffer<T>(int bufferSize)
        {
            return bufferSize.ThrowIfNegative($"Parallel Buffer size cannot be negative. (Value: {bufferSize})")
                .Equals(Unbounded)
                ? new BlockingCollection<T>()
                : new BlockingCollection<T>(bufferSize);
        }
    }
}