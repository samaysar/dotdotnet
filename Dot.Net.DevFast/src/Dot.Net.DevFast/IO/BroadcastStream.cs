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
    /// Stream that can perform concurrent unidirectional write on two underlying streams.
    /// Normally this implementation is NOT for public exposure.
    /// It exists specially for streaming APIs to have concurrent writes.
    /// </summary>
    internal class BroadcastStream : Stream
    {
        private Stream _anotherStream;
        private Stream _pfsStream;
        private bool _anotherStreamInError;
        private readonly bool _disposeAnother;
        private readonly Action<Exception> _streamErrHandler;
        private readonly bool _disposePfs;

        /// <inheritdoc />
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="pfs">Push functional stream to write on.</param>
        /// <param name="writableStream">Another writable stream</param>
        /// <param name="disposeWritable">true to dispose <paramref name="writableStream"/> else false.</param>
        /// <param name="streamErrHandler">Error handler for writable stream</param>
        public BroadcastStream(PushFuncStream pfs, Stream writableStream, bool disposeWritable,
            Action<Stream, Exception> streamErrHandler)
        {
            _anotherStream = writableStream.CanWrite.ThrowIfNot(DdnDfErrorCode.Unspecified,
                "Stream instance is not writable", writableStream);
            _pfsStream = pfs.Writable
                .ThrowIfNull($"{nameof(PushFuncStream)}.{nameof(PushFuncStream.Writable)} is null");
            _disposePfs = pfs.Dispose;
            _disposeAnother = disposeWritable;
            _anotherStreamInError = false;
            _streamErrHandler = e =>
            {
                if (streamErrHandler == null) throw new Exception("Error during Concurrent streaming.", e);
                _anotherStreamInError = true;
                streamErrHandler(_anotherStream, e);
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override void Flush()
        {
            FlushAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            //Safe we can flush both
            var anotherStreamTask = _anotherStreamInError
                ? Task.CompletedTask
                : Task.Run(new Func<Task>(async () =>
                        await _anotherStream.FlushAsync(cancellationToken).ConfigureAwait(false))
                    .ErrorWrapper(_streamErrHandler), cancellationToken);
            var streamTask = Task.Run(async () => await _pfsStream.FlushAsync(cancellationToken).ConfigureAwait(false),
                cancellationToken);
            await Task.WhenAll(streamTask, anotherStreamTask).ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            //unsafe we can NOT do both
            throw new NotImplementedException();
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
        /// Throws NotImplementedException.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var streamTask = Task.Run(async () =>
                    await _pfsStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false),
                cancellationToken);
            var anotherStreamTask = _anotherStreamInError
                ? Task.CompletedTask
                : Task.Run(new Func<Task>(async () => await _anotherStream
                        .WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false))
                    .ErrorWrapper(_streamErrHandler), cancellationToken);
            await Task.WhenAll(streamTask, anotherStreamTask).ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns false.
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
        /// Returns 0.
        /// </summary>
        public override long Length => 0;

        /// <inheritdoc />
        /// <summary>
        /// Gets 0, sets nothing
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