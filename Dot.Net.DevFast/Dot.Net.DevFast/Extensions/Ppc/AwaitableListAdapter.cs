using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Adapter to pack individual items to list of items.
    /// <para>NOTE:</para>
    /// <para>1. It does NOT observes <seealso cref="IProducerFeed{T}.Finished"/> status</para>
    /// <para>2. The first element is ALWAYS (irrespective of provided timeout value) waited for 
    /// <seealso cref="Timeout.Infinite"/>. Then items are added to list as long as they can be recovered
    /// before provided millisecond timeout is reached.</para>
    /// <para>3. Further, items are recovered and populated as long as they are available without
    /// any wait. Then the list if finalized and returned. Step 2 and 3 are same when inifinite timeout 
    /// is suplied.</para>
    /// <para>4. If <see cref="TryGet"/> returns true, then the list would contains at least 1 item.</para>
    /// <para>5. List max size would be respected as provided in Ctor..</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AwaitableListAdapter<T> : IDataAdapter<T, List<T>>
    {
        private readonly int _millisecTimeout;
        private readonly int _maxListSize;

        /// <summary>
        /// Ctor.
        /// <para>NOTE:</para>
        /// <para>1. It does NOT observes <seealso cref="IProducerFeed{T}.Finished"/> status</para>
        /// <para>2. The first element is ALWAYS (irrespective of provided timeout value) waited for 
        /// <seealso cref="Timeout.Infinite"/>. Then items are added to list as long as they can be recovered
        /// before provided millisecond timeout is reached.</para>
        /// <para>3. Further, items are recovered and populated as long as they are available without
        /// any wait. Then the list if finalized and returned. Step 2 and 3 are same when inifinite timeout 
        /// is suplied.</para>
        /// <para>4. If <see cref="TryGet"/> returns true, then the list would contains at least 1 item.</para>
        /// <para>5. List max size would be respected as provided in Ctor..</para>
        /// </summary>
        /// <param name="maxListSize">Max item to be sent in single list instance</param>
        /// <param name="millisecTimeout">Milliseconds time to observe before finalizing the list</param>
        public AwaitableListAdapter(int maxListSize, int millisecTimeout)
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

        /// <summary>
        /// Return true when a consumable instance can be created from
        /// <paramref name="producerDataFeed"/> else returns false.
        /// <para>NOTE:</para>
        /// <para>1. It does NOT observes <seealso cref="IProducerFeed{T}.Finished"/> status</para>
        /// <para>2. The first element is ALWAYS (irrespective of provided timeout value) waited for 
        /// <seealso cref="Timeout.Infinite"/>. Then items are added to list as long as they can be recovered
        /// before provided millisecond timeout is reached.</para>
        /// <para>3. Further, items are recovered and populated as long as they are available without
        /// any wait. Then the list if finalized and returned. Step 2 and 3 are same when inifinite timeout 
        /// is suplied.</para>
        /// <para>4. If method returns true, then the list would contains at least 1 item.</para>
        /// <para>5. List max size would be respected as provided in Ctor.</para>
        /// </summary>
        /// <param name="producerDataFeed">Data feed</param>
        /// <param name="token">token to observe</param>
        /// <param name="consumable">consumable data instance</param>
        public bool TryGet(IProducerFeed<T> producerDataFeed, CancellationToken token, out List<T> consumable)
        {
            return _millisecTimeout == Timeout.Infinite
                ? TryGetWithInfiniteTo(producerDataFeed, token, out consumable)
                : TryGetWithFiniteTo(producerDataFeed, token, out consumable);
        }

        private bool TryGetWithFiniteTo(IProducerFeed<T> producerDataFeed, CancellationToken token,
            out List<T> consumable)
        {
            consumable = null;
            var sw = Stopwatch.StartNew();
            if (!producerDataFeed.TryGet(Timeout.Infinite, token, out var value)) return false;
            consumable = new List<T>(_maxListSize) {value};
            var timeRemains = (int) Math.Max(0, _millisecTimeout - sw.ElapsedMilliseconds);
            while (consumable.Count < _maxListSize)
            {
                if (producerDataFeed.TryGet(timeRemains, token, out value))
                {
                    consumable.Add(value);
                    if (timeRemains != 0)
                    {
                        timeRemains = (int) Math.Max(0, _millisecTimeout - sw.ElapsedMilliseconds);
                    }
                }
                else return true;
            }

            return true;
        }

        private bool TryGetWithInfiniteTo(IProducerFeed<T> producerDataFeed, CancellationToken token,
            out List<T> consumable)
        {
            consumable = null;
            if (!producerDataFeed.TryGet(Timeout.Infinite, token, out var value)) return false;
            consumable = new List<T>(_maxListSize) {value};
            while (consumable.Count < _maxListSize)
            {
                if (producerDataFeed.TryGet(Timeout.Infinite, token, out value))
                {
                    consumable.Add(value);
                }
                else return true;
            }

            return true;
        }
    }
}