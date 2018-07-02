using System.Threading;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// This class is just a wrapper to recover the consumable item from
    /// the <seealso cref="IProducerFeed{TP}"/> in standard way. Actual
    /// tranformation can be implemented inside <see cref="Adapt"/>.
    /// </summary>
    /// <typeparam name="TP">Produced item type</typeparam>
    /// <typeparam name="TC">Consumable item type</typeparam>
    public abstract class AwaitableAdapter<TP, TC> : IDataAdapter<TP, TC>
    {
        /// <summary>
        /// Standardized implementation to recover single item from buffer.
        /// Calls <see cref="Adapt"/> to perform the desired transformed.
        /// <para>Returns true when a consumable instance can be created from
        /// <paramref name="producerDataFeed"/> else returns false.</para>
        /// </summary>
        /// <param name="producerDataFeed">Feed that is getting populated by producers.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="consumable">transfomed item that can be passed to consumer</param>
        public bool TryGet(IProducerFeed<TP> producerDataFeed, CancellationToken token, out TC consumable)
        {
            if (producerDataFeed.TryGet(Timeout.Infinite, token, out var produced))
            {
                consumable = Adapt(produced, token);
                return true;
            }

            consumable = default(TC);
            return false;
        }

        /// <summary>
        /// Will be called to perform the required transformation on all the produced instances.
        /// </summary>
        /// <param name="produced">produced item</param>
        /// <param name="token">cancellation token to observe</param>
        public abstract TC Adapt(TP produced, CancellationToken token);
    }

    internal sealed class IdentityAwaitableAdapter<T> : AwaitableAdapter<T, T>
    {
        internal static readonly IdentityAwaitableAdapter<T> Default = new IdentityAwaitableAdapter<T>();

        private IdentityAwaitableAdapter()
        {
            //singleton as methods are pure
        }

        public override T Adapt(T produced, CancellationToken token)
        {
            return produced;
        }
    }
}