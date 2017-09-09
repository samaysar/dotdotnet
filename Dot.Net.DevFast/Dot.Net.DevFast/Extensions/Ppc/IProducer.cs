using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Producer interface for parallel Producer consumer pattern.
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IProducer<out T>
    {
        /// <summary>
        /// Call to this method starts the data production.
        /// <para>This method call must be return when EITHER all the data 
        /// production is done OR any error has occurred.</para>
        /// <para>All exceptions must be thrown back</para>
        /// </summary>
        /// <param name="distributor">All produced data intances must be added to 
        /// <paramref name="distributor"/> instance, in order to pass on to associated consumers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task BeginProductionAsync(IDataDistributor<T> distributor, CancellationToken cancellationToken);
    }
}