using System;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Interface to expose concurrent producer-consumer pipeline operations.
    /// </summary>
    public interface IConcurrentPipeline<in T> : IDisposable
    {
        /// <summary>
        /// Accepts an item to be consumed by the pipeline-consumers.
        /// This method is thread-safe and can be called concurrently.
        /// <para>NOTE: Calling this method results in <seealso cref="OperationCanceledException"/> when:</para>
        /// <para>1. Either the given token is cancelled.</para>
        /// <para>2. Or any of the consumers ends-up throwing an exception... the whole pipeline would be
        /// destroyed</para>
        /// </summary>
        /// <param name="item">item to be consumed</param>
        void Add(T item);

        /// <summary>
        /// Retruns a running task that performs following operations:
        /// <para>1. Prohibit pipeline to accept any new item</para>
        /// <para>2. Signals consumers that pipeline is closed for new items</para>
        /// <para>3. Let consumers to finish consuming remaining queued items</para>
        /// <para>4. Disposes all consumers</para>
        /// <para>NOTE:This method is NOT thread-safe</para>
        /// </summary>
        Task TearDown();
    }
}