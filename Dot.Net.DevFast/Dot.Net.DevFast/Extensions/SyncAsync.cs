using System;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Contains extention method on synchronous delegates to convert those to corresponding Async delegates
    /// </summary>
    public static class SyncAsync
    {
        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<Task> ToAsync(this Action sync)
        {
            return () =>
            {
                sync();
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T, Task> ToAsync<T>(this Action<T> sync)
        {
            return t =>
            {
                sync(t);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, Task> ToAsync<T1, T2>(this Action<T1, T2> sync)
        {
            return (t1, t2) =>
            {
                sync(t1, t2);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, Task> ToAsync<T1, T2, T3>(this Action<T1, T2, T3> sync)
        {
            return (t1, t2, t3) =>
            {
                sync(t1, t2, t3);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, Task> ToAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> sync)
        {
            return (t1, t2, t3, t4) =>
            {
                sync(t1, t2, t3, t4);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, Task> ToAsync<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> sync)
        {
            return (t1, t2, t3, t4, t5) =>
            {
                sync(t1, t2, t3, t4, t5);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, Task> ToAsync<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> sync)
        {
            return (t1, t2, t3, t4, t5, t6) =>
            {
                sync(t1, t2, t3, t4, t5, t6);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<Task<T>> ToAsync<T>(this Func<T> sync)
        {
            return () => Task.FromResult(sync());
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, Task<T2>> ToAsync<T1, T2>(this Func<T1, T2> sync)
        {
            return t1 => Task.FromResult(sync(t1));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, Task<T3>> ToAsync<T1, T2, T3>(this Func<T1, T2, T3> sync)
        {
            return (t1, t2) => Task.FromResult(sync(t1, t2));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, Task<T4>> ToAsync<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> sync)
        {
            return (t1, t2, t3) => Task.FromResult(sync(t1, t2, t3));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, Task<T5>> ToAsync<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> sync)
        {
            return (t1, t2, t3, t4) => Task.FromResult(sync(t1, t2, t3, t4));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, Task<T6>> ToAsync<T1, T2, T3, T4, T5, T6>(
            this Func<T1, T2, T3, T4, T5, T6> sync)
        {
            return (t1, t2, t3, t4, t5) => Task.FromResult(sync(t1, t2, t3, t4, t5));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, Task<T7>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            this Func<T1, T2, T3, T4, T5, T6, T7> sync)
        {
            return (t1, t2, t3, t4, t5, t6) => Task.FromResult(sync(t1, t2, t3, t4, t5, t6));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, Task<T8>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7) => Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<T9>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8) => Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<T10>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<T11>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<T12>> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<T13>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<T14>> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<T15>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<T16>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
        }

        /// <summary>
        /// Converts a synchronous action to async func that returns a task to await on.
        /// </summary>
        /// <param name="sync"></param>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<T17>> ToAsync<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> sync)
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
                Task.FromResult(sync(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
        }
    }
}