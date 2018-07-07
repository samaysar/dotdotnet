using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.JsonExt
{
    /// <summary>
    /// Extensions of Json serializations.
    /// </summary>
    public static class JsonTxtExt
    {
        #region ToJsonArrayParallely region

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a consumer of objects (alone or among many others),
        /// writes the JSON array representation of objects of <paramref name="source"/> using 
        /// <paramref name="serializer"/> while observing <paramref name="token"/>.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY,
        /// somewhere outside the call of this method, otherwise this method will NEVER terminate (i.e. Deadlock).</para>
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="source">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArrayParallely<T>(this BlockingCollection<T> source,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource producerTokenSource = null, IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder(256);
            source.ToJsonArrayParallely(stringBuilder, serializer, token, producerTokenSource,
                formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a consumer of objects (alone or among many others),
        /// writes the JSON array representation of objects of <paramref name="source"/> using 
        /// <paramref name="serializer"/> to <paramref name="target"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY,
        /// somewhere outside the call of this method, otherwise this method will NEVER terminate (i.e. Deadlock).</para>
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="source">input blocking collection to JSON serialize</param>
        /// <param name="target">target output string builder</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> source,
            StringBuilder target, JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource producerTokenSource = null, IFormatProvider formatProvider = null)
        {
            source.ToJsonArrayParallely(target.CreateWriter(formatProvider), serializer, token,
                producerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a consumer of objects (alone or among many others),
        /// writes the JSON array representation of objects of <paramref name="source"/> using 
        /// <paramref name="serializer"/> to <paramref name="target"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY,
        /// somewhere outside the call of this method, otherwise this method will NEVER terminate (i.e. Deadlock).</para>
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="source">input blocking collection to JSON serialize</param>
        /// <param name="target">target output stream</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> source,
            Stream target, JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource producerTokenSource = null, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeTarget = true)
        {
            using (var streamWriter = target.CreateWriter(enc, bufferSize, disposeTarget))
            {
                source.ToJsonArrayParallely(streamWriter, serializer, token, producerTokenSource, false);
                target.Flush();
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a consumer of objects (alone or among many others),
        /// writes the JSON array representation of objects of <paramref name="source"/> using 
        /// <paramref name="serializer"/> to <paramref name="target"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY,
        /// somewhere outside the call of this method, otherwise this method will NEVER terminate (i.e. Deadlock).</para>
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="source">input blocking collection to JSON serialize</param>
        /// <param name="target">target text writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> source,
            TextWriter target, JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource producerTokenSource = null, bool disposeTarget = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            using (var jsonWriter = nullHandledSerializer.AdaptedJsonWriter(target, disposeTarget))
            {
                source.ToJsonArrayParallely(jsonWriter, nullHandledSerializer, token, producerTokenSource, false);
                target.Flush();
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a consumer of objects (alone or among many others),
        /// writes the JSON array representation of objects of <paramref name="source"/> using 
        /// <paramref name="serializer"/> to <paramref name="target"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY,
        /// somewhere outside the call of this method, otherwise this method will NEVER terminate (i.e. Deadlock).</para>
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="source">input blocking collection to JSON serialize</param>
        /// <param name="target">target JSON writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonWriter)"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> source,
            JsonWriter target, JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource producerTokenSource = null, bool disposeTarget = true)
        {
            try
            {
                target.WriteStartArray();
                var nullHandledSerializer = serializer ?? target.AdaptedJsonSerializer();
                while (source.TryTake(out var obj, Timeout.Infinite, token))
                {
                    nullHandledSerializer.Serialize(target, obj);
                }
                target.WriteEndArray();
                target.Flush();
            }
            catch
            {
                producerTokenSource?.Cancel();
                throw;
            }
            finally
            {
                target.DisposeIfRequired(disposeTarget);
            }
        }

        #endregion ToJsonArrayParallely region

        #region ToJsonArray region

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="source">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArray<T>(this IEnumerable<T> source, JsonSerializer serializer = null,
            CancellationToken token = default(CancellationToken), IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder(256);
            source.ToJsonArray(stringBuilder, serializer, token, formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="source">input object enumerable to JSON serialize</param>
        /// <param name="target">target output string builder</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArray<T>(this IEnumerable<T> source, StringBuilder target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            IFormatProvider formatProvider = null)
        {
            source.ToJsonArray(target.CreateWriter(formatProvider), serializer, token);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="source">input object enumerable to JSON serialize</param>
        /// <param name="target">target output stream</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> source, Stream target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeTarget = true)
        {
            using (var streamWriter = target.CreateWriter(enc, bufferSize, disposeTarget))
            {
                source.ToJsonArray(streamWriter, serializer, token, false);
                target.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="source">input object enumerable to JSON serialize</param>
        /// <param name="target">target text writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> source, TextWriter target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            bool disposeTarget = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            using (var jsonWriter = nullHandledSerializer.AdaptedJsonWriter(target, disposeTarget))
            {
                source.ToJsonArray(jsonWriter, nullHandledSerializer, token, false);
                target.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="source">input object enumerable to JSON serialize</param>
        /// <param name="target">target JSON writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonWriter)"/></param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> source, JsonWriter target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            bool disposeTarget = true)
        {
            try
            {
                target.WriteStartArray();
                var nullHandledSerializer = serializer ?? target.AdaptedJsonSerializer();
                if (token.CanBeCanceled)
                {
                    foreach (var obj in source)
                    {
                        token.ThrowIfCancellationRequested();
                        nullHandledSerializer.Serialize(target, obj);
                    }
                }
                else
                {
                    foreach (var obj in source)
                    {
                        nullHandledSerializer.Serialize(target, obj);
                    }
                }
                target.WriteEndArray();
                target.Flush();
            }
            finally
            {
                target.DisposeIfRequired(disposeTarget);
            }
        }

        #endregion ToJsonArray region

        #region ToJson region

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="source">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJson<T>(this T source, JsonSerializer serializer = null,
            IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder(256);
            source.ToJson(stringBuilder, serializer, formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="source">input object to JSON serialize</param>
        /// <param name="target">target output string builder</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJson<T>(this T source, StringBuilder target, JsonSerializer serializer = null,
            IFormatProvider formatProvider = null)
        {
            source.ToJson(target.CreateWriter(formatProvider), serializer);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="source">input object to JSON serialize</param>
        /// <param name="target">target output stream</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T source, Stream target, JsonSerializer serializer = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeTarget = true)
        {
            using (var textWriter = target.CreateWriter(enc, bufferSize, disposeTarget))
            {
                source.ToJson(textWriter, serializer, false);
                target.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="source">input object to JSON serialize</param>
        /// <param name="target">target text writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T source, TextWriter target, JsonSerializer serializer = null,
            bool disposeTarget = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            using (var jsonWriter = nullHandledSerializer.AdaptedJsonWriter(target, disposeTarget))
            {
                nullHandledSerializer.Serialize(jsonWriter, source);
                target.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="source"/> using <paramref name="serializer"/>
        /// to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="source">input object to JSON serialize</param>
        /// <param name="target">target JSON writer</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonWriter)"/></param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T source, JsonWriter target, JsonSerializer serializer = null,
            bool disposeTarget = true)
        {
            (serializer ?? target.AdaptedJsonSerializer()).Serialize(target, source);
            target.Flush();
            target.DisposeIfRequired(disposeTarget);
        }

        #endregion ToJson region

        #region FromJson region

        /// <summary>
        /// Deserializes the JSON string of <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String Builder</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        public static T FromJson<T>(this StringBuilder source, JsonSerializer serializer = null)
        {
            return new SbReader(source).FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        public static T FromJson<T>(this string source, JsonSerializer serializer = null)
        {
            return source.CreateReader().FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON stream</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this Stream source, JsonSerializer serializer = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeSource = true)
        {
            return source.CreateReader(enc, bufferSize, disposeSource).FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">Text reader as data source</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this TextReader source, JsonSerializer serializer = null,
            bool disposeSource = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            return nullHandledSerializer.AdaptedJsonReader(source, disposeSource).FromJson<T>(nullHandledSerializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="source"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonReader)"/></param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this JsonReader source, JsonSerializer serializer = null,
            bool disposeSource = true)
        {
            try
            {
                return (serializer ?? source.AdaptedJsonSerializer()).Deserialize<T>(source);
            }
            finally
            {
                source.DisposeIfRequired(disposeSource);
            }
        }

        #endregion FromJson region

        #region FromJsonAsEnumerable region

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String builder</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this StringBuilder source,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken))
        {
            return new SbReader(source).FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this string source, JsonSerializer serializer = null, 
            CancellationToken token = default(CancellationToken))
        {
            return source.CreateReader().FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON stream</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this Stream source, JsonSerializer serializer = null, 
            CancellationToken token = default(CancellationToken), Encoding enc = null, 
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = true)
        {
            return source.CreateReader(enc, bufferSize, disposeSource).FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">JSON text reader as data source</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this TextReader source, JsonSerializer serializer = null, 
            CancellationToken token = default(CancellationToken), bool disposeSource = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            return nullHandledSerializer.AdaptedJsonReader(source, disposeSource)
                .FromJsonAsEnumerable<T>(nullHandledSerializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonReader)"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this JsonReader source, JsonSerializer serializer = null, 
            CancellationToken token = default(CancellationToken), bool disposeSource = true)
        {
            try
            {
                if (source.ThrowIfTokenNotStartArray()) yield break;
                var nullHandledSerializer = serializer ?? source.AdaptedJsonSerializer();
                if (token.CanBeCanceled)
                {
                    while (source.NotAnEndArrayToken())
                    {
                        token.ThrowIfCancellationRequested();
                        yield return source.FromJsonGetNext<T>(nullHandledSerializer);
                    }
                }
                else
                {
                    while (source.NotAnEndArrayToken())
                    {
                        yield return source.FromJsonGetNext<T>(nullHandledSerializer);
                    }
                }
            }
            finally
            {
                source.DisposeIfRequired(disposeSource);
            }
        }

        #endregion FromJsonAsEnumerable region

        #region FromJsonArrayParallely region

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a producer of objects (alone or among many others),
        /// performs JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="target"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> anywhere outside of this method 
        /// must be done with the proper setup of <paramref name="closeTarget"/> and <paramref name="forceCloseWhenError"/>
        /// (please look at the comments of those parameters for details).</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String builder</param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static void FromJsonArrayParallely<T>(this StringBuilder source, BlockingCollection<T> target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource consumerTokenSource = null,
            bool closeTarget = true, bool forceCloseWhenError = true)
        {
            new SbReader(source)
                .FromJsonArrayParallely(target, serializer, token, consumerTokenSource, closeTarget, forceCloseWhenError);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a producer of objects (alone or among many others),
        /// performs JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="target"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> anywhere outside of this method 
        /// must be done with the proper setup of <paramref name="closeTarget"/> and <paramref name="forceCloseWhenError"/>
        /// (please look at the comments of those parameters for details).</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON String</param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static void FromJsonArrayParallely<T>(this string source, BlockingCollection<T> target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken), 
            CancellationTokenSource consumerTokenSource = null,
            bool closeTarget = true, bool forceCloseWhenError = true)
        {
            source.CreateReader().FromJsonArrayParallely(target, serializer, token,
                consumerTokenSource, closeTarget, forceCloseWhenError);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a producer of objects (alone or among many others),
        /// performs JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="target"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> anywhere outside of this method 
        /// must be done with the proper setup of <paramref name="closeTarget"/> and <paramref name="forceCloseWhenError"/>
        /// (please look at the comments of those parameters for details).</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">source JSON stream</param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static void FromJsonArrayParallely<T>(this Stream source, BlockingCollection<T> target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken), 
            CancellationTokenSource consumerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeSource = true,
            bool closeTarget = true, bool forceCloseWhenError = true)
        {
            source.CreateReader(enc, bufferSize, disposeSource)
                .FromJsonArrayParallely(target, serializer, token, consumerTokenSource, closeTarget, forceCloseWhenError);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a producer of objects (alone or among many others),
        /// performs JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="target"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> anywhere outside of this method 
        /// must be done with the proper setup of <paramref name="closeTarget"/> and <paramref name="forceCloseWhenError"/>
        /// (please look at the comments of those parameters for details).</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">JSON text reader as data source</param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.Serializer"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static void FromJsonArrayParallely<T>(this TextReader source, BlockingCollection<T> target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource consumerTokenSource = null,
            bool disposeSource = true, bool closeTarget = true, bool forceCloseWhenError = true)
        {
            var nullHandledSerializer = serializer ?? CustomJson.Serializer();
            nullHandledSerializer.AdaptedJsonReader(source, disposeSource)
                .FromJsonArrayParallely(target, nullHandledSerializer, token, consumerTokenSource,
                    closeTarget, forceCloseWhenError);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, as a producer of objects (alone or among many others),
        /// performs JSON data deserialization with an expectation that <paramref name="source"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="target"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> anywhere outside of this method 
        /// must be done with the proper setup of <paramref name="closeTarget"/> and <paramref name="forceCloseWhenError"/>
        /// (please look at the comments of those parameters for details).</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="source">JSON reader as data source</param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer to use, if not supplied then internally uses <seealso cref="CustomJson.AdaptedJsonSerializer(JsonReader)"/></param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeSource">If true, <paramref name="source"/> is disposed after the deserialization</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static void FromJsonArrayParallely<T>(this JsonReader source, BlockingCollection<T> target,
            JsonSerializer serializer = null, CancellationToken token = default(CancellationToken),
            CancellationTokenSource consumerTokenSource = null,
            bool disposeSource = true, bool closeTarget = true, bool forceCloseWhenError = true)
        {
            var inerror = false;
            try
            {
                if (source.ThrowIfTokenNotStartArray()) return;
                var nullHandledSerializer = serializer ?? source.AdaptedJsonSerializer();
                while (source.NotAnEndArrayToken())
                {
                    target.Add(source.FromJsonGetNext<T>(nullHandledSerializer), token);
                }
            }
            catch
            {
                inerror = true;
                consumerTokenSource?.Cancel();
                throw;
            }
            finally
            {
                //obligation to close the collection.
                if (closeTarget || (forceCloseWhenError && inerror)) target.CompleteAdding();
                source.DisposeIfRequired(disposeSource);
            }
        }

        #endregion FromJsonArrayParallely region
    }
}