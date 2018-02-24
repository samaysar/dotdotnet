﻿using System.Threading;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    /// <summary>
    /// This class is just a wrapper to recover the consumable item from
    /// the <seealso cref="IProducerFeed{T}"/> with <seealso cref="Timeout.Infinite"/> timeout.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class IdentityAdapter<T> : IDataAdapter<T, T>
    {
        /// <summary>
        /// This method is just a wrapper to recover the consumable item from
        /// the <seealso cref="IProducerFeed{T}"/> with <seealso cref="Timeout.Infinite"/> timeout.
        /// <para>Returns true when a consumable instance can be created from
        /// <paramref name="producerDataFeed"/> else returns false.</para>
        /// </summary>
        /// <param name="producerDataFeed">Data feed</param>
        /// <param name="token">token to observe</param>
        /// <param name="consumable">consumable data instance</param>
        public bool TryGet(IProducerFeed<T> producerDataFeed, CancellationToken token, out T consumable)
        {
            return producerDataFeed.TryGet(Timeout.Infinite, token, out consumable);
        }
    }
}