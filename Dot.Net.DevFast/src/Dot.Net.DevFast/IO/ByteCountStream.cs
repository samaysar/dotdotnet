using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly Stream _innerStream;
        private readonly bool _disposeInner;

        /// <inheritdoc />
        /// <summary>
        /// Initialize a Counting ONLY instance.
        /// </summary>
        public ByteCountStream() : this(null, false)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes an instance that passes (or reads) all the byte data to (from) inner stream
        /// and counts the number of bytes it observed.
        /// </summary>
        /// <param name="innerStream">inner stream from which to read or to write</param>
        /// <param name="disposeInner">true to dispose inner or false to leave it open.</param>
        public ByteCountStream(Stream innerStream, bool disposeInner)
        {
            _innerStream = innerStream;
            _disposeInner = disposeInner;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Flush"/> on the inner stream if supplied through Ctor else nothing.
        /// </summary>
        public override void Flush()
        {
            _innerStream?.Flush();
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Seek"/> on the inner stream if supplied through Ctor else nothing.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_innerStream == null) throw new NotImplementedException();
            return _innerStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.SetLength"/> on the inner stream if supplied through Ctor else nothing.
        /// </summary>
        public override void SetLength(long value)
        {
            _innerStream?.SetLength(value);
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Read"/> on the inner stream if supplied through Ctor and counts bytes.
        /// </summary>
        /// <exception cref="NotImplementedException">When inner stream is not supplied</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_innerStream == null) throw new NotImplementedException();
            var innerCount = _innerStream.Read(buffer, offset, count);
            ByteCount += innerCount;
            return innerCount;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Write"/> on the inner stream if supplied through Ctor and counts bytes; otherwise,
        /// only counts bytes.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_innerStream == null)
            {
                buffer.ThrowIfNull($"Buffer is null inside {nameof(ByteCountStream)}").Length
                    .ThrowIfLess(
                        offset.ThrowIfNegative($"offset cannot be negative inside {nameof(ByteCountStream)}") +
                        count.ThrowIfNegative($"count cannot be negative inside {nameof(ByteCountStream)}"),
                        $"Invalid offset, count and buffer length inside {nameof(ByteCountStream)}");
            }
            else
            {
                _innerStream.Write(buffer, offset, count);
            }

            ByteCount += count;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanRead"/> on the inner stream if supplied through Ctor else returns False.
        /// </summary>
        public override bool CanRead => _innerStream != null && _innerStream.CanRead;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanSeek"/> on the inner stream if supplied through Ctor else returns False.
        /// </summary>
        public override bool CanSeek => _innerStream != null && _innerStream.CanSeek;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CanWrite"/> on the inner stream if supplied through Ctor else returns True.
        /// </summary>
        public override bool CanWrite => _innerStream == null || _innerStream.CanWrite;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Length"/> on the inner stream if supplied through Ctor else returns 0.
        /// </summary>
        public override long Length => _innerStream?.Length ?? 0;

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.Position"/> on the inner stream if supplied through Ctor else 
        /// getter returns 0 and setter does nothing.
        /// </summary>
        public override long Position
        {
            get => _innerStream?.Position ?? 0;
            set
            {
                if (_innerStream != null) _innerStream.Position = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the <seealso cref="Stream.CopyToAsync(Stream,int,CancellationToken)"/> on the inner stream if supplied through Ctor and counts bytes.
        /// </summary>
        /// <exception cref="NotImplementedException">When inner stream is not supplied</exception>
        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if (_innerStream == null) throw new NotImplementedException();
            await _innerStream.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Count of bytes observed during read/write methods.
        /// </summary>
        public long ByteCount { get; private set; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_innerStream != null && disposing && _disposeInner)
            {
                using (_innerStream)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}