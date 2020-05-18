using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal class WrappedStream : Stream
    {
        private readonly Stream _stream;
        private readonly bool _dispose;

        internal WrappedStream(Stream stream, bool dispose)
        {
            _stream = stream;
            _dispose = dispose;
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override bool CanTimeout => _stream.CanTimeout;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position; set => _stream.Position = value;
        }

        public override int WriteTimeout
        {
            get => _stream.WriteTimeout; set => _stream.WriteTimeout = value;
        }

        public override int ReadTimeout
        {
            get => _stream.ReadTimeout; set => _stream.ReadTimeout = value;
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int GetHashCode()
        {
            return _stream.GetHashCode();
        }

        public override string ToString()
        {
            return _stream.ToString();
        }

        public override int ReadByte()
        {
            return _stream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override bool Equals(object obj)
        {
            return _stream.Equals(obj is WrappedStream objAsWrapper ? objAsWrapper._stream : obj);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _stream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _stream.FlushAsync(cancellationToken);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _dispose)
            {
                using (_stream)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}