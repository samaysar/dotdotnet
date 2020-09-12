using System.Collections.Concurrent;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Parallel buffer related standard values and methods.
    /// </summary>
    public static class ConcurrentBuffer
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
        /// NOTE: <see cref="Unbounded"/> is a special number to create unbounded buffer.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="bufferSize">Size of buffer.</param>
        /// <exception cref="DdnDfException">When given size is negative</exception>
        public static BlockingCollection<T> CreateBuffer<T>(int bufferSize)
        {
            return bufferSize.ThrowIfNegative($"Parallel Buffer size cannot be negative. (Value: {bufferSize})")
                .Equals(Unbounded)
                ? new BlockingCollection<T>()
                : new BlockingCollection<T>(bufferSize);
        }
    }
}