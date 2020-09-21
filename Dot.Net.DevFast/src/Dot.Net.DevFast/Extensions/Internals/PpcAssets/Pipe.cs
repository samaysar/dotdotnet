using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal static class Pipe<TP, TC>
    {
        public static Task Execute(CancellationToken token, int bufferSize, IDataAdapter<TP, TC> adapter,
            IReadOnlyList<IProducer<TP>> producers, IReadOnlyList<IConsumer<TC>> consumers)
        {
            return Task.Run(async () =>
            {
                using var localCts = new CancellationTokenSource();
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(token, localCts.Token);
#if NETASYNCDISPOSE
                var ppcBuffer = new PpcBuffer<TP>(bufferSize, combinedCts.Token);
                await using (ppcBuffer.ConfigureAwait(false))
#else
                using (var ppcBuffer = new PpcBuffer<TP>(bufferSize, combinedCts.Token))
#endif
                {
                    token.ThrowIfCancellationRequested();
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
            }, CancellationToken.None);
        }
        
        internal static Task RunConsumers(IReadOnlyList<IConsumer<TC>> consumers,
            IConsumerBuffer<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            return new Func<int, CancellationToken, Task>(async (i, t) =>
                    await RunConsumer(consumers[i], feed, adapter, t, tokenSrc).ConfigureAwait(false))
                .WhenAll(consumers.Count, token);
        }

        private static async Task RunConsumer(IConsumer<TC> parallelConsumer,
            IConsumerBuffer<TP> feed, IDataAdapter<TP, TC> adapter,
            CancellationToken token, CancellationTokenSource tokenSrc)
        {
            try
            {
#if !NETASYNCDISPOSE
                using (parallelConsumer)
#else
                await using (parallelConsumer.ConfigureAwait(false))
#endif
                {
                    await parallelConsumer.InitAsync().StartIfNeeded().ConfigureAwait(false);
                    token.ThrowIfCancellationRequested();
                    while (adapter.TryGet(feed, token, out var consumable))
                    {
                        await parallelConsumer.ConsumeAsync(consumable, token).StartIfNeeded().ConfigureAwait(false);
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
            IProducerBuffer<TP> feed, CancellationToken token,
            CancellationTokenSource tokenSrc)
        {
            try
            {
#if !NETASYNCDISPOSE
                using (parallelProducer)
#else
                await using (parallelProducer.ConfigureAwait(false))
#endif
                {
                    await parallelProducer.InitAsync().StartIfNeeded().ConfigureAwait(false);
                    token.ThrowIfCancellationRequested();
                    await parallelProducer.ProduceAsync(feed, token).StartIfNeeded().ConfigureAwait(false);
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