using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Extensions for PPC patterns.
    /// </summary>
    public static class PpcExts
    {
        /// <summary>
        /// Creates Parallel producer-consumer between 1 producer and 1 consumer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T> producer, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemDistributor<T>(token, bufferSize).ConnectAsync(new[] {producer}, consumer);
        }

        /// <summary>
        /// Creates Parallel producer-consumer among 1 producer and multiple consumers.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">consumer instances</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T> producer, IConsumer<T>[] consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemDistributor<T>(token, bufferSize).ConnectAsync(new[] {producer}, consumers);
        }

        /// <summary>
        /// Creates Parallel producer-consumer among multiple producers and consumers.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producers">producer instances</param>
        /// <param name="consumers">consumer instances</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T>[] producers, IConsumer<T>[] consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemDistributor<T>(token, bufferSize).ConnectAsync(producers, consumers);
        }

        /// <summary>
        /// Creates Parallel producer-consumer among multiple producers and 1 consumer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producers">producer instances</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T>[] producers, IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemDistributor<T>(token, bufferSize).ConnectAsync(producers, consumer);
        }

        /// <summary>
        /// Creates Parallel producer-consumer between 1 producer and 1 consumer.
        /// <para>Useful when data items are produced in unity but it is possible to do batch processing.
        /// <paramref name="listMaxSize"/> represents the data batch size.</para>
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T> producer, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemCollectionDistributor<T>(listMaxSize, token, bufferSize).ConnectAsync(new[] {producer},
                consumer);
        }

        /// <summary>
        /// Creates Parallel producer-consumer between 1 producer and 1 consumer.
        /// <para>Useful when data items are produced in unity but it is possible to do batch processing.
        /// <paramref name="listMaxSize"/> represents the data batch size.</para>
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producer">producer instance</param>
        /// <param name="consumers">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T> producer, IConsumer<List<T>>[] consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemCollectionDistributor<T>(listMaxSize, token, bufferSize).ConnectAsync(new[] {producer},
                consumers);
        }

        /// <summary>
        /// Creates Parallel producer-consumer between 1 producer and 1 consumer.
        /// <para>Useful when data items are produced in unity but it is possible to do batch processing.
        /// <paramref name="listMaxSize"/> represents the data batch size.</para>
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producers">producer instance</param>
        /// <param name="consumers">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T>[] producers, IConsumer<List<T>>[] consumers,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemCollectionDistributor<T>(listMaxSize, token, bufferSize).ConnectAsync(producers, consumers);
        }

        /// <summary>
        /// Creates Parallel producer-consumer between 1 producer and 1 consumer.
        /// <para>Useful when data items are produced in unity but it is possible to do batch processing.
        /// <paramref name="listMaxSize"/> represents the data batch size.</para>
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="producers">producer instance</param>
        /// <param name="consumer">consumer instance</param>
        /// <param name="listMaxSize">Maximum number of items to be in the list given to consumer. 
        /// Basically, all the list will have max items, but the last one (it would contain
        /// remaining produced items).</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="bufferSize">buffer size</param>
        public static Task CreatePpc<T>(this IProducer<T>[] producers, IConsumer<List<T>> consumer,
            int listMaxSize, CancellationToken token = default(CancellationToken),
            int bufferSize = ParallelBuffer.StandardSize)
        {
            return new ItemCollectionDistributor<T>(listMaxSize, token, bufferSize).ConnectAsync(producers, consumer);
        }
    }
}