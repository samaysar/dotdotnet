using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Consumer interface for parallel Producer consumer pattern.
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IConsumer<in T> : IDisposable
    {
        /// <summary>
        /// This method is called ONCE before any call is made to <see cref="ConsumeAsync"/>.
        /// <para>Similarly, <seealso cref="IDisposable.Dispose"/> will be called after
        /// all the calls to <see cref="ConsumeAsync"/> are done.</para>
        /// </summary>
        Task InitAsync();

        /// <summary>
        /// Multiple calls to this function everytime some data is available from 
        /// <seealso cref="IProducer{T}"/>.
        /// <para><seealso cref="IDisposable.Dispose"/> will be called when there is 
        /// NO more data available.</para>
        /// <para>Method must return as soon as EITHER data consumption is done
        /// OR some error has occurred.</para>
        /// <para>Explicit thread safety is NOT required, as this method will NOT be called
        /// in parallel.</para>
        /// <para>All exceptions must be thrown back.</para>
        /// </summary>
        /// <param name="item">instance to be consumed</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ConsumeAsync(T item, CancellationToken cancellationToken);
    }
}