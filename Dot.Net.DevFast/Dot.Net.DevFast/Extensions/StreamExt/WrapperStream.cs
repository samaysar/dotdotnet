using System;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.StreamExt
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

        public override void Flush()
        {
            _stream.Flush();
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

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public override bool Equals(object obj)
        {
            var objAsWrapper = obj as WrappedStream;
            return _stream.Equals(objAsWrapper != null ? objAsWrapper._stream : obj);
        }

        public override int GetHashCode()
        {
            return _stream.GetHashCode();
        }

        public override string ToString()
        {
            return _stream.ToString();
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

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override bool CanTimeout => _stream.CanTimeout;

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return _stream.CreateObjRef(requestedType);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _stream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }

        public override object InitializeLifetimeService()
        {
            return _stream.InitializeLifetimeService();
        }

        public override int ReadByte()
        {
            return _stream.ReadByte();
        }

        public override int ReadTimeout
        {
            get { return _stream.ReadTimeout; }
            set { _stream.ReadTimeout = value; }
        }

        public override void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public override int WriteTimeout
        {
            get { return _stream.WriteTimeout; }
            set { _stream.WriteTimeout = value; }
        }

        public override void Close()
        {
            if (_dispose) _stream.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _dispose) _stream.Dispose();
        }
    }
}