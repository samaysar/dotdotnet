using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal class AwaitableListAdapter<T> : IDataAdapter<T, List<T>>
    {
        private readonly int _millisecTimeout;
        private readonly int _maxListSize;

        internal AwaitableListAdapter(int maxListSize, int millisecTimeout)
        {
            if (millisecTimeout != Timeout.Infinite)
            {
                _millisecTimeout = millisecTimeout
                    .ThrowIfNegative($"Timeout cannot be negative. (Value:{millisecTimeout})");
            }
            else
            {
                _millisecTimeout = Timeout.Infinite;
            }
            _maxListSize = maxListSize.ThrowIfLess(2, $"List size cannot be less than 2. (Value: {maxListSize})");
        }

        public bool TryGet(IProducerFeed<T> producerDataFeed, out List<T> consumable)
        {
            return _millisecTimeout == Timeout.Infinite
                ? TryGetWithInfiniteTo(producerDataFeed, out consumable)
                : TryGetWithFiniteTo(producerDataFeed, out consumable);
        }

        private bool TryGetWithFiniteTo(IProducerFeed<T> producerDataFeed, out List<T> consumable)
        {
            consumable = null;
            var sw = Stopwatch.StartNew();
            if (!producerDataFeed.TryGet(Timeout.Infinite, out var value)) return false;
            consumable = new List<T>(_maxListSize) { value };
            var timeRemains = (int)Math.Max(0, _millisecTimeout - sw.ElapsedMilliseconds);
            while (consumable.Count < _maxListSize &&
                   timeRemains >= 0)
            {
                if (producerDataFeed.TryGet(timeRemains, out value))
                {
                    consumable.Add(value);
                    if (timeRemains != 0)
                    {
                        timeRemains = (int) Math.Max(0, _millisecTimeout - sw.ElapsedMilliseconds);
                    }
                }
                else
                {
                    timeRemains = -1;
                }
            }
            return true;
        }

        private bool TryGetWithInfiniteTo(IProducerFeed<T> producerDataFeed, out List<T> consumable)
        {
            consumable = null;
            if (!producerDataFeed.TryGet(Timeout.Infinite, out var value)) return false;
            consumable = new List<T>(_maxListSize) {value};
            var hasItems = true;
            while (consumable.Count < _maxListSize && hasItems)
            {
                if (producerDataFeed.TryGet(Timeout.Infinite, out value))
                {
                    consumable.Add(value);
                }
                else
                {
                    hasItems = false;
                }
            }

            return true;
        }
    }
}