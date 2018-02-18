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
        /// Creates and returns a wrapped task that awaits on all concurrent tasks created by repeatatively (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">action to repeat. The first <seealso cref="int"/> argument is the ith value (starting with 0) of the repeatation</param>
        /// <param name="count">number of times <paramref name="action"/> needs to be repeated (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="action"/></param>
        public static Task WhenAll(this Action<int, CancellationToken> action, int count,
            CancellationToken token = default(CancellationToken))
        {
            return action.ToAsync(false).WhenAll(count, token);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on all consurrent tasks created by repeatatively (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat. The first <seealso cref="int"/> argument is the ith value (starting with 0) of the repeatation</param>
        /// <param name="count">number of times <paramref name="func"/> needs to be repeated (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="func"/></param>
        public static Task WhenAll(this Func<int, CancellationToken, Task> func, int count,
            CancellationToken token = default(CancellationToken))
        {
            return func.Repeat(count.ThrowIfLess(1, "repeatation count value is less than 1"))
                .WhenAll(count, token);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="actions"/></param>
        public static Task WhenAll(this IEnumerable<Action<CancellationToken>> actions,
            int maxConcurrency, CancellationToken token = default(CancellationToken))
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency, token);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is passed to <paramref name="funcs"/></param>
        public static Task WhenAll(this IEnumerable<Func<CancellationToken, Task>> funcs,
            int maxConcurrency, CancellationToken token = default(CancellationToken))
        {
            return funcs.Select(x => new Func<Task>(() => x(token))).WhenAll(maxConcurrency);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// </summary>
        /// <param name="actions">collection of actions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        public static Task WhenAll(this IEnumerable<Action> actions, int maxConcurrency)
        {
            return actions.Select(x => x.ToAsync(false)).WhenAll(maxConcurrency);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, enumeration will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// </summary>
        /// <param name="funcs">collection of functions to execute and await on</param>
        /// <param name="maxConcurrency">maximum number of task to span (possible min value: 1)</param>
        public static Task WhenAll(this IEnumerable<Func<Task>> funcs, int maxConcurrency)
        {
            maxConcurrency.ThrowIfLess(1, "concurrency value is less than 1");
            var etor = funcs.GetEnumerator();
            return etor.Loop(new object()).RepeatNWhenAll(maxConcurrency).WithDispose(etor);
        }

        private static IEnumerable<Func<CancellationToken, Task>> Repeat(this Func<int, CancellationToken, Task> func,
            int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return func.InnerFunc(i);
            }
        }

        private static Func<CancellationToken, Task> InnerFunc(this Func<int, CancellationToken, Task> func, int i)
        {
            return t => func(i, t);
        }

        private static Func<Task> Loop(this IEnumerator<Func<Task>> funcs, object syncRoot)
        {
            return () => Task.Run(async () =>
            {
                while (funcs.TryGetNext(syncRoot, out var next))
                {
                    await next().StartIfNeeded().ConfigureAwait(false);
                }
            });
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

        private static Task WithDispose(this Task parent, IDisposable disposeIt)
        {
            return parent.ContinueWith(t =>
            {
                using (disposeIt)
                {
                }
            });
        }
    }
}