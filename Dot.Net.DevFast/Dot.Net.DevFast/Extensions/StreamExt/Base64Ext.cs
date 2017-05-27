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
    /// Class that provides extensions to perform Base64 string operations
    /// </summary>
    public static class Base64Ext
    {
        /// <summary>
        /// Converts the input string to Base64 string.
        /// <para>Expect all base64 transformation related exceptions</para>
        /// </summary>
        /// <param name="input">UTF8 string</param>
        /// <param name="options">Options to use for the transformation</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is null</exception>
        public static string ToBase64(this string input, Base64FormattingOptions options = Base64FormattingOptions.None, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(input).ToBase64(options);
        }

        /// <summary>
        /// Converts the <paramref name="input"/> to Base64 string.
        /// <para>Expect all base64 transformation related exceptions</para>
        /// </summary>
        /// <param name="input">Input byte array</param>
        /// <param name="options">Options to use for the transformation</param>
        /// <param name="offset">Starting position in array</param>
        /// <param name="length">length to convert, if not supplied then length is <seealso cref="Array.Length"/> minus <paramref name="offset"/></param>
        /// <exception cref="NullReferenceException">if <paramref name="input"/> is null</exception>
        public static string ToBase64(this byte[] input, Base64FormattingOptions options = Base64FormattingOptions.None,
            int offset = 0, int length = -1)
        {
            var adjustedLength = length == -1 ? input.Length - offset : length;
            return Convert.ToBase64String(input, offset, adjustedLength, options);
        }

        ///// <summary>
        ///// Reads <paramref name="input"/> and writes Base64 data on <paramref name="outputBase64Stream"/>,
        ///// using <paramref name="copyBuffer"/> as bufferSize and <paramref name="encoding"/> as encoding (if given).
        ///// </summary>
        ///// <param name="input">String to convert</param>
        ///// <param name="outputBase64Stream">Stream to write base64 data to.</param>
        ///// <param name="copyBuffer">buffer size to use.</param>
        ///// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        ///// <exception cref="ArgumentException"></exception>
        //public static async Task ToBase64Async(this string input, Stream outputBase64Stream,
        //    int copyBuffer = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        //{
        //    using (var strReader = new StringReader(input))
        //    {
                
        //    }
        //}

        /// <summary>
        /// Reads from <paramref name="inputStream"/> and writes Base64 data on <paramref name="outputBase64Stream"/>,
        /// using <paramref name="copyBuffer"/> as bufferSize.
        /// <para>NOTE: <paramref name="inputStream"/> is NOT disposed inside this operation.</para>
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="outputBase64Stream">Stream to write base64 data to.</param>
        /// <param name="copyBuffer">buffer size to use during stream copy.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task ToBase64Async(this Stream inputStream, Stream outputBase64Stream,
            int copyBuffer = StdLookUps.DefaultBufferSize)
        {
            await inputStream.ToBase64Async(outputBase64Stream, CancellationToken.None, copyBuffer).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads from <paramref name="inputStream"/> and writes Base64 data on <paramref name="outputBase64Stream"/>,
        /// using <paramref name="copyBuffer"/> as bufferSize, while observing <paramref name="token"/>.
        /// <para>NOTE: <paramref name="inputStream"/> is NOT disposed inside this operation.</para>
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="outputBase64Stream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="copyBuffer">buffer size to use during stream copy.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task ToBase64Async(this Stream inputStream, Stream outputBase64Stream, CancellationToken token,
            int copyBuffer = StdLookUps.DefaultBufferSize)
        {
            using (var base64Writer = new CryptoStream(outputBase64Stream, new ToBase64Transform(), CryptoStreamMode.Write))
            {
                await inputStream.CopyToAsync(base64Writer, copyBuffer, token).ConfigureAwait(false);
                await base64Writer.FlushAsync(token).ConfigureAwait(false);
                await outputBase64Stream.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Converts the <paramref name="base64"/> to unencoded byte array.
        /// <para>Expect all base64 transformation related exceptions</para>
        /// </summary>
        /// <param name="base64">Base64 string</param>
        /// <param name="encoding">Encoding to use during byte to string transformations.
        /// <para>If null is supplied then encoding is detected from byte order mark.</para></param>
        public static string FromBase64(this string base64, Encoding encoding)
        {
            if (encoding != null)
            {
                return encoding.GetString(base64.FromBase64());
            }
            using (var memStrm = new MemoryStream(base64.FromBase64()))
            {
                using (var reader = new StreamReader(memStrm, true))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Converts the <paramref name="base64"/> to unencoded byte array.
        /// <para>Expect all base64 transformation related exceptions</para>
        /// </summary>
        /// <param name="base64">Base64 string</param>
        public static byte[] FromBase64(this string base64)
        {
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="inputBase64Stream"/> and writes unencoded data on <paramref name="outputStream"/>,
        /// using <paramref name="copyBuffer"/> as bufferSize.
        /// <para>NOTE: <paramref name="inputBase64Stream"/> is NOT disposed inside this operation.</para>
        /// </summary>
        /// <param name="inputBase64Stream">Stream to read Base64 data from</param>
        /// <param name="outputStream">Stream to write unencoded data to.</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="copyBuffer">buffer size to use during stream copy.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task FromBase64Async(this Stream inputBase64Stream, Stream outputStream, 
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            int copyBuffer = StdLookUps.DefaultBufferSize)
        {
            await inputBase64Stream.FromBase64Async(outputStream, CancellationToken.None, mode, copyBuffer).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="inputBase64Stream"/> and writes unencoded data on <paramref name="outputStream"/>,
        /// using <paramref name="copyBuffer"/> as bufferSize, while observing <paramref name="token"/>.
        /// <para>NOTE: <paramref name="inputBase64Stream"/> is NOT disposed inside this operation.</para>
        /// </summary>
        /// <param name="inputBase64Stream">Stream to read Base64 data from</param>
        /// <param name="outputStream">Stream to write unencoded data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="copyBuffer">buffer size to use during stream copy.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task FromBase64Async(this Stream inputBase64Stream, Stream outputStream, CancellationToken token,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            int copyBuffer = StdLookUps.DefaultBufferSize)
        {
            using (var base64Writer = new CryptoStream(outputStream, new FromBase64Transform(mode), CryptoStreamMode.Write))
            {
                await inputBase64Stream.CopyToAsync(base64Writer, copyBuffer, token).ConfigureAwait(false);
                await base64Writer.FlushAsync(token).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }
    }
}