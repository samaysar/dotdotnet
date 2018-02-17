using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal static class PpcPipeline<TP, TC>
    {
        public static async Task RunPpcAsync(CancellationToken token, int bufferSize, IDataAdapter<TP, TC> adapter,
            IReadOnlyList<IProducer<TP>> producers, IReadOnlyList<IConsumer<TC>> consumers)
        {
            using (var localCts = new CancellationTokenSource())
            {
                using (var combinedCts = CancellationTokenSource
                    .CreateLinkedTokenSource(token, localCts.Token))
                {
                    using (var ppcBuffer = new PpcBuffer<TP>(bufferSize, combinedCts.Token))
                    {
                        try
                        {
                            var runningConsumers =
                                RunConsumers(consumers, ppcBuffer, adapter, combinedCts.Token, localCts);
                            var runningProducers = RunProducers(producers, ppcBuffer, combinedCts.Token, localCts);
                            await Task.WhenAll(runningProducers, runningConsumers).ConfigureAwait(false);
                        }
                        catch(Exception e)
                        {
                            if (token.IsCancellationRequested)
                                throw new OperationCanceledException("PpcCancelled", e, token);
                            throw;
                        }
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
                        while (adapter.TryGet(feed, out var consumable))
                        {
                            await parallelConsumer.ConsumeAsync(consumable, token)
                                .ConfigureAwait(false);
                        }
                    }
                }
                catch
                {
                    if (!token.IsCancellationRequested) tokenSrc.Cancel();
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
                    using (parallelProducer)
                    {
                        await parallelProducer.InitAsync().ConfigureAwait(false);
                        await parallelProducer.ProduceAsync(feed, token).ConfigureAwait(false);
                    }
                }
                catch
                {
                    if (!token.IsCancellationRequested) tokenSrc.Cancel();
                    throw;
                }
            }, CancellationToken.None);
        }
    }
}