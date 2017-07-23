using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.JsonExt
{
    /// <summary>
    /// Extensions of Json serializations.
    /// </summary>
    public static class JsonTxtExts
    {
        #region ToJson region

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="obj">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, textwriter is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(IEnumerable<T> obj, JsonSerializer serializer,
            TextWriter textWriter, bool disposeWriter = true)
        {
            return obj.ToJsonArrayAsync(serializer, textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="obj">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, textwriter is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> obj, JsonSerializer serializer,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            //todo
            return Task.CompletedTask;
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, textwriter is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            TextWriter textWriter, bool disposeWriter = true)
        {
            return obj.ToJsonAsync(serializer, textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, textwriter is disposed after the serialization</param>
        public static async Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            var jsonWriter = serializer.JsonWriter(textWriter, disposeWriter);
            using (jsonWriter)
            {
                serializer.Serialize(jsonWriter, obj);
                await jsonWriter.FlushAsync(token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        private static JsonWriter JsonWriter(this JsonSerializer serializer, TextWriter writer, bool disposeWriter)
        {
            return new JsonTextWriter(writer)
            {
                Culture = serializer.Culture,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                FloatFormatHandling = serializer.FloatFormatHandling,
                Formatting = serializer.Formatting,
                StringEscapeHandling = serializer.StringEscapeHandling,
                CloseOutput = disposeWriter
            };
        }

        #endregion ToJson region

        #region FromJson region

        private static T FromJson<T>(JsonSerializer serializer, JsonReader reader)
        {
            using (reader)
            {
                return serializer.Deserialize<T>(reader);
            }
        }

        #endregion ToJson region
    }
}