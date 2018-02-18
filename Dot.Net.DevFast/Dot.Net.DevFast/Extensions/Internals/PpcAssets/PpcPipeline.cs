using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal static class PpcPipeline<TP, TC>
    {
        public static Task Execute(CancellationToken token, int bufferSize, IDataAdapter<TP, TC> adapter,
            IReadOnlyList<IProducer<TP>> producers, IReadOnlyList<IConsumer<TC>> consumers)
        {
            return Task.Run(async () =>
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
                                var rc = RunConsumers(consumers, ppcBuffer, adapter, combinedCts.Token, localCts);
                                var rp = RunProducers(producers, ppcBuffer, combinedCts.Token, localCts);
                                await Task.WhenAll(rc, rp).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                if (token.IsCancellationRequested)
                                    throw new OperationCanceledException("PpcCancelled", e, token);
                                throw;
                            }
                        }
                    }
                }
            }, token);
        }
        
        private static Task RunConsumers(IReadOnlyList<IConsumer<TC>> consumers,
            IProducerFeed<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            return new Func<int, CancellationToken, Task>(async (i, t) =>
                    await RunConsumer(consumers[i], feed, adapter, t, tokenSrc).ConfigureAwait(false))
                .WhenAll(consumers.Count, token);
        }

        private static async Task RunConsumer(IConsumer<TC> parallelConsumer,
            IProducerFeed<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            try
            {
                using (parallelConsumer)
                {
                    await parallelConsumer.InitAsync().ConfigureAwait(false);
                    while (adapter.TryGet(feed, out var consumable))
                    {
                        await parallelConsumer.ConsumeAsync(consumable, token).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                if (!token.IsCancellationRequested) tokenSrc.Cancel();
                throw;
            }
        }

        private static Task RunProducers(IReadOnlyList<IProducer<TP>> producers,
            PpcBuffer<TP> buffer, CancellationToken token,
            CancellationTokenSource tokenSrc)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await new Func<int, CancellationToken, Task>(async (i, t) =>
                            await RunProducer(producers[i], buffer, t, tokenSrc).ConfigureAwait(false))
                        .WhenAll(producers.Count, token).ConfigureAwait(false);
                }
                finally
                {
                    buffer.Close();
                }
            }, CancellationToken.None);
        }

        private static async Task RunProducer(IProducer<TP> parallelProducer,
            IConsumerFeed<TP> feed, CancellationToken token,
            CancellationTokenSource tokenSrc)
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
        }
    }
}