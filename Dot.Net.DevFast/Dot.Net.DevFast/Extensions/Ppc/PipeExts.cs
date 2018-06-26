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
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(new[] { consumer }, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE CONSUMER

        /// <summary>
        /// Accepts an async producer action and a consumer action collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer instance and a collection of consumers.
        /// Executes producer and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(consumers, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE CONSUMER

        /// <summary>
        /// Accepts an async producer action collection and a consumer action instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function collection and a consumer action instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer collection and an async consumer action instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer collection</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            Action<T, CancellationToken> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action collection and a consumer function instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer collection and a consumer function instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer collection and an async consumer function instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer collection</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            Func<T, CancellationToken, Task> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action collection and a consumer instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IConsumer<T> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function collection and a consumer instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and a consumer instance.
        /// Executes producers and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(new[] { consumer }, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE CONSUMER

        /// <summary>
        /// Accepts an async producer action and a consumer action collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer action collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer action collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance collection</param>
        /// <param name="consumer">consumer action collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Action<T, CancellationToken>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer action and a consumer function collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer and a consumer function collection instances.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and an async consumer function collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance collection</param>
        /// <param name="consumer">consumer function collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Func<T, CancellationToken, Task>> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), token, bufferSize);
        }

        /// <summary>
        /// Accepts a synchronous producer action and a consumer collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action collection</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts an async producer function and a consumer collection instance.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function collection</param>
        /// <param name="consumer">consumer instance collection</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<IConsumer<T>> consumer, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<T>> consumers, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers, IdentityAwaitableAdapter<T>.Default, token, bufferSize);
        }

        #endregion

        #region LIST ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, Action<List<T>, CancellationToken> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IConsumer<List<T>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer instance and collection of consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumers, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            Action<List<T>, CancellationToken> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            Func<List<T>, CancellationToken, Task> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IConsumer<List<T>> consumer, int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumer, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE LIST CONSUMER

        /// <summary>
        /// Accepts a producer action and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer action instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer action</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer function instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer function</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producer,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.Pipe(consumer.ToConsumer(), listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer action and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer action</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer function and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer function</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producer,
            IReadOnlyList<IConsumer<List<T>>> consumer, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producer.ToProducer().Pipe(consumer, listMaxSize, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).
        /// <para>Minimum acceptable size = 2.</para>
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers, listMaxSize, Timeout.Infinite, token, bufferSize);
        }

        #endregion

        #region AWAITABLE LIST ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE AWAITABLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IConsumer<List<T>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IConsumer<List<T>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IConsumer<List<T>> consumer, int listMaxSize,
            int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(new[] { consumer }, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE AWAITABLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Action<IConsumerFeed<T>, CancellationToken> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this Func<IConsumerFeed<T>, CancellationToken, Task> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer instance and collection of consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producer and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IProducer<T> producer, IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(consumers, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE AWAITABLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            Action<List<T>, CancellationToken> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            Func<List<T>, CancellationToken, Task> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers, IConsumer<List<T>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers, IConsumer<List<T>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and a consumer instance, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers, IConsumer<List<T>> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(new[] { consumer }, listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE AWAITABLE LIST CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<Action<List<T>, CancellationToken>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), listMaxSize, millisecondTimeout, token,
                bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Action<IConsumerFeed<T>, CancellationToken>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(
            this IReadOnlyList<Func<IConsumerFeed<T>, CancellationToken, Task>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer()
                .Pipe(consumers, listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, while using an internal
        /// list adapter (with given <paramref name="listMaxSize"/> and <paramref name="millisecondTimeout"/>) along to transforms the produced data into consumable list.
        /// Executes producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="T">Produced data type.</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. This is similar
        /// to using another overloaded version without the <paramref name="millisecondTimeout" />, though,
        /// provides an improvement when you do NOT want to wait for the SLOW producer to produce items
        /// to fill the whole list instead would prefer to consume what is available within this timeout.
        /// </param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task Pipe<T>(this IReadOnlyList<IProducer<T>> producers,
            IReadOnlyList<IConsumer<List<T>>> consumers, int listMaxSize, int millisecondTimeout,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers,
                new IdentityAwaitableListAdapter<T>(listMaxSize, millisecondTimeout),
                token, bufferSize);
        }

        #endregion

        #region GENERIC ADAPTER

        //<<<<<<<<<<< SINGLE PRODUCER SINGLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers, IConsumer<TC> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers, IConsumer<TC> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer and a consumer instance, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producer and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producer,
            IConsumer<TC> consumer, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(new[] { consumer }, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< SINGLE PRODUCER MULTIPLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Action<IConsumerFeed<TP>, CancellationToken> producers,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this Func<IConsumerFeed<TP>, CancellationToken, Task> producers,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a producer instance and a collection of consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producer and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IProducer<TP> producer,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] { producer }.Pipe(consumers, adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE PRODUCER SINGLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            Action<TC, CancellationToken> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            Func<TC, CancellationToken, Task> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            IConsumer<TC> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            IConsumer<TC> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and a consumer instance, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumer concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            IConsumer<TC> consumer, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(new[] { consumer }, adapter, token, bufferSize);
        }
        //<<<<<<<<<<< MULTIPLE PRODUCER MULTIPLE ADAPTER CONSUMER

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            IReadOnlyList<Action<TC, CancellationToken>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.Pipe(consumers.ToConsumer(), adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Action<IConsumerFeed<TP>, CancellationToken>> producers,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(
            this IReadOnlyList<Func<IConsumerFeed<TP>, CancellationToken, Task>> producers,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.ToProducer().Pipe(consumers, adapter, token, bufferSize);
        }

        /// <summary>
        /// Accepts a collection of producers and consumers, along with an instance of
        /// data adapter which transforms the produced data into consumable data type. Executes
        /// producers and consumers concurrently (parallel producer-consumer pattern) while mediating
        /// data transfer using a buffer of given size (refer <seealso cref="ConcurrentBuffer" /> properties
        /// for available standard buffer size); at the same time, observing given cancellation token.
        /// <para>IMPORTANT: Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" /></para>
        /// <para>
        /// Upon receiving exception from either producer/consumer instances would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </para>
        /// </summary>
        /// <typeparam name="TP">Produced data type</typeparam>
        /// <typeparam name="TC">Consumable data type</typeparam>
        /// <param name="producers">Collection of producers</param>
        /// <param name="consumers">Collection of consumers</param>
        /// <param name="adapter">data adapter</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">size of data buffer</param>
        public static Task Pipe<TP, TC>(this IReadOnlyList<IProducer<TP>> producers,
            IReadOnlyList<IConsumer<TC>> consumers, IDataAdapter<TP, TC> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return PpcPipeline<TP, TC>.Execute(token, bufferSize, adapter, producers, consumers);
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