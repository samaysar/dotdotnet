using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Elementary class that would support the whole functional streaming pipeline.
    /// </summary>
    public sealed class StreamPipe
    {
        private Func<Stream, bool, CancellationToken, Task> _writerFunc;

        internal StreamPipe(Action<Stream, bool, CancellationToken> writerAction) : this(writerAction.ToAsync())
        {
        }

        internal StreamPipe(Func<Stream, bool, CancellationToken, Task> writerFunc)
        {
            _writerFunc = writerFunc;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that
        /// starts writing on the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">underlying stream</param>
        /// <param name="dispose">If true, stream is dispose once operation completes.</param>
        /// <param name="token">Cancellation token to observe</param>
        public async Task StreamAsync(Stream stream, bool dispose = true,
            CancellationToken token = default(CancellationToken))
        {
            await _writerFunc(stream, dispose, token).ConfigureAwait(false);
        }

        internal StreamPipe Mutate(Func<Func<Stream, bool, CancellationToken, Task>,
            Func<Stream, bool, CancellationToken, Task>> mutable)
        {
            _writerFunc = mutable(_writerFunc);
            return this;
        }
    }
}