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
        public static StreamPipe SerializeAsJson<T>(this T obj, 
            JsonSerializer serializer = null,
            Encoding enc = null, 
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new StreamPipe((s, d, t) => obj.ToJson(s, serializer, enc, writerBuffer, d));
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
        public static StreamPipe SerializeAsJson<T>(this IEnumerable<T> obj, 
            JsonSerializer serializer = null,
            Encoding enc = null, 
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new StreamPipe((s, d, t) => obj.ToJsonArray(s, serializer, t, enc, writerBuffer, d));
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
        public static StreamPipe SerializeAsJson<T>(this BlockingCollection<T> obj, 
            JsonSerializer serializer = null,
            Encoding enc = null, 
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationTokenSource pcts = default (CancellationTokenSource))
        {
            return new StreamPipe((s, d, t) => obj.ToJsonArrayParallely(s, serializer, t, pcts, enc, writerBuffer, d));
        }

        #endregion SerializeAsJson

        /// <summary>
        /// Compresses the data of given Stream pipe as source.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        /// <param name="level">Compression level to use.</param>
        public static StreamPipe ThenCompress(this StreamPipe src, 
            bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return src.Mutate(pipe => pipe.AddCompression(true, level));
        }

        /// <summary>
        /// Computes the hash of the data of the given stream pipe as source.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed (after awaiting on
        /// <seealso cref="StreamPipe.StreamAsync"/>, one of SaveAsFileAsync methods etc.).
        /// Thus, calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        public static StreamPipe AndComputeHash(this StreamPipe src, HashAlgorithm ha)
        {
            return src.Mutate(pipe => pipe.ComputeHash(ha));
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
        public static async Task<FileInfo> SaveAsFileAsync(this StreamPipe src, 
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
        public static async Task<FileInfo> SaveAsFileAsync(this StreamPipe src, 
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
        public static async Task SaveAsFileAsync(this StreamPipe src, 
            FileInfo fileinfo, 
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, 
            CancellationToken token = default(CancellationToken))
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await src.StreamAsync(strm, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }
    
        #endregion SaveAsFileAsync
    }
}