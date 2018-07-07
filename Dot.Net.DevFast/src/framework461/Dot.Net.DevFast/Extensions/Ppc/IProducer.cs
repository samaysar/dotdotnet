using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Producer interface for parallel Producer consumer pattern.
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IProducer<out T> : IDisposable
    {
        /// <summary>
        /// This method is called ONCE before any call is made to <see cref="ProduceAsync"/>.
        /// <para>Similarly, <seealso cref="IDisposable.Dispose"/> will be called after
        /// the call to <see cref="ProduceAsync"/> are done.</para>
        /// <para>If this method results in an exception, running consumers will be signaled to quit consuming
        /// as soon as possible and the whole pipeline will be destroyed.</para>
        /// </summary>
        Task InitAsync();

        /// <summary>
        /// Call to this method MUST start the data production.
        /// <para>NOTE: This method is called just ONCE after calling <see cref="InitAsync"/> 
        /// this method call must return ONLY WHEN EITHER all the data 
        /// production is done OR any error has occurred.</para>
        /// <para>Upon returning from this function, call to <seealso cref="IDisposable.Dispose"/>
        /// will be made.</para>
        /// <para>If this method results in an exception, running consumers will be signaled to quit consuming
        /// as soon as possible and the whole pipeline will be destroyed.</para>
        /// </summary>
        /// <param name="feedToPopulate">All produced data intances must be added to 
        /// <paramref name="feedToPopulate"/> instance, in order to pass on to associated consumers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ProduceAsync(IProducerBuffer<T> feedToPopulate, CancellationToken cancellationToken);
    }
}