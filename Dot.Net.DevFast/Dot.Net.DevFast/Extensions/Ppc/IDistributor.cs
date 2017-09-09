using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Content distributor interface for Parallel Producer-Consumer.
    /// <para>Collects data from producer(s) and transfers it to consumer(s)</para>
    /// </summary>
    /// <typeparam name="T">Content type</typeparam>
    public interface IDataDistributor<in T>
    {
        /// <summary>
        /// Collects the item from the producer to distribute it to the consumer down the lane.
        /// <para>Thread-safe, can be called in parallel from multiple producers.</para>
        /// </summary>
        /// <param name="item">data instance</param>
        /// <exception cref="OperationCanceledException"></exception>
        void Distribute(T item);
    }

    internal sealed class ItemCollectionDistributor<TIn> : DataDistributor<TIn, List<TIn>>
    {
        private readonly int _maxListSize;

        public ItemCollectionDistributor(int maxListSize, CancellationToken externalToken, int bufferSize)
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

    internal sealed class ItemDistributor<TIn> : DataDistributor<TIn, TIn>
    {
        public ItemDistributor(CancellationToken externalToken, int bufferSize) : base(externalToken, bufferSize)
        {
        }

        protected override bool TryGetData(BlockingCollection<TIn> collection, CancellationToken token,
            out TIn consumable)
        {
            return collection.TryTake(out consumable, Timeout.Infinite, token);
        }
    }

    internal abstract class DataDistributor<TIn, TOut> : IDataDistributor<TIn>, IDisposable
    {
        private readonly CancellationTokenSource _globalCts;
        private readonly CancellationTokenSource _localCts;
        private readonly CancellationToken _token;
        private readonly BlockingCollection<TIn> _collection;

        protected DataDistributor(CancellationToken externalToken, int bufferSize)
        {
            _collection = ParallelBuffer.CreateBuffer<TIn>(bufferSize);
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

        public void Distribute(TIn item)
        {
            _collection.Add(item, _token);
        }

        public async Task ConnectAsync(IProducer<TIn>[] producers, params IConsumer<TOut>[] consumers)
        {
            var runningConsumers = RunConsumers(consumers);
            var runningProducers = RunProducers(producers);
            await Task.WhenAll(runningProducers, runningConsumers).ConfigureAwait(false);
        }

        private Task RunConsumers(IReadOnlyList<IConsumer<TOut>> consumers)
        {
            var consumerTasks = new Task[consumers.Count];
            for (var i = 0; i < consumers.Count; i++)
            {
                consumerTasks[i] = RunConsumer(consumers[i]);
            }
            return Task.WhenAll(consumerTasks);
        }

        private Task RunConsumer(IConsumer<TOut> parallelConsumer)
        {
            return Task.Run(async () =>
            {
                try
                {
                    while (TryGetData(_collection, _token, out TOut consumable))
                    {
                        await parallelConsumer.BeginConsumptionAsync(consumable, _token).ConfigureAwait(false);
                    }
                }
                catch
                {
                    _localCts.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }

        private Task RunProducers(IReadOnlyList<IProducer<TIn>> producers)
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

        private Task RunProducer(IProducer<TIn> parallelProducer)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await parallelProducer.BeginProductionAsync(this, _token).ConfigureAwait(false);
                }
                catch
                {
                    _localCts.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }

        protected abstract bool TryGetData(BlockingCollection<TIn> collection, CancellationToken token,
            out TOut consumable);
    }
}