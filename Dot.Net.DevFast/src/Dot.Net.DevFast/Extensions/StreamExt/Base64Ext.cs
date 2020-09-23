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
        #region BUFFER based extensions, useful for small strings

        /// <summary>
        /// Converts the input string to Base64 string.
        /// <para>Ecoding's Preamable is NOT injected</para>
        /// </summary>
        /// <param name="input">UTF8 string</param>
        /// <param name="options">Options to use for the transformation</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        public static string ToBase64(this string input, Base64FormattingOptions options = Base64FormattingOptions.None,
            Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(input).ToBase64(options);
        }

        /// <summary>
        /// Converts the whole <paramref name="input"/> array to Base64 string.
        /// <para>Ecoding's Preamable is NOT injected</para>
        /// <para>Refer to <see cref="ToBase64(ArraySegment{byte},Base64FormattingOptions)"/> to perform
        /// conversion on a segment of the array</para>
        /// </summary>
        /// <param name="input">Input byte array</param>
        /// <param name="options">Options to use for the transformation</param>
        public static string ToBase64(this byte[] input, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return input.ToBase64(0, input.Length, options);
        }

        /// <summary>
        /// Converts the segment of the <paramref name="input"/> to Base64 string.
        /// <para>Ecoding's Preamable is NOT injected</para>
        /// <para>Refer to <see cref="ToBase64(byte[],Base64FormattingOptions)"/> to perform
        /// conversion on full array</para>
        /// </summary>
        /// <param name="input">Input byte array</param>
        /// <param name="options">Options to use for the transformation</param>
        /// <exception cref="NullReferenceException">if <paramref name="input"/> is null</exception>
        public static string ToBase64(this ArraySegment<byte> input,
            Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return input.Array.ToBase64(input.Offset, input.Count, options);
        }

        private static string ToBase64(this byte[] input, int offset, int length,
            Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return Convert.ToBase64String(input, offset, length, options);
        }

        /// <summary>
        /// Converts the <paramref name="base64"/> to string for given encoding.
        /// </summary>
        /// <param name="base64">Base64 string</param>
        /// <param name="encoding">Encoding to use during byte to string transformations.
        /// <para>If null is supplied then UTF-8 encoding is used as default with the possibility
        /// to detect encoding from byte order mark.</para></param>
        public static string FromBase64(this string base64, Encoding encoding)
        {
            if (encoding != null) return encoding.GetString(base64.FromBase64());
            var memStrm = new MemoryStream(base64.FromBase64());
            using var reader = new StreamReader(memStrm, Encoding.UTF8, true, StdLookUps.DefaultBufferSize, false);
            return reader.ReadToEnd();
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

        #endregion BUFFER based extensions, useful for small strings

        #region Stream based extensions, useful for larger strings (though some string returning method would consume memory)

        /// <summary>
        /// Reads <paramref name="input"/> bytes and writes Base64 data on <paramref name="targetStream"/>.
        /// <para>NOTE: <paramref name="targetStream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this byte[] input, Stream targetStream,
            bool disposeOutput = false)
        {
            return input.ToBase64Async(targetStream, CancellationToken.None, disposeOutput);
        }

        /// <summary>
        /// Reads <paramref name="input"/> bytes and writes Base64 data on <paramref name="targetStream"/>
        /// while observing <paramref name="token"/> for cancellation.
        /// <para>NOTE: <paramref name="targetStream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this byte[] input, Stream targetStream, CancellationToken token,
            bool disposeOutput = false)
        {
            return input.TransformAsync(new ToBase64Transform(), targetStream, token, disposeOutput);
        }

        /// <summary>
        /// Reads <paramref name="input"/> byte segment and writes Base64 data on <paramref name="targetStream"/>.
        /// <para>NOTE: <paramref name="targetStream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this ArraySegment<byte> input, Stream targetStream,
            bool disposeOutput = false)
        {
            return input.ToBase64Async(targetStream, CancellationToken.None, disposeOutput);
        }

        /// <summary>
        /// Reads <paramref name="input"/> byte segment and writes Base64 data on <paramref name="targetStream"/>
        /// while observing <paramref name="token"/> for cancellation.
        /// <para>NOTE: <paramref name="targetStream"/> is NEITHER closed NOR disposed inside this operation.
        /// Only Flush is performed.</para>
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        public static Task ToBase64Async(this ArraySegment<byte> input, Stream targetStream, CancellationToken token,
            bool disposeOutput = false)
        {
            return input.TransformAsync(new ToBase64Transform(), targetStream, token, disposeOutput);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="targetStream"/>, 
        /// using <paramref name="encoding"/>.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this string input, Stream targetStream, bool disposeOutput = false,
            Encoding encoding = null, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.ToBase64Async(targetStream, CancellationToken.None, disposeOutput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="targetStream"/>, 
        /// using <paramref name="encoding"/>, while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this string input, Stream targetStream, CancellationToken token,
            bool disposeOutput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.TransformAsync(new ToBase64Transform(), targetStream, token, disposeOutput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="targetStream"/>, 
        /// using <paramref name="encoding"/>.
        /// </summary>
        /// <param name="input">StringBuilder whose String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this StringBuilder input, Stream targetStream,
            bool disposeOutput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.ToBase64Async(targetStream, CancellationToken.None, disposeOutput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads characters from <paramref name="input"/> and writes Base64 data on <paramref name="targetStream"/>, 
        /// using <paramref name="encoding"/>, while observing <paramref name="token"/> for cancellation.
        /// </summary>
        /// <param name="input">StringBuilder whose String to convert</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this StringBuilder input, Stream targetStream,
            CancellationToken token, bool disposeOutput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return input.TransformAsync(new ToBase64Transform(), targetStream, token, disposeOutput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and writes Base64 data on <paramref name="targetStream"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this Stream sourceStream, Stream targetStream,
            bool disposeInput = false, bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.ToBase64Async(targetStream, CancellationToken.None, disposeInput, disposeOutput,
                bufferSize);
        }

        /// <summary>
        /// Reads from <paramref name="sourceStream"/> and writes Base64 data on <paramref name="targetStream"/>,
        /// while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="targetStream">Stream to write base64 data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task ToBase64Async(this Stream sourceStream, Stream targetStream,
            CancellationToken token, bool disposeInput = false, bool disposeOutput = false,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsync(new ToBase64Transform(), targetStream, token, disposeInput,
                disposeOutput, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and writes decoded data on <paramref name="targetStream"/>,
        /// with <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="targetStream">Stream to write decoded data to</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task FromBase64Async(this Stream sourceStream, Stream targetStream,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.FromBase64Async(targetStream, CancellationToken.None, mode, disposeInput,
                disposeOutput, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and writes unencoded data on <paramref name="targetStream"/>,
        /// with <paramref name="bufferSize"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="targetStream">Stream to write unencoded data to.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="disposeOutput">If true, disposes <paramref name="targetStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task FromBase64Async(this Stream sourceStream, Stream targetStream,
            CancellationToken token, FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, bool disposeOutput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsync(new FromBase64Transform(mode), targetStream, token, disposeInput,
                disposeOutput, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and appends decoded string to <paramref name="appendTo"/>,
        /// with <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="appendTo">Stream to write decoded data to</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task FromBase64Async(this Stream sourceStream, StringBuilder appendTo,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.FromBase64Async(appendTo, CancellationToken.None, mode, disposeInput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and appends decoded string to <paramref name="appendTo"/>,
        /// with <paramref name="bufferSize"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="appendTo">Stream to write decoded data to</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task FromBase64Async(this Stream sourceStream, StringBuilder appendTo, CancellationToken token,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsync(new FromBase64Transform(mode), appendTo, token, disposeInput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded string (return value),
        /// with <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied UTF8 is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<string> FromBase64AsStringAsync(this Stream sourceStream,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.FromBase64AsStringAsync(CancellationToken.None, mode, disposeInput,
                encoding ?? Encoding.UTF8, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded string (return value),
        /// with <paramref name="bufferSize"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="encoding">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<string> FromBase64AsStringAsync(this Stream sourceStream, CancellationToken token,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, Encoding encoding = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsStringAsync(new FromBase64Transform(mode), encoding, token, disposeInput,
                bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded byte array (return value),
        /// with <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<byte[]> FromBase64Async(this Stream sourceStream,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.FromBase64Async(CancellationToken.None, mode, disposeInput, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded byte array (return value),
        /// with <paramref name="bufferSize"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<byte[]> FromBase64Async(this Stream sourceStream, CancellationToken token,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsync(new FromBase64Transform(mode), token, disposeInput,
                bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded byte array, (return as segment, 
        /// idea is to save on array copy to remain low on latency n memory as perhaps segment can serve the purpose),
        /// with <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<ArraySegment<byte>> FromBase64AsSegmentAsync(this Stream sourceStream,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.FromBase64AsSegmentAsync(CancellationToken.None, mode, disposeInput, bufferSize);
        }

        /// <summary>
        /// Reads Base64 data from <paramref name="sourceStream"/> and prepares decoded byte array, (return as segment, 
        /// idea is to save on array copy to remain low on latency n memory as perhaps segment can serve the purpose),
        /// with <paramref name="bufferSize"/>, while observing <paramref name="token"/>.
        /// </summary>
        /// <param name="sourceStream">Stream to read Base64 data from</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="mode">transformation mode to use</param>
        /// <param name="disposeInput">If true, disposes <paramref name="sourceStream"/> upon operation completion, else leaves it open</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Task<ArraySegment<byte>> FromBase64AsSegmentAsync(this Stream sourceStream,
            CancellationToken token, FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool disposeInput = false, int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return sourceStream.TransformAsSegmentAsync(new FromBase64Transform(mode), token, disposeInput,
                bufferSize);
        }

        #endregion Stream based extensions, useful for larger strings (though some string returning method would consume memory)
    }
}