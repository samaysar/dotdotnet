using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class AsyncProducer<T> : IProducer<T>
    {
        private readonly Func<IProducerBuffer<T>, CancellationToken, Task> _producerFunc;

        public AsyncProducer(Func<IProducerBuffer<T>, CancellationToken, Task> producerFunc)
        {
            _producerFunc = producerFunc;
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        public async Task ProduceAsync(IProducerBuffer<T> feedToPopulate, CancellationToken cancellationToken)
        {
            await _producerFunc(feedToPopulate, cancellationToken).ConfigureAwait(false);
        }

#if OLDNETUSING
        public void Dispose()
        {
            
        }
#else
        public ValueTask DisposeAsync()
        {
            return default;
        }
#endif
    }

    internal sealed class AsyncConsumer<T> : IConsumer<T>
    {
        private readonly Func<T, CancellationToken, Task> _consumerFunc;

        public AsyncConsumer(Func<T, CancellationToken, Task> consumerFunc)
        {
            _consumerFunc = consumerFunc;
        }

#if OLDNETUSING
        public void Dispose()
        {
            
        }
#else
        public ValueTask DisposeAsync()
        {
            return default;
        }
#endif

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        public async Task ConsumeAsync(T item, CancellationToken cancellationToken)
        {
            await _consumerFunc(item, cancellationToken).ConfigureAwait(false);
        }
    }
}