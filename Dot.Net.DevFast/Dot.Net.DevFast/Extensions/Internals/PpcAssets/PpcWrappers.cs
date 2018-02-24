using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class AsyncProducer<T> : IProducer<T>
    {
        private readonly Func<IConsumerFeed<T>, CancellationToken, Task> _producerFunc;

        public AsyncProducer(Func<IConsumerFeed<T>, CancellationToken, Task> producerFunc)
        {
            _producerFunc = producerFunc;
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        public async Task ProduceAsync(IConsumerFeed<T> feedToPopulate, CancellationToken cancellationToken)
        {
            await _producerFunc(feedToPopulate, cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            
        }
    }

    internal sealed class AsyncConsumer<T> : IConsumer<T>
    {
        private readonly Func<T, CancellationToken, Task> _consumerFunc;

        public AsyncConsumer(Func<T, CancellationToken, Task> consumerFunc)
        {
            _consumerFunc = consumerFunc;
        }

        public void Dispose()
        {
            
        }

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