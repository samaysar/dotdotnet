using System.Collections.Generic;
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer, IConsumer<T>[] consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(consumers, token, bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T>[] producers, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(new[] {consumer}, token, bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T>[] producers, IConsumer<T>[] consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(consumers, new IdentityAdapter<T>(), token,
                bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(new[] {consumer}, listMaxSize, token, bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T> producer, IConsumer<List<T>>[] consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {producer}.RunProducerConsumerAsync(consumers, listMaxSize, token, bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T>[] producers, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(new[] {consumer}, listMaxSize, token, bufferSize);
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
        public static Task RunProducerConsumerAsync<T>(this IProducer<T>[] producers, IConsumer<List<T>>[] consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return producers.RunProducerConsumerAsync(consumers, new ListAdapter<T>(listMaxSize),
                token, bufferSize);
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
        public static Task RunProducerConsumerAsync<TProducer, TConsumer>(this IProducer<TProducer>[] producers,
            IConsumer<TConsumer>[] consumers, IDataAdapter<TProducer, TConsumer> adapter,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new PpcPipeline<TProducer, TConsumer>(token, bufferSize, adapter).RunPpcAsync(producers, consumers);
        }
    }
}