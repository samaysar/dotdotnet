using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Dot.Net.DevFast.Etc;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension library to produce conditional error based on a predicate
    /// </summary>
    public static class ThrowIfPredicate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowDfError(this DdnDfErrorCode code)
        {
            throw new DdnDfException(code);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowDfError(this DdnDfErrorCode code, string message)
        {
            throw new DdnDfException(code, message);
        }

        #region ThrowIf

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="obj">Object to return if condition is false</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIf<T>(this bool truthValue, DdnDfErrorCode errorCode, T obj)
        {
            if (truthValue) errorCode.ThrowDfError();
            return obj;
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="obj">Object to return if condition is false</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIf<T>(this bool truthValue, DdnDfErrorCode errorCode, string errorMessage, T obj)
        {
            if (truthValue) errorCode.ThrowDfError(errorMessage);
            return obj;
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessageDelegate">Error message delegate</param>
        /// <param name="obj">Object to return if condition is false</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIf<T>(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate,
            T obj)
        {
            if (truthValue) errorCode.ThrowDfError(errorMessageDelegate());
            return obj;
        }

        #endregion

        #region ThrowIfNot

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="obj">Object to return if condition is true</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNot<T>(this bool truthValue, DdnDfErrorCode errorCode, T obj)
        {
            return (!truthValue).ThrowIf(errorCode, obj);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="obj">Object to return if condition is true</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNot<T>(this bool truthValue, DdnDfErrorCode errorCode, string errorMessage, T obj)
        {
            return (!truthValue).ThrowIf(errorCode, errorMessage, obj);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessageDelegate">Error message delegate</param>
        /// <param name="obj">Object to return if condition is true</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNot<T>(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate,
            T obj)
        {
            return (!truthValue).ThrowIf(errorCode, errorMessageDelegate, obj);
        }

        #endregion

        #region ThrowIfNull

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj) where T : class
        {
            return ThrowIfNullPredicate(obj).ThrowIf(DdnDfErrorCode.NullObject, obj);
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj, string errorMessage) where T : class
        {
            return ThrowIfNullPredicate(obj).ThrowIf(DdnDfErrorCode.NullObject, errorMessage, obj);
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj, Func<string> errorMessageDelegate) where T : class
        {
            return ThrowIfNullPredicate(obj).ThrowIf(DdnDfErrorCode.NullObject, errorMessageDelegate, obj);
        }

        private static bool ThrowIfNullPredicate<T>(T obj) where T : class
        {
            return ReferenceEquals(obj, null);
        }

        #endregion

        #region ThrowIfNullOrEmpty

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyCollection"/></exception>
        public static T ThrowIfNullOrEmpty<T>(this T obj) where T : ICollection
        {
            return ThrowIfNullOrEmptyPredicate(obj).ThrowIf(DdnDfErrorCode.NullOrEmptyCollection, obj);
        }

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyCollection"/></exception>
        public static T ThrowIfNullOrEmpty<T>(this T obj, string errorMessage) where T : ICollection
        {
            return ThrowIfNullOrEmptyPredicate(obj).ThrowIf(DdnDfErrorCode.NullOrEmptyCollection, errorMessage, obj);
        }

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyCollection"/></exception>
        public static T ThrowIfNullOrEmpty<T>(this T obj, Func<string> errorMessageDelegate) where T : ICollection
        {
            return ThrowIfNullOrEmptyPredicate(obj)
                .ThrowIf(DdnDfErrorCode.NullOrEmptyCollection, errorMessageDelegate, obj);
        }

        private static bool ThrowIfNullOrEmptyPredicate<T>(T obj) where T : ICollection
        {
            return (ReferenceEquals(obj, null) || obj.Count < 1);
        }

        #endregion

        #region ThrowOnMiss Collection

        /// <summary>
        /// Throws exception when provided value is not in the collection. Else collection instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <typeparam name="TV">Generic param type of the Collection</typeparam>
        /// <param name="collection">collection instance</param>
        /// <param name="lookUpValue">look up value instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotInCollection"/></exception>
        public static T ThrowOnMiss<T, TV>(this T collection, TV lookUpValue) where T : ICollection<TV>
        {
            return ThrowOnMissPredicate(collection, lookUpValue)
                .ThrowIfNot(DdnDfErrorCode.ValueNotInCollection, collection);
        }

        /// <summary>
        /// Throws exception when provided value is not in the collection. Else collection instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <typeparam name="TV">Generic param type of the Collection</typeparam>
        /// <param name="collection">collection instance</param>
        /// <param name="lookUpValue">look up value instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotInCollection"/></exception>
        public static T ThrowOnMiss<T, TV>(this T collection, TV lookUpValue, string errorMessage)
            where T : ICollection<TV>
        {
            return ThrowOnMissPredicate(collection, lookUpValue)
                .ThrowIfNot(DdnDfErrorCode.ValueNotInCollection, errorMessage, collection);
        }

        /// <summary>
        /// Throws exception when provided value not ont in the collection. Else collection instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <typeparam name="TV">Generic param type of the Collection</typeparam>
        /// <param name="collection">collection instance</param>
        /// <param name="lookUpValue">look up value instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotInCollection"/></exception>
        public static T ThrowOnMiss<T, TV>(this T collection, TV lookUpValue, Func<string> errorMessageDelegate)
            where T : ICollection<TV>
        {
            return ThrowOnMissPredicate(collection, lookUpValue)
                .ThrowIfNot(DdnDfErrorCode.ValueNotInCollection, errorMessageDelegate, collection);
        }

        private static bool ThrowOnMissPredicate<T, TV>(T collection, TV lookUpValue) where T : ICollection<TV>
        {
            return collection.Contains(lookUpValue);
        }

        #endregion

        #region ThrowOnMiss Dictionary

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this IReadOnlyDictionary<TK, TV> dictionary, TK key)
        {
            return ThrowOnMissPredicate(dictionary, key, out TV value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
        }

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this IReadOnlyDictionary<TK, TV> dictionary, TK key, string errorMessage)
        {
            return ThrowOnMissPredicate(dictionary, key, out TV value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this IReadOnlyDictionary<TK, TV> dictionary, TK key,
            Func<string> errorMessageDelegate)
        {
            return ThrowOnMissPredicate(dictionary, key, out TV value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

        private static bool ThrowOnMissPredicate<TK, TV>(IReadOnlyDictionary<TK, TV> dictionary, TK key, out TV value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        #endregion

        #region ThrowIf On Int

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfNegative(this int value)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfNegative(this int value, string errorMessage)
        {
            return value.ThrowIfLess(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfNegative(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfLess(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold)
        {
            return value.ThrowIfLess(threshold, $"{value} < {threshold}");

        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold, string errorMessage)
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfPositive(this int value)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfPositive(this int value, string errorMessage)
        {
            return value.ThrowIfGreater(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfPositive(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfGreater(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold)
        {
            return value.ThrowIfGreater(threshold, $"{value} > {threshold}");
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold, string errorMessage)
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static int ThrowIfNotBounded(this int value, int lowerBoundExcl, int upperBoundExcl)
        {
            return value.ThrowIfNotBounded(lowerBoundExcl, upperBoundExcl, $"Either {value} < {lowerBoundExcl} Or {value} > {upperBoundExcl}");
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static int ThrowIfNotBounded(this int value, int lowerBoundExcl, int upperBoundExcl, string errorMessage)
        {
            return OutOfBoundPredicate(value, lowerBoundExcl, upperBoundExcl).ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static int ThrowIfNotBounded(this int value, int lowerBoundExcl, int upperBoundExcl, Func<string> errorMessageDelegate)
        {
            return OutOfBoundPredicate(value, lowerBoundExcl, upperBoundExcl).ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static int ThrowIfBounded(this int value, int lowerBoundIncl, int upperBoundIncl)
        {
            return value.ThrowIfBounded(lowerBoundIncl, upperBoundIncl, $"{lowerBoundIncl} <= {value} <= {upperBoundIncl}");
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static int ThrowIfBounded(this int value, int lowerBoundIncl, int upperBoundIncl, string errorMessage)
        {
            return OutOfBoundPredicate(value, lowerBoundIncl, upperBoundIncl).ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static int ThrowIfBounded(this int value, int lowerBoundIncl, int upperBoundIncl, Func<string> errorMessageDelegate)
        {
            return OutOfBoundPredicate(value, lowerBoundIncl, upperBoundIncl).ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessageDelegate, value);
        }

        private static bool LessThanPredicate(int value, int threshold)
        {
            return value < threshold;
        }

        private static bool OutOfBoundPredicate(int value, int lowValue, int highValue)
        {
            return LessThanPredicate(value, lowValue) || LessThanPredicate(highValue, value);
        }


        #endregion

        #region ThrowIf On Long

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value, string errorMessage)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold)
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold, string errorMessage)
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value, string errorMessage)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold)
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold, string errorMessage)
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than given threshold.
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static long ThrowIfNotBounded(this long value, long lowerBoundExcl, long upperBoundExcl)
        {
            return value.ThrowIfNotBounded(lowerBoundExcl, upperBoundExcl, $"Either {value} < {lowerBoundExcl} Or {value} > {upperBoundExcl}");
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static long ThrowIfNotBounded(this long value, long lowerBoundExcl, long upperBoundExcl, string errorMessage)
        {
            return OutOfBoundPredicate(value, lowerBoundExcl, upperBoundExcl).ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; <paramref name="lowerBoundExcl"/> OR <paramref name="value"/> &gt; <paramref name="upperBoundExcl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundExcl">Lower bound of comparison</param>
        /// <param name="upperBoundExcl">Upper bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static long ThrowIfNotBounded(this long value, long lowerBoundExcl, long upperBoundExcl, Func<string> errorMessageDelegate)
        {
            return OutOfBoundPredicate(value, lowerBoundExcl, upperBoundExcl).ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static long ThrowIfBounded(this long value, long lowerBoundIncl, long upperBoundIncl)
        {
            return value.ThrowIfBounded(lowerBoundIncl, upperBoundIncl, $"{lowerBoundIncl} <= {value} <= {upperBoundIncl}");
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static long ThrowIfBounded(this long value, long lowerBoundIncl, long upperBoundIncl, string errorMessage)
        {
            return OutOfBoundPredicate(value, lowerBoundIncl, upperBoundIncl).ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given value is within the bounds (both bound inclusive, i.e.,
        /// throws when <paramref name="lowerBoundIncl"/> &lt;= <paramref name="value"/> &lt;= <paramref name="upperBoundIncl"/>).
        /// Else value is returned to performed method chaining on the value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="lowerBoundIncl">Lower bound of comparison</param>
        /// <param name="upperBoundIncl">Upper bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static long ThrowIfBounded(this long value, long lowerBoundIncl, long upperBoundIncl, Func<string> errorMessageDelegate)
        {
            return OutOfBoundPredicate(value, lowerBoundIncl, upperBoundIncl).ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessageDelegate, value);
        }

        private static bool LessThanPredicate(long value, long threshold)
        {
            return value < threshold;
        }

        private static bool OutOfBoundPredicate(long value, long lowValue, long highValue)
        {
            return LessThanPredicate(value, lowValue) || LessThanPredicate(highValue, value);
        }

        #endregion
    }
}