using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Consumer interface for parallel Producer consumer pattern.
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
#if !NETASYNCDISPOSE
    public interface IConsumer<in T> : IDisposable
#else
    public interface IConsumer<in T> : IAsyncDisposable
#endif
    {
        /// <summary>
        /// This method is called ONCE before any call is made to <see cref="ConsumeAsync"/>.
        /// <para>Similarly, <seealso cref="IDisposable.Dispose"/> will be called after
        /// all the calls to <see cref="ConsumeAsync"/> are done.</para>
        /// <para>If this method results in an exception, running producers will be signaled to quit producing data
        /// as soon as possible and the whole pipeline will be destroyed.</para>
        /// </summary>
        Task InitAsync();

#if NETASYNCDISPOSE
        /// <summary>
        /// Expect multiple calls to this function as calls are made whenever some data is 
        /// available from <seealso cref="IProducer{T}"/>.
        /// <para><seealso cref="IAsyncDisposable.DisposeAsync"/> will be called when there is 
        /// NO more data available.</para>
        /// <para>Method must return as soon as EITHER data consumption is done
        /// OR some error has occurred.</para>
        /// <para>Explicit thread safety is NOT required as long as consumer instances are wholly distinct.</para>
        /// <para><seealso cref="IAsyncDisposable.DisposeAsync"/> will be called when there is no more
        /// data available.</para>
        /// <para>If this method results in an exception, running producers will be signaled to quit producing data
        /// as soon as possible and the whole pipeline will be destroyed.</para>
        /// </summary>
        /// <param name="item">instance to be consumed</param>
        /// <param name="cancellationToken">Cancellation token</param>
#else
        /// <summary>
        /// Expect multiple calls to this function as calls are made whenever some data is 
        /// available from <seealso cref="IProducer{T}"/>.
        /// <para><seealso cref="IDisposable.Dispose"/> will be called when there is 
        /// NO more data available.</para>
        /// <para>Method must return as soon as EITHER data consumption is done
        /// OR some error has occurred.</para>
        /// <para>Explicit thread safety is NOT required as long as consumer instances are wholly distinct.</para>
        /// <para><seealso cref="IDisposable.Dispose"/> will be called when there is no more
        /// data available.</para>
        /// <para>If this method results in an exception, running producers will be signaled to quit producing data
        /// as soon as possible and the whole pipeline will be destroyed.</para>
        /// </summary>
        /// <param name="item">instance to be consumed</param>
        /// <param name="cancellationToken">Cancellation token</param>
#endif
        Task ConsumeAsync(T item, CancellationToken cancellationToken);
    }
}