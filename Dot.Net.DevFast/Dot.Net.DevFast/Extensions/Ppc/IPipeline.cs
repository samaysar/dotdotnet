using System;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Interface to expose concurrent producer-consumer pipeline operations.
    /// </summary>
    public interface IPipeline<in T> : IProducerBuffer<T>, IDisposable
    {
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