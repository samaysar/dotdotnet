using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class Pipeline<TP,TC> : IPipeline<TP>
    {
        private readonly CancellationTokenSource _mergedCts;
        private readonly PpcBuffer<TP> _feed;
        private readonly Task _consumerTask;
        private CancellationTokenSource _localCts;

        public Pipeline(IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter, CancellationToken token, int bufferSize)
        {
            _localCts = new CancellationTokenSource();
            _mergedCts = CancellationTokenSource.CreateLinkedTokenSource(token, _localCts.Token);

            //we pass "_mergedCts.Token" so that it starts throwing error when either consumer is in error
            //or tear-down is called or the Ctor token is cancelled.
            _feed = new PpcBuffer<TP>(bufferSize, _mergedCts.Token);
            //we give original token to consumers to listen to... so we can cancel "_localCts" in dispose
            //and still let consumer run to finish remaining objects.
            //However, if something goes wrong with consumers, we need to cancel "_localCts"
            //so that Add/TryAdd method also start throwing error
            _consumerTask = Pipe<TP, TC>.RunConsumers(consumers, _feed, adapter, token, _localCts);
        }

#if !NETFRAMEWORK
        public async ValueTask DisposeAsync()
        {
            await TearDown().ConfigureAwait(false);
        }
#endif
        public void Dispose()
        {
            TearDown().GetAwaiter().GetResult();
        }

        public void Add(TP item, CancellationToken token)
        {
            TryAdd(item, Timeout.Infinite, token);
        }

        public bool TryAdd(TP item, int millisecTimeout, CancellationToken token)
        {
            if (_localCts == null)
            {
                throw new ObjectDisposedException(nameof(Pipeline<TP, TC>), "instance is disposed");
            }
            return _feed.TryAdd(item, millisecTimeout, token);
        }

        public async Task TearDown()
        {
            if (_localCts == null) return;
            try
            {
                using (_localCts)
                {
                    using (_mergedCts)
                    {
#if !NETFRAMEWORK
                        await using (_feed.ConfigureAwait(false))
#else
                        using (_feed)
#endif
                        {
                            _localCts.Cancel();
                            _feed.Close();
                            await _consumerTask.ConfigureAwait(false);
                        }
                    }
                }
            }
            finally
            {
                _localCts = null;
            }
        }

        public int UnconsumedCount => _feed.Unprocessed;
    }
}