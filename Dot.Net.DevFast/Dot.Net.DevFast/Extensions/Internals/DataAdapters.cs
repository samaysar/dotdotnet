using System.Collections.Generic;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal class ListAdapter<T> : IDataAdapter<T, List<T>>
    {
        private readonly int _maxListSize;

        internal ListAdapter(int maxListSize)
        {
            _maxListSize = maxListSize.ThrowIfLess(2, $"List size cannot be less than 2. (Value: {maxListSize})");
        }

        public bool TryGet(IProducerFeed<T> producerDataFeed, out List<T> consumable)
        {
            consumable = new List<T>(_maxListSize);
            var collectionNonEmpty = true;
            while (collectionNonEmpty &&
                   (consumable.Count < _maxListSize))
            {
                if (producerDataFeed.TryGet(out T value))
                {
                    consumable.Add(value);
                }
                else
                {
                    collectionNonEmpty = false;
                }
            }
            return consumable.Count > 0;
        }
    }

    internal class IdentityAdapter<T> : IDataAdapter<T, T>
    {
        public bool TryGet(IProducerFeed<T> producerDataFeed, out T consumable)
        {
            return producerDataFeed.TryGet(out consumable);
        }
    }
}