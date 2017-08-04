using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;

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
        public static Task TransformAsync(this Stream input, ICryptoTransform transform,
            Stream output, CancellationToken token = default(CancellationToken), bool disposeInput = false,
            bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return output.CreateCryptoStream(transform, CryptoStreamMode.Write, disposeOutput)
                .CopyFromWithDisposeAsync(input, bufferSize, token, output, disposeInput);
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
            return output.TransformChunksAsync(transform, source.Length, encoding, token, disposeOutput, bufferSize,
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
            return output.TransformChunksAsync(transform, source.Length, encoding, token, disposeOutput, bufferSize,
                source.CopyTo);
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
        public static Task<ArraySegment<byte>> TransformAsSegmentAsync(this Stream input,
            ICryptoTransform transform, CancellationToken token = default(CancellationToken),
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.CreateCryptoStream(transform, CryptoStreamMode.Read, disposeInput)
                .CopyToSegmentWithDisposeAsync(bufferSize, token);
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
        public static Task TransformAsync(this Stream input, ICryptoTransform transform,
            StringBuilder target, CancellationToken token = default(CancellationToken), bool disposeInput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.CreateCryptoStream(transform, CryptoStreamMode.Read, disposeInput)
                .CopyToBuilderAsync(target, token, encoding ?? Encoding.UTF8, bufferSize, true);
        }

        private static Task TransformChunksAsync(this Stream writable, ICryptoTransform transform,
            int length, Encoding enc, CancellationToken token, bool disposeOutput, int chunkSize,
            Action<int, char[], int, int> copyToAction)
        {
            return writable.CreateCryptoStream(transform, CryptoStreamMode.Write, disposeOutput)
                .CopyFromWithDisposeAsync(length, enc ?? Encoding.UTF8, token, chunkSize, copyToAction, writable);
        }

        private static Task TransformAsync(this byte[] input, ICryptoTransform transform,
            Stream targetStream, CancellationToken token, bool disposeOutput,
            int byteOffset, int byteCount)
        {
            return targetStream.CreateCryptoStream(transform, CryptoStreamMode.Write, disposeOutput)
                .CopyFromWithDisposeAsync(input, byteOffset, byteCount, token, targetStream);
        }
    }
}