using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Extensions for PPC patterns.
    /// </summary>
    public static class PpcExts
    {
        //<<<<<<<<<<< SINGLE PRODUCER SINGLE CONSUMER

        /// <summary>
        /// Accepts an async producer action and a consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer, Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer, Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.ToAsync(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer, Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer, Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(new AsyncConsumer<T>(consumer), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(new[] {consumer}, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE CONSUMER

        /// <summary>
        /// Accepts an async producer action and a consumer action collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.Select(x => new AsyncConsumer<T>(x.ToAsync())).ToList(),
                token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.Select(x => new AsyncConsumer<T>(x)).ToList(), token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Action<IConsumerFeed<T>, CancellationToken> producer, IReadOnlyList<IConsumer<T>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToAsync().RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this Func<IConsumerFeed<T>, CancellationToken, Task> producer, IReadOnlyList<IConsumer<T>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new AsyncProducer<T>(producer).RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer instance and a collection of consumers. 
        /// Executes producer and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(consumers, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE CONSUMER

        /// <summary>
        /// Accepts an async producer action collection and a consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function collection and a consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer collection and an async consumer action instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producer,
            Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.ToAsync(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action collection and a consumer function instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer collection and a consumer function instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer collection and an async consumer function instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producer,
            Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(new AsyncConsumer<T>(consumer), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action collection and a consumer instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function collection and a consumer instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and a consumer instance. 
        /// Executes producers and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producers,
            IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(new[] {consumer}, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE CONSUMER

        /// <summary>
        /// Accepts an async producer action and a consumer action collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.Select(x => new AsyncConsumer<T>(x.ToAsync())).ToList(),
                token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function collection instances. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.RunProducerConsumerAsync(consumer.Select(x => new AsyncConsumer<T>(x)).ToList(), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<IConsumer<T>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x.ToAsync())).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer collection instance. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<IConsumer<T>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Select(x => new AsyncProducer<T>(x)).ToList()
                .RunProducerConsumerAsync(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers. 
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(consumers, new IdentityAdapter<T>(), token,
                bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(new[] {consumer}, listMaxSize, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer instance and collection of consumers, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer,
            IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(consumers, listMaxSize, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producers and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producers,
            IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(new[] {consumer}, listMaxSize, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task RunProducerConsumerAsync<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(consumers, new ListAdapter<T>(listMaxSize),
                token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a producer and a consumer instance, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="TProducer">Produced data type</typeparam>
        /// <typeparam name="TConsumer">Consumable data type</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task RunProducerConsumerAsync<TProducer, TConsumer>(this IProducer<TProducer> producer,
            IConsumer<TConsumer> consumer, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(new[] {consumer}, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a producer instance and a collection of consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producer and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="TProducer">Produced data type</typeparam>
        /// <typeparam name="TConsumer">Consumable data type</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task RunProducerConsumerAsync<TProducer, TConsumer>(this IProducer<TProducer> producer,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(consumers, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and a consumer instance, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="TProducer">Produced data type</typeparam>
        /// <typeparam name="TConsumer">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task RunProducerConsumerAsync<TProducer, TConsumer>(
            this IReadOnlyList<IProducer<TProducer>> producers,
            IConsumer<TConsumer> consumer, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(new[] {consumer}, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="TProducer">Produced data type</typeparam>
        /// <typeparam name="TConsumer">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task RunProducerConsumerAsync<TProducer, TConsumer>(
            this IReadOnlyList<IProducer<TProducer>> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return PpcPipeline<TProducer, TConsumer>.RunPpcAsync(token, bufferSize, adapter, producers, consumers);
        }
    }
}