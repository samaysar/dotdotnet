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
        #region ToJsonArrayAsync region

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
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, textwriter is disposed after the serialization</param>
        public static async Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            var jsonWriter = serializer.JsonWriter(textWriter, disposeWriter);
            using (jsonWriter)
            {
                await collection.ToJsonArrayAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            JsonWriter jsonWriter)
        {
            return collection.ToJsonArrayAsync(serializer, jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        public static async Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            JsonWriter jsonWriter, CancellationToken token)
        {
            jsonWriter.WriteStartArray();
            foreach (var obj in collection)
            {
                await obj.ToJsonAsync(serializer, jsonWriter, token).ConfigureAwait(false);
            }
            jsonWriter.WriteEndArray();
            await jsonWriter.FlushAsync(token).ConfigureAwait(false);
        }

        #endregion ToJsonArrayAsync region

        #region ToJsonAsync region

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
                await obj.ToJsonAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            JsonWriter jsonWriter)
        {
            return obj.ToJsonAsync(serializer, jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        public static async Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            JsonWriter jsonWriter, CancellationToken token)
        {
            serializer.Serialize(jsonWriter, obj);
            await jsonWriter.FlushAsync(token).ConfigureAwait(false);
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