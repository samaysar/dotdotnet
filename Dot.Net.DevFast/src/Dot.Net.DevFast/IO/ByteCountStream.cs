using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.IO
{
    /// <summary>
    /// Interface to impose the byte countting getter.
    /// </summary>
    public interface IByteCounter
    {
        /// <summary>
        /// Count of bytes observed during Streaming operations.
        /// </summary>
        long ByteCount { get; }
    }

    /// <inheritdoc cref="Stream"/>
    /// <summary>
    /// Stream implementation that counts the number of BYTEs (exposed by <see cref="P:Dot.Net.DevFast.IO.ByteCountingStream.ByteCount" /> property)
    /// passed through it. It works as a pass-through stream if another stream is supplied through one of the Ctor.
    /// </summary>
    internal class ByteCountStream : Stream, IByteCounter
    {
        private bool _disposeInner;

        /// <inheritdoc />
        /// <summary>
        /// Initializes an instance that passes (or reads) all the byte data to (from) inner stream
        /// and counts the number of bytes it observed.
        /// <para>If <paramref name="innerStream"/> is NOT supplied then <seealso cref="Stream.Null"/> will be used internally</para>
        /// </summary>
        /// <param name="innerStream">inner stream from which to read or to write. If <paramref name="innerStream"/> is NOT supplied then <seealso cref="Stream.Null"/> will be used internally.</param>
        /// <param name="leaveOpen">false to dispose inner or true to leave it open.</param>
        public ByteCountStream(Stream innerStream = null, bool leaveOpen = false)
        {
            InnerStream = innerStream ?? Null;
            _disposeInner = !leaveOpen;
        }

        /// <summary>
        /// This is a workaround to support out param in streaming APIs.
        /// </summary>
        internal void ResetWith(Stream stream, bool dispose)
        {
            //!!! Hacky sln for the moment.

            //First we dispose old inner as needed
            Dispose(true);

            //We change
            InnerStream = stream;
            _disposeInner = dispose;
            //we change the stream we reset byte count
            ByteCount = 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Flush"/> on the inner stream.
        /// </summary>
        public override void Flush() => ThrowIfDisposed().Flush();

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Seek"/> on the inner stream.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin) => ThrowIfDisposed().Seek(offset, origin);

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.SetLength"/> on the inner stream.
        /// </summary>
        public override void SetLength(long value) => ThrowIfDisposed().SetLength(value);

        /// <inheritdoc />
        /// <summary>
        /// Calls <seealso cref="Stream.Read"/> on the inner stream and counts bytes.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return ReadAsync(buffer, offset, count, CancellationToken.None).Result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls <seealso cref="Stream.ReadAsync(byte[],int,int,CancellationToken)"/> on the inner stream and counts bytes.
        /// </summary>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var innerCount = await ThrowIfDisposed().ReadAsync(buffer, offset, count, cancellationToken)
                .ConfigureAwait(false);
            AddByteCount(innerCount);
            return innerCount;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls <seealso cref="Stream.Write"/> on the inner stream and counts bytes.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteAsync(buffer, offset, count, CancellationToken.None).Wait();
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls <seealso cref="Stream.WriteAsync(byte[],int,int, CancellationToken)"/> on the inner stream and counts bytes.
        /// </summary>
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (ReferenceEquals(ThrowIfDisposed(), Null))
            {
                buffer.ThrowIfNull($"Buffer is null inside {nameof(ByteCountStream)}").Length
                    .ThrowIfLess(
                        offset.ThrowIfNegative($"offset cannot be negative inside {nameof(ByteCountStream)}") +
                        count.ThrowIfNegative($"count cannot be negative inside {nameof(ByteCountStream)}"),
                        $"Invalid offset (={offset}), count (={count}) and buffer length " +
                        $"(={buffer.Length}) inside {nameof(ByteCountStream)}");
            }
            else
            {
                await InnerStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            }
            AddByteCount(count);
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanRead"/> on the inner stream.
        /// </summary>
        public override bool CanRead => ThrowIfDisposed().CanRead;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanSeek"/> on the inner stream.
        /// </summary>
        public override bool CanSeek => ThrowIfDisposed().CanSeek;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanWrite"/> on the inner stream.
        /// </summary>
        public override bool CanWrite => ThrowIfDisposed().CanWrite;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Length"/> on the inner stream.
        /// </summary>
        public override long Length => ThrowIfDisposed().Length;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Position"/> on the inner stream.
        /// </summary>
        public override long Position
        {
            get => ThrowIfDisposed().Position;
            set => ThrowIfDisposed().Position = value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Count of bytes observed during read/write methods.
        /// <para>Property remains accessible after dispose.</para>
        /// </summary>
        public long ByteCount { get; private set; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposeInner && InnerStream != null)
            {
                using (InnerStream)
                {
                }

                InnerStream = null;
            }
        }

        /// <summary>
        /// Gets the associated inner stream.
        /// </summary>
        public Stream InnerStream { get; private set; }

        internal void AddByteCount(int count)
        {
            ByteCount += count;
        }

        private Stream ThrowIfDisposed()
        {
            if (InnerStream is null) throw new ObjectDisposedException(nameof(ByteCountStream));
            return InnerStream;
        }
    }
}