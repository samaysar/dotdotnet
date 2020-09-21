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
using Dot.Net.DevFast.Extensions.Ppc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.IO;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Extensions methods on stream pipes.
    /// </summary>
    public static partial class StreamPipeExts
    {
        #region Various Push

        /// <summary>
        /// Pushes the string content and returns a new pipe for chaining.
        /// <para>NOTE: If you already have a string builder, then use the overloaded method instead
        /// of doing yourStringBuilder.ToString().LoadString(...), as overloaded method is optimized.</para>
        /// Supplied <paramref name="stringTask"/> is awaited during bootstrapping (NOT during chaining)
        /// </summary>
        /// <param name="stringTask">Task returning the source string. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<PushFuncStream, Task> Push(this Task<string> stringTask,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async pfs =>
            {
                var str = await stringTask.StartIfNeeded().ConfigureAwait(false);
                await str.Push(enc, bufferSize)(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes the string content and returns a new pipe for chaining.
        /// <para>NOTE: If you already have a string builder, then use the overloaded method instead
        /// of doing yourStringBuilder.ToString().LoadString(...), as overloaded method is optimized.</para>
        /// </summary>
        /// <param name="s">source string</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<PushFuncStream, Task> Push(this string s,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return new Action<int, char[], int, int>(s.CopyTo).ApplyLoad(s.Length, enc ?? new UTF8Encoding(false),
                bufferSize);
        }

        /// <summary>
        /// Pushes the string content of the builder and returns a new pipe for chaining.
        /// Supplied <paramref name="sbTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="sbTask">Task returning source string builder. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<PushFuncStream, Task> Push(this Task<StringBuilder> sbTask,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async pfs =>
            {
                var sb = await sbTask.StartIfNeeded().ConfigureAwait(false);
                await sb.Push(enc, bufferSize)(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes the string content of the builder and returns a new pipe for chaining.
        /// </summary>
        /// <param name="sb">source string builder</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="bufferSize">Buffer size (as number of char instead of bytes)</param>
        public static Func<PushFuncStream, Task> Push(this StringBuilder sb,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return new Action<int, char[], int, int>(sb.CopyTo).ApplyLoad(sb.Length, enc ?? new UTF8Encoding(false),
                bufferSize);
        }

        /// <summary>
        /// Pushes the data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="folder">Parent folder of the file</param>
        /// <param name="filename">An existing readable file's name with extension</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PushFuncStream, Task> Push(this DirectoryInfo folder,
            string filename,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return folder.CreateFileInfo(filename).Push(fileStreamBuffer, options);
        }

        /// <summary>
        /// Pushes the data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="fileinfo">Fileinfo instance of an existing readable file</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PushFuncStream, Task> Push(this FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return fileinfo.CreateStream(FileMode.Open, FileAccess.Read, FileShare.Read, fileStreamBuffer, options)
                .Push(fileStreamBuffer);
        }

        /// <summary>
        /// Pushes bytes from given array and returns a new pipe for chaining.
        /// Supplied <paramref name="byteTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteTask">Task returning Source byte array. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<PushFuncStream, Task> Push(this Task<byte[]> byteTask)
        {
            return async pfs =>
            {
                var bytes = await byteTask.StartIfNeeded().ConfigureAwait(false);
                await bytes.Push()(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes bytes from given array and returns a new pipe for chaining.
        /// </summary>
        /// <param name="bytes">Source byte array</param>
        public static Func<PushFuncStream, Task> Push(this byte[] bytes)
        {
            return new ArraySegment<byte>(bytes, 0, bytes.Length).Push();
        }

        /// <summary>
        /// Pushes bytes from given byte segment and returns a new pipe for chaining.
        /// Supplied <paramref name="byteSegTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteSegTask">task returning Source array segment. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<PushFuncStream, Task> Push(this Task<ArraySegment<byte>> byteSegTask)
        {
            return async pfs =>
            {
                var byteSeg = await byteSegTask.StartIfNeeded().ConfigureAwait(false);
                await byteSeg.Push()(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes bytes from given byte segment and returns a new pipe for chaining.
        /// </summary>
        /// <param name="byteSeg">Source byte array</param>
        public static Func<PushFuncStream, Task> Push(this ArraySegment<byte> byteSeg)
        {
            return async pfs =>
            {
                await pfs.Writable.CopyFromAsync(byteSeg, pfs.Token, pfs.Dispose).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes bytes from given source stream and returns a new pipe for chaining.
        /// Supplied <paramref name="streamTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="streamTask">Task returning Source data stream. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="streamBuffer">Buffer size to use during data loading</param>
        /// <param name="disposeSourceStream">If true, source stream is disposed</param>
        public static Func<PushFuncStream, Task> Push(this Task<Stream> streamTask,
            int streamBuffer = StdLookUps.DefaultFileBufferSize,
            bool disposeSourceStream = true)
        {
            return async pfs =>
            {
                var stream = await streamTask.StartIfNeeded().ConfigureAwait(false);
                await stream.Push(streamBuffer, disposeSourceStream)(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes bytes from given source stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="source">Source data stream</param>
        /// <param name="streamBuffer">Buffer size to use during data loading</param>
        /// <param name="disposeSourceStream">If true, source stream is disposed</param>
        public static Func<PushFuncStream, Task> Push(this Stream source,
            int streamBuffer = StdLookUps.DefaultFileBufferSize,
            bool disposeSourceStream = true)
        {
            return async pfs =>
            {
                await source.CopyToAsync(pfs.Writable, streamBuffer, pfs.Token, disposeSourceStream, pfs.Dispose)
                    .ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes the equivalent json representation of the object and returns a new pipe for chaining.
        /// Supplied <paramref name="objTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="objTask">Task returning Object to serialize as json text. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        public static Func<PushFuncStream, Task> PushJsonAsync<T>(this Task<T> objTask,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            bool autoFlush = false)
        {
            return async pfs =>
            {
                var obj = await objTask.StartIfNeeded().ConfigureAwait(false);
                await obj.PushJson(serializer, enc, writerBuffer, autoFlush)(pfs).ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Pushes the equivalent json representation of the object and returns a new pipe for chaining.
        /// <para>NOTE: Use <see cref="PushJsonAsync{T}"/> for <seealso cref="Task{T}"/>
        /// and use <see cref="PushJsonArray{T}(IEnumerable{T},JsonSerializer,Encoding,int,CancellationTokenSource,bool)"/> for any implementation of <seealso cref="IEnumerable{T}"/>.</para>
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        public static Func<PushFuncStream, Task> PushJson<T>(this T obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            bool autoFlush = false)
        {
            return new Action<PushFuncStream>(pfs => obj.ToJson(pfs.Writable, serializer,
                enc ?? new UTF8Encoding(false), writerBuffer, pfs.Dispose, autoFlush)).ToAsync(false);
        }

        /// <summary>
        /// Pushes the equivalent json array representation of the objects in the given blocking collection
        /// and returns a new pipe for chaining.
        /// <para>IMPORTANT: If passed collection is <seealso cref="BlockingCollection{T}"/>, then
        /// the method uses the instance of <paramref name="pcts"/> to suport the error signaling in
        /// concurrent producer-consumer. For any other kind of collection (including other concurrent collection),
        /// the implementation simply ignores the <paramref name="pcts"/> instance.</para>
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="pcts">Source to cancel in case some error is encountered. Normally,
        /// this source token is observed at data producer side.
        /// <para>NOTE: Used ONLY when supplied collection is <seealso cref="BlockingCollection{T}"/></para></param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        public static Func<PushFuncStream, Task> PushJsonArray<T>(this IEnumerable<T> obj,
            JsonSerializer serializer = null,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationTokenSource pcts = default,
            bool autoFlush = false)
        {
            return obj is BlockingCollection<T> collection
                ? collection.PushJsonEnumeration(serializer, enc ?? new UTF8Encoding(false), writerBuffer, pcts,
                    autoFlush)
                : obj.PushJsonEnumeration(serializer, enc ?? new UTF8Encoding(false), writerBuffer, autoFlush);
        }

        /// <summary>
        /// Pushes the equivalent json array representation of the objects produced by the producer's action
        /// implementation and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="producerAction">Producer lambda responsible of object production</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        /// <exception cref="AggregateException"></exception>
        public static Func<PushFuncStream, Task> PushJsonArray<T>(
            this Action<IProducerBuffer<T>, CancellationToken> producerAction,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            bool autoFlush = false)
        {
            return producerAction.ToAsync(false).PushJsonArray(serializer, ppcBufferSize, enc, writerBuffer, autoFlush);
        }

        /// <summary>
        /// Pushes the equivalent json array representation of the objects produced by the producer's action
        /// implementation and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="producerFunc">Producer async lambda responsible of object production</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        /// <exception cref="AggregateException"></exception>
        public static Func<PushFuncStream, Task> PushJsonArray<T>(
            this Func<IProducerBuffer<T>, CancellationToken, Task> producerFunc,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            bool autoFlush = false)
        {
            return producerFunc.ToProducer().PushJsonArray(serializer, ppcBufferSize, enc, writerBuffer, autoFlush);
        }

        /// <summary>
        /// Pushes the equivalent json array representation of the objects produced by the producer implementation
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="producer">Producer side responsible of object production</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="autoFlush">True to enable auto-flushing else false</param>
        /// <exception cref="AggregateException"></exception>
        public static Func<PushFuncStream, Task> PushJsonArray<T>(this IProducer<T> producer,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            Encoding enc = null,
            int writerBuffer = StdLookUps.DefaultFileBufferSize,
            bool autoFlush = false)
        {
            return async pfs =>
            {
                using var serialBc = ConcurrentBuffer.CreateBuffer<T>(ppcBufferSize);
                using var localCts = new CancellationTokenSource();
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(pfs.Token, localCts.Token);
                await serialBc.PushPpcJsonEnumeration(serializer, enc ?? new UTF8Encoding(false),
                    writerBuffer, localCts, autoFlush,
                    combinedCts.Token, pfs, producer, ppcBufferSize).ConfigureAwait(false);
            };
        }

        #endregion Various Push

        #region Then Clauses

        /// <summary>
        /// Concurrently writes on <paramref name="writableStream"/> and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableStream">Stream on which to write concurrently</param>
        /// <param name="disposeWritableStream">true to dispose <paramref name="writableStream"/> else false.</param>
        /// <param name="errorHandler">Lambda to call in case an error is encountered during stream operations.
        /// If NOT supplied then the exception is immediately rethrown, otherwise, it is passed to the lambda
        /// along with the <paramref name="writableStream"/> instance. It is then up to lambda whether to
        /// rethrow it or not.</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenConcurrentlyWriteTo(this Func<PushFuncStream, Task> src,
            Stream writableStream,
            bool disposeWritableStream,
            Action<Stream, Exception> errorHandler = null,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyConcurrentStream(writableStream, disposeWritableStream, errorHandler),
                include);
        }

        /// <summary>
        /// Counts the number of bytes observed during push based streaming (exposed through 
        /// <seealso cref="IByteCounter"/>.<seealso cref="IByteCounter.ByteCount"/>) 
        /// and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="IByteCounter.ByteCount"/> ONLY AFTER the full
        /// pipeline is bootstrapped and processed, i.e., calling <paramref name="byteCounter"/>.ByteCount immediately
        /// after this call will not provide the correct count.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="byteCounter">out param which exposes <seealso cref="IByteCounter.ByteCount"/>) property.</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenCountBytes(this Func<PushFuncStream, Task> src,
            out IByteCounter byteCounter,
            bool include = true)
        {
            var bcs = new ByteCountStream();
            byteCounter = bcs;
            return src.ThenApply(s => s.ApplyByteCount(bcs), include);
        }

        /// <summary>
        /// Applies compression on the data of given functional Stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        /// <param name="level">Compression level to use.</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenCompress(this Func<PushFuncStream, Task> src,
            bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyCompression(gzip, level), include);
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
        public static Func<PushFuncStream, Task> ThenComputeHash(this Func<PushFuncStream, Task> src,
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
        public static Func<PushFuncStream, Task> ThenToBase64(
            this Func<PushFuncStream, Task> src,
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
        public static Func<PushFuncStream, Task> ThenFromBase64(
            this Func<PushFuncStream, Task> src,
            FromBase64TransformMode mode = FromBase64TransformMode.DoNotIgnoreWhiteSpaces,
            bool include = true)
        {
            return src.ThenTransform(new FromBase64Transform(mode), include);
        }

#if NETHASHCRYPTO
        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<PushFuncStream, Task> ThenEncrypt<T>(this Func<PushFuncStream, Task> src,
            string password,
            string salt,
#if NETHASHCRYPTO
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NETHASHCRYPTO
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), true), include);
        }

        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenEncrypt<T>(this Func<PushFuncStream, Task> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, true), include);
        }

#if NETHASHCRYPTO
        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<PushFuncStream, Task> ThenDecrypt<T>(this Func<PushFuncStream, Task> src,
            string password,
            string salt,
#if NETHASHCRYPTO
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NETHASHCRYPTO
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), false), include);
        }

        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenDecrypt<T>(this Func<PushFuncStream, Task> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, false), include);
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given functional stream pipe
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<PushFuncStream, Task> ThenTransform(
            this Func<PushFuncStream, Task> src,
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
        /// <param name="include">If true is passed, given func is applied else ignored</param>
        public static Func<PushFuncStream, Task> ThenApply(this Func<PushFuncStream, Task> src,
            Func<Func<PushFuncStream, Task>, Func<PushFuncStream, Task>> applyFunc,
            bool include = true)
        {
            return include ? applyFunc(src) : src;
        }


        #endregion Then Clauses

        #region Finalization

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PushFuncStream, Task> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            return await src.AndWriteFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PushFuncStream, Task> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"));
            await src.AndWriteFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
            return targetFile;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline to the file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteFileAsync(this Func<PushFuncStream, Task> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
#if !NETASYNCDISPOSE
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
#else
            var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options);
            await using (strm.ConfigureAwait(false))
#endif
            {
                await src.AndWriteStreamAsync(strm, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        public static async Task<byte[]> AndWriteBytesAsync(this Func<PushFuncStream, Task> src,
            CancellationToken token = default,
            int estimatedSize = StdLookUps.DefaultBufferSize)
        {
            return (await src.AndWriteBufferAsync(token, false, estimatedSize).ConfigureAwait(false)).ToArray();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline and returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        /// <param name="initialSize">Initial Memory buffer Size</param>
        public static async Task<MemoryStream> AndWriteBufferAsync(this Func<PushFuncStream, Task> src,
            CancellationToken token = default,
            bool seekToOrigin = false,
            int initialSize = StdLookUps.DefaultBufferSize)
        {
            var ms = new MemoryStream(initialSize);
            await src.AndWriteStreamAsync(ms, false, token).ConfigureAwait(false);
            if (seekToOrigin) ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline.
        /// <para>NOTE: Calling this function will result in running the streaming pipeline, but, you won't receive
        /// anything in the end. Normally, usage of this function is to avoid use of <seealso cref="MemoryStream"/>
        /// to reduce runtime memory pressure, and at the same time counting bytes, calculate crypto-hashes etc.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndExecuteAsync(this Func<PushFuncStream, Task> src,
            CancellationToken token = default)
        {
            await src.AndWriteStreamAsync(Stream.Null, false, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the total bytes observed at the end.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<long> AndCountBytesAsync(this Func<PushFuncStream, Task> src,
            CancellationToken token = default)
        {
            var bcs = new ByteCountStream();
            await src.AndWriteStreamAsync(bcs, true, token).ConfigureAwait(false);
            return bcs.ByteCount;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline and appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStreamAsync(this Func<PushFuncStream, Task> src,
            Stream writableTarget,
            bool disposeTarget = false,
            CancellationToken token = default)
        {
            await src(new PushFuncStream(writableTarget, disposeTarget, token)).StartIfNeeded().ConfigureAwait(false);
        }

        #endregion Finalization
    }
}