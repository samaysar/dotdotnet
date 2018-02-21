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
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this Action<T, CancellationToken> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this Func<T, CancellationToken, Task> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this IConsumer<T> consumer,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.ConcurrentPipeline(token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(
            this IReadOnlyList<Action<T, CancellationToken>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(
            this IReadOnlyList<Func<T, CancellationToken, Task>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and <seealso cref="IConsumer{T}" />
        /// will consume
        /// </typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this IReadOnlyList<IConsumer<T>> consumers,
            CancellationToken token = default(CancellationToken), int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ConcurrentPipeline(new IdentityAdapter<T>(), token, bufferSize);
        }

        #endregion

        #region AWAITABLE LIST ADAPTER

        //<<<<<<<<<<< SINGLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this Action<List<T>, CancellationToken> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this Func<List<T>, CancellationToken, Task> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this IConsumer<List<T>> consumer,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.ConcurrentPipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(
            this IReadOnlyList<Action<List<T>, CancellationToken>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(
            this IReadOnlyList<Func<List<T>, CancellationToken, Task>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(listMaxSize, millisecondTimeout, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Unbounded buffer size is represented by <seealso cref="ConcurrentBuffer.Unbounded" />
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
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
        /// Type of items <seealso cref="IConcurrentPipeline{T}" /> will accept and whose items list
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
        /// (see <seealso cref="AwaitableListAdapter{T}"/> for more info)
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<T> ConcurrentPipeline<T>(this IReadOnlyList<IConsumer<List<T>>> consumers,
            int listMaxSize, int millisecondTimeout, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ConcurrentPipeline(new AwaitableListAdapter<T>(listMaxSize, millisecondTimeout), token,
                bufferSize);
        }

        #endregion

        #region GENERIC ADAPTER

        //<<<<<<<<<<< SINGLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(this Action<TC, CancellationToken> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(
            this Func<TC, CancellationToken, Task> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumer.ToConsumer().ConcurrentPipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumer">consumer</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(this IConsumer<TC> consumer,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new[] {consumer}.ConcurrentPipeline(adapter, token, bufferSize);
        }

        //<<<<<<<<<<< MULTIPLE CONSUMER

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(
            this IReadOnlyList<Action<TC, CancellationToken>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(
            this IReadOnlyList<Func<TC, CancellationToken, Task>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return consumers.ToConsumer().ConcurrentPipeline(adapter, token, bufferSize);
        }

        /// <summary>
        /// Creates and returns an instance of <seealso cref="IConcurrentPipeline{TP}" /> as an end-point to accept data while
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
        /// <seealso cref="IConcurrentPipeline{TP}.TearDown" /> (as documented) should be called only after it is
        /// certain no
        /// more calls to <seealso cref="IConcurrentPipeline{TP}.Add" /> will be made, to avoid unexpected errors.
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
        /// <seealso cref="IConcurrentPipeline{T}.Add" /> method:
        /// <list type="bullet">
        /// <item>
        /// <description>is Thread-safe and can be called concurrently.</description>
        /// </item>
        /// <item>
        /// <description>
        /// throws <seealso cref="OperationCanceledException" /> when either
        /// <paramref name="token" /> is cancelled or any of the consumers ends-up throwing an
        /// exception
        /// (<seealso cref="IConcurrentPipeline{T}.TearDown" /> or
        /// <seealso cref="IDisposable.Dispose" /> might not have been called at
        /// this moment, but, at least all the consumers are disposed)
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TP">Type of items <seealso cref="IConcurrentPipeline{TP}" /> will accept</typeparam>
        /// <typeparam name="TC">Type of items <seealso cref="IConsumer{TC}" /> is able to consume</typeparam>
        /// <param name="consumers">consumers</param>
        /// <param name="adapter">
        /// adapter instance to create instances of type <typeparamref name="TC" /> from the instances
        /// of type <typeparamref name="TP" />
        /// </param>
        /// <param name="token">cancellation token to observe</param>
        /// <param name="bufferSize">buffer size</param>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(this IReadOnlyList<IConsumer<TC>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new ConcurrentPipeline<TP, TC>(consumers, adapter, token, bufferSize);
        }

        #endregion
    }
}