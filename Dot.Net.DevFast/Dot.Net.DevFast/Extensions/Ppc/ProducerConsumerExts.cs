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
    public static partial class PpcExts
    {
        #region NO ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance. 
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(new[] {consumer}, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(consumers, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(new[] {consumer}, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, token, bufferSize);
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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<T>> consumers, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers, new IdentityAdapter<T>(), token, bufferSize);
        }

        #endregion

        #region LIST ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer, Action<List<T>, CancellationToken> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IConsumer<List<T>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

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
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumers, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumer, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ProducerConsumer(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal 
        /// list adapter (with given list max size) along to transforms the produced data into consumable list. 
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating 
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer"/> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded"/></para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para></param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().ProducerConsumer(consumer, listMaxSize, token, bufferSize);
        }

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
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        #endregion

        #region AWAITABLE LIST ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE AWAITABLE LIST CONSUMER

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IConsumer<List<T>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IConsumer<List<T>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IConsumer<List<T>> consumer, int listMaxSize,
            int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(new[] {consumer}, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE AWAITABLE LIST CONSUMER

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE AWAITABLE LIST CONSUMER

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers, IConsumer<List<T>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers, IConsumer<List<T>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<List<T>> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(new[] {consumer}, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE AWAITABLE LIST CONSUMER

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .ProducerConsumer(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

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
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.</param>
        /// <param name="millisecondTimeout">Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout"/>, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task ProducerConsumer<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers,
                new AwaitableListAdapter<T>(listMaxSize, millisecondTimeout),
                token, bufferSize);
        }

        #endregion

        #region GENERIC ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE ADAPTER CONSUMER

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers, IConsumer<TConsumer> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers, IConsumer<TConsumer> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producer,
            IConsumer<TConsumer> consumer, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(new[] {consumer}, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE ADAPTER CONSUMER

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Action<IConsumerFeed<TProducer>, CancellationToken> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this Func<IConsumerFeed<TProducer>, CancellationToken, Task> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IProducer<TProducer> producer,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.ProducerConsumer(consumers, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE ADAPTER CONSUMER

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            Action<TConsumer, CancellationToken> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            Func<TConsumer, CancellationToken, Task> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            IConsumer<TConsumer> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            IConsumer<TConsumer> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            IConsumer<TConsumer> consumer, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(new[] {consumer}, adapter, token, bufferSize);
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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            IReadOnlyList<Action<TConsumer, CancellationToken>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            IReadOnlyList<Func<TConsumer, CancellationToken, Task>> consumers,
            IDataAdapter<TProducer, TConsumer> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ProducerConsumer(consumers.ToConsumer(), adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Action<IConsumerFeed<TProducer>, CancellationToken>> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(
            this IReadOnlyList<Func<IConsumerFeed<TProducer>, CancellationToken, Task>> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().ProducerConsumer(consumers, adapter, token, bufferSize);
        }

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
        public static Task ProducerConsumer<TProducer, TConsumer>(this IReadOnlyList<IProducer<TProducer>> producers,
            IReadOnlyList<IConsumer<TConsumer>> consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return PpcPipeline<TProducer, TConsumer>.Execute(token, bufferSize, adapter, producers, consumers);
        }

        #endregion

        #region Convertors

        private static IProducer<T> ToProducer<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer)
        {
            return new AsyncProducer<T>(producer);
        }

        private static IProducer<T> ToProducer<T>(this Action<IConsumerFeed<T>, CancellationToken> producer)
        {
            return producer.ToAsync(false).ToProducer();
        }

        private static IReadOnlyList<IProducer<T>> ToProducer<T>(
            this IEnumerable<Func<IConsumerFeed<T>, CancellationToken, Task>> producers)
        {
            return producers.Select(x => x.ToProducer()).ToList();
        }

        private static IReadOnlyList<IProducer<T>> ToProducer<T>(
            this IEnumerable<Action<IConsumerFeed<T>, CancellationToken>> producers)
        {
            return producers.Select(x => x.ToProducer()).ToList();
        }

        private static IConsumer<T> ToConsumer<T>(this Func<T, CancellationToken, Task> consumer)
        {
            return new AsyncConsumer<T>(consumer);
        }

        private static IConsumer<T> ToConsumer<T>(this Action<T, CancellationToken> consumer)
        {
            return consumer.ToAsync(false).ToConsumer();
        }

        private static IReadOnlyList<IConsumer<T>> ToConsumer<T>(
            this IEnumerable<Func<T, CancellationToken, Task>> consumers)
        {
            return consumers.Select(x => x.ToConsumer()).ToList();
        }

        private static IReadOnlyList<IConsumer<T>> ToConsumer<T>(
            this IEnumerable<Action<T, CancellationToken>> consumers)
        {
            return consumers.Select(x => x.ToConsumer()).ToList();
        }

        #endregion
    }
}