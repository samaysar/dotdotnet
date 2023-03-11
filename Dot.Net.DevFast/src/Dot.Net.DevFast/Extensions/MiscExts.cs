using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Contains miscellaneous extensions method.
    /// </summary>
    public static class MiscExts
    {
        /// <summary>
        /// Creates an Enumerable out of supplied <paramref name="collection"></paramref> with <seealso cref="Timeout.Infinite"/>
        /// while observing given token. In case of error, cancels the provided token source to signal problem on this leg
        /// of PPC (Parallel-Producer-Consumer) pattern and raises the error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection to enumerate on</param>
        /// <param name="toObserve">Token to observe for cancellation</param>
        /// <param name="toCancel">Token source to cancel in case of error (if not already canceled.)</param>
        public static IEnumerable<T> ToPpcEnumerableWithException<T>(this BlockingCollection<T> collection,
            CancellationToken toObserve,
            CancellationTokenSource toCancel)
        {
            bool itemTaken;
            do
            {
                T item;
                try
                {
                    itemTaken = collection.TryTake(out item, Timeout.Infinite, toObserve);
                }
                catch
                {
                    if(!toObserve.IsCancellationRequested) toCancel.Cancel();
                    throw;
                }
                if (itemTaken) yield return item;
            } while (itemTaken);
        }

        /// <summary>
        /// Creates an Enumerable out of supplied <paramref name="collection"></paramref> with <seealso cref="Timeout.Infinite"/>
        /// while observing given token. In case of error, cancels the provided token source to signal problem on this leg
        /// of PPC (Parallel-Producer-Consumer) pattern and passes the error to supplied <paramref name="errorHandler"/>
        /// without raising it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection to enumerate on</param>
        /// <param name="toObserve">Token to observe for cancellation</param>
        /// <param name="toCancel">Token source to cancel in case of error (if not already canceled.)</param>
        /// <param name="errorHandler">Error handler</param>
        public static IEnumerable<T> ToPpcEnumerable<T>(this BlockingCollection<T> collection,
            CancellationToken toObserve,
            CancellationTokenSource toCancel,
            Action<Exception> errorHandler)
        {
            bool itemTaken;
            do
            {
                T item = default;
                try
                {
                    itemTaken = collection.TryTake(out item, Timeout.Infinite, toObserve);
                }
                catch (Exception e)
                {
                    itemTaken = false;
                    if (!toObserve.IsCancellationRequested) toCancel.Cancel();
                    errorHandler(e);
                }
                if (itemTaken) yield return item;
            } while (itemTaken);
        }

        /// <summary>
        /// Checks that collection is not null and it has at least one (1) element.
        /// </summary>
        /// <param name="collection">Collection to check</param>
        public static bool HasElements(this ICollection collection)
        {
            return collection?.Count > 0;
        }

        /// <summary>
        /// Enumerates over given <paramref name="itemCollection"/> and finds the
        /// maximums (up to <paramref name="totalMaximums"/>) items using <paramref name="comparer"/>.
        /// Returns another enumerable based of required <paramref name="sorting"/>.
        /// </summary>
        /// <typeparam name="T">Type of the items in the input collection</typeparam>
        /// <param name="itemCollection">Input collection</param>
        /// <param name="totalMaximums">Count of total number of maximum to find.
        /// Error is thrown if value is negative or zero (0).</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="sorting">Output sorting (it uses <paramref name="comparer"/>). As the interest is only to find all the required
        /// number of maximums. It is best to use <seealso cref="SortOrder.None"/> as sorting value
        /// otherwise, an additional latency might be observed.</param>
        public static IEnumerable<T> FindMaximums<T>(this IEnumerable<T> itemCollection,
            int totalMaximums,
            IComparer<T> comparer = null,
            SortOrder sorting = SortOrder.None)
        {
            var heap = new MaxLimitHeap<T>(totalMaximums, comparer);
            heap.AddAll(itemCollection);
            return heap.GetSorted(comparer, sorting, true);
        }

        /// <summary>
        /// Enumerates over given <paramref name="itemCollection"/> and finds the
        /// minimums (up to <paramref name="totalMinimums"/>) items using <paramref name="comparer"/>.
        /// Returns another enumerable based of required <paramref name="sorting"/>.
        /// </summary>
        /// <typeparam name="T">Type of the items in the input collection</typeparam>
        /// <param name="itemCollection">Input collection</param>
        /// <param name="totalMinimums">Count of total number of minimum to find.
        /// Error is thrown if value is negative or zero (0).</param>
        /// <param name="comparer">Comparer instance. If not provided, then <seealso cref="Comparer{T}.Default"/> will be used.</param>
        /// <param name="sorting">Output sorting (it uses <paramref name="comparer"/>). As the interest is only to find all the required
        /// number of minimums. It is best to use <seealso cref="SortOrder.None"/> as sorting value
        /// otherwise, an additional latency might be observed.</param>
        public static IEnumerable<T> FindMinimums<T>(this IEnumerable<T> itemCollection,
            int totalMinimums,
            IComparer<T> comparer = null,
            SortOrder sorting = SortOrder.None)
        {
            var heap = new MinLimitHeap<T>(totalMinimums, comparer);
            heap.AddAll(itemCollection);
            return heap.GetSorted(comparer, sorting, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T> GetSorted<T>(this AbstractBinaryHeap<T> heap, 
            IComparer<T> comparer, SortOrder sorting, bool sortDesc)
        {
            switch (sorting)
            {
                case SortOrder.Asc:
                    return sortDesc ? heap.PopAll() : heap.PopAll().OrderBy(x => x, comparer);
                case SortOrder.Desc:
                    return sortDesc ? heap.PopAll().OrderByDescending(x => x, comparer) : heap.PopAll();
                case SortOrder.None:
                    return heap.InternalStateAsEnumerable();
                default:
                    throw new ArgumentOutOfRangeException(nameof(sorting), sorting, "Sorting not implemented");
            }
        }

        /// <summary>
        /// Simply creates an instance of <see cref="Tuple{T1, T2}"/> and sets <paramref name="t1"/> as <see cref="Tuple{T1, T2}.Item1"/>
        /// and <paramref name="t2"/> as <see cref="Tuple{T1, T2}.Item2"/>.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1">T1 Instance</param>
        /// <param name="t2">T2 Instance</param>
        public static Tuple<T1, T2> ToTuple<T1, T2>(this T1 t1, T2 t2)
        {
            return new Tuple<T1, T2>(t1, t2);
        }

        /// <summary>
        /// Simply creates an instance of <see cref="KeyValuePair{TKey, TValue}"/> and sets <paramref name="key"/> as <see cref="KeyValuePair{TKey, TValue}.Key"/>
        /// and <paramref name="value"/> as <see cref="KeyValuePair{TKey, TValue}.Value"/>.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">TKey Instance</param>
        /// <param name="value">TValue Instance</param>
        public static KeyValuePair<TKey, TValue> ToKvp<TKey, TValue>(this TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}