using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Extensions methods on stream pipes.
    /// </summary>
    public static class StreamPipeExts
    {
        /// <summary>
        /// Writes the equivalent json representation to file.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        public static IJsonPipe WriteJson<T>(this T obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new JsonObjectPipe<T>(obj, serializer, enc ?? new UTF8Encoding(false), writerBuffer);
        }

        /// <summary>
        /// Writes the equivalent json representation to file.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="token">token to observe for cancellation</param>
        public static IJsonPipe WriteJson<T>(this IEnumerable<T> obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationToken token = default (CancellationToken))
        {
            return new JsonEnumeratorPipe<T>(obj, serializer, token, enc ?? new UTF8Encoding(false), writerBuffer);
        }

        /// <summary>
        /// Writes the equivalent json representation to file.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize as json text</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (WwithOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="writerBuffer">Buffer size for the stream writer</param>
        /// <param name="token">token to observe for cancellation</param>
        /// <param name="pcts">source to cancel in case <paramref name="token"/> is cancelled. Normally,
        /// this source token is observed at data producer side.</param>
        public static IJsonPipe WriteJson<T>(this BlockingCollection<T> obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize,
            CancellationToken token = default(CancellationToken),
            CancellationTokenSource pcts = default (CancellationTokenSource))
        {
            return new JsonBcPipe<T>(obj, serializer, token, pcts, enc ?? new UTF8Encoding(false), writerBuffer);
        }
    }
}