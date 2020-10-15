using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    }
}