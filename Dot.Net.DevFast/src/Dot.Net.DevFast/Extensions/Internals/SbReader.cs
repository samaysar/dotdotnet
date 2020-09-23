﻿using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal sealed class SbReader : TextReader
    {
        private StringBuilder _sb;
        private int _position;
        private int _length;
                                                   
        public SbReader(StringBuilder s)
        {
            _sb = s.ThrowIfNull($"{nameof(StringBuilder)} null");
            _length = s?.Length ?? 0;
            _position = 0;
        }

        // Closes this StringReader. Following a call to this method, the String
        // Reader will throw an ObjectDisposedException.
        public override void Close()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            _sb = null;
            _position = 0;
            _length = 0;
            base.Dispose(disposing);
        }

        public override int Peek()
        {
            _sb.ThrowIfNull("Source is closed/disposed");
            if (_position == _length) return -1;
            return _sb[_position];
        }

        public override int Read()
        {
            _sb.ThrowIfNull("Source is closed/disposed");
            if (_position == _length) return -1;
            return _sb[_position++];
        }

        public override int Read(char[] buffer, int index, int count)
        {
            _sb.ThrowIfNull("Source is closed/disposed");
            var n = _length - _position;
            if (n > 0)
            {
                if (n > count) n = count;
                _sb.CopyTo(_position, buffer, index, n);
                _position += n;
            }
            return n;
        }

        public override string ReadToEnd()
        {
            _sb.ThrowIfNull("Source is closed/disposed");
            var s = _position == 0 ? _sb.ToString() : _sb.ToString(_position, _length - _position);
            _position = _length;
            return s;
        }

        public override string ReadLine()
        {
            _sb.ThrowIfNull("Source is closed/disposed");
            var i = _position;
            while (i < _length)
            {
                var ch = _sb[i];
                if (ch == '\r' || ch == '\n')
                {
                    var result = _sb.ToString(_position, i - _position);
                    _position = i + 1;
                    if (ch == '\r' && _position < _length && _sb[_position] == '\n') _position++;
                    return result;
                }
                i++;
            }
            if (i > _position)
            {
                var result = _sb.ToString(_position, i - _position);
                _position = i;
                return result;
            }
            return null;
        }

        public override async Task<string> ReadLineAsync()
        {
            return await new ValueTask<string>(ReadLine()).ConfigureAwait(false);

        }

        public override async Task<string> ReadToEndAsync()
        {
            return await new ValueTask<string>(ReadToEnd()).ConfigureAwait(false);
        }

        public override async Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            // ReSharper disable once MethodHasAsyncOverload
            return await new ValueTask<int>(ReadBlock(buffer, index, count)).ConfigureAwait(false);
        }

        public override async Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return await new ValueTask<int>(Read(buffer, index, count)).ConfigureAwait(false);
        }
    }
}