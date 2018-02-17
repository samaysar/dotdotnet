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
        /// Create and returns a wrapped task that awaits on all the repeatated tasks (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">action to repeat</param>
        /// <param name="count">number of times <paramref name="action"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="action"/></param>
        public static Task RepeatNAwaitAllTask(this Action<int, CancellationToken> action, int count,
            CancellationToken token = default(CancellationToken))
        {
            return action.ToAsync().RepeatNAwaitAllTask(count, token, false);
        }

        /// <summary>
        /// Create and returns a wrapped task that awaits on all the repeatated tasks (as specified by <paramref name="count"/>)
        /// executing the given <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat</param>
        /// <param name="count">number of times <paramref name="func"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="func"/></param>
        /// <param name="asyncFunc">When true, each instance of <paramref name="func"/> will be run in a dedicated task.</param>
        public static Task RepeatNAwaitAllTask(this Func<int, CancellationToken, Task> func, int count,
            CancellationToken token = default(CancellationToken), bool asyncFunc = true)
        {
            return Task.Run(async () =>
                {
                    await func.RepeatNAwaitAllAsync(count, token, asyncFunc).ConfigureAwait(false);
                }, token);
        }

        /// <summary>
        /// Creates and awaits on repeatated tasks (as specified by <paramref name="count"/>) executing the given 
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="func">function to repeat</param>
        /// <param name="count">number of times <paramref name="func"/> needs to be repeated</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="func"/></param>
        /// <param name="asyncFunc">When true, each instance of <paramref name="func"/> will be run in a dedicated task.</param>
        public static async Task RepeatNAwaitAllAsync(this Func<int, CancellationToken, Task> func, int count,
            CancellationToken token = default(CancellationToken), bool asyncFunc = true)
        {
            var tasks = new Task[count];
            try
            {
                for (var i = 0; i < count; i++)
                {
                    tasks[i] = asyncFunc ? func.WrapIntoTask(i, token) : func(i, token);
                }
            }
            finally
            {
                await Task.WhenAll(tasks.Where(x => x != null)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates and awaits on tasks generated during enumeration on <paramref name="funcs"/> while respecting the
        /// concurrency as specified by <paramref name="maxConcurrency"/> (i.e. at no time, this loop will span more tasks
        /// then specified by <paramref name="maxConcurrency"/>).
        /// </summary>
        /// <param name="funcs"></param>
        /// <param name="maxConcurrency">max number of tasks to span.</param>
        /// <param name="token">Cancellation token, if any. This cancellation token is also passed to <paramref name="funcs"/></param>
        /// <param name="asyncFunc">When true, each instance of <paramref name="funcs"/> will be run in a dedicated task.</param>
        /// <param name="stopOnError"></param>
        /// <returns></returns>
        public static async Task RepeatNAwaitAllAsync(this IEnumerable<Func<CancellationToken, Task>> funcs,
            int maxConcurrency, CancellationToken token = default(CancellationToken), bool asyncFunc = true,
            bool stopOnError = true)
        {
            using (var enumrator = funcs.ThrowIfNull("null enumerator").GetEnumerator())
            {
                var errors = new List<Exception>();
                var hasEle = enumrator.MoveNext();
                while (hasEle)
                {
                    var tasks = new List<Task>(maxConcurrency);
                    try
                    {
                        while (tasks.Count < maxConcurrency && hasEle)
                        {
                            var current = enumrator.Current.ThrowIfNull("enumerator returned null function instance");
                            tasks.Add(asyncFunc ? current.WrapIntoTask(token) : current(token));
                            hasEle = enumrator.MoveNext();
                        }
                    }
                    catch
                    {
                        if (tasks.Count > 0) await Task.WhenAll(tasks).ConfigureAwait(false);
                        throw;
                    }

                    try
                    {
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        if (stopOnError) throw;
                        errors.Add(e);
                    }
                    token.ThrowIfCancellationRequested();
                }
                if (errors.Count > 0) throw new AggregateException("Some of the tasks ended up in errors", errors);
            }
        }

        private static Task WrapIntoTask(this Func<int, CancellationToken, Task> repeatable, int i,
            CancellationToken token)
        {
            return Task.Run(() => repeatable(i, token), token);
        }

        private static Task WrapIntoTask(this Func<CancellationToken, Task> repeatable, CancellationToken token)
        {
            return Task.Run(() => repeatable(token), token);
        }
    }
}