using System;
using System.Collections;
using System.Collections.Generic;
#if !NETFRAMEWORK
using System.Runtime.CompilerServices;
using Dot.Net.DevFast.Etc;
#endif
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
#if NETFRAMEWORK
    /// <summary>
    /// Contains extension methods on <see cref="IEnumerable{T}"/>, <see cref="IEnumerable"/>.
    /// </summary>
#else
    /// <summary>
    /// Contains extension methods on <see cref="IEnumerable{T}"/>, <see cref="IEnumerable"/> and <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
#endif
    public static class EnumerableExts
    {
        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action, CancellationToken token = default)
        {
            foreach (var item in items)
            {
                action(item);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static void ForEach(this IEnumerable items, Action<object> action, CancellationToken token = default)
        {
            foreach (var item in items)
            {
                action(item);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, CancellationToken> action,
            CancellationToken token = default)
        {
            foreach (var item in items)
            {
                action(item, token);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static void ForEach(this IEnumerable items, Action<object, CancellationToken> action,
            CancellationToken token = default)
        {
            foreach (var item in items)
            {
                action(item, token);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided async <paramref name="action"/> on every item of the given enumerable,
        /// asynchronously, while observing for cancellation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> items,
            Func<T, CancellationToken, Task> action,
            CancellationToken token = default)
        {
            foreach (var item in items)
            {
                await action(item, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
            }
        }

#if !NETFRAMEWORK
        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items,
            Action<T, CancellationToken> action,
            CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                action(item, token);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="action"/> on every item of the given enumerable while observing for cancellation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="action">Action to apply</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, Task> action,
            CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                await action(item, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="selector"/> lambda on every item of the asynchronous collection asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="selector">Selector lambda</param>
        /// <param name="token">Token to observe</param>
        public static async IAsyncEnumerable<TR> SelectAsync<T, TR>(this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, TR> selector,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                yield return selector(item, token);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="selector"/> lambda on every item of the asynchronous collection asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="selector">Selector lambda</param>
        /// <param name="token">Token to observe</param>
        public static async IAsyncEnumerable<TR> SelectAsync<T, TR>(this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, Task<TR>> selector,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                yield return await selector(item, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="predicate"/> lambda on every item of the asynchronous collection asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="predicate">Selector lambda</param>
        /// <param name="token">Token to observe</param>
        public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, bool> predicate,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                if (predicate(item, token)) yield return item;
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Applies provided <paramref name="predicate"/> lambda on every item of the asynchronous collection asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="predicate">Selector lambda</param>
        /// <param name="token">Token to observe</param>
        public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, Task<bool>> predicate,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                if (await predicate(item, token).ConfigureAwait(false)) yield return item;
                token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Creates a new list using all the items of the given asynchronous enumerable asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="token">Token to observe</param>
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            CancellationToken token = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                results.Add(item);
                token.ThrowIfCancellationRequested();
            }

            return results;
        }

        /// <summary>
        /// Creates a new list using all the items of the given asynchronous enumerable asynchronously; while,
        /// respecting the <paramref name="limit"/> provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items</param>
        /// <param name="limit">Max. items in the list</param>
        /// <param name="token">Token to observe</param>
        /// <exception cref="DdnDfException">When enumeration returns more elements than the <paramref name="limit"/> count.
        /// Exception ErrorCode is <see cref="DdnDfErrorCode.OverAllocationDemanded"/></exception>
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            int limit,
            CancellationToken token = default)
        {
            limit.ThrowIfLess(1, $"{limit} cannot be 0 or lesser.");
            var results = new List<T>();
            var count = 0;
            await foreach (var item in items.WithCancellation(token).ConfigureAwait(false))
            {
                (count++ >= limit)
                    .ThrowIf(DdnDfErrorCode.OverAllocationDemanded, $"Limit of {limit} breached.", token)
                    .ThrowIfCancellationRequested();
                results.Add(item);
            }

            return results;
        }
#endif
    }
}