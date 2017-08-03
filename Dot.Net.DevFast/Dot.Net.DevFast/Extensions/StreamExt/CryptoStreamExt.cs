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
        /// Reads byte segment from <paramref name="source"/> and writes transformed data on <paramref name="output"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="source">Bytes to transform</param>
        /// <param name="transform">transform to use</param>
        /// <param name="output">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="output"/> upon operation completion, else leaves it open</param>
        public static Task TransformAsync(this ArraySegment<byte> source, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeOutput = false)
        {
            return source.Array.TransformAsync(transform, output, token, disposeOutput, source.Offset, source.Count);
        }

        /// <summary>
        /// Reads full <paramref name="source"/> array and writes transformed data on <paramref name="output"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="source">Bytes to transform</param>
        /// <param name="transform">transform to use</param>
        /// <param name="output">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="output"/> upon operation completion, else leaves it open</param>
        public static Task TransformAsync(this byte[] source, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeOutput = false)
        {
            return source.TransformAsync(transform, output, token, disposeOutput, 0, source.Length);
        }

        /// <summary>
        /// Reads from <paramref name="input"/> and writes transformed data on <paramref name="output"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="output">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="input"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="output"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task TransformAsync(this Stream input, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeInput = false,
            bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var outputWrapper = new WrappedStream(output, disposeOutput))
            {
                using (var transformer = new CryptoStream(outputWrapper, transform, CryptoStreamMode.Write))
                {
                    using (var inputWrapper = new WrappedStream(input, disposeInput))
                    {
                        await inputWrapper.CopyToAsync(transformer, bufferSize, token).ConfigureAwait(false);
                        await transformer.FlushAsync(token).ConfigureAwait(false);
                        await output.FlushAsync(token).ConfigureAwait(false);
                    }
                }
            }
        }
        
        /// <summary>
        /// Reads from <paramref name="input"/> and prepares decoded byte array (return value),
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="input"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<byte[]> TransformAsync(this Stream input, ICryptoTransform transform,
            CancellationToken token = default(CancellationToken), bool disposeInput = false,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await input.TransformAsSegmentAsync(transform, token,
                disposeInput, bufferSize).ConfigureAwait(false)).CreateBytes();
        }

        /// <summary>
        /// Reads from <paramref name="input"/> and prepares decoded byte array, (return as segment, 
        /// idea is to save on array copy to remain low on latency n memory as perhaps segment can serve the purpose),
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="input"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<ArraySegment<byte>> TransformAsSegmentAsync(this Stream input,
            ICryptoTransform transform, CancellationToken token = default(CancellationToken),
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var inputWrapper = new WrappedStream(input, disposeInput))
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
        /// Reads from <paramref name="input"/> and prepares transformed string (return value),
        /// using <paramref name="transform"/> and <paramref name="encoding"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="input"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to compose string characters, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<string> TransformAsStringAsync(this Stream input, ICryptoTransform transform,
            Encoding encoding = null, CancellationToken token = default(CancellationToken), 
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var strBuilder = new StringBuilder();
            await input.TransformAsync(transform, strBuilder, token, disposeInput, encoding, bufferSize)
                .ConfigureAwait(false);
            return strBuilder.ToString();
        }

        /// <summary>
        /// Reads from <paramref name="input"/> and appends transformed string to <paramref name="target"/>,
        /// using <paramref name="transform"/> and <paramref name="encoding"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="target">StringBuilder to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="input"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to compose string characters, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task TransformAsync(this Stream input, ICryptoTransform transform,
            StringBuilder target, CancellationToken token = default(CancellationToken), bool disposeInput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var inputWrapper = new WrappedStream(input, disposeInput))
            {
                using (var transformer = new CryptoStream(inputWrapper, transform,
                    CryptoStreamMode.Read))
                {
                    using (var streamReader = transformer.CreateReader(encoding, bufferSize))
                    {
                        var charBuffer = new char[bufferSize];
                        int charCnt;
                        if (token.CanBeCanceled)
                        {
                            while ((charCnt = await streamReader.ReadBlockAsync(charBuffer, 0, bufferSize)
                                       .ConfigureAwait(false)) != 0)
                            {
                                target.Append(charBuffer, 0, charCnt);
                                token.ThrowIfCancellationRequested();
                            }
                        }
                        else
                        {
                            while ((charCnt = await streamReader.ReadBlockAsync(charBuffer, 0, bufferSize)
                                       .ConfigureAwait(false)) != 0)
                            {
                                target.Append(charBuffer, 0, charCnt);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads characters from <paramref name="source"/> and writes transformed data on <paramref name="output"/>,
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing 
        /// <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="source">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="output">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="output"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task TransformAsync(this StringBuilder source, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeOutput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return TransformChunksAsync(output, transform, source.Length, encoding, token, disposeOutput, bufferSize,
                source.CopyTo);
        }

        /// <summary>
        /// Reads characters from <paramref name="source"/> and writes transformed data on <paramref name="output"/>, 
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="source">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="output">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="output"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task TransformAsync(this string source, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeOutput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return TransformChunksAsync(output, transform, source.Length, encoding, token, disposeOutput, bufferSize,
                source.CopyTo);
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

        private static async Task TransformChunksAsync(Stream writable, ICryptoTransform transform,
            int length, Encoding enc, CancellationToken token, bool disposeOutput, int chunkSize,
            Action<int, char[], int, int> copyToAction)
        {
            using (var outputWrapper = new WrappedStream(writable, disposeOutput))
            {
                using (var transformer = new CryptoStream(outputWrapper, transform,
                    CryptoStreamMode.Write))
                {
                    await EncodedCharacterCopyAsync(transformer, length, enc ?? Encoding.UTF8,
                        token, chunkSize, copyToAction).ConfigureAwait(false);
                    await writable.FlushAsync(token).ConfigureAwait(false);
                }
            }
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
                    await targetStream.FlushAsync(token).ConfigureAwait(false);
                }
            }
        }
    }
}