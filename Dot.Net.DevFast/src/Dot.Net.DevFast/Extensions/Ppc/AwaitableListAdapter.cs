using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Adapter to pack individual items to list of items.
    /// <para>NOTE:</para>
    /// <para>1. It does NOT observes <seealso cref="IConsumerBuffer{T}.Finished"/> status</para>
    /// <para>2. The first element is ALWAYS (irrespective of provided timeout value) waited for 
    /// <seealso cref="Timeout.Infinite"/>. Then items are added to list as long as they can be recovered
    /// before provided millisecond timeout is reached.</para>
    /// <para>3. Further, items are recovered and populated as long as they are available without
    /// any wait. Then the list if finalized and returned. Step 2 and 3 are same when inifinite timeout 
    /// is suplied.</para>
    /// <para>4. If <see cref="TryGet"/> returns true, then the list would contains at least 1 item.</para>
    /// <para>5. List max size would be respected as provided in Ctor.</para>
    /// <para>6. A call to <see cref="Adapt"/> will be made to perform the instance transformation</para>
    /// </summary>
    /// <typeparam name="TP"></typeparam>
    /// <typeparam name="TC"></typeparam>
    public abstract class AwaitableListAdapter<TP, TC> : IDataAdapter<TP, List<TC>>
    {
        private readonly int _millisecTimeout;
        private readonly int _maxListSize;

        /// <summary>
        /// Ctor.
        /// <para>NOTE:</para>
        /// <para>1. It does NOT observes <seealso cref="IConsumerBuffer{T}.Finished"/> status</para>
        /// <para>2. The first element is ALWAYS (irrespective of provided timeout value) waited for 
        /// <seealso cref="Timeout.Infinite"/>. Then items are added to list as long as they can be recovered
        /// before provided millisecond timeout is reached.</para>
        /// <para>3. Further, items are recovered and populated as long as they are available without
        /// any wait. Then the list if finalized and returned. Step 2 and 3 are same when inifinite timeout 
        /// is suplied.</para>
        /// <para>4. If <see cref="TryGet"/> returns true, then the list would contains at least 1 item.</para>
        /// <para>5. List max size would be respected as provided in Ctor..</para>
        /// <para>6. A call to <see cref="Adapt"/> will be made to perform the instance transformation.</para>
        /// </summary>
        /// <param name="maxListSize">Max item to be sent in single list instance</param>
        /// <param name="millisecTimeout">Milliseconds time to observe before finalizing the list</param>
        protected AwaitableListAdapter(int maxListSize, int millisecTimeout)
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
        /// <para>1. It does NOT observes <seealso cref="IConsumerBuffer{T}.Finished"/> status</para>
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
        public bool TryGet(IConsumerBuffer<TP> producerDataFeed, CancellationToken token, out List<TC> consumable)
        {
            consumable = default;
            if (!producerDataFeed.TryGet(Timeout.Infinite, token, out var value)) return false;
            consumable = new List<TC>(_maxListSize) {Adapt(value, token)};
            return _millisecTimeout == Timeout.Infinite
                ? TryGetWithInfiniteTo(producerDataFeed, token, consumable)
                : TryGetWithFiniteTo(producerDataFeed, token, consumable);
        }

        private bool TryGetWithFiniteTo(IConsumerBuffer<TP> producerDataFeed, CancellationToken token,
            ICollection<TC> consumable)
        {
            var timeRemains = _millisecTimeout;
            var sw = Stopwatch.StartNew();
            while (consumable.Count < _maxListSize)
            {
                if (producerDataFeed.TryGet(timeRemains, token, out var value))
                {
                    consumable.Add(Adapt(value, token));
                    if (timeRemains != 0)
                    {
                        timeRemains = (int) Math.Max(0, _millisecTimeout - sw.ElapsedMilliseconds);
                    }
                }
                else return true;
            }

            return true;
        }

        private bool TryGetWithInfiniteTo(IConsumerBuffer<TP> producerDataFeed, CancellationToken token,
            ICollection<TC> consumable)
        {
            while (consumable.Count < _maxListSize)
            {
                if (producerDataFeed.TryGet(Timeout.Infinite, token, out var value))
                {
                    consumable.Add(Adapt(value, token));
                }
                else return true;
            }

            return true;
        }

        /// <summary>
        /// Will be called to perform the required transformation on all the produced instances.
        /// </summary>
        /// <param name="produced">produced item</param>
        /// <param name="token">cancellation token to observe</param>
        public abstract TC Adapt(TP produced, CancellationToken token);
    }

    internal sealed class IdentityAwaitableListAdapter<T> : AwaitableListAdapter<T, T>
    {
        internal IdentityAwaitableListAdapter(int maxListSize, int millisecTimeout) : base(maxListSize, millisecTimeout)
        {
        }

        public override T Adapt(T produced, CancellationToken token)
        {
            return produced;
        }
    }
}