using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Consumer interface for parallel Producer consumer pattern.
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IConsumer<in T>
    {
        /// <summary>
        /// Method to call once some data is available from <seealso cref="IProducer{T}"/>.
        /// <para>Method must return as soon as EITHER data consumption is done
        /// OR some error has occurred.</para>
        /// <para>Explicit thread safety is NOT required, as this method will NOT be called
        /// in parallel.</para>
        /// <para>All exceptions must be thrown back.</para>
        /// </summary>
        /// <param name="item">instance to be consumed</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task BeginConsumptionAsync(T item, CancellationToken cancellationToken);
    }
}