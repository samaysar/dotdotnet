using System;
using System.Collections;
using System.Collections.Generic;
#if !NETFRAMEWORK
using System.Runtime.CompilerServices;
using Dot.Net.DevFast.Etc;
#endif
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Collections;

namespace Dot.Net.DevFast.Extensions
{
#if NETFRAMEWORK || NETSTANDARD2_0
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
        /// Returns <see langword="true"/> when both (<paramref name="first"/> and <paramref name="second"/>) collections
        /// have equal items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">First collection</param>
        /// <param name="second">Second collection</param>
        /// <param name="equalityComparer">Equality comparator of T, if any.</param>
        /// <param name="sameItemOrder">If <see langword="true"/> then item order is taken into account.</param>
        public static bool EqualsItemWise<T>(this ICollection<T> first,
            ICollection<T> second,
            IEqualityComparer<T> equalityComparer = null,
            bool sameItemOrder = false)
        {
            if (first == null || second == null) return false;
            if (ReferenceEquals(first, second)) return true;
            if (first.Count != second.Count) return false;
            if (sameItemOrder)
            {
                equalityComparer ??= EqualityComparer<T>.Default;
                using var c1 = first.GetEnumerator();
                using var c2 = second.GetEnumerator();
                while (c1.MoveNext() && c2.MoveNext())
                {
                    if (!equalityComparer.Equals(c1.Current, c2.Current))
                    {
                        return false;
                    }
                }

                return true;
            }

            var hash = new Dictionary<T, int>(second.Count, equalityComparer);
            second.ForEach(x =>
            {
                if (hash.TryGetValue(x, out var count))
                {
                    hash[x] = count + 1;
                }
                else
                {
                    hash[x] = 1;
                }
            });
            return first.All(x =>
            {
                if (!hash.TryGetValue(x, out var count)) return false;
                if (count == 1)
                {
                    hash.Remove(x);
                }
                else
                {
                    hash[x] = count - 1;
                }

                return true;
            });
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="OneToManyDictionary{TKey, TValue}"/> from <paramref name="collection"/> items
        /// using <paramref name="keyFinder"/> lambda and provided key <paramref name="comparer"/> (if any).
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection">Collection of Value items</param>
        /// <param name="keyFinder">Key finder lambda</param>
        /// <param name="comparer">Key comparer</param>
        public static OneToManyDictionary<TKey, TValue> ToOneToManyDictionary<TKey, TValue>(
            this IEnumerable<TValue> collection, 
            Func<TValue, TKey> keyFinder, 
            IEqualityComparer<TKey> comparer = null)
        {
            return collection.ToOneToManyDictionary(keyFinder, x => x, comparer);
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="OneToManyDictionary{TKey, TValue}"/> from <paramref name="collection"/> items
        /// using <paramref name="keyFinder"/> lambda and <paramref name="valueFinder"/> lambda; with provided key <paramref name="comparer"/> (if any).
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection of Value items</param>
        /// <param name="keyFinder">Key finder lambda</param>
        /// <param name="valueFinder">Value finder lambda</param>
        /// <param name="comparer">Key comparer</param>
        public static OneToManyDictionary<TKey, TValue> ToOneToManyDictionary<T, TKey, TValue>(
            this IEnumerable<T> collection,
            Func<T, TKey> keyFinder,
            Func<T, TValue> valueFinder,
            IEqualityComparer<TKey> comparer = null)
        {
            var instance = new OneToManyDictionary<TKey, TValue>(comparer);
            collection.ForEach(x => instance.Add(keyFinder(x), valueFinder(x)));
            return instance;
        }

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

#if !NETFRAMEWORK && !NETSTANDARD2_0
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