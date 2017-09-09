using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Content distributor interface for Parallel Producer-Consumer.
    /// <para>Collects data from producer(s) and transfers it to consumer(s)</para>
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IDistributor<in T>
    {
        /// <summary>
        /// Collects the item from the producer to distribute it to the consumer down the lane.
        /// <para>Thread-safe, can be called in parallel from multiple producers.</para>
        /// </summary>
        /// <param name="item">data instance</param>
        /// <exception cref="OperationCanceledException"></exception>
        void Distribute(T item);
    }

    internal class ListDistributor<TIn> : PpcPipeline<TIn, List<TIn>>
    {
        private readonly int _maxListSize;

        public ListDistributor(int maxListSize, CancellationToken externalToken, int bufferSize)
            : base(externalToken, bufferSize)
        {
            _maxListSize = maxListSize.ThrowIfLess(2, $"List size cannot be less than 2. (Value: {maxListSize})");
        }

        protected override bool TryGetData(BlockingCollection<TIn> collection, CancellationToken token,
            out List<TIn> consumable)
        {
            consumable = new List<TIn>(_maxListSize);
            var collectionNonEmpty = true;
            while (collectionNonEmpty &&
                   (consumable.Count < _maxListSize))
            {
                if (collection.TryTake(out TIn value, Timeout.Infinite, token))
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

    internal class IdentityDistributor<TIn> : PpcPipeline<TIn, TIn>
    {
        public IdentityDistributor(CancellationToken externalToken, int bufferSize) : base(externalToken, bufferSize)
        {
        }

        protected override bool TryGetData(BlockingCollection<TIn> collection, CancellationToken token,
            out TIn consumable)
        {
            return collection.TryTake(out consumable, Timeout.Infinite, token);
        }
    }

    internal abstract class PpcPipeline<TProduce, TConsume> : IDistributor<TProduce>, IDisposable
    {
        private readonly CancellationTokenSource _globalCts;
        private readonly CancellationTokenSource _localCts;
        private readonly CancellationToken _token;
        private readonly BlockingCollection<TProduce> _collection;

        protected PpcPipeline(CancellationToken externalToken, int bufferSize)
        {
            _collection = ConcurrentBuffer.CreateBuffer<TProduce>(bufferSize);
            _localCts = new CancellationTokenSource();
            _globalCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken, _localCts.Token);
            _token = _globalCts.Token;
        }

        public void Dispose()
        {
            _globalCts?.Dispose();
            _localCts?.Dispose();
            _collection?.Dispose();
        }

        public void Distribute(TProduce item)
        {
            _collection.Add(item, _token);
        }

        public Task RunPpcAsync(IProducer<TProduce>[] producers, params IConsumer<TConsume>[] consumers)
        {
            var runningConsumers = RunConsumers(consumers);
            var runningProducers = RunProducers(producers);
            return Task.WhenAll(runningProducers, runningConsumers);
        }

        private Task RunConsumers(IReadOnlyList<IConsumer<TConsume>> consumers)
        {
            return Task.Run(async () =>
            {
                var consumerTasks = new Task[consumers.Count];
                for (var i = 0; i < consumers.Count; i++)
                {
                    consumerTasks[i] = RunConsumer(consumers[i]);
                }
                await Task.WhenAll(consumerTasks).ConfigureAwait(false);
            }, CancellationToken.None);
        }

        private Task RunConsumer(IConsumer<TConsume> parallelConsumer)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (parallelConsumer)
                    {
                        await parallelConsumer.InitAsync().ConfigureAwait(false);
                        while (TryGetData(_collection, _token, out TConsume consumable))
                        {
                            await parallelConsumer.ConsumeAsync(consumable, _token).ConfigureAwait(false);
                        }
                    }
                }
                catch
                {
                    _localCts.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }

        private Task RunProducers(IReadOnlyList<IProducer<TProduce>> producers)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var producerTasks = new Task[producers.Count];
                    for (var i = 0; i < producers.Count; i++)
                    {
                        producerTasks[i] = RunProducer(producers[i]);
                    }
                    await Task.WhenAll(producerTasks).ConfigureAwait(false);
                }
                finally
                {
                    _collection.CompleteAdding();
                }
            }, CancellationToken.None);
        }

        private Task RunProducer(IProducer<TProduce> parallelProducer)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await parallelProducer.ProduceAsync(this, _token).ConfigureAwait(false);
                }
                catch
                {
                    _localCts.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }

        protected abstract bool TryGetData(BlockingCollection<TProduce> collection, CancellationToken token,
            out TConsume consumable);
    }
}