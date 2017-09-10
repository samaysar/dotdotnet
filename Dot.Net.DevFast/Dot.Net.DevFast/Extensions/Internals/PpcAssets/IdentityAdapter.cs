using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal class IdentityAdapter<T> : IDataAdapter<T, T>
    {
        public bool TryGet(IProducerFeed<T> producerDataFeed, out T consumable)
        {
            return producerDataFeed.TryGet(out consumable);
        }
    }
}