using System;
using System.Collections.Generic;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Extensions for PPC patterns.
    /// </summary>
    public static partial class PpcExts
    {
        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}"/> as an end-point to accept data while
        /// executing given <paramref name="consumers"/> concurrently. Pipeline is responcible for data transfer with the help 
        /// of a buffer (with size= <paramref name="bufferSize"/>) and <paramref name="adapter"/>.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item><description>Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></description></item>
        /// <item><description><seealso cref="IConcurrentPipeline{TP}.TearDown"/> (as documented) should be called only after it is certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Accept"/> will be made, to avoid unexpected errors.</description></item>
        /// <item><description>Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)</description></item>
        /// <item><description><seealso cref="IConcurrentPipeline{T}.Accept"/> method:
        ///     <list type="bullet">
        ///     <item><description>is Thread-safe and can be called concurrently.</description></item>
        ///     <item><description>throws <seealso cref="OperationCanceledException"/> when either 
        /// <paramref name="token"/> is cancelled or any of the consumers ends-up throwing an exception... 
        /// and the pipeline is destroyed (<seealso cref="IConcurrentPipeline{T}.TearDown"/> or <seealso cref="IDisposable.Dispose"/> 
        /// might not have been called at this moment)</description></item>
        ///     </list></description></item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}"/> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}"/> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">adapter instance to create instances of type <typeparamref name="TC"/> from the instances 
        /// of type <typeparamref name="TP"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(this IReadOnlyList<IConsumer<TC>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new ConcurrentPipeline<TP, TC>(consumers, adapter, token, bufferSize);
        }
    }
}