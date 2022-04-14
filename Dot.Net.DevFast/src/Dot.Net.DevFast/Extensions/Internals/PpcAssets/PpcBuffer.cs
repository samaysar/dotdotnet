using System.Collections.Concurrent;
using System.Threading;
#if !NETFRAMEWORK || NET5_0_OR_GREATER
using System.Threading.Tasks;
#endif
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
            //we do not create merge token, as user should be able to
            //extract queued items once pipeline is closed for addition.
            return _collection.TryTake(out data, millisecTimeout, token);
        }

        public bool Finished => _collection.IsCompleted;

        public int Unprocessed => _collection.Count;

        public void Add(T item, CancellationToken token)
        {
            TryAdd(item, Timeout.Infinite, token);
        }

        public bool TryAdd(T item, int millisecTimeout, CancellationToken token)
        {
            using var mergeToken = CancellationTokenSource.CreateLinkedTokenSource(token, _token);
            return _collection.TryAdd(item, millisecTimeout, mergeToken.Token);
        }

        public void Close()
        {
            _collection.CompleteAdding();
        }

#if !NETFRAMEWORK || NET5_0_OR_GREATER
        public ValueTask DisposeAsync()
        {
            Dispose();
            return default;
        }
#endif

        public void Dispose()
        {
            if (_collection != null)
            {
                using (_collection)
                {
                    //do nothing
                }
            }
            _collection = null;
        }
    }
}