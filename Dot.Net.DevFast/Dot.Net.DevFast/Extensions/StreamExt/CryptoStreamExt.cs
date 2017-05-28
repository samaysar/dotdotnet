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
        /// Reads from <paramref name="inputStream"/> and writes transformed data on <paramref name="outputStream"/>,
        /// using <paramref name="transform"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="transform">transform to use</param>
        /// <param name="outputStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="inputStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="outputStream"/> upon operation completion, else leaves it open</param>
        /// <param name="copyBufferSize">Buffer size for stream copy operation</param>
        public static async Task TransformAsync(this Stream inputStream, ICryptoTransform transform,
            Stream outputStream, CancellationToken token, bool disposeInput = true,
            bool disposeOutput = true, int copyBufferSize = StdLookUps.DefaultBufferSize)
        {
            using (var outputWrapper = new WrappedStream(outputStream, disposeOutput))
            {
                using (var base64Writer = new CryptoStream(outputWrapper, transform, CryptoStreamMode.Write))
                {
                    using (var inputWrapper = new WrappedStream(inputStream, disposeInput))
                    {
                        await inputWrapper.CopyToAsync(base64Writer, copyBufferSize, token).ConfigureAwait(false);
                        await base64Writer.FlushAsync(token).ConfigureAwait(false);
                        await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes transformed data on <paramref name="outputStream"/>,
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing 
        /// <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="outputStream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="outputStream"/> upon operation completion, else leaves it open</param>
        /// <param name="charBufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static async Task TransformAsync(this StringBuilder input, ICryptoTransform transform,
            Stream outputStream, CancellationToken token, bool disposeOutput = true,
            int charBufferSize = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        {
            using (var outputWrapper = new WrappedStream(outputStream, disposeOutput))
            {
                using (var base64Writer = new CryptoStream(outputWrapper, transform,
                    CryptoStreamMode.Write))
                {
                    var charArr = new char[charBufferSize];
                    var enc = encoding ?? Encoding.UTF8;
                    var bytes = new byte[enc.GetMaxByteCount(charBufferSize)];
                    int pos = 0, posFrwd = charBufferSize, len = input.Length;
                    while (posFrwd <= len)
                    {
                        input.CopyTo(pos, charArr, 0, charBufferSize);
                        var byteSize = enc.GetBytes(charArr, 0, charBufferSize, bytes, 0);
                        await base64Writer.WriteAsync(bytes, 0, byteSize, token).ConfigureAwait(false);
                        pos = posFrwd;
                        posFrwd += charBufferSize;
                    }
                    if (pos < len)
                    {
                        input.CopyTo(pos, charArr, 0, (len - pos));
                        var byteSize = enc.GetBytes(charArr, 0, (len - pos), bytes, 0);
                        await base64Writer.WriteAsync(bytes, 0, byteSize, token).ConfigureAwait(false);
                    }
                    await base64Writer.FlushAsync(token).ConfigureAwait(false);
                    await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes transformed data on <paramref name="outputBase64Stream"/>, 
        /// using <paramref name="transform"/> and <paramref name="encoding"/> while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="transform">transform to use</param>
        /// <param name="outputBase64Stream">Stream to write transformed data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="outputBase64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="charBufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static async Task TransformAsync(this string input, ICryptoTransform transform,
            Stream outputBase64Stream, CancellationToken token, bool disposeOutput = true,
            int charBufferSize = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        {
            using (var strReader = new StringReader(input))
            {
                using (var outputWrapper = new WrappedStream(outputBase64Stream, disposeOutput))
                {
                    using (var base64Writer = new CryptoStream(outputWrapper, transform,
                        CryptoStreamMode.Write))
                    {
                        var charArr = new char[charBufferSize];
                        var enc = encoding ?? Encoding.UTF8;
                        var bytes = new byte[enc.GetMaxByteCount(charBufferSize)];
                        int charCnt;
                        while ((charCnt = strReader.Read(charArr, 0, charBufferSize)) != 0)
                        {
                            var byteSize = enc.GetBytes(charArr, 0, charCnt, bytes, 0);
                            await base64Writer.WriteAsync(bytes, 0, byteSize, token).ConfigureAwait(false);
                        }
                        await base64Writer.FlushAsync(token).ConfigureAwait(false);
                        await outputWrapper.FlushAsync(token).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}