using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal static class StreamBased
    {
        internal static async Task CopyFromWithDisposeAsync(this Stream writable,
            int length, Encoding enc, CancellationToken token, int chunkSize,
            Action<int, char[], int, int> copyToAction, Stream writableInner)
        {
#if NETFRAMEWORK
            using (writable)
#else
            await using (writable.ConfigureAwait(false))
#endif 
            {
                await writable.CopyFromAsync(length, enc, token, chunkSize, copyToAction)
                    .ConfigureAwait(false);
                await writableInner.FlushAsync(token).ConfigureAwait(false);
            }
        }

        internal static async Task CopyFromAsync(this Stream writable,
            int length, Encoding enc, CancellationToken token, int chunkSize,
            Action<int, char[], int, int> copyToAction)
        {
            var bytes = enc.GetPreamble();
            if (bytes.Length > 0)
            {
                await writable.WriteAsync(bytes, 0, bytes.Length, token)
                    .ConfigureAwait(false);
            }
            var charArr = new char[chunkSize];
            bytes = new byte[enc.GetMaxByteCount(chunkSize)];
            var charCnt = length;
            var position = 0;
            while (charCnt > 0)
            {
                if (charCnt > chunkSize) charCnt = chunkSize;
                copyToAction(position, charArr, 0, charCnt);
                var byteCnt = enc.GetBytes(charArr, 0, charCnt, bytes, 0);
                await writable.WriteAsync(bytes, 0, byteCnt, token).ConfigureAwait(false);
                position += charCnt;
                charCnt = length - position;
            }
            await writable.FlushAsync(token).ConfigureAwait(false);
        }

        internal static async Task CopyToBuilderAsync(this Stream readable,
            StringBuilder target, CancellationToken token, Encoding encoding, int bufferSize,
            bool disposeReadable, bool detectEncodingFromBom = true)
        {
            using var reader = readable.CreateReader(encoding, bufferSize, disposeReadable, detectEncodingFromBom);
            var charBuffer = new char[bufferSize];
            int charCnt;
            if (token.CanBeCanceled)
            {
                while ((charCnt = await reader.ReadBlockAsync(charBuffer, 0, bufferSize)
                    .ConfigureAwait(false)) != 0)
                {
                    target.Append(charBuffer, 0, charCnt);
                    token.ThrowIfCancellationRequested();
                }
            }
            else
            {
                while ((charCnt = await reader.ReadBlockAsync(charBuffer, 0, bufferSize)
                    .ConfigureAwait(false)) != 0)
                {
                    target.Append(charBuffer, 0, charCnt);
                }
            }
        }

        internal static async Task CopyFromWithDisposeAsync(this Stream writable, Stream readable,
            int bufferSize, CancellationToken token, Stream writableInner, bool disposeReadable)
        {
            try
            {
#if NETFRAMEWORK
                using (writable)
#else
                await using (writable.ConfigureAwait(false))
#endif
                {
                    await readable.CopyToAsync(writable, bufferSize, token).ConfigureAwait(false);
                    await writable.FlushAsync(token).ConfigureAwait(false);
                    await writableInner.FlushAsync(token).ConfigureAwait(false);
                }
            }
            finally
            {
#if NETFRAMEWORK
                readable.DisposeIfRequired(disposeReadable);
#else
                await readable.DisposeIfRequiredAsync(disposeReadable).ConfigureAwait(false);
#endif
            }
        }

        internal static async Task CopyToWithDisposeAsync(this Stream from, Stream to,
            int bufferSize, CancellationToken token, bool disposeTo)
        {
            try
            {
#if NETFRAMEWORK
                using (from)
#else
                await using (from.ConfigureAwait(false))
#endif
                {
                    await from.CopyToAsync(to, bufferSize, token).ConfigureAwait(false);
                    await to.FlushAsync(token).ConfigureAwait(false);
                }
            }
            finally
            {
#if NETFRAMEWORK
                to.DisposeIfRequired(disposeTo);
#else
                await to.DisposeIfRequiredAsync(disposeTo).ConfigureAwait(false);
#endif
            }
        }

        internal static async Task CopyFromWithDisposeAsync(this Stream writable, byte[] input,
            int byteOffset, int byteCount, CancellationToken token, Stream writableInner)
        {
#if NETFRAMEWORK
            using (writable)
#else
            await using (writable.ConfigureAwait(false))
#endif
            {
                await writable.WriteAsync(input, byteOffset, byteCount, token).ConfigureAwait(false);
                await writable.FlushAsync(token).ConfigureAwait(false);
                await writableInner.FlushAsync(token).ConfigureAwait(false);
            }
        }

        internal static async Task CopyToAsync(this Stream readable, Stream writable,
            int bufferSize, CancellationToken token, bool disposeReadable, bool disposeWritable)
        {
            try
            {
                await readable.CopyToAsync(writable, bufferSize, token).ConfigureAwait(false);
                await writable.FlushAsync(token).ConfigureAwait(false);
#if NETFRAMEWORK
                readable.DisposeIfRequired(disposeReadable);
#else
                await readable.DisposeIfRequiredAsync(disposeReadable).ConfigureAwait(false);
#endif
            }
            finally
            {
#if NETFRAMEWORK
                writable.DisposeIfRequired(disposeWritable);
#else
                await writable.DisposeIfRequiredAsync(disposeWritable).ConfigureAwait(false);
#endif
            }
        }

        internal static async Task<ArraySegment<byte>> CopyToSegmentWithDisposeAsync(this Stream readable,
            int bufferSize, CancellationToken token)
        {
#if NETFRAMEWORK
            using (var localBuffer = new MemoryStream())
#else
            var localBuffer = new MemoryStream();
            await using (localBuffer.ConfigureAwait(false))
#endif
            {
#if NETFRAMEWORK
                using (readable)
#else
                await using (readable.ConfigureAwait(false))
#endif
                {
                    await readable.CopyToAsync(localBuffer, bufferSize, token).ConfigureAwait(false);
                }

                return localBuffer.ThrowIfNoBuffer();
            }
        }

        internal static async Task CopyFromAsync(this Stream writable, ArraySegment<byte> input,
            CancellationToken token, bool disposeWritable)
        {
            try
            {
                await writable.WriteAsync(input.Array, input.Offset, input.Count, token).ConfigureAwait(false);
                await writable.FlushAsync(token).ConfigureAwait(false);
            }
            finally
            {
#if NETFRAMEWORK
                writable.DisposeIfRequired(disposeWritable);
#else
                await writable.DisposeIfRequiredAsync(disposeWritable).ConfigureAwait(false);
#endif
            }
        }
    }
}