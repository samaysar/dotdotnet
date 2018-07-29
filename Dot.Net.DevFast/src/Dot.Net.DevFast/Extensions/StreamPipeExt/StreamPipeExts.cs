using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Extensions methods on stream pipes.
    /// </summary>
    public static partial class StreamPipeExts
    {
        #region SerializeAsJson

        /// <summary>
        /// Creates the equivalent json representation of the object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static Func<Stream, bool, CancellationToken, Task> SerializeAsJson<T>(this T obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJson(s, serializer, enc, writerBuffer, d)).ToAsync();
        }

        /// <summary>
        /// Creates the equivalent json array representation using the enumeration.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file.
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static Func<Stream, bool, CancellationToken, Task> SerializeAsJson<T>(this IEnumerable<T> obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJsonArray(s, serializer, t, enc, writerBuffer, d)).ToAsync();
        }

        /// <summary>
        /// Creates the equivalent json array representation of the objects in the given blocking collection.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="pcts">source to cancel in case some error is encountered. Normally,
        /// this source token is observed at data producer side.</param>
        public static Func<Stream, bool, CancellationToken, Task> SerializeAsJson<T>(this BlockingCollection<T> obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationTokenSource pcts = default(CancellationTokenSource))
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJsonArrayParallely(s, serializer, t, pcts, enc, writerBuffer, d)).ToAsync();
        }

        #endregion SerializeAsJson

        /// <summary>
        /// Compresses the data of given Stream pipe as source.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        /// <param name="level">Compression level to use.</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenCompress(
            this Func<Stream, bool, CancellationToken, Task> src,
            bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return src.Mutate(pipe => pipe.AddCompression(true, level));
        }

        /// <summary>
        /// Computes the hash of the data of the given stream pipe as source.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed (i.e. awaited on methods that returns <seealso cref="Task"/>).
        /// Thus, calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenComputeHash(
            this Func<Stream, bool, CancellationToken, Task> src, HashAlgorithm ha)
        {
            return src.ThenCryptoTransform(ha);
        }

        /// <summary>
        /// Converts the data, of the given stream pipe as source, to equivalent Base64.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenToBase64(
            this Func<Stream, bool, CancellationToken, Task> src)
        {
            return src.ThenCryptoTransform(new ToBase64Transform());
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given stream pipe as source.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenCryptoTransform(
            this Func<Stream, bool, CancellationToken, Task> src,
            ICryptoTransform transformation)
        {
            return src.Mutate(pipe => pipe.ApplyTransform(transformation));
        }

        #region SaveAsFileAsync

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// <para>An appropriate extension will be added to <paramref name="filename" /></para>
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> SaveAsFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous,
            CancellationToken token = default(CancellationToken))
        {
            return await src.SaveAsFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// <para>An appropriate extension will be added to <paramref name="filename" /></para>
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> SaveAsFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous,
            CancellationToken token = default(CancellationToken))
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"));
            await src.SaveAsFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
            return targetFile;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task SaveAsFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous,
            CancellationToken token = default(CancellationToken))
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await src(strm, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }

        #endregion SaveAsFileAsync

        #region Finalization

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// returns the contents as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<byte[]> ToBytesAsync(this Func<Stream, bool, CancellationToken, Task> src,
            CancellationToken token = default(CancellationToken))
        {
            return (await src.ToBufferAsync(token).ConfigureAwait(false)).ToArray();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        public static async Task<MemoryStream> ToBufferAsync(this Func<Stream, bool, CancellationToken, Task> src,
            CancellationToken token = default(CancellationToken), bool seekToOrigin = false)
        {
            var ms = new MemoryStream(StdLookUps.DefaultBufferSize);
            await src.AppendAsync(ms, false, token).ConfigureAwait(false);
            if (seekToOrigin) ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="target">Target stream</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AppendAsync(this Func<Stream, bool, CancellationToken, Task> src,
            Stream target,
            bool disposeTarget,
            CancellationToken token = default(CancellationToken))
        {
            await src(target, disposeTarget, token).ConfigureAwait(false);
        }

        #endregion String Finalization

        internal static Func<Stream, bool, CancellationToken, Task> Mutate(
            this Func<Stream, bool, CancellationToken, Task> mutable,
            Func<Func<Stream, bool, CancellationToken, Task>,
                Func<Stream, bool, CancellationToken, Task>> mutation)
        {
            return mutation(mutable);
        }
    }
}