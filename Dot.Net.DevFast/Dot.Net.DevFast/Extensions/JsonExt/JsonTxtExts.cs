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
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection, CancellationToken token,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(DefaultJsonSerializer(), token, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(serializer, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            CancellationToken token, IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            await collection.ToJsonArrayAsync(serializer, stringBuilder, token, formatProvider).ConfigureAwait(false);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="output"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, StringBuilder output, 
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(DefaultJsonSerializer(), output, token, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            StringBuilder output, IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(serializer, output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            using (var textWriter = output.CreateWriter(formatProvider))
            {
                await collection.ToJsonArrayAsync(serializer, textWriter, token, false).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            Stream outputStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return collection.ToJsonArrayAsync(outputStream, CancellationToken.None, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            Stream outputStream, CancellationToken token, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return collection.ToJsonArrayAsync(DefaultJsonSerializer(), outputStream, token, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            Stream outputStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return collection.ToJsonArrayAsync(serializer, outputStream, CancellationToken.None, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static async Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            Stream outputStream, CancellationToken token, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            using (var textWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                await collection.ToJsonArrayAsync(serializer, textWriter, token, false).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            TextWriter textWriter, bool disposeWriter = true)
        {
            return collection.ToJsonArrayAsync(textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            return collection.ToJsonArrayAsync(DefaultJsonSerializer(), textWriter, token, disposeWriter);
        }
        
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
            var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter);
            using (jsonWriter)
            {
                await collection.ToJsonArrayAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter)
        {
            return collection.ToJsonArrayAsync(jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object array to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter, CancellationToken token)
        {
            return collection.ToJsonArrayAsync(DefaultJsonSerializer(), jsonWriter, token);
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
        /// Returns the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/>
        /// asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonAsync<T>(this T obj, IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/>
        /// asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonAsync<T>(this T obj, CancellationToken token, 
            IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(DefaultJsonSerializer(), token, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(serializer, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task<string> ToJsonAsync<T>(this T obj, JsonSerializer serializer,
            CancellationToken token, IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            await obj.ToJsonAsync(serializer, stringBuilder, token, formatProvider).ConfigureAwait(false);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="output"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonAsync<T>(this T obj, StringBuilder output,
            IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonAsync<T>(this T obj, StringBuilder output, 
            CancellationToken token, IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(DefaultJsonSerializer(), output, token, formatProvider);
        }

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
            using (var textWriter = output.CreateWriter(formatProvider))
            {
                await obj.ToJsonAsync(serializer, textWriter, token, false).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, Stream outputStream, Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return obj.ToJsonAsync(outputStream, CancellationToken.None, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, Stream outputStream, CancellationToken token, 
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return obj.ToJsonAsync(DefaultJsonSerializer(), outputStream, token, enc, bufferSize, disposeStream);
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
            using (var textWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                await obj.ToJsonAsync(serializer, textWriter, token, false).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, TextWriter textWriter, bool disposeWriter = true)
        {
            return obj.ToJsonAsync(textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, TextWriter textWriter,
            CancellationToken token, bool disposeWriter = true)
        {
            return obj.ToJsonAsync(DefaultJsonSerializer(), textWriter, token, disposeWriter);
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
            var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter);
            using (jsonWriter)
            {
                await obj.ToJsonAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonAsync<T>(this T obj, JsonWriter jsonWriter)
        {
            return obj.ToJsonAsync(jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>In case of <typeparamref name="T"/> as Large Collection, use 
        /// <see cref="ToJsonArrayAsync{T}(IEnumerable{T},JsonSerializer,TextWriter,bool)"/> (or its variants), instead.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        public static Task ToJsonAsync<T>(this T obj, JsonWriter jsonWriter, CancellationToken token)
        {
            return obj.ToJsonAsync(jsonWriter.DefaultJsonSerializer(), jsonWriter, token);
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

        private static JsonSerializer DefaultJsonSerializer(this JsonWriter writer)
        {
            var serializer = DefaultJsonSerializer();
            serializer.Culture = writer.Culture;
            serializer.DateFormatHandling = writer.DateFormatHandling;
            serializer.DateFormatString = writer.DateFormatString;
            serializer.DateTimeZoneHandling = writer.DateTimeZoneHandling;
            serializer.FloatFormatHandling = writer.FloatFormatHandling;
            serializer.Formatting = writer.Formatting;
            serializer.StringEscapeHandling = writer.StringEscapeHandling;
            return serializer;
        }

        private static JsonSerializer DefaultJsonSerializer()
        {
            return new JsonSerializer()
            {
                Culture = CultureInfo.CurrentCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                Formatting = Formatting.None,
                StringEscapeHandling = StringEscapeHandling.Default,
                CheckAdditionalContent = false,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                DateParseHandling = DateParseHandling.DateTime,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                FloatParseHandling = FloatParseHandling.Double,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }
    }
}