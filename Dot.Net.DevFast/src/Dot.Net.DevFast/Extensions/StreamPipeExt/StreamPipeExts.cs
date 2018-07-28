using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Extensions methods on stream pipes.
    /// </summary>
    public static partial class StreamPipeExts
    {
        /// <summary>
        /// Creates the equivalent json representation of the object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static IJsonPipe SerializeAsJson<T>(this T obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new JsonObjectPipe<T>(obj, serializer, enc ?? new UTF8Encoding(false), writerBuffer);
        }

        /// <summary>
        /// Creates the equivalent json array representation using the enumeration.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static IJsonPipe SerializeAsJson<T>(this IEnumerable<T> obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new JsonEnumeratorPipe<T>(obj, serializer, enc ?? new UTF8Encoding(false), writerBuffer);
        }

        /// <summary>
        /// Creates the equivalent json array representation of the objects in the given blocking collection.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="pcts">source to cancel in case some error is encountered. Normally,
        /// this source token is observed at data producer side.</param>
        public static IJsonPipe SerializeAsJson<T>(this BlockingCollection<T> obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationTokenSource pcts = default (CancellationTokenSource))
        {
            return new JsonBcPipe<T>(obj, serializer, pcts, enc ?? new UTF8Encoding(false), writerBuffer);
        }

        /// <summary>
        /// Compresses the data of given Stream pipe as source.
        /// </summary>
        /// <param name="src">Source whose data to compress</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else <seealso cref="DeflateStream"/> is used</param>
        /// <param name="level">Compression level to use.</param>
        public static ICompressedPipe ThenCompress(this IStreamPipe src, bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal)
        {
            return new CompressedPipe(src, gzip, level);
        }
    }
}