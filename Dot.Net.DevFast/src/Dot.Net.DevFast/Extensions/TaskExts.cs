using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension class that contains methods on Tasks
    /// </summary>
    public static class TaskExts
    {
        /// <summary>
        /// Creates and returns a wrapped task that awaits on all concurrent tasks created by repeatatively (as specified by <paramref name="repeatCount"/>)
        /// executing the given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">action to repeat. The first <seealso cref="int"/> argument is the 0-based index of repeatation loop</param>
        /// <param name="repeatCount">number of times <paramref name="action"/> needs to be repeated (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="action"/></param>
        public static Task WhenAll(this Action<int, CancellationToken> action, int repeatCount,
            CancellationToken token = default)
        {
            return action.ToAsync(false).WhenAll(repeatCount, token);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on all consurrent tasks created by repeatatively (as specified by <paramref name="repeatCount"/>)
        /// executing the given <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat. The first <seealso cref="int"/> argument is the 0-based index of repeatation loop</param>
        /// <param name="repeatCount">number of times <paramref name="func"/> needs to be repeated (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="func"/></param>
        public static Task WhenAll(this Func<int, CancellationToken, Task> func, int repeatCount,
            CancellationToken token = default)
        {
            return func.Repeat(repeatCount.ThrowIfLess(1, "repeatation count value is less than 1"), token)
                .WhenAll(repeatCount, CancellationToken.None);
            //we supply CancellationToken.None to whenall coz we have already passing token to individual
            //tasks (in repeat call)
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: Internally no measures are taken against exceptions raised by the passed delegates, thus, it is important to
        /// understand the usage of <paramref name="errorHandler"/>.</para>
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="actions"/>.
        /// (Note: <seealso cref="OperationCanceledException"/> will be thrown back on await of task and will not be passed 
        /// to <paramref name="errorHandler"/>)</param>
        /// <param name="stopOnCancel">If true, when token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised. If false, enumeration would continue irrespective of token state.</param>
        /// <param name="errorHandler">If errorHandler is passed and an exception occurs during the execution of these tasks, then
        /// the exception will be passed to it and enumeration would continue. If error handler not passed, then that perticular
        /// concurrent enumeration would stop (i.e. concurrency would reduce by 1, but remaining delegates would be executed by other
        /// concurrent enumerations). Thus, if N (N = <paramref name="maxConcurrency"/>) such exceptions occurs, then all enumerations 
        /// would stop. In any case, if <paramref name="errorHandler"/> is not supplied and at least one exception occurs
        /// then the await on this task would yield in exception, irrespective of the state of the those concurrent enumerations.</param>
        public static Task WhenAll(this IEnumerable<Action<CancellationToken>> actions,
            int maxConcurrency, CancellationToken token = default, bool stopOnCancel = true,
            Action<Exception> errorHandler = null)
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency, token, stopOnCancel, errorHandler);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: Internally no measures are taken against exceptions raised by the passed delegates, thus, it is important to
        /// understand the usage of <paramref name="errorHandler"/>.</para>
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="funcs"/>.
        /// (Note: <seealso cref="OperationCanceledException"/> will be thrown back on await of task and will not be passed 
        /// to <paramref name="errorHandler"/>)</param>
        /// <param name="stopOnCancel">If true, when token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised. If false, enumeration would continue irrespective of token state.
        /// </param>
        /// <param name="errorHandler">If errorHandler is passed and an exception occurs during the execution of these tasks, then
        /// the exception will be passed to it and enumeration would continue. If error handler not passed, then that perticular
        /// concurrent enumeration would stop (i.e. concurrency would reduce by 1, but remaining delegates would be executed by other
        /// concurrent enumerations). Thus, if N (N = <paramref name="maxConcurrency"/>) such exceptions occurs, then all enumerations 
        /// would stop. In any case, if <paramref name="errorHandler"/> is not supplied and at least one exception occurs
        /// then the await on this task would yield in exception, irrespective of the state of the those concurrent enumerations.</param>
        public static Task WhenAll(this IEnumerable<Func<CancellationToken, Task>> funcs,
            int maxConcurrency, CancellationToken token = default, bool stopOnCancel = true, 
            Action<Exception> errorHandler = null)
        {
            return funcs.Select(x => new Func<Task>(() => x(token)))
                .WhenAll(maxConcurrency, stopOnCancel ? token : CancellationToken.None, errorHandler);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: Internally no measures are taken against exceptions raised by the passed delegates, thus, it is important to
        /// understand the usage of <paramref name="errorHandler"/>.</para>
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Token to observe. When token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised (Note: This error will be thrown back on await of task and will not be passed to <paramref name="errorHandler"/>)</param>
        /// <param name="errorHandler">If errorHandler is passed and an exception occurs during the execution of these tasks, then
        /// the exception will be passed to it and enumeration would continue. If error handler not passed, then that perticular
        /// concurrent enumeration would stop (i.e. concurrency would reduce by 1, but remaining delegates would be executed by other
        /// concurrent enumerations). Thus, if N (N = <paramref name="maxConcurrency"/>) such exceptions occurs, then all enumerations 
        /// would stop. In any case, if <paramref name="errorHandler"/> is not supplied and at least one exception occurs
        /// then the await on this task would yield in exception, irrespective of the state of the those concurrent enumerations.</param>
        public static Task WhenAll(this IEnumerable<Action> actions, int maxConcurrency,
            CancellationToken token = default, Action<Exception> errorHandler = null)
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency, token, errorHandler);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: Internally no measures are taken against exceptions raised by the passed delegates, thus, it is important to
        /// understand the usage of <paramref name="errorHandler"/>.</para>
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Token to observe. When token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised (Note: This error will be thrown back on await of task and will not be passed to <paramref name="errorHandler"/>)</param>
        /// <param name="errorHandler">If errorHandler is passed and an exception occurs during the execution of these tasks, then
        /// the exception will be passed to it and enumeration would continue. If error handler not passed, then that perticular
        /// concurrent enumeration would stop (i.e. concurrency would reduce by 1, but remaining delegates would be executed by other
        /// concurrent enumerations). Thus, if N (N = <paramref name="maxConcurrency"/>) such exceptions occurs, then all enumerations 
        /// would stop. In any case, if <paramref name="errorHandler"/> is not supplied and at least one exception occurs
        /// then the await on this task would yield in exception, irrespective of the state of the those concurrent enumerations.</param>
        public static Task WhenAll(this IEnumerable<Func<Task>> funcs, int maxConcurrency,
            CancellationToken token = default, Action<Exception> errorHandler = null)
        {
            maxConcurrency.ThrowIfLess(1, "concurrency value is less than 1");
            var etor = funcs.GetEnumerator();
            return etor.Loop(new object(), token, errorHandler).RepeatNWhenAll(maxConcurrency).AwaitNDispose(etor);
        }

        private static IEnumerable<Func<Task>> Repeat(this Func<int, CancellationToken, Task> func,
            int count, CancellationToken token)
        {
            for (var i = 0; i < count; i++)
            {
                yield return func.InnerFunc(i, token);
            }
        }

        private static Func<Task> InnerFunc(this Func<int, CancellationToken, Task> func, int i,
            CancellationToken token)
        {
            return () => func(i, token);
        }

        private static Func<Task> Loop(this IEnumerator<Func<Task>> funcs, object syncRoot, CancellationToken token,
            Action<Exception> errorHandler)
        {
            return () => Task.Run(async () =>
            {
                while (funcs.TryGetNext(syncRoot, out var next))
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        await next().StartIfNeeded().ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        if (errorHandler == null) throw;
                        errorHandler(e);
                    }
                }
            }, CancellationToken.None);
        }

        private static Task RepeatNWhenAll(this Func<Task> func, int count)
        {
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = func();
            }

            return Task.WhenAll(tasks);
        }

        private static bool TryGetNext<T>(this IEnumerator<T> enumerator, object syncRoot, out T obj)
        {
            lock (syncRoot)
            {
                if (enumerator.MoveNext())
                {
                    obj = enumerator.Current;
                    return true;
                }

                obj = default;
                return false;
            }
        }

        /// <summary>
        /// Starts the given task if not already started. Returns it back after starting for chaining or awaiting.
        /// </summary>
        /// <typeparam name="T">Task param type</typeparam>
        /// <param name="task">Task to start</param>
        public static Task<T> StartIfNeeded<T>(this Task<T> task)
        {
            if (task.Status == TaskStatus.Created) task.Start();
            return task;
        }

        /// <summary>
        /// Starts the given task if not already started. Returns it back after starting for chaining or awaiting.
        /// </summary>
        /// <param name="task">Task to start</param>
        public static Task StartIfNeeded(this Task task)
        {
            if (task.Status == TaskStatus.Created) task.Start();
            return task;
        }

        /// <summary>
        /// Awaits on the given task and once task finishes (irrespective of its state), disposes
        /// the given disposable instance. Runs everything as a new task.
        /// </summary>
        /// <param name="awaitOn">Task to await on. If not started, then it will started before it is awaited on.</param>
        /// <param name="disposeIt">Disposable instance</param>
        public static Task AwaitNDispose(this Task awaitOn, IDisposable disposeIt)
        {
            return Task.Run(async () =>
            {
                using (disposeIt)
                {
                    await awaitOn.StartIfNeeded().ConfigureAwait(false);
                }
            });
        }

#if !NETFRAMEWORK && !NETSTANDARD2_0
        /// <summary>
        /// Awaits on the given task and once task finishes (irrespective of its state), disposes
        /// the given disposable instance. Runs everything as a new task.
        /// </summary>
        /// <param name="awaitOn">Task to await on. If not started, then it will started before it is awaited on.</param>
        /// <param name="disposeIt">Disposable instance</param>
        public static Task AwaitNDispose(this Task awaitOn, IAsyncDisposable disposeIt)
        {
            return Task.Run(async () =>
            {
                await using (disposeIt.ConfigureAwait(false))
                {
                    await awaitOn.StartIfNeeded().ConfigureAwait(false);
                }
            });
        }
#endif

        /// <summary>
        /// Awaits on the given task and once task finishes (irrespective of its state), disposes
        /// the given disposable instance. New task is NOT created.
        /// </summary>
        /// <param name="awaitOn">Task to await on. If not started, then it will started before it is awaited on.</param>
        /// <param name="disposeIt">Disposable instance</param>
        public static async Task AwaitNDisposeAsync(this Task awaitOn, IDisposable disposeIt)
        {
            using (disposeIt)
            {
                await awaitOn.StartIfNeeded().ConfigureAwait(false);
            }
        }

#if !NETFRAMEWORK && !NETSTANDARD2_0
        /// <summary>
        /// Awaits on the given task and once task finishes (irrespective of its state), disposes
        /// the given disposable instance. New task is NOT created.
        /// </summary>
        /// <param name="awaitOn">Task to await on. If not started, then it will started before it is awaited on.</param>
        /// <param name="disposeIt">Disposable instance</param>
        public static async Task AwaitNDisposeAsync(this Task awaitOn, IAsyncDisposable disposeIt)
        {
            await using (disposeIt.ConfigureAwait(false))
            {
                await awaitOn.StartIfNeeded().ConfigureAwait(false);
            }
        }
#endif
    }
}