using System.Collections.Concurrent;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Ppc;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class PpcBuffer<T> : IPpcFeed<T>
    {
        private readonly CancellationToken _token;
        private BlockingCollection<T> _collection;

        public PpcBuffer(int bufferSize, CancellationToken token)
        {
            _collection = ConcurrentBuffer.CreateBuffer<T>(bufferSize);
            _token = token;
        }

        public bool TryGet(int millisecTimeout, CancellationToken token, out T data)
        {
            return _collection.TryTake(out data, millisecTimeout, token);
        }

        public bool Finished => _collection.IsCompleted;

        public void Add(T item)
        {
            TryAdd(item, Timeout.Infinite);
        }

        public bool TryAdd(T item, int millisecTimeout)
        {
            return _collection.TryAdd(item, millisecTimeout, _token);
        }

        public void Close()
        {
            _collection.CompleteAdding();
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