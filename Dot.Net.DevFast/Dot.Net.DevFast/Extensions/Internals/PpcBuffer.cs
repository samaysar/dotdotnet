using System;
using System.Collections.Concurrent;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal sealed class PpcBuffer<TProducer> : IDataFeed<TProducer>, IDistributor<TProducer>, IDisposable
    {
        private readonly CancellationToken _token;
        private BlockingCollection<TProducer> _collection;

        public PpcBuffer(int bufferSize, CancellationToken token)
        {
            _collection = ConcurrentBuffer.CreateBuffer<TProducer>(bufferSize);
            _token = token;
        }

        public bool TryGet(out TProducer data)
        {
            return _collection.TryTake(out data, Timeout.Infinite, _token);
        }

        public void Distribute(TProducer item)
        {
            _collection.Add(item, _token);
        }

        public void Dispose()
        {
            if (_collection != null)
            {
                using (_collection)
                {
                }
            }
            _collection = null;
        }
    }
}