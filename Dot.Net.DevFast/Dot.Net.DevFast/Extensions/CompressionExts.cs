//using System.IO;
//using System.IO.Compression;
//using System.Threading;
//using System.Threading.Tasks;
//using Dot.Net.DevFast.Etc;
//using Dot.Net.DevFast.Extensions.Internals;

//namespace Dot.Net.DevFast.Extensions
//{
//    /// <summary>
//    /// Data Compression related extensions.
//    /// </summary>
//    public static class CompressionExts
//    {
//        /// <summary>
//        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
//        /// to <paramref name="target"/> with given <paramref name="level"/> while observing 
//        /// <paramref name="token"/> asynchronously.
//        /// </summary>
//        /// <param name="source">Source data stream to read data from</param>
//        /// <param name="target">Target data stream to write compressed data to</param>
//        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
//        /// is created.</param>
//        /// <param name="token">Cancellation token to observe</param>
//        /// <param name="level">Compression level</param>
//        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
//        public static async Task CompressAsync(this byte[] source, Stream target, bool gzip = true,
//            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
//            bool disposeTarget = true)
//        {
//            using (var compressor = target.CreateCompressionStream(gzip, level, disposeTarget))
//            {
//                await compressor.WriteAsync(source, 0, source.Length, token).ConfigureAwait(false);
//            }
//        }

//        /// <summary>
//        /// Using compressor (GZip/Deflate), compresses <paramref name="source"/> data and writes 
//        /// to <paramref name="target"/> with given <paramref name="level"/> and <paramref name="bufferSize"/>
//        /// while observing <paramref name="token"/> asynchronously.
//        /// </summary>
//        /// <param name="source">Source data stream to read data from</param>
//        /// <param name="target">Target data stream to write compressed data to</param>
//        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
//        /// is created.</param>
//        /// <param name="token">Cancellation token to observe</param>
//        /// <param name="level">Compression level</param>
//        /// <param name="bufferSize">Buffer size</param>
//        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the operation.</param>
//        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
//        public static Task CompressAsync(this Stream source, Stream target, bool gzip = true,
//            CancellationToken token = default(CancellationToken), CompressionLevel level = CompressionLevel.Optimal,
//            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = true, bool disposeTarget = true)
//        {
//            return target.CreateCompressionStream(gzip, level, disposeTarget).
//                CopyFromWithDisposeAsync(source, bufferSize, token, target, disposeSource);
//        }

//        //private static async Task CompressAsync(this byte[] source, Stream target, int byteOffset, 
//        //    int byteCount, CancellationToken token, bool disposeTarget = true)
//        //{
//        //    using (var compressor = target.CreateCompressionStream(gzip, level, disposeTarget))
//        //    {
//        //        await compressor.WriteAsync(source, 0, source.Length, token).ConfigureAwait(false);
//        //    }
//        //}
//    }
//}