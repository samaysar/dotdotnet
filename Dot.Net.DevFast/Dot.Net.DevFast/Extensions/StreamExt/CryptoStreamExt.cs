using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StreamExt
{
    /// <summary>
    /// Extensions on Cypto stream for data transformation.
    /// </summary>
    public static class CryptoStreamExt
    {
        /// <summary>
        /// Reads byte segment from <paramref name="input"/> and writes transformed data on <paramref name="targetStream"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Bytes to transform</param>
        /// <param name="transform">transform to use</param>
        /// <param name="targetStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task TransformAsync(this ArraySegment<byte> input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput = false)
        {
            return input.Array.TransformAsync(transform, targetStream, token, disposeOutput, input.Offset, input.Count);
        }

        /// <summary>
        /// Reads full <paramref name="input"/> array and writes transformed data on <paramref name="targetStream"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Bytes to transform</param>
        /// <param name="transform">transform to use</param>
        /// <param name="targetStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task TransformAsync(this byte[] input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput = false)
        {
            return input.TransformAsync(transform, targetStream, token, disposeOutput, 0, input.Length);
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and writes transformed data on <paramref name="targetStream"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="targetStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task TransformAsync(this Stream sourceStream, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeInput = false,
            bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var outputWrapper = new WrappedStream(targetStream, disposeOutput))
            {
                using (var transformer = new CryptoStream(outputWrapper, transform, CryptoStreamMode.Write))
                {
                    using (var inputWrapper = new WrappedStream(sourceStream, disposeInput))
                    {
                        await inputWrapper.CopyToAsync(transformer, bufferSize, token).ConfigureAwait(false);
                        await transformer.FlushAsync(token).ConfigureAwait(false);
                        await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                        await targetStream.FlushAsync(token).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and prepares transformed string (return value),
        /// using <paramref name="transform"/> and <paramref name="encoding"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to compose string characters, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<string> TransformAsync(this Stream sourceStream,
            ICryptoTransform transform, CancellationToken token, bool disposeInput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var strBuilder = new StringBuilder();
            await sourceStream.TransformAsync(transform, strBuilder, token, disposeInput,
                encoding ?? Encoding.UTF8, bufferSize).ConfigureAwait(false);
            return strBuilder.ToString();
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and prepares decoded byte array (return value),
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<byte[]> TransformAsync(this Stream sourceStream, ICryptoTransform transform,
            CancellationToken token, bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await sourceStream.TransformAsSegmentAsync(transform, token,
                disposeInput, bufferSize).ConfigureAwait(false)).CreateBytes();
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and prepares decoded byte array, (return as segment, 
        /// idea is to save on array copy to remain low on latency n memory as perhaps segment can serve the purpose),
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<ArraySegment<byte>> TransformAsSegmentAsync(this Stream sourceStream,
            ICryptoTransform transform, CancellationToken token, bool disposeInput = false, 
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var inputWrapper = new WrappedStream(sourceStream, disposeInput))
            {
                using (var transformer = new CryptoStream(inputWrapper, transform,
                    CryptoStreamMode.Read))
                {
                    using (var localBuffer = new MemoryStream())
                    {
                        await transformer.CopyToAsync(localBuffer, bufferSize, token).ConfigureAwait(false);
                        return localBuffer.TryGetBuffer(out ArraySegment<byte> buffer)
                            .ThrowIfNot(DdnDfErrorCode.NullObject,
                                "Something horribly went wrong with" +
                                $" {nameof(MemoryStream)} implementation", buffer);
                    }
                }
            }
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and appends transformed string to <paramref name="appendTo"/>,
        /// using <paramref name="transform"/> and <paramref name="encoding"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="appendTo">StringBuilder to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to compose string characters, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task TransformAsync(this Stream sourceStream, ICryptoTransform transform,
            StringBuilder appendTo, CancellationToken token, bool disposeInput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var inputWrapper = new WrappedStream(sourceStream, disposeInput))
            {
                using (var transformer = new CryptoStream(inputWrapper, transform,
                    CryptoStreamMode.Read))
                {
                    using (var streamReader = new StreamReader(transformer, encoding ?? Encoding.UTF8,
                        true, bufferSize, true))
                    {
                        var charBuffer = new char[bufferSize];
                        int charCnt;
                        while ((charCnt = await streamReader.ReadAsync(charBuffer, 0, bufferSize)
                                   .ConfigureAwait(false)) != 0)
                        {
                            appendTo.Append(charBuffer, 0, charCnt);
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes transformed data on <paramref name="targetStream"/>,
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing 
        /// <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="targetStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task TransformAsync(this StringBuilder input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return TransformChunksAsync(targetStream, transform, input.Length, encoding ?? Encoding.UTF8,
                token, disposeOutput, bufferSize, input.CopyTo);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes transformed data on <paramref name="targetStream"/>, 
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="targetStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task TransformAsync(this string input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return TransformChunksAsync(targetStream, transform, input.Length, encoding ?? Encoding.UTF8,
                token, disposeOutput, bufferSize, input.CopyTo);
        }

        private static async Task TransformChunksAsync(Stream writable, ICryptoTransform transform,
            int length, Encoding enc, CancellationToken token, bool disposeOutput, int chunkSize,
            Action<int, char[], int, int> copyToAction)
        {
            using (var outputWrapper = new WrappedStream(writable, disposeOutput))
            {
                using (var transformer = new CryptoStream(outputWrapper, transform,
                    CryptoStreamMode.Write))
                {
                    await EncodedCharacterCopyAsync(transformer, length, enc,
                        token, chunkSize, copyToAction).ConfigureAwait(false);
                    await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                    await writable.FlushAsync(token).ConfigureAwait(false);
                }
            }
        }

        internal static async Task EncodedCharacterCopyAsync(Stream writable,
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

        private static async Task TransformAsync(this byte[] input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput,
            int byteOffset, int byteCount)
        {
            using (var outputWrapper = new WrappedStream(targetStream, disposeOutput))
            {
                using (var transformer = new CryptoStream(outputWrapper, transform, CryptoStreamMode.Write))
                {
                    await transformer.WriteAsync(input, byteOffset, byteCount, token).ConfigureAwait(false);
                    await transformer.FlushAsync(token).ConfigureAwait(false);
                    await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                    await targetStream.FlushAsync(token).ConfigureAwait(false);
                }
            }
        }
    }
}