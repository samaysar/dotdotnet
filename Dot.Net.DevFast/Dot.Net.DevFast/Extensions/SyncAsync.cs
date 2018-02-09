using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Contains extention method on synchronous delegates to convert those to corresponding Async delegates
    /// </summary>
    public static class SyncAsync
    {
        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<Task> ToAsync(this Action sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return () =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync();
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return () =>
            {
                sync();
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T, Task> ToAsync<T>(this Action<T> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return t =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return t =>
            {
                sync(t);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, Task> ToAsync<T1, T2>(this Action<T1, T2> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2) =>
            {
                sync(t1, t2);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, Task> ToAsync<T1, T2, T3>(this Action<T1, T2, T3> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3) =>
            {
                sync(t1, t2, t3);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, Task> ToAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4) =>
            {
                sync(t1, t2, t3, t4);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, Task> ToAsync<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5) =>
            {
                sync(t1, t2, t3, t4, t5);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, Task> ToAsync<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6) =>
            {
                sync(t1, t2, t3, t4, t5, t6);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on with an option to
        /// delegate the action execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous action method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the action might still be executing.
        /// Thus, to truly stop the action execution one must find a way to observe the same token
        /// inside the action body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                                tcs.TrySetResult(null);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<Task<T>> ToAsync<T>(this Func<T> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return () =>
                {
                    var tcs = new TaskCompletionSource<T>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync();
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return () => sync().ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, Task<T2>> ToAsync<T1, T2>(this Func<T1, T2> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return t1 =>
                {
                    var tcs = new TaskCompletionSource<T2>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return t1 => sync(t1).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, Task<T3>> ToAsync<T1, T2, T3>(this Func<T1, T2, T3> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2) =>
                {
                    var tcs = new TaskCompletionSource<T3>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2) => sync(t1, t2).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, Task<T4>> ToAsync<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3) =>
                {
                    var tcs = new TaskCompletionSource<T4>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3) => sync(t1, t2, t3).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, Task<T5>> ToAsync<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4) =>
                {
                    var tcs = new TaskCompletionSource<T5>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4) => sync(t1, t2, t3, t4).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, Task<T6>> ToAsync<T1, T2, T3, T4, T5, T6>(
            this Func<T1, T2, T3, T4, T5, T6> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5) =>
                {
                    var tcs = new TaskCompletionSource<T6>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5) => sync(t1, t2, t3, t4, t5).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, Task<T7>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            this Func<T1, T2, T3, T4, T5, T6, T7> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6) =>
                {
                    var tcs = new TaskCompletionSource<T7>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6) => sync(t1, t2, t3, t4, t5, t6).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, Task<T8>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7) =>
                {
                    var tcs = new TaskCompletionSource<T8>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7) => sync(t1, t2, t3, t4, t5, t6, t7).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<T9>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8) =>
                {
                    var tcs = new TaskCompletionSource<T9>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8) => sync(t1, t2, t3, t4, t5, t6, t7, t8).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<T10>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                {
                    var tcs = new TaskCompletionSource<T10>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => sync(t1, t2, t3, t4, t5, t6, t7, t8, t9).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<T11>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                {
                    var tcs = new TaskCompletionSource<T11>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<T12>> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                {
                    var tcs = new TaskCompletionSource<T12>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<T13>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                {
                    var tcs = new TaskCompletionSource<T13>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<T14>> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                {
                    var tcs = new TaskCompletionSource<T14>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<T15>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync, bool withDelgation = true,
            TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                {
                    var tcs = new TaskCompletionSource<T15>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<T16>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                {
                    var tcs = new TaskCompletionSource<T16>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).ResultTask();
        }

        /// <summary>
        /// Converts a synchronous func to async func that returns a typed-task to await on with an option to
        /// delegate the func execution as a separate new task.
        /// </summary>
        /// <param name="sync">synchronous func method</param>
        /// <param name="withDelgation">when true, a new task is created else a task containing the results
        /// is returned. Setting is false is good when the await is just made on the function call.</param>
        /// <param name="options">Options to use when <paramref name="withDelgation"/> is set to true else those
        /// are ignored.</param>
        /// <param name="token">cancellation token to observe. When the cancellation token is cancelled the task
        /// yields <seealso cref="TaskCanceledException"/> but the func might still be executing.
        /// Thus, to truly stop the func execution one must find a way to observe the same token
        /// inside the func body by other means.</param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<T17>> ToAsync<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> sync,
            bool withDelgation = true, TaskCreationOptions options = TaskCreationOptions.RunContinuationsAsynchronously,
            CancellationToken token = default(CancellationToken))
        {
            if (withDelgation)
            {
                return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                {
                    var tcs = new TaskCompletionSource<T17>();
                    Task.Run(() =>
                    {
                        try
                        {
                            using (token.Register(() => tcs.TrySetCanceled(token)))
                            {
                                var t = sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                                tcs.TrySetResult(t);
                            }
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }, token);
                    return tcs.Task;
                };
            }

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).ResultTask();
        }

        private static Task<T> ResultTask<T>(this T obj)
        {
            return Task.FromResult(obj);
        }
    }
}