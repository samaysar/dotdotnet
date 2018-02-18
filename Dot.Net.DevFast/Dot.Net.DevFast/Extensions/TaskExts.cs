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
            CancellationToken token = default(CancellationToken))
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
            CancellationToken token = default(CancellationToken))
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
        /// <para>NOTE: No measures are taken against exceptions raised by the passed delegates (i.e. if delegate execution
        /// results in an error, then there is no guarantee that remaining delgates will be executed and awaited)</para>
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="actions"/></param>
        /// <param name="stopOnCancel">If true, when token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised. If false, enumeration would continue irrespective of token state.</param>
        public static Task WhenAll(this IEnumerable<Action<CancellationToken>> actions,
            int maxConcurrency, CancellationToken token = default(CancellationToken), bool stopOnCancel = true)
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency, token, stopOnCancel);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: No measures are taken against exceptions raised by the passed delegates (i.e. if delegate execution
        /// results in an error, then there is no guarantee that remaining delgates will be executed and awaited)</para>
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="funcs"/></param>
        /// <param name="stopOnCancel">If true, when token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised. If false, enumeration would continue irrespective of token state.</param>
        public static Task WhenAll(this IEnumerable<Func<CancellationToken, Task>> funcs,
            int maxConcurrency, CancellationToken token = default(CancellationToken), bool stopOnCancel = true)
        {
            return funcs.Select(x => new Func<Task>(() => x(token)))
                .WhenAll(maxConcurrency, stopOnCancel ? token : CancellationToken.None);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: No measures are taken against exceptions raised by the passed delegates (i.e. if delegate execution
        /// results in an error, then there is no guarantee that remaining delgates will be executed and awaited)</para>
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Token to observe. When token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised</param>
        public static Task WhenAll(this IEnumerable<Action> actions, int maxConcurrency,
            CancellationToken token = default(CancellationToken))
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency, token);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// <para>NOTE: No measures are taken against exceptions raised by the passed delegates (i.e. if delegate execution
        /// results in an error, then there is no guarantee that remaining delgates will be executed and awaited)</para>
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Token to observe. When token is canceled, enumeration loop will stop as soon as possible 
        /// (i.e. all the running tasks will be awaited but no new task will be created) and <seealso cref="OperationCanceledException"/>
        /// will be raised</param>
        public static Task WhenAll(this IEnumerable<Func<Task>> funcs, int maxConcurrency,
            CancellationToken token = default(CancellationToken))
        {
            maxConcurrency.ThrowIfLess(1, "concurrency value is less than 1");
            var etor = funcs.GetEnumerator();
            return etor.Loop(new object(), token).RepeatNWhenAll(maxConcurrency).AwaitNDispose(etor);
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

        private static Func<Task> Loop(this IEnumerator<Func<Task>> funcs, object syncRoot, CancellationToken token)
        {
            return () => Task.Run(async () =>
            {
                while (funcs.TryGetNext(syncRoot, out var next))
                {
                    token.ThrowIfCancellationRequested();
                    await next().StartIfNeeded().ConfigureAwait(false);
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

                obj = default(T);
                return false;
            }
        }

        private static Task StartIfNeeded(this Task task)
        {
            if (task.Status == TaskStatus.Created) task.Start();
            return task;
        }

        private static Task AwaitNDispose(this Task awaitOn, IDisposable disposeIt)
        {
            return Task.Run(async () =>
            {
                using (disposeIt)
                {
                    await awaitOn.ConfigureAwait(false);
                }
            });
        }
    }
}