using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class ConcurrentPipeline<TP,TC> : IConcurrentPipeline<TP>
    {
        private readonly CancellationTokenSource _mergedCts;
        private readonly PpcBuffer<TP> _feed;
        private readonly Task _consumerTask;
        private CancellationTokenSource _localCts;

        public ConcurrentPipeline(IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter, CancellationToken token, int bufferSize)
        {
            _localCts = new CancellationTokenSource();
            _mergedCts = CancellationTokenSource.CreateLinkedTokenSource(token, _localCts.Token);

            //we pass "_mergedCts.Token" so that it starts throwing error when either consumer is in error
            //or tear-down is called or the Ctor token is cancelled.
            _feed = new PpcBuffer<TP>(bufferSize, _mergedCts.Token);
            //we give original token to consumers to listen to... so we can cancel "_token" in dispose
            //and still let consumer run to finish remaining objects.
            //However, if something goes wrong with consumers, we need to cancel "_localCts"
            //so that Accept() method also start throwing error
            _consumerTask = PpcPipeline<TP, TC>.RunConsumers(consumers, _feed, adapter, token, _localCts);
        }

        public void Dispose()
        {
            TearDown().Wait(CancellationToken.None);
        }

        public void Accept(TP item)
        {
            if (_localCts == null)
            {
                throw new ObjectDisposedException(nameof(ConcurrentPipeline<TP, TC>), "instance is disposed");
            }
            _feed.Add(item);
        }

        public Task TearDown()
        {
            return Task.Run(async () =>
            {
                if (_localCts == null) return;
                try
                {
                    using (_localCts)
                    {
                        using (_mergedCts)
                        {
                            using (_feed)
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
            });
        }
    }
}