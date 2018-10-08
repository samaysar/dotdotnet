using System;
using System.IO;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.IO
{
    /// <inheritdoc />
    /// <summary>
    /// Stream implementation that counts the number of BYTEs (exposed by <see cref="P:Dot.Net.DevFast.IO.ByteCountingStream.ByteCount" /> property)
    /// passed through it. It works as a pass-through stream if another stream is supplied through one of the Ctor.
    /// </summary>
    public class ByteCountStream : Stream
    {
        private readonly bool _disposeInner;

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

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Flush"/> on the inner stream.
        /// </summary>
        public override void Flush()
        {
            InnerStream.Flush();
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Seek"/> on the inner stream.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.SetLength"/> on the inner stream.
        /// </summary>
        public override void SetLength(long value)
        {
            InnerStream.SetLength(value);
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Read"/> on the inner stream and counts bytes.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var innerCount = InnerStream.Read(buffer, offset, count);
            ByteCount += innerCount;
            return innerCount;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Write"/> on the inner stream and counts bytes.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (ReferenceEquals(this, Null))
            {
                buffer.ThrowIfNull($"Buffer is null inside {nameof(ByteCountStream)}").Length
                    .ThrowIfLess(
                        offset.ThrowIfNegative($"offset cannot be negative inside {nameof(ByteCountStream)}") +
                        count.ThrowIfNegative($"count cannot be negative inside {nameof(ByteCountStream)}"),
                        $"Invalid offset, count and buffer length inside {nameof(ByteCountStream)}");
            }
            else
            {
                InnerStream.Write(buffer, offset, count);
            }

            ByteCount += count;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanRead"/> on the inner stream.
        /// </summary>
        public override bool CanRead => InnerStream.CanRead;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanSeek"/> on the inner stream.
        /// </summary>
        public override bool CanSeek => InnerStream.CanSeek;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanWrite"/> on the inner stream.
        /// </summary>
        public override bool CanWrite => InnerStream.CanWrite;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Length"/> on the inner stream.
        /// </summary>
        public override long Length => InnerStream.Length;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Position"/> on the inner stream.
        /// </summary>
        public override long Position
        {
            get => InnerStream.Position;
            set => InnerStream.Position = value;
        }

        /// <summary>
        /// Count of bytes observed during read/write methods.
        /// </summary>
        public long ByteCount { get; private set; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (InnerStream != null && disposing && _disposeInner)
            {
                using (InnerStream)
                {
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the associated inner stream.
        /// </summary>
        public Stream InnerStream { get; }
    }
}