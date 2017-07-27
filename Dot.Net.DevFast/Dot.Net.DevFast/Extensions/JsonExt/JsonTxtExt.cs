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
    public static class JsonTxtExt
    {
        #region ToJsonArrayParallelyAsync region

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            CancellationTokenSource producerTokenSource = null, IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) asynchronously while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CustomJsonSerializer(), token, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(serializer, CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// prepares the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> asynchronously while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task<string> ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            var stringBuilder = new StringBuilder();
            await blockingCollection.ToJsonArrayParallelyAsync(serializer, stringBuilder, token, producerTokenSource,
                formatProvider).ConfigureAwait(false);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="output"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            StringBuilder output, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(output, CancellationToken.None, producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="output"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            StringBuilder output, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CustomJsonSerializer(), output, token,
                producerTokenSource, formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="output"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer,
            StringBuilder output, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(serializer, output, CancellationToken.None,
                producerTokenSource,
                formatProvider);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="output"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static async Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer,
            StringBuilder output, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            IFormatProvider formatProvider = null)
        {
            using (var textWriter = output.CreateWriter(formatProvider))
            {
                await blockingCollection.ToJsonArrayParallelyAsync(serializer, textWriter, token, producerTokenSource,
                    false).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="outputStream"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            Stream outputStream, CancellationTokenSource producerTokenSource = null, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeStream = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(outputStream, CancellationToken.None,
                producerTokenSource, enc,
                bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="outputStream"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            Stream outputStream, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CustomJsonSerializer(), outputStream, token,
                producerTokenSource, enc,
                bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="outputStream"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, Stream outputStream, CancellationTokenSource producerTokenSource = null,
            Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(serializer, outputStream, CancellationToken.None,
                producerTokenSource, enc,
                bufferSize, disposeStream);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="outputStream"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static async Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, Stream outputStream, CancellationToken token,
            CancellationTokenSource producerTokenSource = null, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            using (var streamWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                await blockingCollection.ToJsonArrayParallelyAsync(serializer, streamWriter, token, producerTokenSource,
                    false).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="textWriter"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            TextWriter textWriter, CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(textWriter, CancellationToken.None, producerTokenSource,
                disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="textWriter"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            TextWriter textWriter, CancellationToken token, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CustomJsonSerializer(), textWriter, token,
                producerTokenSource,
                disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="textWriter"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, TextWriter textWriter, CancellationTokenSource producerTokenSource = null,
            bool disposeWriter = true)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(serializer, textWriter, CancellationToken.None,
                producerTokenSource,
                disposeWriter);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="textWriter"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static async Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, TextWriter textWriter, CancellationToken token,
            CancellationTokenSource producerTokenSource = null, bool disposeWriter = true)
        {
            var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter);
            using (jsonWriter)
            {
                await blockingCollection.ToJsonArrayParallelyAsync(serializer, jsonWriter, token, producerTokenSource)
                    .ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="jsonWriter"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonWriter jsonWriter, CancellationTokenSource producerTokenSource = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(jsonWriter, CancellationToken.None, producerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>) to <paramref name="jsonWriter"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonWriter jsonWriter, CancellationToken token, CancellationTokenSource producerTokenSource = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(CustomJsonSerializer(), jsonWriter, token,
                producerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="jsonWriter"/> asynchronously.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        public static Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, JsonWriter jsonWriter, CancellationTokenSource producerTokenSource = null)
        {
            return blockingCollection.ToJsonArrayParallelyAsync(serializer, jsonWriter, CancellationToken.None,
                producerTokenSource);
        }

        /// <summary>
        /// When employed into Parallel Producer-Consumer pattern, parallely (as consumer of objects)
        /// writes the JSON serialized string of objects of <paramref name="blockingCollection"/> using 
        /// <paramref name="serializer"/> to <paramref name="jsonWriter"/> asynchronously
        /// while observing <paramref name="token"/>.
        /// <para>IMPORTANT: When blocking collection is populated in parallel (producer side of objects),
        /// call to <seealso cref="BlockingCollection{T}.CompleteAdding"/> is MANDATORY before
        /// awaiting on this method otherwise the await will NEVER terminate (i.e. Deadlock).
        /// Best would be to wrap the <seealso cref="BlockingCollection{T}.CompleteAdding"/> call inside 
        /// finally block at producer side.</para>
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the blockingCollection data</typeparam>
        /// <param name="blockingCollection">input blocking collection to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="producerTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the producer side in case of error during json serialization (consumer side),
        /// pass the source of cancellation token which producer is observing.</param>
        public static async Task ToJsonArrayParallelyAsync<T>(this BlockingCollection<T> blockingCollection,
            JsonSerializer serializer, JsonWriter jsonWriter, CancellationToken token,
            CancellationTokenSource producerTokenSource = null)
        {
            try
            {
                jsonWriter.WriteStartArray();
                while (blockingCollection.TryTake(out T obj, Timeout.Infinite, token))
                {
                    await obj.ToJsonAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                }
                jsonWriter.WriteEndArray();
                await jsonWriter.FlushAsync(token).ConfigureAwait(false);
            }
            catch
            {
                producerTokenSource?.Cancel();
                throw;
            }
        }

        #endregion ToJsonArrayParallelyAsync region

        #region ToJsonArrayAsync region

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonArrayAsync<T>(this IEnumerable<T> collection, CancellationToken token,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(CustomJsonSerializer(), token, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, StringBuilder output,
            IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(output, CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            StringBuilder output, CancellationToken token, IFormatProvider formatProvider = null)
        {
            return collection.ToJsonArrayAsync(CustomJsonSerializer(), output, token, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="outputStream">target output stream</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="outputStream"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            Stream outputStream, CancellationToken token, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return collection.ToJsonArrayAsync(CustomJsonSerializer(), outputStream, token, enc, bufferSize,
                disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
            using (var streamWriter = outputStream.CreateWriter(enc, bufferSize, disposeStream))
            {
                await collection.ToJsonArrayAsync(serializer, streamWriter, token, false).ConfigureAwait(false);
                await outputStream.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            TextWriter textWriter, bool disposeWriter = true)
        {
            return collection.ToJsonArrayAsync(textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            return collection.ToJsonArrayAsync(CustomJsonSerializer(), textWriter, token, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonArrayAsync<T>(IEnumerable<T> collection, JsonSerializer serializer,
            TextWriter textWriter, bool disposeWriter = true)
        {
            return collection.ToJsonArrayAsync(serializer, textWriter, CancellationToken.None, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="serializer">JSON serializer to use</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static async Task ToJsonArrayAsync<T>(this IEnumerable<T> collection, JsonSerializer serializer,
            TextWriter textWriter, CancellationToken token, bool disposeWriter = true)
        {
            using (var jsonWriter = serializer.CreateJsonWriter(textWriter, disposeWriter))
            {
                await collection.ToJsonArrayAsync(serializer, jsonWriter, token).ConfigureAwait(false);
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter)
        {
            return collection.ToJsonArrayAsync(jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        /// <param name="token">cancellation token to observe</param>
        public static Task ToJsonArrayAsync<T>(this IEnumerable<T> collection,
            JsonWriter jsonWriter, CancellationToken token)
        {
            return collection.ToJsonArrayAsync(CustomJsonSerializer(), jsonWriter, token);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="collection"/> using <paramref name="serializer"/>
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object array</typeparam>
        /// <param name="collection">input object enumerable to JSON serialize</param>
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
        /// Returns the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonAsync<T>(this T obj, IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(CancellationToken.None, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task<string> ToJsonAsync<T>(this T obj, CancellationToken token,
            IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(CustomJsonSerializer(), token, formatProvider);
        }

        /// <summary>
        /// Returns the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="output"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="output">target output string builder</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static Task ToJsonAsync<T>(this T obj, StringBuilder output,
            CancellationToken token, IFormatProvider formatProvider = null)
        {
            return obj.ToJsonAsync(CustomJsonSerializer(), output, token, formatProvider);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="output"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="outputStream"/> asynchronously while observing <paramref name="token"/>.
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
            return obj.ToJsonAsync(CustomJsonSerializer(), outputStream, token, enc, bufferSize, disposeStream);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="outputStream"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="textWriter"/> asynchronously while observing <paramref name="token"/>.
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static Task ToJsonAsync<T>(this T obj, TextWriter textWriter,
            CancellationToken token, bool disposeWriter = true)
        {
            return obj.ToJsonAsync(CustomJsonSerializer(), textWriter, token, disposeWriter);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using <paramref name="serializer"/>
        /// to <paramref name="textWriter"/> asynchronously.
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
        /// Writes the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/> asynchronously.
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of the input object to serialize</typeparam>
        /// <param name="obj">input object to JSON serialize</param>
        /// <param name="jsonWriter">target JSON writer (NOT disposed after the operation)</param>
        public static Task ToJsonAsync<T>(this T obj, JsonWriter jsonWriter)
        {
            return obj.ToJsonAsync(jsonWriter, CancellationToken.None);
        }

        /// <summary>
        /// Writes the JSON serialized string of <paramref name="obj"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>)
        /// to <paramref name="jsonWriter"/> asynchronously while observing <paramref name="token"/>.
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
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
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
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
        /// <para>IMPORTANT: <paramref name="jsonWriter"/> is NOT disposed in this operation.</para>
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

        /// <summary>
        /// Deserializes the JSON string of <paramref name="jsonStringBuilder"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStringBuilder">source JSON String Builder</param>
        public static T FromJson<T>(this StringBuilder jsonStringBuilder)
        {
            return jsonStringBuilder.FromJson<T>(CustomJsonSerializer());
        }

        /// <summary>
        /// Deserializes the JSON string of <paramref name="jsonStringBuilder"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonStringBuilder">source JSON String Builder</param>
        /// <param name="serializer">JSON serializer</param>
        public static T FromJson<T>(this StringBuilder jsonStringBuilder, JsonSerializer serializer)
        {
            return jsonStringBuilder.ToString().FromJson<T>(serializer);
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
        /// Deserializes the JSON data of <paramref name="jsonTextReader"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// <para>IMPORTANT: <paramref name="jsonTextReader"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonTextReader">Text reader as data source</param>
        /// <param name="disposeReader">If true, <paramref name="jsonTextReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this TextReader jsonTextReader, bool disposeReader = true)
        {
            return jsonTextReader.FromJson<T>(CustomJsonSerializer(), disposeReader);
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonTextReader"/> using <paramref name="serializer"/>.
        /// <para>IMPORTANT: <paramref name="jsonTextReader"/> is NOT disposed in this operation.</para>
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonTextReader">Text reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        /// <param name="disposeReader">If true, <paramref name="jsonTextReader"/> is disposed after the deserialization</param>
        public static T FromJson<T>(this TextReader jsonTextReader, JsonSerializer serializer, bool disposeReader = true)
        {
            using (var jsonReader = serializer.CreateJsonReader(jsonTextReader, disposeReader))
            {
                return jsonReader.FromJson<T>(serializer);
            }
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonReader"/> using custom <seealso cref="JsonSerializer"/> (use <see cref="CustomJsonSerializer"/>).
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        public static T FromJson<T>(JsonReader jsonReader)
        {
            return jsonReader.FromJson<T>(CustomJsonSerializer());
        }

        /// <summary>
        /// Deserializes the JSON data of <paramref name="jsonReader"/> using <paramref name="serializer"/>.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="jsonReader">JSON reader as data source</param>
        /// <param name="serializer">JSON serializer</param>
        public static T FromJson<T>(this JsonReader jsonReader, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(jsonReader);
        }

        #endregion ToJson region

        private static JsonSerializer DefaultJsonSerializer(this JsonWriter writer)
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

        //private class DecimalConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        JToken.FromObject((value as decimal?)?.ToString(serializer.Culture) ?? string.Empty).WriteTo(writer);
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        //        JsonSerializer serializer)
        //    {
        //        var token = JToken.Load(reader);
        //        return (token.Type == JTokenType.Null && objectType == typeof(decimal?))
        //            ? (object) null
        //            : token.ToString().ToDecimal(formatProvider: serializer.Culture);
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        //    }
        //}
    }
}