using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.IO
{
    /// <inheritdoc />
    /// <summary>
    /// Stream that can perform concurrent unidirectionl write on multiple underlying stream.
    /// Normally this implementation is NOT needed. It exists specially for streaming APIs to have concurrent writes.
    /// </summary>
    internal class ConcurrentWritableStream : Stream
    {
        private Stream _anotherStream;
        private Stream _pfsStream;
        private readonly bool _disposeAnother;
        private readonly bool _disposePfs;
        private readonly CancellationToken _token;

        /// <inheritdoc />
        /// <summary>
        /// Cotr.
        /// </summary>
        /// <param name="pfs">Push functional stream to write on.</param>
        /// <param name="writableStream">Another writable stream</param>
        /// <param name="disposeWritable">true to dispose <paramref name="writableStream"/> else false.</param>
        public ConcurrentWritableStream(PushFuncStream pfs, Stream writableStream, bool disposeWritable)
        {
            _anotherStream = writableStream.CanWrite.ThrowIfNot(DdnDfErrorCode.Unspecified,
                "Stream instance is not writable", writableStream);
            _pfsStream = pfs.Writable
                .ThrowIfNull($"{nameof(PushFuncStream)}.{nameof(PushFuncStream.Writable)} is null");
            _disposePfs = pfs.Dispose;
            _disposeAnother = disposeWritable;
            _token = pfs.Token;
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override void Flush()
        {
            FlushAsync(_token).Wait(_token);
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            //Safe we can flush both
            var streamTask = Task.Run(async () => await _anotherStream.FlushAsync(_token).ConfigureAwait(false),
                _token);
            var anotherStreamTask = Task.Run(async () => await _pfsStream.FlushAsync(_token).ConfigureAwait(false),
                _token);
            await Task.WhenAll(streamTask, anotherStreamTask).ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Does nothing
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            //unsafe we can NOT do both
            return 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Does nothing
        /// </summary>
        public override void SetLength(long value)
        {
            //unsafe we can NOT do both
        }

        /// <inheritdoc />
        /// <summary>
        /// Not implementted. Calling this function will yield in error.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteAsync(buffer, offset, count, _token).Wait(_token);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var streamTask = Task.Run(async () =>
                await _pfsStream.WriteAsync(buffer, offset, count, _token).ConfigureAwait(false), _token);
            var anotherStreamTask = Task.Run(async () =>
                await _anotherStream.WriteAsync(buffer, offset, count, _token).ConfigureAwait(false), _token);
            await Task.WhenAll(streamTask, anotherStreamTask).ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Always returns false.
        /// </summary>
        public override bool CanRead => false;

        /// <inheritdoc />
        /// <summary>
        /// Always returns false.
        /// </summary>
        public override bool CanSeek => false;

        /// <inheritdoc />
        /// <summary>
        /// Always returns true.
        /// </summary>
        public override bool CanWrite => true;

        /// <inheritdoc />
        /// <summary>
        /// Does nothing
        /// </summary>
        public override long Length => 0;

        /// <inheritdoc />
        /// <summary>
        /// Does nothing
        /// </summary>
        public override long Position
        {
            get => 0;
            set { }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_disposeAnother && _anotherStream != null)
                {
                    using (_anotherStream)
                    {
                    }
                    _anotherStream = null;
                }
                if (_disposePfs && _pfsStream != null)
                {
                    using (_pfsStream)
                    {
                    }
                    _pfsStream = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}