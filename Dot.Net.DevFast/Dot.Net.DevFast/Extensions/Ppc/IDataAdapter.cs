using System.Threading;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Adapter interface between produced data and consumable data.
    /// </summary>
    /// <typeparam name="TProducer">Produced data type</typeparam>
    /// <typeparam name="TConsumer">Consumable data type</typeparam>
    public interface IDataAdapter<TProducer, TConsumer>
    {
        /// <summary>
        /// Must return true when a consumable instance can be created from
        /// <paramref name="producerDataFeed"/>. Must return false when <paramref name="consumable"/>
        /// cannot be created; in this case associated consumer will be distroyed.
        /// <para>This method must be thread safe.</para>
        /// <para>Must throw all exceptions.</para>
        /// </summary>
        /// <param name="producerDataFeed">Data feed</param>
        /// <param name="token">token to observe while adapting data</param>
        /// <param name="consumable">consumable data instance</param>
        bool TryGet(IProducerFeed<TProducer> producerDataFeed, CancellationToken token, out TConsumer consumable);
    }
}