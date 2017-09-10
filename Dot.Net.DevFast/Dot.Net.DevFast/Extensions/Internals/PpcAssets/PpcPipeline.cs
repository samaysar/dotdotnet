using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class PpcPipeline<TP, TC>
    {
        private readonly CancellationToken _externalToken;
        private readonly int _bufferSize;
        private readonly IDataAdapter<TP, TC> _adapter;

        internal PpcPipeline(CancellationToken externalToken, int bufferSize, IDataAdapter<TP, TC> adapter)
        {
            _externalToken = externalToken;
            _bufferSize = bufferSize;
            _adapter = adapter;
        }

        public async Task RunPpcAsync(IProducer<TP>[] producers, params IConsumer<TC>[] consumers)
        {
            using (var localCts = new CancellationTokenSource())
            {
                using (var combinedCts = CancellationTokenSource
                    .CreateLinkedTokenSource(_externalToken, localCts.Token))
                {
                    using (var ppcBuffer = new PpcBuffer<TP>(_bufferSize, combinedCts.Token))
                    {
                        var runningConsumers = RunConsumers(consumers, ppcBuffer, _adapter, combinedCts.Token, localCts);
                        var runningProducers = RunProducers(producers, ppcBuffer, combinedCts.Token, localCts);
                        await Task.WhenAll(runningProducers, runningConsumers).ConfigureAwait(false);
                    }
                }
            }
        }

        private static Task RunConsumers(IReadOnlyList<IConsumer<TC>> consumers,
            IProducerFeed<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            return Task.Run(async () =>
            {
                var consumerTasks = new Task[consumers.Count];
                for (var i = 0; i < consumers.Count; i++)
                {
                    consumerTasks[i] = RunConsumer(consumers[i], feed, adapter, token, tokenSrc);
                }
                await Task.WhenAll(consumerTasks).ConfigureAwait(false);
            }, CancellationToken.None);
        }

        private static Task RunConsumer(IConsumer<TC> parallelConsumer,
            IProducerFeed<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (parallelConsumer)
                    {
                        await parallelConsumer.InitAsync().ConfigureAwait(false);
                        while (adapter.TryGet(feed, out TC consumable))
                        {
                            await parallelConsumer.ConsumeAsync(consumable, token)
                                .ConfigureAwait(false);
                        }
                    }
                }
                catch
                {
                    tokenSrc.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }

        private static Task RunProducers(IReadOnlyList<IProducer<TP>> producers,
            PpcBuffer<TP> buffer, CancellationToken token,
            CancellationTokenSource tokenSrc)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var producerTasks = new Task[producers.Count];
                    for (var i = 0; i < producers.Count; i++)
                    {
                        producerTasks[i] = RunProducer(producers[i], buffer, token, tokenSrc);
                    }
                    await Task.WhenAll(producerTasks).ConfigureAwait(false);
                }
                finally
                {
                    buffer.Close();
                }
            }, CancellationToken.None);
        }

        private static Task RunProducer(IProducer<TP> parallelProducer,
            IConsumerFeed<TP> feed, CancellationToken token,
            CancellationTokenSource tokenSrc)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await parallelProducer.ProduceAsync(feed, token).ConfigureAwait(false);
                }
                catch
                {
                    tokenSrc.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }
    }
}