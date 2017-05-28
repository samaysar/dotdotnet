using System;
using System.IO;
using System.IO.Compression;
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
        public static string ToBase64(this string input, Base64FormattingOptions options = Base64FormattingOptions.None,
            Encoding encoding = null)
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

        /// <summary>
        /// Reads <paramref name="input"/> bytes and writes Base64 data on <paramref name="base64Stream"/>.
        /// <para>NOTE: <paramref name="base64Stream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this byte[] input, Stream base64Stream, 
            bool disposeOutput = false)
        {
            return input.ToBase64Async(base64Stream, CancellationToken.None, disposeOutput);
        }

        /// <summary>
        /// Reads <paramref name="input"/> bytes and writes Base64 data on <paramref name="base64Stream"/>
        /// while observing <paramref name="token"/> for cancellation.
        /// <para>NOTE: <paramref name="base64Stream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this byte[] input, Stream base64Stream, CancellationToken token,
            bool disposeOutput = false)
        {
            return input.TransformAsync(new ToBase64Transform(), base64Stream, token, disposeOutput);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="base64Stream"/>, 
        /// using <paramref name="encoding"/>.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static Task ToBase64Async(this string input, Stream base64Stream, bool disposeOutput = false, 
            int bufferSize = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        {
            return input.ToBase64Async(base64Stream, CancellationToken.None, disposeOutput, bufferSize,
                encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="base64Stream"/>, 
        /// using <paramref name="encoding"/>, while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static Task ToBase64Async(this string input, Stream base64Stream, CancellationToken token,
            bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        {
            return input.TransformAsync(new ToBase64Transform(), base64Stream, token, disposeOutput, bufferSize,
                encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="base64Stream"/>, 
        /// using <paramref name="encoding"/>.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static Task ToBase64Async(this StringBuilder input, Stream base64Stream,
            bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize, Encoding encoding = null)
        {
            return input.ToBase64Async(base64Stream, CancellationToken.None, disposeOutput, bufferSize,
                encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="base64Stream"/>, 
        /// using <paramref name="encoding"/>, while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size for character reading</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static Task ToBase64Async(this StringBuilder input, Stream base64Stream,
            CancellationToken token, bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize, 
            Encoding encoding = null)
        {
            return input.TransformAsync(new ToBase64Transform(), base64Stream, token, disposeOutput, bufferSize,
                encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Reads from <paramref name="inputStream"/> and writes Base64 data on <paramref name="base64Stream"/>.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="disposeInput">If true, disposes <paramref name="inputStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="copyBufferSize">Buffer size for stream copy operation</param>
        public static Task ToBase64Async(this Stream inputStream, Stream base64Stream,
            bool disposeInput = false, bool disposeOutput = false, int copyBufferSize = StdLookUps.DefaultBufferSize)
        {
            return inputStream.ToBase64Async(base64Stream, CancellationToken.None, disposeInput, disposeOutput,
                copyBufferSize);
        }

        /// <summary>
        /// Reads from <paramref name="inputStream"/> and writes Base64 data on <paramref name="base64Stream"/>,
        /// while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="base64Stream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="inputStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="base64Stream"/> upon operation completion, else leaves it open</param>
        /// <param name="copyBufferSize">Buffer size for stream copy operation</param>
        public static Task ToBase64Async(this Stream inputStream, Stream base64Stream,
            CancellationToken token, bool disposeInput = false, bool disposeOutput = false,
            int copyBufferSize = StdLookUps.DefaultBufferSize)
        {
            return inputStream.TransformAsync(new ToBase64Transform(), base64Stream, token, disposeInput,
                disposeOutput, copyBufferSize);
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
            await inputBase64Stream.FromBase64Async(outputStream, CancellationToken.None, mode, copyBuffer)
                .ConfigureAwait(false);
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
        public static async Task FromBase64Async(this Stream inputBase64Stream, Stream outputStream,
            CancellationToken token, FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            int copyBuffer = StdLookUps.DefaultBufferSize)
        {
            using (
                var base64Writer = new CryptoStream(outputStream, new FromBase64Transform(mode), CryptoStreamMode.Write)
            )
            {
                await inputBase64Stream.CopyToAsync(base64Writer, copyBuffer, token).ConfigureAwait(false);
                await base64Writer.FlushAsync(token).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }
    }
}