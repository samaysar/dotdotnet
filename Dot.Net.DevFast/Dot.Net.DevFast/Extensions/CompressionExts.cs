using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Data Compression related extensions.
    /// </summary>
    public static class CompressionExts
    {
        /// <summary>
        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="level"/> while observing 
        /// <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source byte array segment to read data from</param>
        /// <param name="target">Target data stream to write compressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="level">Compression level</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task CompressAsync(this ArraySegment<byte> source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
            bool disposeTarget = false)
        {
            return source.Array.CompressAsync(target, gzip, level, token, disposeTarget, source.Offset, source.Count);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="level"/> while observing 
        /// <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source byte array to read data from</param>
        /// <param name="target">Target data stream to write compressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="level">Compression level</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task CompressAsync(this byte[] source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
            bool disposeTarget = false)
        {
            return source.CompressAsync(target, gzip, level, token, disposeTarget, 0, source.Length);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="level"/> and <paramref name="bufferSize"/>
        /// while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read data from</param>
        /// <param name="target">Target data stream to write compressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="level">Compression level</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task CompressAsync(this Stream source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = false, bool disposeTarget = false)
        {
            return target.CreateCompressionStream(gzip, level, disposeTarget).
                CopyFromWithDisposeAsync(source, bufferSize, token, target, disposeSource);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="enc"/>, <paramref name="level"/> 
        /// and <paramref name="bufferSize"/> while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source string builder to read data from</param>
        /// <param name="target">Target data stream to write compressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="level">Compression level</param>
        /// <param name="enc">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task CompressAsync(this StringBuilder source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeTarget = false)
        {
            return target.CompressAsync(gzip, level, source.Length, enc, token, disposeTarget, bufferSize,
                source.CopyTo);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="enc"/>, <paramref name="level"/> 
        /// and <paramref name="bufferSize"/> while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source string to read</param>
        /// <param name="target">Target data stream to write compressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="level">Compression level</param>
        /// <param name="enc">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task CompressAsync(this string source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeTarget = false)
        {
            return target.CompressAsync(gzip, level, source.Length, enc, token, disposeTarget, bufferSize,
                source.CopyTo);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), decompresses <paramref name="source"/> data with given 
        /// <paramref name="enc"/> and <paramref name="bufferSize"/> while observing <paramref name="token"/> 
        /// asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read compressed data from</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        public static async Task<string> DecompressAsStringAsync(this Stream source, bool gzip = true,
            CancellationToken token = default(CancellationToken), Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = false)
        {
            var strBuilder = new StringBuilder();
            await source.DecompressAsync(strBuilder, gzip, token, enc, bufferSize, disposeSource).ConfigureAwait(false);
            return strBuilder.ToString();
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), decompresses <paramref name="source"/> data with given 
        /// <paramref name="enc"/> and <paramref name="bufferSize"/> while observing <paramref name="token"/> 
        /// asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read compressed data from</param>
        /// <param name="target"></param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use to get string bytes, if not supplied <seealso cref="Encoding.UTF8"/> is used</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        public static Task DecompressAsync(this Stream source, StringBuilder target, bool gzip = true,
            CancellationToken token = default(CancellationToken), Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = false)
        {
            return source.CreateDecompressionStream(gzip, disposeSource)
                .CopyToBuilderAsync(target, token, enc ?? Encoding.UTF8, bufferSize, true);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), decompresses <paramref name="source"/> data and writes 
        /// to <paramref name="target"/> with given <paramref name="bufferSize"/>
        /// while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read compressed data from</param>
        /// <param name="target">Target data stream to write decompressed data to</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static Task DecompressAsync(this Stream source, Stream target, bool gzip = true,
            CancellationToken token = default(CancellationToken), int bufferSize = StdLookUps.DefaultBufferSize, 
            bool disposeSource = false, bool disposeTarget = false)
        {
            return target.CreateDecompressionStream(gzip, disposeTarget)
                .CopyFromWithDisposeAsync(source, bufferSize, token, target, disposeSource);
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), decompresses <paramref name="source"/> data with given
        /// <paramref name="bufferSize"/> while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read compressed data from</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        public static async Task<byte[]> DecompressAsync(this Stream source, bool gzip = true,
            CancellationToken token = default(CancellationToken), int bufferSize = StdLookUps.DefaultBufferSize, 
            bool disposeSource = false)
        {
            return (await source.DecompressAsSegmentAsync(gzip, token, bufferSize, disposeSource)
                .ConfigureAwait(false)).CreateBytes();
        }

        /// <summary>
        /// Using compressor (GZip/Deflate), decompresses <paramref name="source"/> data with given
        /// <paramref name="bufferSize"/> while observing <paramref name="token"/> asynchronously.
        /// </summary>
        /// <param name="source">Source data stream to read compressed data from</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
        public static Task<ArraySegment<byte>> DecompressAsSegmentAsync(this Stream source, bool gzip = true,
            CancellationToken token = default(CancellationToken), int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeSource = false)
        {
            return source.CreateDecompressionStream(gzip, disposeSource)
                .CopyToSegmentWithDisposeAsync(bufferSize, token);
        }

        private static Task CompressAsync(this Stream target, bool gzip, CompressionLevel level,
            int length, Encoding enc, CancellationToken token, bool disposeTarget, int chunkSize,
            Action<int, char[], int, int> copyToAction)
        {
            return target.CreateCompressionStream(gzip, level, disposeTarget)
                .CopyFromWithDisposeAsync(length, enc ?? Encoding.UTF8, token, chunkSize, copyToAction, target);
        }

        private static Task CompressAsync(this byte[] source, Stream target, bool gzip, CompressionLevel level, 
            CancellationToken token, bool disposeTarget, int byteOffset, int byteCount)
        {
            return target.CreateCompressionStream(gzip, level, disposeTarget)
                .CopyFromWithDisposeAsync(source, byteOffset, byteCount, token, target);
        }
    }
}