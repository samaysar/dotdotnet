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
using Dot.Net.DevFast.Extensions.Internals;
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
        #region Various Load

        /// <summary>
        /// Loads the string content and returns a new pipe for functional stream chaining.
        /// <para>NOTE: If you already have a string builder, then use the overloaded method instead
        /// of doing yourStringBuilder.ToString().LoadString(...), as overloaded method is optimized.</para>
        /// </summary>
        /// <param name="stringTask">Task returning the source string. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadString(this Task<string> stringTask,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async (s, d, t) =>
            {
                await (await stringTask.StartIfNeeded().ConfigureAwait(false)).LoadString(enc, bufferSize)(s, d, t)
                    .ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Loads the string content and returns a new pipe for functional stream chaining.
        /// <para>NOTE: If you already have a string builder, then use the overloaded method instead
        /// of doing yourStringBuilder.ToString().LoadString(...), as overloaded method is optimized.</para>
        /// </summary>
        /// <param name="s">source string</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadString(this string s,
            Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return new Action<int, char[], int, int>(s.CopyTo).ApplyLoad(s.Length, enc ?? new UTF8Encoding(false),
                bufferSize);
        }

        /// <summary>
        /// Loads the string content of the builder and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="sbTask">Task returning source string builder. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadString(this Task<StringBuilder> sbTask,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async (s, d, t) =>
            {
                await (await sbTask.StartIfNeeded().ConfigureAwait(false)).LoadString(enc, bufferSize)(s, d, t)
                    .ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Loads the string content of the builder and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="sb">source string builder</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadString(this StringBuilder sb,
            Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return new Action<int, char[], int, int>(sb.CopyTo).ApplyLoad(sb.Length, enc ?? new UTF8Encoding(false),
                bufferSize);
        }

        /// <summary>
        /// Loads the data of the given file as byte stream and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="folder">Parent folder of the file</param>
        /// <param name="filename">File name with extension</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadFromFile(this DirectoryInfo folder,
            string filename,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return folder.CreateFileInfo(filename).LoadFromFile(fileStreamBuffer, options);
        }

        /// <summary>
        /// Loads the data of the given file as byte stream and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="fileinfo">Fileinfo instance of the file</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadFromFile(this FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return fileinfo.CreateStream(FileMode.Open, FileAccess.Read, FileShare.ReadWrite, fileStreamBuffer, options)
                .LoadBytes(fileStreamBuffer);
        }

        /// <summary>
        /// Loads bytes from given array and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">Task returning Source byte array. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this Task<byte[]> source)
        {
            return async (s, d, t) =>
            {
                await (await source.StartIfNeeded().ConfigureAwait(false)).LoadBytes()(s, d, t).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Loads bytes from given array and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">Source byte array</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this byte[] source)
        {
            return new ArraySegment<byte>(source, 0, source.Length).LoadBytes();
        }

        /// <summary>
        /// Loads bytes from given byte segment and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">task returning Source array segment. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this Task<ArraySegment<byte>> source)
        {
            return async (s, d, t) =>
            {
                await (await source.StartIfNeeded().ConfigureAwait(false)).LoadBytes()(s, d, t).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Loads bytes from given byte segment and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">Source byte array</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this ArraySegment<byte> source)
        {
            return async (s, d, t) =>
            {
                try
                {
                    await s.WriteAsync(source.Array, source.Offset, source.Count, t).ConfigureAwait(false);
                    await s.FlushAsync(t).ConfigureAwait(false);
                }
                finally
                {
                    s.DisposeIfRequired(d);
                }
            };
        }

        /// <summary>
        /// Loads bytes from given source stream and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">Task returning Source data stream. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="streamBuffer">Buffer size to use during data loading</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this Task<Stream> source,
            int streamBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return async (s, d, t) =>
            {
                await (await source.StartIfNeeded().ConfigureAwait(false)).LoadBytes(streamBuffer)(s, d, t)
                    .ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Loads bytes from given source stream and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="source">Source data stream</param>
        /// <param name="streamBuffer">Buffer size to use during data loading</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadBytes(this Stream source,
            int streamBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return async (s, d, t) =>
            {
                try
                {
                    await source.CopyToAsync(s, streamBuffer, t).ConfigureAwait(false);
                }
                finally
                {
                    s.DisposeIfRequired(d);
                }
            };
        }

        /// <summary>
        /// Creates the equivalent json representation of the object and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="objTask">Task returning Object to serialize as json text. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadJson<T>(this Task<T> objTask,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return async (s, d, t) =>
            {
                await (await objTask.StartIfNeeded().ConfigureAwait(false)).LoadJson(serializer, enc, writerBuffer)
                    (s, d, t).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Creates the equivalent json representation of the object and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadJson<T>(this T obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJson(s, serializer, enc, writerBuffer, d)).ToAsync(false);
        }

        /// <summary>
        /// Creates the equivalent json array representation using the enumeration and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file.
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static Func<Stream, bool, CancellationToken, Task> LoadJson<T>(this IEnumerable<T> obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJsonArray(s, serializer, t, enc, writerBuffer, d)).ToAsync(false);
        }

        /// <summary>
        /// Creates the equivalent json array representation of the objects in the given blocking collection
        /// and returns a new pipe for functional stream chaining.
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
        public static Func<Stream, bool, CancellationToken, Task> LoadJson<T>(this BlockingCollection<T> obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationTokenSource pcts = default(CancellationTokenSource))
        {
            return new Action<Stream, bool, CancellationToken>((s, d, t) =>
                obj.ToJsonArrayParallely(s, serializer, t, pcts, enc, writerBuffer, d)).ToAsync(false);
        }

        #endregion Various Load

        #region Then Clauses

        /// <summary>
        /// Compresses the data of given Stream pipe as source and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        /// <param name="level">Compression level to use.</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenCompress(
            this Func<Stream, bool, CancellationToken, Task> src,
            bool include = true,
            bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return include ? src.ApplyCompression(true, level) : src;
        }

        /// <summary>
        /// Computes the hash of the data of the given stream pipe as source and returns a new pipe for functional stream chaining.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed (i.e. awaited on methods that returns <seealso cref="Task"/>).
        /// Thus, calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        /// <param name="include">If true is passed, hash is computed else ignored</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenComputeHash(
            this Func<Stream, bool, CancellationToken, Task> src, 
            HashAlgorithm ha,
            bool include = true)
        {
            return include ? src.ThenApplyTransform(ha) : src;
        }

        /// <summary>
        /// Converts the data, of the given stream pipe as source, to equivalent Base64
        /// and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, ToBase64 conversion is performed else ignored</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenToBase64(
            this Func<Stream, bool, CancellationToken, Task> src,
            bool include = true)
        {
            return include ? src.ThenApplyTransform(new ToBase64Transform()) : src;
        }

        /// <summary>
        /// Decodes the Base64 data, of the given stream pipe as source, 
        /// and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="mode">Base64 transform mode</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenFromBase64(
            this Func<Stream, bool, CancellationToken, Task> src,
            bool include = true,
            FromBase64TransformMode mode = FromBase64TransformMode.DoNotIgnoreWhiteSpaces)
        {
            return include ? src.ThenApplyTransform(new FromBase64Transform(mode)) : src;
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given stream pipe as source
        /// and returns a new pipe for functional stream chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<Stream, bool, CancellationToken, Task> ThenApplyTransform(
            this Func<Stream, bool, CancellationToken, Task> src,
            ICryptoTransform transformation,
            bool include = true)
        {
            return include ? src.ApplyTransform(transformation) : src;
        }

        #endregion Then Clauses

        #region Finalization

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
        public static async Task<FileInfo> FinallyToFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            return await src.FinallyToFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token)
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
        public static async Task<FileInfo> FinallyToFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"));
            await src.FinallyToFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
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
        public static async Task FinallyToFileAsync(this Func<Stream, bool, CancellationToken, Task> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await src(strm, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// returns the contents as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<byte[]> FinallyToBytesAsync(this Func<Stream, bool, CancellationToken, Task> src,
            CancellationToken token = default(CancellationToken))
        {
            return (await src.FinallyToBufferAsync(token).ConfigureAwait(false)).ToArray();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        public static async Task<MemoryStream> FinallyToBufferAsync(this Func<Stream, bool, CancellationToken, Task> src,
            CancellationToken token = default(CancellationToken), bool seekToOrigin = false)
        {
            var ms = new MemoryStream(StdLookUps.DefaultBufferSize);
            await src.FinallyToStreamAsync(ms, false, token).ConfigureAwait(false);
            if (seekToOrigin) ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task FinallyToStreamAsync(this Func<Stream, bool, CancellationToken, Task> src,
            Stream writableTarget,
            bool disposeTarget = false,
            CancellationToken token = default(CancellationToken))
        {
            await src(writableTarget, disposeTarget, token).ConfigureAwait(false);
        }

        #endregion String Finalization
    }
}