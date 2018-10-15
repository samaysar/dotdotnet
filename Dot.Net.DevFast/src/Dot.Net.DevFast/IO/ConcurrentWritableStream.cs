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
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override void Flush()
        {
            FlushAsync(CancellationToken.None).Wait();
        }

        /// <inheritdoc />
        /// <summary>
        /// Flushes both streams.
        /// </summary>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            //Safe we can flush both
            var streamTask = Task.Run(async () => await _anotherStream.FlushAsync(cancellationToken).ConfigureAwait(false),
                cancellationToken);
            var anotherStreamTask = Task.Run(async () => await _pfsStream.FlushAsync(cancellationToken).ConfigureAwait(false),
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
            WriteAsync(buffer, offset, count, CancellationToken.None).Wait();
        }

        /// <inheritdoc />
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var streamTask = Task.Run(async () =>
                    await _pfsStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false),
                cancellationToken);
            var anotherStreamTask = Task.Run(async () =>
                    await _anotherStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false),
                cancellationToken);
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