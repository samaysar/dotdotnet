using System.Text;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExts
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
        public static IFilePipe WriteJsonFile<T>(this T obj, JsonSerializer serializer = null,
            Encoding enc = null, int writerBuffer = StdLookUps.DefaultFileBufferSize)
        {
            return new JsonObjectPipe<T>(obj, serializer, enc ?? new UTF8Encoding(false), writerBuffer);
        }
    }
}