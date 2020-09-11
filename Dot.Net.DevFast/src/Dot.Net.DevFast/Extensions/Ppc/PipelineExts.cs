using System;
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
    public static partial class PpcExts
    {
        #region NO ADAPTER

        //<<<<<<<<<<< SINGLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this Action<T, CancellationToken> consumer,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this IConsumer<T> consumer,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.Pipeline(token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(
            this IReadOnlyList<Action<T, CancellationToken>> consumers,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(
            this IReadOnlyList<Func<T, CancellationToken, Task>> consumers,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default, int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.Pipeline(IdentityAwaitableAdapter<T>.Default, token, bufferSize);
        }

        #endregion

        #region AWAITABLE LIST ADAPTER

        //<<<<<<<<<<< SINGLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this Action<List<T>, CancellationToken> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this Func<List<T>, CancellationToken, Task> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this IConsumer<List<T>> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.Pipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(
            this IReadOnlyList<Action<List<T>, CancellationToken>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(
            this IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{T}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and list-adapter (with given
        /// <paramref name="listMaxSize" /> and
        /// <paramref name="millisecondTimeout" />).
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Be careful with <seealso cref="Timeout.Infinite" /> timeouts as items in the list will get processed
        /// only when the list reaches its capacity. In effect, if for long time, no item was added to the pipeline
        /// item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{T}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{T}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// Type of items <seealso cref="IPipeline{T}" /> will accept and whose items list
        /// <seealso cref="IConsumer{T}" /> will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="listMaxSize">
        /// Maximum number of items to be in the list given to consumer.
        /// Basically, all the list will have AT LEAST 1 item and maximum this given size.
        /// </param>
        /// <param name="millisecondTimeout">
        /// Maximum time to await on produced items. Be careful with <seealso cref="Timeout.Infinite" /> timeouts as
        /// items in the list will get processed only when the list reaches its capacity. In effect, if for long time,
        /// no item was added to the pipeline item processing will get delayed. On the other hand, 0 as timeout value is very well accepted.
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<T> Pipeline<T>(this IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.Pipeline(new IdentityAwaitableListAdapter<T>(listMaxSize, millisecondTimeout), token,
                bufferSize);
        }

        #endregion

        #region GENERIC ADAPTER

        //<<<<<<<<<<< SINGLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(this Action<TC, CancellationToken> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(
            this Func<TC, CancellationToken, Task> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().Pipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumer" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(this IConsumer<TC> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.Pipeline(adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(
            this IReadOnlyList<Action<TC, CancellationToken>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(
            this IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().Pipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IPipeline{TP}" /> as an end-point to accept data while
        /// executing given <paramref name="consumers" /> concurrently. Pipeline is responcible for data transfer with the help
        /// of a buffer (with size= <paramref name="bufferSize" />) and <paramref name="adapter" />.
        /// <para>Ideally, this instance one would like to use as a singleton for the application life time.</para>
        /// <para>IMPORTANT:</para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IProducerBuffer{TP}.Add" /> will be made, to avoid unexpected errors.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Upon receiving exception from any of consumer instance would result in the destruction of the
        /// pipeline (all producers and consumers will be destroyed including the queued data)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IProducerBuffer{TP}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IPipeline{TP}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IPipeline<TP> Pipeline<TP, TC>(this IReadOnlyList<IConsumer<TC>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default,
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new Pipeline<TP, TC>(consumers, adapter, token, bufferSize);
        }

        #endregion
    }
}