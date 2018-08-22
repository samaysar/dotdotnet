using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        #region Various Pull

        /// <summary>
        /// Pulls underlying data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="folder">Parent folder of the file</param>
        /// <param name="filename">An existing readable file's name with extension</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PullFuncStream> Pull(this DirectoryInfo folder,
            string filename,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return folder.CreateFileInfo(filename).Pull(fileStreamBuffer, options);
        }

        /// <summary>
        /// Pulls underlying data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="fileinfo">Fileinfo instance of an existing readable file</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PullFuncStream> Pull(this FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return fileinfo.CreateStream(FileMode.Open, FileAccess.Read, FileShare.Read, fileStreamBuffer, options)
                .Pull();
        }

        /// <summary>
        /// Pulls bytes from given array and returns a new pipe for chaining.
        /// Supplied <paramref name="byteTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteTask">Task returning Source byte array. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<byte[]> byteTask)
        {
            return async () =>
            {
                var bytes = await byteTask.StartIfNeeded().ConfigureAwait(false);
                return bytes.Pull()();
            };
        }

        /// <summary>
        /// Pulls bytes from given array and returns a new pipe for chaining.
        /// </summary>
        /// <param name="bytes">Source byte array</param>
        public static Func<PullFuncStream> Pull(this byte[] bytes)
        {
            return new ArraySegment<byte>(bytes, 0, bytes.Length).Pull();
        }

        /// <summary>
        /// Pulls bytes from given byte segment and returns a new pipe for chaining.
        /// Supplied <paramref name="byteSegTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteSegTask">task returning Source array segment. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<ArraySegment<byte>> byteSegTask)
        {
            return async () =>
            {
                var byteSeg = await byteSegTask.StartIfNeeded().ConfigureAwait(false);
                return byteSeg.Pull()();
            };
        }

        /// <summary>
        /// Pulls bytes from given byte segment and returns a new pipe for chaining.
        /// </summary>
        /// <param name="byteSeg">Source byte array</param>
        public static Func<PullFuncStream> Pull(this ArraySegment<byte> byteSeg)
        {
            return new MemoryStream(byteSeg.Array, byteSeg.Offset, byteSeg.Count, false, false).Pull();
        }

        /// <summary>
        /// Pulls bytes from given source stream and returns a new pipe for chaining.
        /// Supplied <paramref name="streamTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="streamTask">Task returning Source data stream. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="disposeSourceStream">If true, source stream is disposed</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<Stream> streamTask,
            bool disposeSourceStream = true)
        {
            return async () =>
            {
                var stream = await streamTask.StartIfNeeded().ConfigureAwait(false);
                return stream.Pull(disposeSourceStream)();
            };
        }

        /// <summary>
        /// Pulls the underlying data from the stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="stream">A readable stream</param>
        /// <param name="disposeSource">If true, stream is disposed at the end of streaming else left open</param>
        public static Func<PullFuncStream> Pull(this Stream stream,
            bool disposeSource = true)
        {
            return () => new PullFuncStream(stream, disposeSource);
        }

        #endregion Various Pull

        #region Then Clauses NoTASK

        /// <summary>
        /// Applies decompression on the data of given functional Stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="pullSrc">Current pipe of the PUSH pipeline</param>
        /// <param name="include">If true is passed, decompression is performed else ignored</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        public static Func<PullFuncStream> ThenDecompress(this Func<PullFuncStream> pullSrc,
            bool gzip = true,
            bool include = true)
        {
            return pullSrc.ThenApply(p => p.ApplyDecompression(gzip), include);
        }

        /// <summary>
        /// Computes the hash on the data of the given functional stream pipe and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        /// <param name="include">If true is passed, hash is computed else ignored</param>
        public static Func<PullFuncStream> ThenComputeHash(this Func<PullFuncStream> src,
            HashAlgorithm ha,
            bool include = true)
        {
            return src.ThenTransform(ha, include);
        }

        /// <summary>
        /// Converts the data, of the given functional stream pipe to equivalent Base64
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, ToBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenToBase64(this Func<PullFuncStream> src,
            bool include = true)
        {
            return src.ThenTransform(new ToBase64Transform(), include);
        }

        /// <summary>
        /// Decodes the Base64 data, of the given functional stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="mode">Base64 transform mode</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenFromBase64(this Func<PullFuncStream> src,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool include = true)
        {
            return src.ThenTransform(new FromBase64Transform(mode), include);
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given functional stream pipe
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<PullFuncStream> ThenTransform(this Func<PullFuncStream> src,
            ICryptoTransform transformation,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyTransform(transformation), include);
        }

        /// <summary>
        /// Appends the given arbitrary custom functional stream pipe (i.e. <paramref name="applyFunc"/>) to the pipeline
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="applyFunc">Yet another custom functional stream pipe</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<PullFuncStream> ThenApply(this Func<PullFuncStream> src,
            Func<Func<PullFuncStream>, Func<PullFuncStream>> applyFunc,
            bool include = true)
        {
            return include ? applyFunc(src) : src;
        }

        #endregion Then Clauses NoTASK

        #region Then Clauses TASK

        /// <summary>
        /// Applies decompression on the data of given functional Stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="pullSrc">Current pipe of the PUSH pipeline</param>
        /// <param name="include">If true is passed, decompression is performed else ignored</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        public static Func<Task<PullFuncStream>> ThenDecompress(this Func<Task<PullFuncStream>> pullSrc,
            bool gzip = true,
            bool include = true)
        {
            return pullSrc.ThenApply(p => p.ApplyDecompression(gzip), include);
        }

        /// <summary>
        /// Computes the hash on the data of the given functional stream pipe and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        /// <param name="include">If true is passed, hash is computed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenComputeHash(this Func<Task<PullFuncStream>> src,
            HashAlgorithm ha,
            bool include = true)
        {
            return src.ThenTransform(ha, include);
        }

        /// <summary>
        /// Converts the data, of the given functional stream pipe to equivalent Base64
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, ToBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenToBase64(this Func<Task<PullFuncStream>> src,
            bool include = true)
        {
            return src.ThenTransform(new ToBase64Transform(), include);
        }

        /// <summary>
        /// Decodes the Base64 data, of the given functional stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="mode">Base64 transform mode</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenFromBase64(this Func<Task<PullFuncStream>> src,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool include = true)
        {
            return src.ThenTransform(new FromBase64Transform(mode), include);
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given functional stream pipe
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenTransform(this Func<Task<PullFuncStream>> src,
            ICryptoTransform transformation,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyTransform(transformation), include);
        }

        /// <summary>
        /// Appends the given arbitrary custom functional stream pipe (i.e. <paramref name="applyFunc"/>) to the pipeline
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="applyFunc">Yet another custom functional stream pipe</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenApply(this Func<Task<PullFuncStream>> src,
            Func<Func<Task<PullFuncStream>>, Func<Task<PullFuncStream>>> applyFunc,
            bool include = true)
        {
            return include ? applyFunc(src) : src;
        }

        #endregion Then Clauses TASK

        #region ConvertorToPush

        /// <summary>
        /// Converts the PULL pipeline to PUSH pipeline and returns it for chaining.
        /// </summary>
        /// <param name="src">Current PULL source pipe</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Func<PushFuncStream, Task> ThenConvertToPush(this Func<PullFuncStream> src,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return src.ToAsync(false).ThenConvertToPush(bufferSize);
        }

        /// <summary>
        /// Converts the PULL pipeline to PUSH pipeline and returns it for chaining.
        /// </summary>
        /// <param name="src">Current PULL source pipe</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Func<PushFuncStream, Task> ThenConvertToPush(this Func<Task<PullFuncStream>> src,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async pfs =>
            {
                await src.AndWriteStreamAsync(pfs.Writable, bufferSize, pfs.Dispose, pfs.Token)
                    .ConfigureAwait(false);
            };
        }

        #endregion ConvertorToPush

        #region Finalization NoTASK

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PullFuncStream> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            return await src.ToAsync(false).AndWriteFileAsync(folder, filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PullFuncStream> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            return await src.ToAsync(false).AndWriteFileAsync(folder, filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteFileAsync(this Func<PullFuncStream> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            await src.ToAsync(false).AndWriteFileAsync(fileinfo, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<byte[]> AndWriteBytesAsync(this Func<PullFuncStream> src,
            CancellationToken token = default(CancellationToken),
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteBytesAsync(token, estimatedSize, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<ArraySegment<byte>> AndWriteByteSegAsync(this Func<PullFuncStream> src,
            CancellationToken token = default(CancellationToken),
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteByteSegAsync(token, estimatedSize, bufferSize)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        /// <param name="initialSize">Initial Memory buffer Size</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<MemoryStream> AndWriteBufferAsync(this Func<PullFuncStream> src,
            CancellationToken token = default(CancellationToken),
            bool seekToOrigin = false,
            int initialSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteBufferAsync(token, seekToOrigin, initialSize, bufferSize)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStreamAsync(this Func<PullFuncStream> src,
            Stream writableTarget,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeTarget = false,
            CancellationToken token = default(CancellationToken))
        {
            await src.ToAsync(false).AndWriteStreamAsync(writableTarget, bufferSize, disposeTarget, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as string.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of the string (optimization on resizing ops)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<string> AndWriteStringAsync(this Func<PullFuncStream> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            return await src.ToAsync(false)
                .AndWriteStringAsync(initialSize, enc, detectEncodingFromBom, bufferSize, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as newly created <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of StringBuilder (optimization on array resizing)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<StringBuilder> AndWriteStringBuilderAsync(this Func<PullFuncStream> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            return await src.ToAsync(false)
                .AndWriteStringBuilderAsync(initialSize, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false);

        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="sbToAppend">String builder to append data</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStringBuilderAsync(this Func<PullFuncStream> src,
            StringBuilder sbToAppend,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            await src.ToAsync(false)
                .AndWriteStringBuilderAsync(sbToAppend, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false);
        }

        #endregion Finalization NoTASK

        #region Finalization TASK

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            return await src.AndWriteFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"));
            await src.AndWriteFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
            return targetFile;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default(CancellationToken))
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await src.AndWriteStreamAsync(strm, fileStreamBuffer, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<byte[]> AndWriteBytesAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default(CancellationToken),
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await src.AndWriteByteSegAsync(token, estimatedSize, bufferSize).ConfigureAwait(false))
                .CreateBytes();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<ArraySegment<byte>> AndWriteByteSegAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default(CancellationToken),
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await src.AndWriteBufferAsync(token, false, estimatedSize, bufferSize).ConfigureAwait(false))
                .ThrowIfNoBuffer();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        /// <param name="initialSize">Initial Memory buffer Size</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<MemoryStream> AndWriteBufferAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default(CancellationToken),
            bool seekToOrigin = false,
            int initialSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var ms = new MemoryStream(initialSize);
            await src.AndWriteStreamAsync(ms, bufferSize, false, token).ConfigureAwait(false);
            if (seekToOrigin) ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStreamAsync(this Func<Task<PullFuncStream>> src,
            Stream writableTarget,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeTarget = false,
            CancellationToken token = default(CancellationToken))
        {
            var data = await src().ConfigureAwait(false);
            await data.Readable.CopyToAsync(writableTarget, bufferSize, token, data.Dispose, disposeTarget)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as string.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of the string (optimization on resizing ops)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<string> AndWriteStringAsync(this Func<Task<PullFuncStream>> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            return (await src.AndWriteStringBuilderAsync(initialSize, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false)).ToString();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as newly created <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of StringBuilder (optimization on array resizing)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<StringBuilder> AndWriteStringBuilderAsync(this Func<Task<PullFuncStream>> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            var sb = new StringBuilder(initialSize);
            await src.AndWriteStringBuilderAsync(sb, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false);
            return sb;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="sbToAppend">String builder to append data</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStringBuilderAsync(this Func<Task<PullFuncStream>> src,
            StringBuilder sbToAppend,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default(CancellationToken))
        {
            var data = await src().ConfigureAwait(false);
            await data.Readable.CopyToBuilderAsync(sbToAppend, token, enc ?? new UTF8Encoding(false), bufferSize,
                data.Dispose, detectEncodingFromBom).ConfigureAwait(false);
        }

        #endregion Finalization TASK

        /// <summary>
        /// Data structure to facilitate Pull based functional streaming,
        /// i.e., 2ndst Pipe reads from 1st, 3rd reads from 2nd and so on and so forth... /// </summary>
        public struct PullFuncStream
        {
            /// <summary>
            /// Readable stream
            /// </summary>
            public Stream Readable { get; }

            /// <summary>
            /// If true, stream is disposed at the end of streaming else left open
            /// </summary>
            public bool Dispose { get; }

            /// <summary>
            /// Ctor.
            /// </summary>
            /// <param name="readable">readable stream</param>
            /// <param name="dispose">true to dispose at the end of streaming else false</param>
            /// <exception cref="DdnDfException"></exception>
            public PullFuncStream(Stream readable, bool dispose)
            {
                Readable = readable.CanRead.ThrowIfNot(DdnDfErrorCode.Unspecified, "Cannot read from the stream",
                    readable);
                Dispose = dispose;
            }
        }
    }
}