using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dot.Net.DevFast.Extensions.JsonExt
{
    /// <summary>
    /// Extensions of Json serializations.
    /// </summary>
    public static class JsonTxtExt
    {
        #region ToJsonArrayParallely region

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            CancellationTokenSource producerTokenSource = null, IFormatProvider formatProvider = null)
        {
            return sourceCollection.ToJsonArrayParallely(CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return sourceCollection.ToJsonArrayParallely(CustomJsonSerializer(), token, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return sourceCollection.ToJsonArrayParallely(serializer, CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            sourceCollection.ToJsonArrayParallely(serializer, stringBuilder, token, producerTokenSource,
                formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="output"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            StringBuilder output, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            sourceCollection.ToJsonArrayParallely(output, CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="output"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            StringBuilder output, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            sourceCollection.ToJsonArrayParallely(CustomJsonSerializer(), output, token,
                producerTokenSource, formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="output"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, StringBuilder output, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            sourceCollection.ToJsonArrayParallely(serializer, output, CancellationToken.None,
                producerTokenSource, formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="output"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, StringBuilder output, CancellationToken token,
            CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            sourceCollection.ToJsonArrayParallely(serializer, output.CreateWriter(formatProvider), token,
                producerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="outputStream"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            Stream outputStream, CancellationTokenSource producerTokenSource = null, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeStream = true)
        {
            sourceCollection.ToJsonArrayParallely(outputStream, CancellationToken.None,
                producerTokenSource, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="outputStream"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            Stream outputStream, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            sourceCollection.ToJsonArrayParallely(CustomJsonSerializer(), outputStream, token,
                producerTokenSource, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="outputStream"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, Stream outputStream, CancellationTokenSource producerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            sourceCollection.ToJsonArrayParallely(serializer, outputStream, CancellationToken.None,
                producerTokenSource, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="outputStream"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, Stream outputStream, CancellationToken token,
            CancellationTokenSource producerTokenSource = null, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            using (var streamWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                sourceCollection.ToJsonArrayParallely(serializer, streamWriter, token, producerTokenSource, false);
                outputStream.Flush();
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="textWriter"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            TextWriter textWriter, CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(textWriter, CancellationToken.None, producerTokenSource,
                disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="textWriter"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            TextWriter textWriter, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(CustomJsonSerializer(), textWriter, token,
                producerTokenSource, disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="textWriter"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, TextWriter textWriter, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(serializer, textWriter, CancellationToken.None,
                producerTokenSource, disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="textWriter"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, TextWriter textWriter, CancellationToken token,
            CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            using (var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter))
            {
                sourceCollection.ToJsonArrayParallely(serializer, jsonWriter, token, producerTokenSource, false);
                textWriter.Flush();
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="jsonWriter"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonWriter jsonWriter, CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(jsonWriter, CancellationToken.None, producerTokenSource,
                disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="jsonWriter"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonWriter jsonWriter, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(jsonWriter.AdaptJsonSerializer(), jsonWriter, token,
                producerTokenSource, disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="jsonWriter"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, JsonWriter jsonWriter, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            sourceCollection.ToJsonArrayParallely(serializer, jsonWriter, CancellationToken.None,
                producerTokenSource, disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="sourceCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="jsonWriter"/>
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the sourceCollection data</typeparam>
        /// <param name="sourceCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArrayParallely<T>(this BlockingCollection<T> sourceCollection,
            JsonSerializer serializer, JsonWriter jsonWriter, CancellationToken token,
            CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            try
            {
                jsonWriter.WriteStartArray();
                while (sourceCollection.TryTake(out T obj, Timeout.Infinite, token))
                {
                    obj.ToJson(serializer, jsonWriter, false);
                }
                jsonWriter.WriteEndArray();
                jsonWriter.Flush();
            }
            catch
            {
                producerTokenSource?.Cancel();
                throw;
            }
            finally
            {
                jsonWriter.DisposeIfRequired(disposeWriter);
            }
        }

        #endregion ToJsonArrayParallely region

        #region ToJsonArray region

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArray<T>(this IEnumerable<T> collection,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArray(CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArray<T>(this IEnumerable<T> collection, CancellationToken token,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArray(CustomJsonSerializer(), token, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArray(serializer, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            CancellationToken token, IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            collection.ToJsonArray(serializer, stringBuilder, token, formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, StringBuilder output,
            IFormatProvider formatProvider = null)
        {
            collection.ToJsonArray(output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            collection.ToJsonArray(CustomJsonSerializer(), output, token, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            StringBuilder output, IFormatProvider formatProvider = null)
        {
            collection.ToJsonArray(serializer, output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            collection.ToJsonArray(serializer, output.CreateWriter(formatProvider), token);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            Stream outputStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            collection.ToJsonArray(outputStream, CancellationToken.None, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            Stream outputStream, CancellationToken token, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            collection.ToJsonArray(CustomJsonSerializer(), outputStream, token, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            Stream outputStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            collection.ToJsonArray(serializer, outputStream, CancellationToken.None, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            Stream outputStream, CancellationToken token, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            using (var streamWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                collection.ToJsonArray(serializer, streamWriter, token, false);
                outputStream.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            TextWriter textWriter, bool disposeWriter = true)
        {
            collection.ToJsonArray(textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            collection.ToJsonArray(CustomJsonSerializer(), textWriter, token, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(IEnumerable<T> collection, JsonSerializer serializer,
            TextWriter textWriter, bool disposeWriter = true)
        {
            collection.ToJsonArray(serializer, textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            using (var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter))
            {
                collection.ToJsonArray(serializer, jsonWriter, token, false);
                textWriter.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter, bool disposeWriter = true)
        {
            collection.ToJsonArray(jsonWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter, CancellationToken token, bool disposeWriter = true)
        {
            collection.ToJsonArray(jsonWriter.AdaptJsonSerializer(), jsonWriter, token, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            JsonWriter jsonWriter, bool disposeWriter = true)
        {
            collection.ToJsonArray(serializer, jsonWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJsonArray<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            JsonWriter jsonWriter, CancellationToken token, bool disposeWriter = true)
        {
            try
            {
                jsonWriter.WriteStartArray();
                foreach (var obj in collection)
                {
                    token.ThrowIfCancellationRequested();
                    obj.ToJson(serializer, jsonWriter, false);
                }
                jsonWriter.WriteEndArray();
                jsonWriter.Flush();
            }
            finally
            {
                jsonWriter.DisposeIfRequired(disposeWriter);
            }
        }

        #endregion ToJsonArray region

        #region ToJson region

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJson<T>(this T obj, IFormatProvider formatProvider = null)
        {
            return obj.ToJson(CustomJsonSerializer(), formatProvider);
        }
        
        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static string ToJson<T>(this T obj, JsonSerializer serializer,
            IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            obj.ToJson(serializer, stringBuilder, formatProvider);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJson<T>(this T obj, StringBuilder output,
            IFormatProvider formatProvider = null)
        {
            obj.ToJson(CustomJsonSerializer(), output, formatProvider);
        }
        
        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static void ToJson<T>(this T obj, JsonSerializer serializer,
            StringBuilder output, IFormatProvider formatProvider = null)
        {
            obj.ToJson(serializer, output.CreateWriter(formatProvider));
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, Stream outputStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            obj.ToJson(CustomJsonSerializer(), outputStream, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, JsonSerializer serializer,
            Stream outputStream, Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeStream = true)
        {
            using (var textWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                obj.ToJson(serializer, textWriter, false);
                outputStream.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, TextWriter textWriter, bool disposeWriter = true)
        {
            obj.ToJson(CustomJsonSerializer(), textWriter, disposeWriter);
        }
        
        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, JsonSerializer serializer,
            TextWriter textWriter, bool disposeWriter = true)
        {
            using (var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter))
            {
                obj.ToJson(serializer, jsonWriter, false);
                textWriter.Flush();
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, JsonWriter jsonWriter, bool disposeWriter = true)
        {
            obj.ToJson(jsonWriter.AdaptJsonSerializer(), jsonWriter, disposeWriter);
        }
        
        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer</param>
        /// <param name="disposeWriter">If true, <paramref name="jsonWriter"/> is disposed after the serialization</param>
        public static void ToJson<T>(this T obj, JsonSerializer serializer, JsonWriter jsonWriter, bool disposeWriter = true)
        {
            try
            {
                serializer.Serialize(jsonWriter, obj);
                jsonWriter.Flush();
            }
            finally
            {
                jsonWriter.DisposeIfRequired(disposeWriter);
            }
        }

        #endregion ToJson region

        #region FromJson region

        /// <summary>
        /// Deserializes the JSON string of <paramref name="jsonSource"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String Builder</param>
        public static T FromJson<T>(this StringBuilder jsonSource)
        {
            return jsonSource.FromJson<T>(CustomJsonSerializer());
        }

        /// <summary>
        /// Deserializes the JSON string of <paramref name="jsonSource"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String Builder</param>
        /// <param name="serializer">JSON serializer</param>
        public static T FromJson<T>(this StringBuilder jsonSource, JsonSerializer serializer)
        {
            return jsonSource.ToString().FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes <paramref name="jsonString"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        public static T FromJson<T>(this string jsonString)
        {
            return jsonString.FromJson<T>(CustomJsonSerializer());
        }

        /// <summary>
        /// Deserializes <paramref name="jsonString"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="serializer">JSON serializer</param>
        public static T FromJson<T>(this string jsonString, JsonSerializer serializer)
        {
            return new StringReader(jsonString).FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonStream"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this Stream jsonStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.FromJson<T>(CustomJsonSerializer(), enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonStream"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this Stream jsonStream, JsonSerializer serializer,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.CreateReader(enc, bufferSize, disposeStream).FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="textReader"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">Text reader as data source</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this TextReader textReader, bool disposeReader = true)
        {
            return textReader.FromJson<T>(CustomJsonSerializer(), disposeReader);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="textReader"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">Text reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this TextReader textReader, JsonSerializer serializer, bool disposeReader = true)
        {
            return serializer.CreateJsonReader(textReader, disposeReader).FromJson<T>(serializer);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonReader"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(JsonReader jsonReader, bool disposeReader = true)
        {
            return jsonReader.FromJson<T>(jsonReader.AdaptJsonSerializer(), disposeReader);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonReader"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this JsonReader jsonReader, JsonSerializer serializer, bool disposeReader = true)
        {
            try
            {
                return serializer.Deserialize<T>(jsonReader);
            }
            finally
            {
                jsonReader.DisposeIfRequired(disposeReader);
            }
        }

        #endregion FromJson region

        #region FromJsonAsEnumerable region

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this StringBuilder jsonSource)
        {
            return jsonSource.FromJsonAsEnumerable<T>(CancellationToken.None);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this StringBuilder jsonSource,
            CancellationToken token)
        {
            return jsonSource.FromJsonAsEnumerable<T>(CustomJsonSerializer(), token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="serializer">JSON serializer</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this StringBuilder jsonSource,
            JsonSerializer serializer)
        {
            return jsonSource.FromJsonAsEnumerable<T>(serializer, CancellationToken.None);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this StringBuilder jsonSource,
            JsonSerializer serializer, CancellationToken token)
        {
            return jsonSource.ToString().FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this string jsonString)
        {
            return jsonString.FromJsonAsEnumerable<T>(CancellationToken.None);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this string jsonString, CancellationToken token)
        {
            return jsonString.FromJsonAsEnumerable<T>(CustomJsonSerializer(), token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="serializer">JSON serializer</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this string jsonString, JsonSerializer serializer)
        {
            return jsonString.FromJsonAsEnumerable<T>(CustomJsonSerializer(), CancellationToken.None);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this string jsonString, JsonSerializer serializer,
            CancellationToken token)
        {
            return new StringReader(jsonString).FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this Stream jsonStream,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.FromJsonAsEnumerable<T>(CancellationToken.None, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this Stream jsonStream, CancellationToken token,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.FromJsonAsEnumerable<T>(CustomJsonSerializer(), token, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this Stream jsonStream, JsonSerializer serializer,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.FromJsonAsEnumerable<T>(serializer, CancellationToken.None, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this Stream jsonStream, JsonSerializer serializer,
            CancellationToken token, Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return jsonStream.CreateReader(enc, bufferSize, disposeStream).FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this TextReader textReader, bool disposeReader = true)
        {
            return textReader.FromJsonAsEnumerable<T>(CancellationToken.None, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this TextReader textReader, CancellationToken token,
            bool disposeReader = true)
        {
            return textReader.FromJsonAsEnumerable<T>(CustomJsonSerializer(), token, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this TextReader textReader, JsonSerializer serializer,
            bool disposeReader = true)
        {
            return textReader.FromJsonAsEnumerable<T>(serializer, CancellationToken.None, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this TextReader textReader, JsonSerializer serializer,
            CancellationToken token, bool disposeReader = true)
        {
            return serializer.CreateJsonReader(textReader, disposeReader).FromJsonAsEnumerable<T>(serializer, token);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this JsonReader jsonReader, bool disposeReader = false)
        {
            return jsonReader.FromJsonAsEnumerable<T>(CancellationToken.None, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this JsonReader jsonReader, CancellationToken token,
            bool disposeReader = false)
        {
            return jsonReader.FromJsonAsEnumerable<T>(jsonReader.AdaptJsonSerializer(), token, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this JsonReader jsonReader, JsonSerializer serializer,
            bool disposeReader = false)
        {
            return jsonReader.FromJsonAsEnumerable<T>(serializer, CancellationToken.None, disposeReader);
        }

        /// <summary>
        /// A simple enumerator on JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time for enumeration until <seealso cref="JsonToken.EndArray"/> is encountered OR <paramref name="token"/> is cancelled.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static IEnumerable<T> FromJsonAsEnumerable<T>(this JsonReader jsonReader, JsonSerializer serializer,
            CancellationToken token, bool disposeReader = true)
        {
            try
            {
                if (jsonReader.ThrowIfTokenNotStartArray()) yield break;
                while (jsonReader.NotAnEndArrayToken())
                {
                    token.ThrowIfCancellationRequested();
                    yield return jsonReader.FromJsonGetNext<T>(serializer);
                }
            }
            finally
            {
                jsonReader.DisposeIfRequired(disposeReader);
            }
        }

        #endregion FromJsonAsEnumerable region

        #region FromJsonArrayParallely region

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this StringBuilder jsonSource, BlockingCollection<T> targetCollection,
            CancellationTokenSource consumerTokenSource = null)
        {
            jsonSource.FromJsonArrayParallely(targetCollection, CancellationToken.None, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this StringBuilder jsonSource, BlockingCollection<T> targetCollection,
            CancellationToken token, CancellationTokenSource consumerTokenSource = null)
        {
            jsonSource.FromJsonArrayParallely(targetCollection, CustomJsonSerializer(), token, consumerTokenSource);
        }
        
        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this StringBuilder jsonSource, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationTokenSource consumerTokenSource = null)
        {
            jsonSource.FromJsonArrayParallely(targetCollection, serializer, CancellationToken.None, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonSource"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonSource">source JSON String builder</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this StringBuilder jsonSource, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource consumerTokenSource = null)
        {
            jsonSource.ToString().FromJsonArrayParallely(targetCollection, serializer, token, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this string jsonString, BlockingCollection<T> targetCollection,
            CancellationTokenSource consumerTokenSource = null)
        {
            jsonString.FromJsonArrayParallely(targetCollection, CancellationToken.None, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this string jsonString, BlockingCollection<T> targetCollection,
            CancellationToken token, CancellationTokenSource consumerTokenSource = null)
        {
            jsonString.FromJsonArrayParallely(targetCollection, CustomJsonSerializer(), token, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this string jsonString, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationTokenSource consumerTokenSource = null)
        {
            jsonString.FromJsonArrayParallely(targetCollection, serializer, CancellationToken.None,consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonString"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonString">source JSON String</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        public static void FromJsonArrayParallely<T>(this string jsonString, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource consumerTokenSource = null)
        {
            new StringReader(jsonString).FromJsonArrayParallely(targetCollection, serializer, token,
                consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this Stream jsonStream, BlockingCollection<T> targetCollection,
            CancellationTokenSource consumerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            jsonStream.FromJsonArrayParallely(targetCollection, CancellationToken.None, consumerTokenSource, enc,
                bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this Stream jsonStream, BlockingCollection<T> targetCollection,
            CancellationToken token, CancellationTokenSource consumerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            jsonStream.FromJsonArrayParallely(targetCollection, CustomJsonSerializer(), token, consumerTokenSource,
                enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this Stream jsonStream, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationTokenSource consumerTokenSource = null,
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            jsonStream.FromJsonArrayParallely(targetCollection, serializer, CancellationToken.None, consumerTokenSource,
                enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonStream"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStream">source JSON stream</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeStream">If true, <paramref name="jsonStream"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this Stream jsonStream, BlockingCollection<T> targetCollection, 
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource consumerTokenSource = null, 
            Encoding enc = null, int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            jsonStream.CreateReader(enc, bufferSize, disposeStream)
                .FromJsonArrayParallely(targetCollection, serializer, token, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this TextReader textReader, BlockingCollection<T> targetCollection,
            CancellationTokenSource consumerTokenSource = null, bool disposeReader = true)
        {
            textReader.FromJsonArrayParallely(targetCollection, CancellationToken.None, consumerTokenSource, disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this TextReader textReader, BlockingCollection<T> targetCollection,
            CancellationToken token, CancellationTokenSource consumerTokenSource = null, bool disposeReader = true)
        {
            textReader.FromJsonArrayParallely(targetCollection, CustomJsonSerializer(), token, consumerTokenSource,
                disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this TextReader textReader, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationTokenSource consumerTokenSource = null,
            bool disposeReader = true)
        {
            textReader.FromJsonArrayParallely(targetCollection, serializer, CancellationToken.None,
                consumerTokenSource, disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="textReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="textReader">JSON text reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this TextReader textReader, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource consumerTokenSource = null,
            bool disposeReader = true)
        {
            serializer.CreateJsonReader(textReader, disposeReader)
                .FromJsonArrayParallely(targetCollection, serializer, token, consumerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this JsonReader jsonReader, BlockingCollection<T> targetCollection,
            CancellationTokenSource consumerTokenSource = null, bool disposeReader = true)
        {
            jsonReader.FromJsonArrayParallely(targetCollection, CancellationToken.None, consumerTokenSource,
                disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>),
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this JsonReader jsonReader, BlockingCollection<T> targetCollection,
            CancellationToken token, CancellationTokenSource consumerTokenSource = null,
            bool disposeReader = true)
        {
            jsonReader.FromJsonArrayParallely(targetCollection, jsonReader.AdaptJsonSerializer(), token,
                consumerTokenSource, disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this JsonReader jsonReader, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationTokenSource consumerTokenSource = null,
            bool disposeReader = true)
        {
            jsonReader.FromJsonArrayParallely(targetCollection, serializer, CancellationToken.None,
                consumerTokenSource, disposeReader);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as producer of objects),
        /// performs JSON data deserialization with an expectation that <paramref name="jsonReader"/> will
        /// start reading from <seealso cref="JsonToken.StartArray"/>. Parses array objects, using <paramref name="serializer"/>,
        /// one at a time to populate <paramref name="targetCollection"/> until <seealso cref="JsonToken.EndArray"/> is encountered
        /// OR <paramref name="token"/> is cancelled.
        /// <para>IMPORTANT: When blocking collection is consumed in parallel (consumer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is FORBIDDEN anywhere outside of this method.
        /// Call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is properly handled inside this method.</para>
        /// You may think of passing <seealso cref="BlockingCollection{T}"/> with some pre-instrumented
        /// <seealso cref="BlockingCollection{T}.BoundedCapacity"/> (in most of the cases, 1 is sufficient) to minimize memory consumption.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="targetCollection">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="consumerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer side in case of error during json deserialization (producer side),
        /// pass the source of cancellation token which consumer is observing.</param>
        /// <param name="disposeReader">If true, <paramref name="jsonReader"/> is disposed after the deserialization</param>
        public static void FromJsonArrayParallely<T>(this JsonReader jsonReader, BlockingCollection<T> targetCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource consumerTokenSource = null,
            bool disposeReader = true)
        {
            try
            {
                if (jsonReader.ThrowIfTokenNotStartArray()) return;
                while (jsonReader.NotAnEndArrayToken())
                {
                    targetCollection.Add(jsonReader.FromJsonGetNext<T>(serializer), token);
                }
            }
            catch
            {
                consumerTokenSource?.Cancel();
                throw;
            }
            finally
            {
                //obligation to close the collection.
                targetCollection.CompleteAdding();
                jsonReader.DisposeIfRequired(disposeReader);
            }
        }

        #endregion FromJsonArrayParallely region

        #region Private Methods

        private static T FromJsonGetNext<T>(this JsonReader jsonReader, JsonSerializer serializer)
        {
            return ((jsonReader.TokenType == JsonToken.StartObject)
                ? JObject.Load(jsonReader)
                : JToken.Load(jsonReader)).ToObject<T>(serializer);
        }

        private static void DisposeIfRequired(this IDisposable disposable, bool dispose)
        {
            if (!dispose) return;
            using (disposable)
            {
                //to dispose
            }
        }

        private static bool NotAnEndArrayToken(this JsonReader jsonReader)
        {
            return (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray);
        }

        private static bool ThrowIfTokenNotStartArray(this JsonReader jsonReader)
        {
            if (!jsonReader.Read()) return true;
            return (jsonReader.TokenType == JsonToken.StartArray).ThrowIfNot(DdnDfErrorCode.JsonIsNotAnArray,
                () =>
                    $"JSON string does not start with start array token. Found token type is {jsonReader.TokenType:G}",
                false);
        }

        private static JsonSerializer AdaptJsonSerializer(this JsonWriter writer)
        {
            var serializer = CustomJsonSerializer();
            serializer.Culture = writer.Culture;
            serializer.DateFormatHandling = writer.DateFormatHandling;
            serializer.DateFormatString = writer.DateFormatString;
            serializer.DateTimeZoneHandling = writer.DateTimeZoneHandling;
            serializer.FloatFormatHandling = writer.FloatFormatHandling;
            serializer.Formatting = writer.Formatting;
            serializer.StringEscapeHandling = writer.StringEscapeHandling;
            return serializer;
        }

        private static JsonSerializer AdaptJsonSerializer(this JsonReader reader)
        {
            var serializer = CustomJsonSerializer();
            serializer.Culture = reader.Culture;
            serializer.DateFormatString = reader.DateFormatString;
            serializer.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            serializer.FloatParseHandling = reader.FloatParseHandling;
            serializer.DateParseHandling = reader.DateParseHandling;
            serializer.MaxDepth = reader.MaxDepth;
            return serializer;
        }

        #endregion Private Methods

        /// <summary>
        /// Returns a new instance of custom <seealso cref="JsonSerializer"/>.
        /// </summary>
        public static JsonSerializer CustomJsonSerializer()
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