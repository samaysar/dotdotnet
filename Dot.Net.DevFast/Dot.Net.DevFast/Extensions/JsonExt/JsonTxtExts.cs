using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
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
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
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
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
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
        /// to <paramref name="output"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            StringBuilder output, IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(serializer, output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            using (var textWriter = new StringWriter(output, formatProvider ?? CultureInfo.CurrentCulture))
            {
                await obj.ToJsonAsync(serializer, textWriter, token, false).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            Stream outputStream, Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, 
            bool disposeStream = true)
        {
            return obj.ToJsonAsync(serializer, outputStream, CancellationToken.None, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static async Task ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            Stream outputStream, CancellationToken token, Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            using (var textWriter = new StreamWriter(outputStream, enc ?? Encoding.UTF8,
                bufferSize, !disposeStream)
            {
                AutoFlush = true
            })
            {
                await obj.ToJsonAsync(serializer, textWriter, token, false).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
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
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
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
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
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