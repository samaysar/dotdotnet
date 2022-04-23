﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Contains extension method on synchronous delegates to convert those to corresponding Async delegates
    /// </summary>
    public static class SyncAsync
    {
        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<Task> ToAsync(this Action sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return () =>
                {
                    token.ThrowIfCancellationRequested();
                    sync();
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return () => Task.Run(sync, CancellationToken.None);
            }

            return () =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync();
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<Task<T0>> ToAsync<T0>(this Func<T0> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async () =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync().AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return () => Task.Run(sync, CancellationToken.None);
            }

            return () =>
            {
                var tcs = new TaskCompletionSource<T0>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync();
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, Task> ToAsync<T0>(this Action<T0> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return t0 =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return t0 => Task.Run(() => sync(t0), CancellationToken.None);
            }

            return t0 =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, Task<T1>> ToAsync<T0, T1>(this Func<T0, T1> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async t0 =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return t0 => Task.Run(() => sync(t0), CancellationToken.None);
            }

            return t0 =>
            {
                var tcs = new TaskCompletionSource<T1>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, Task> ToAsync<T0, T1>(this Action<T0, T1> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1) => Task.Run(() => sync(t0, t1), CancellationToken.None);
            }

            return (t0, t1) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, Task<T2>> ToAsync<T0, T1, T2>(this Func<T0, T1, T2> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1) => Task.Run(() => sync(t0, t1), CancellationToken.None);
            }

            return (t0, t1) =>
            {
                var tcs = new TaskCompletionSource<T2>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, Task> ToAsync<T0, T1, T2>(this Action<T0, T1, T2> sync,
            bool runAsync = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2) => Task.Run(() => sync(t0, t1, t2), CancellationToken.None);
            }

            return (t0, t1, t2) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, Task<T3>> ToAsync<T0, T1, T2, T3>(this Func<T0, T1, T2, T3> sync,
            bool runAsync = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2) => Task.Run(() => sync(t0, t1, t2), CancellationToken.None);
            }

            return (t0, t1, t2) =>
            {
                var tcs = new TaskCompletionSource<T3>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, Task> ToAsync<T0, T1, T2, T3>(this Action<T0, T1, T2, T3> sync,
            bool runAsync = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3) => Task.Run(() => sync(t0, t1, t2, t3), CancellationToken.None);
            }

            return (t0, t1, t2, t3) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, Task<T4>> ToAsync<T0, T1, T2, T3, T4>(this Func<T0, T1, T2, T3, T4> sync,
            bool runAsync = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3) => Task.Run(() => sync(t0, t1, t2, t3), CancellationToken.None);
            }

            return (t0, t1, t2, t3) =>
            {
                var tcs = new TaskCompletionSource<T4>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, Task> ToAsync<T0, T1, T2, T3, T4>(this Action<T0, T1, T2, T3, T4> sync,
            bool runAsync = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4) => Task.Run(() => sync(t0, t1, t2, t3, t4), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, Task<T5>> ToAsync<T0, T1, T2, T3, T4, T5>(
            this Func<T0, T1, T2, T3, T4, T5> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4) => Task.Run(() => sync(t0, t1, t2, t3, t4), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4) =>
            {
                var tcs = new TaskCompletionSource<T5>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, Task> ToAsync<T0, T1, T2, T3, T4, T5>(
            this Action<T0, T1, T2, T3, T4, T5> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5) => Task.Run(() => sync(t0, t1, t2, t3, t4, t5), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, Task<T6>> ToAsync<T0, T1, T2, T3, T4, T5, T6>(
            this Func<T0, T1, T2, T3, T4, T5, T6> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5) => Task.Run(() => sync(t0, t1, t2, t3, t4, t5), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5) =>
            {
                var tcs = new TaskCompletionSource<T6>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, Task> ToAsync<T0, T1, T2, T3, T4, T5, T6>(
            this Action<T0, T1, T2, T3, T4, T5, T6> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, Task<T7>> ToAsync<T0, T1, T2, T3, T4, T5, T6, T7>(
            this Func<T0, T1, T2, T3, T4, T5, T6, T7> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6) =>
            {
                var tcs = new TaskCompletionSource<T7>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, Task> ToAsync<T0, T1, T2, T3, T4, T5, T6, T7>(
            this Action<T0, T1, T2, T3, T4, T5, T6, T7> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, Task<T8>> ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7) =>
            {
                var tcs = new TaskCompletionSource<T8>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, Task> ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> sync, bool runAsync = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, Task<T9>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                var tcs = new TaskCompletionSource<T9>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<T10>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                var tcs = new TaskCompletionSource<T10>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<T11>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                var tcs = new TaskCompletionSource<T11>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<T12>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                var tcs = new TaskCompletionSource<T12>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => Task.Run(
                    () => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<T13>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => Task.Run(
                    () => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                var tcs = new TaskCompletionSource<T13>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync, bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => Task.Run(
                    () => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<T14>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => Task.Run(
                    () => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13), CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                var tcs = new TaskCompletionSource<T14>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14),
                        CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<T15>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14),
                        CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                var tcs = new TaskCompletionSource<T15>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Action)"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
                this Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                {
                    token.ThrowIfCancellationRequested();
                    sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                    return Task.CompletedTask;
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15),
                        CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                var tcs = new TaskCompletionSource<object>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                            tcs.TrySetResult(null);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// <para>NOTE: When <paramref name="runAsync"/> is set to true and <paramref name="token"/> is not supplied
        /// calling this function simply returns a newly running task (<seealso cref="Task.Run(Func{Task})"/>).</para>
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="runAsync">when true, delegate executes in a new task. When false a task containing the results
        /// is returned, i.e. <paramref name="sync"/> executes inline no matter when the returned task is awaited.</param>
        /// <param name="options">Options to use when <paramref name="runAsync"/> is set to true and cancelable token is supplied else options
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<T16>>
            ToAsync<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
                this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> sync,
                bool runAsync = true,
                TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
                CancellationToken token = default)
        {
            if (!runAsync)
            {
                return async (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                {
                    token.ThrowIfCancellationRequested();
                    return await sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).AsTaskResultAsync().ConfigureAwait(false);
                };
            }

            if (!token.CanBeCanceled)
            {
                return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                    Task.Run(() => sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15),
                        CancellationToken.None);
            }

            return (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                var tcs = new TaskCompletionSource<T16>(options);
#if NETFRAMEWORK || NETSTANDARD2_0
                Task.Run(() =>
#else
                Task.Run(async () =>
#endif
                {
                    try
                    {
#if NETFRAMEWORK || NETSTANDARD2_0
                        using (token.Register(() => tcs.TrySetCanceled(token)))
#else
                        await using (token.Register(() => tcs.TrySetCanceled(token)).ConfigureAwait(false))
#endif
                        {
                            token.ThrowIfCancellationRequested();
                            var t = sync(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                            tcs.TrySetResult(t);
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, CancellationToken.None);
                return tcs.Task;
            };
        }

        /// <summary>
        /// Executes the <paramref name="lambdaToExecute"/> inside try and passes the caught exception to <paramref name="errorHandler"/>.
        /// </summary>
        /// <param name="lambdaToExecute">Action sync Lambda</param>
        /// <param name="errorHandler">Error handler</param>
        public static void ExecuteErrorWrapped(this Action lambdaToExecute, Action<Exception> errorHandler)
        {
            lambdaToExecute.ErrorWrapper(errorHandler)();
        }

        /// <summary>
        /// Executes the <paramref name="lambdaToExecute"/> inside try and passes the caught exception to <paramref name="errorHandler"/>.
        /// </summary>
        /// <param name="lambdaToExecute">Func sync Lambda</param>
        /// <param name="errorHandler">Error handler</param>
        public static T ExecuteErrorWrapped<T>(this Func<T> lambdaToExecute, Func<Exception, T> errorHandler)
        {
            return lambdaToExecute.ErrorWrapper(errorHandler)();
        }

        /// <summary>
        /// Executes the <paramref name="lambdaToExecute"/> inside try and passes the caught exception to <paramref name="errorHandler"/>.
        /// </summary>
        /// <param name="lambdaToExecute">Func async Lambda</param>
        /// <param name="errorHandler">Error handler</param>
        public static async Task ExecuteErrorWrappedAsync(this Func<Task> lambdaToExecute, Action<Exception> errorHandler)
        {
            await lambdaToExecute.ErrorWrapper(errorHandler)().ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the <paramref name="lambdaToExecute"/> inside try and passes the caught exception to <paramref name="errorHandler"/>.
        /// </summary>
        /// <param name="lambdaToExecute">Func async Lambda</param>
        /// <param name="errorHandler">Error handler</param>
        public static async Task<T> ExecuteErrorWrappedAsync<T>(this Func<Task<T>> lambdaToExecute, Func<Exception, T> errorHandler)
        {
            return await lambdaToExecute.ErrorWrapper(errorHandler)().ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a new sync lambda which executes given lambda inside try-catch and passes any exception
        /// to error handler.
        /// </summary>
        /// <param name="lambdaToExecute">Sync lambda to execute inside try-catch</param>
        /// <param name="errorHandler">Error handler to call in case of error.</param>
        public static Action ErrorWrapper(this Action lambdaToExecute, Action<Exception> errorHandler)
        {
            return () =>
            {
                try
                {
                    lambdaToExecute();
                }
                catch (Exception e)
                {
                    errorHandler(e);
                }
            };
        }

        /// <summary>
        /// Returns a new sync lambda which executes given lambda inside try-catch and passes any exception
        /// to error handler.
        /// </summary>
        /// <param name="lambdaToExecute">Sync lambda to execute inside try-catch</param>
        /// <param name="errorHandler">Error handler to call in case of error.</param>
        public static Func<T> ErrorWrapper<T>(this Func<T> lambdaToExecute, Func<Exception, T> errorHandler)
        {
            return () =>
            {
                try
                {
                    return lambdaToExecute();
                }
                catch (Exception e)
                {
                    return errorHandler(e);
                }
            };
        }

        /// <summary>
        /// Returns a new async lambda which executes given lambda inside try-catch and passes any exception
        /// to error handler.
        /// </summary>
        /// <param name="lambdaToExecute">Async lambda to execute inside try-catch</param>
        /// <param name="errorHandler">Error handler to call in case of error.</param>
        public static Func<Task> ErrorWrapper(this Func<Task> lambdaToExecute, Action<Exception> errorHandler)
        {
            return async () =>
            {
                try
                {
                    await lambdaToExecute().StartIfNeeded().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    errorHandler(e);
                }
            };
        }

        /// <summary>
        /// Returns a new async lambda which executes given lambda inside try-catch and passes any exception
        /// to error handler.
        /// </summary>
        /// <param name="lambdaToExecute">Async lambda to execute inside try-catch</param>
        /// <param name="errorHandler">Error handler to call in case of error.</param>
        public static Func<Task<T>> ErrorWrapper<T>(this Func<Task<T>> lambdaToExecute, Func<Exception, T> errorHandler)
        {
            return async () =>
            {
                try
                {
                    return await lambdaToExecute().StartIfNeeded().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    return errorHandler(e);
                }
            };
        }

        /// <summary>
        /// Converts the given object to result of an awaitable task.
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="obj">instance</param>
        public static async Task<T> AsTaskResultAsync<T>(this T obj)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return obj;
        }
    }
}