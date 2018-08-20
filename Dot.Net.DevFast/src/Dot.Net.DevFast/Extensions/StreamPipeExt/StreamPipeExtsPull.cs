using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        #region Various Pull

        /// <summary>
        /// Pulls the underlying data from the stream and returns a func stream pipe for chaining.
        /// </summary>
        /// <param name="readable">A readable stream</param>
        /// <param name="disposeSource">If true, stream is disposed at the end of streaming else left open</param>
        public static Func<PullFuncStream> Pull(this Stream readable,
            bool disposeSource = false)
        {
            return () => new PullFuncStream(readable, disposeSource);
        }

        #endregion Various Pull

        #region Then Clauses

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

        /// <summary>
        /// Converts the PULL pipeline to PUSH pipeline and returns it for chaining.
        /// </summary>
        /// <param name="src">Current PULL source pipe</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Func<PushFuncStream, Task> ThenConvertToPush(this Func<PullFuncStream> src,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async pfs => await src.AndWriteStreamAsync(pfs.Writable, bufferSize, pfs.Dispose, pfs.Token)
                .ConfigureAwait(false);
        }

        #endregion Then Clauses

        #region Finalization

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
            var data = src();
            await data.Readable.CopyToAsync(writableTarget, bufferSize, token, data.Dispose, disposeTarget)
                .ConfigureAwait(false);
        }

        #endregion Finalization

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