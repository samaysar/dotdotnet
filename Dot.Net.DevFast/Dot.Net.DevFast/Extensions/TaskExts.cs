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
        ///// <summary>
        ///// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        ///// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, this loop will span more tasks
        ///// then specified by <paramref name="maxConcurrency"/>).
        ///// </summary>
        ///// <param name="actions">collection of actions to execute and await on</param>
        ///// <param name="maxConcurrency">max number of tasks to span at a given time.</param>
        ///// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="actions"/></param>
        ///// <returns></returns>
        //public static Task RepeatNAwaitAllTask(this IEnumerable<Action<CancellationToken>> actions,
        //    int maxConcurrency, CancellationToken token = default(CancellationToken))
        //{
        //    return Task.Run(async () => await actions.RepeatNAwaitAllAsync(maxConcurrency, token).ConfigureAwait(false),
        //        token);
        //}

        ///// <summary>
        ///// Creates and awaits on tasks generated during enumeration on <paramref name="actions"/> while respecting the
        ///// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, this loop will span more tasks
        ///// then specified by <paramref name="maxConcurrency"/>).
        ///// </summary>
        ///// <param name="actions">collection of actions to execute and await on</param>
        ///// <param name="maxConcurrency">max number of tasks to span.</param>
        ///// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="actions"/></param>
        ///// <returns></returns>
        //public static async Task RepeatNAwaitAllAsync(this IEnumerable<Action<CancellationToken>> actions,
        //    int maxConcurrency, CancellationToken token = default(CancellationToken))
        //{
        //    using (var etor = actions.GetEnumerator())
        //    {
        //        await etor.CreateLoop(new object()).RepeatNAwaitAllAsync(maxConcurrency, token)
        //            .ConfigureAwait(false);
        //    }
        //}

        ///// <summary>
        ///// Creates and returns a wrapped task that awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        ///// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, this loop will span more tasks
        ///// then specified by <paramref name="maxConcurrency"/>).
        ///// </summary>
        ///// <param name="funcs">collection of functions to execute and await on</param>
        ///// <param name="maxConcurrency">max number of tasks to span at a given time.</param>
        ///// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="funcs"/></param>
        ///// <returns></returns>
        //public static Task RepeatNAwaitAllTask(this IEnumerable<Func<CancellationToken, Task>> funcs,
        //    int maxConcurrency, CancellationToken token = default(CancellationToken))
        //{
        //    return Task.Run(async () => await funcs.RepeatNAwaitAllAsync(maxConcurrency, token).ConfigureAwait(false),
        //        token);
        //}

        ///// <summary>
        ///// Creates and awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        ///// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, this loop will span more tasks
        ///// then specified by <paramref name="maxConcurrency"/>).
        ///// </summary>
        ///// <param name="funcs">collection of functions to execute and await on</param>
        ///// <param name="maxConcurrency">max number of tasks to span.</param>
        ///// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="funcs"/></param>
        ///// <returns></returns>
        //public static async Task RepeatNAwaitAllAsync(this IEnumerable<Func<CancellationToken, Task>> funcs,
        //    int maxConcurrency, CancellationToken token = default(CancellationToken))
        //{
        //    using (var etor = funcs.GetEnumerator())
        //    {
        //        await etor.CreateLoop(new object()).RepeatNAwaitAllAsync(maxConcurrency, token, false)
        //            .ConfigureAwait(false);
        //    }
        //}
        
        /// <summary>
        /// Creates and awaits on all the repeatated tasks (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">action to repeat</param>
        /// <param name="count">number of times <paramref name="action"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="action"/></param>
        public static async Task RepeatNAwaitAllAsync(this Action<int, CancellationToken> action, int count,
            CancellationToken token = default(CancellationToken))
        {
            await action.RepeatNAwaitAllTask(count, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on all the repeatated tasks (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">action to repeat</param>
        /// <param name="count">number of times <paramref name="action"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="action"/></param>
        public static Task RepeatNAwaitAllTask(this Action<int, CancellationToken> action, int count,
            CancellationToken token = default(CancellationToken))
        {
            return action.ToAsync(true).RepeatNAwaitAllTask(count, token, false);
        }

        /// <summary>
        /// Creates and awaits on repeatated tasks (as specified by <paramref name="count"/>) executing the given 
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat</param>
        /// <param name="count">number of times <paramref name="func"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="func"/></param>
        /// <param name="asyncFunc">When true, each instance of <paramref name="func"/> will run in a dedicated task.</param>
        public static async Task RepeatNAwaitAllAsync(this Func<int, CancellationToken, Task> func, int count,
            CancellationToken token = default(CancellationToken), bool asyncFunc = true)
        {
            await func.RepeatNAwaitAllTask(count, token, asyncFunc).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and returns a wrapped task that awaits on all the repeatated tasks (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat</param>
        /// <param name="count">number of times <paramref name="func"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="func"/></param>
        /// <param name="asyncFunc">When true, each instance of <paramref name="func"/> will run in a dedicated task.</param>
        public static Task RepeatNAwaitAllTask(this Func<int, CancellationToken, Task> func, int count,
            CancellationToken token = default(CancellationToken), bool asyncFunc = true)
        {
            var tasks = new Task[count.ThrowIfLess(1, "repeatation count cannot be less than 1")];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = asyncFunc ? func.WrappedTask(i, token) : func(i, token).StartIfNeeded();
            }
            return Task.WhenAll(tasks); 
        }

        private static Task WrappedTask(this Func<int, CancellationToken, Task> func, int i,
            CancellationToken token)
        {
            return Task.Run(() => func(i, token), CancellationToken.None);
        }

        private static Func<CancellationToken, Task> CreateLoop(
            this IEnumerator<Func<int, CancellationToken, Task>> funcs, object syncRoot, int i)
        {
            return t => Task.Run(async () =>
            {
                while (funcs.TryGetNext(syncRoot, out var next))
                {
                    await next(i, t).StartIfNeeded().ConfigureAwait(false);
                }
            }, t);
        }

        private static Action<CancellationToken> CreateLoop(
            this IEnumerator<Action<int, CancellationToken>> actions, object syncRoot, int i)
        {
            return t =>
            {
                while (actions.TryGetNext(syncRoot, out var next))
                {
                    next(i, t);
                }
            };
        }

        private static bool TryGetNext<T>(this IEnumerator<T> funcs, object syncRoot, out T obj)
        {
            lock (syncRoot)
            {
                if (funcs.MoveNext())
                {
                    obj = funcs.Current;
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
    }
}