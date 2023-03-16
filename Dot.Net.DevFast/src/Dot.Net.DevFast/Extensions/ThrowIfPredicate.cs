using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Dot.Net.DevFast.Etc;
using System.Collections.Generic;
using Dot.Net.DevFast.Extensions.StringExt;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowIfNullPredicate<T>(T obj) where T : class => obj is null;

        #endregion

        #region ThrowIfNullOrEmpty

        /// <summary>
        /// Throws error if the string is empty or contains only whitespaces
        /// else returns the original string value.
        /// </summary>
        /// <param name="val">Val to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.EmptyOrWhiteSpacedString"/></exception>
        public static string ThrowIfNullOrEmpty(this string val)
        {
            return val.ThrowIfNullOrEmpty("String is either empty of contains only whitespaces.");
        }

        /// <summary>
        /// Throws error if the string is empty or contains only whitespaces
        /// else returns the original string value.
        /// </summary>
        /// <param name="val">Val to check</param>
        /// <param name="errorMessage">Error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.EmptyOrWhiteSpacedString"/></exception>
        public static string ThrowIfNullOrEmpty(this string val, string errorMessage)
        {
            return val.IsNows().ThrowIf(DdnDfErrorCode.EmptyOrWhiteSpacedString, errorMessage, val);
        }

        /// <summary>
        /// Throws error if the string is empty or contains only whitespaces
        /// else returns the original string value.
        /// </summary>
        /// <param name="val">Val to check</param>
        /// <param name="errorMessageDelegate">Error message delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.EmptyOrWhiteSpacedString"/></exception>
        public static string ThrowIfNullOrEmpty(this string val, Func<string> errorMessageDelegate)
        {
            return val.IsNows().ThrowIf(DdnDfErrorCode.EmptyOrWhiteSpacedString, errorMessageDelegate, val);
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowIfNullOrEmptyPredicate<T>(T obj) where T : ICollection
        {
            return ReferenceEquals(obj, null) || obj.Count < 1;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static TV ThrowOnMiss<TK, TV>(this Dictionary<TK, TV> dictionary, TK key)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
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
        public static TV ThrowOnMiss<TK, TV>(this Dictionary<TK, TV> dictionary, TK key, string errorMessage)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
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
        public static TV ThrowOnMiss<TK, TV>(this Dictionary<TK, TV> dictionary, TK key,
            Func<string> errorMessageDelegate)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this ConcurrentDictionary<TK, TV> dictionary, TK key)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
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
        public static TV ThrowOnMiss<TK, TV>(this ConcurrentDictionary<TK, TV> dictionary, TK key, string errorMessage)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
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
        public static TV ThrowOnMiss<TK, TV>(this ConcurrentDictionary<TK, TV> dictionary, TK key,
            Func<string> errorMessageDelegate)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this Collections.Concurrent.FastDictionary<TK, TV> dictionary, TK key)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
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
        public static TV ThrowOnMiss<TK, TV>(this Collections.Concurrent.FastDictionary<TK, TV> dictionary, TK key, string errorMessage)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
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
        public static TV ThrowOnMiss<TK, TV>(this Collections.Concurrent.FastDictionary<TK, TV> dictionary, TK key,
            Func<string> errorMessageDelegate)
        {
            return ThrowOnMissPredicate(dictionary, key, out var value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when provided key is not found in dictionary. Else associated value instance is returned
        /// to performed method chaining on the value.
        /// </summary>
        /// <typeparam name="TK">Type of key of the dictionary</typeparam>
        /// <typeparam name="TV">Type of value of the dictionary</typeparam>
        /// <param name="dictionary">dictionary instance</param>
        /// <param name="key">key instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.KeyNotFound"/></exception>
        public static TV ThrowOnMiss<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
        {
            return ThrowOnMissPredicateIDict(dictionary, key, out var value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
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
        public static TV ThrowOnMiss<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, string errorMessage)
        {
            return ThrowOnMissPredicateIDict(dictionary, key, out var value)
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
        public static TV ThrowOnMiss<TK, TV>(this IDictionary<TK, TV> dictionary, TK key,
            Func<string> errorMessageDelegate)
        {
            return ThrowOnMissPredicateIDict(dictionary, key, out var value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

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
            return ThrowOnMissPredicate(dictionary, key, out var value).ThrowIfNot(DdnDfErrorCode.KeyNotFound, value);
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
            return ThrowOnMissPredicate(dictionary, key, out var value)
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
            return ThrowOnMissPredicate(dictionary, key, out var value)
                .ThrowIfNot(DdnDfErrorCode.KeyNotFound, errorMessageDelegate, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowOnMissPredicate<TK, TV>(IReadOnlyDictionary<TK, TV> dictionary, TK key, out TV value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowOnMissPredicateIDict<TK, TV>(IDictionary<TK, TV> dictionary, TK key, out TV value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        #endregion

        #region ThrowIf On Int

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfZero(this int value)
        {
            return value.ThrowIfEqual(0);
        }

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfZero(this int value, string errorMessage)
        {
            return value.ThrowIfEqual(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfZero(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfEqual(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotZero(this int value)
        {
            return value.ThrowIfNotEqual(0);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotZero(this int value, string errorMessage)
        {
            return value.ThrowIfNotEqual(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotZero(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfNotEqual(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfEqual(this int value, int comperand)
        {
            return value.ThrowIfEqual(comperand, () => $"{value} == {comperand}");
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfEqual(this int value, int comperand, string errorMessage)
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static int ThrowIfEqual(this int value, int comperand, Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotEqual(this int value, int comperand)
        {
            return value.ThrowIfNotEqual(comperand, () => $"{value} != {comperand}");
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotEqual(this int value, int comperand, string errorMessage)
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static int ThrowIfNotEqual(this int value, int comperand, Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfNegative(this int value)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
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
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfNegative(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfLess(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value instance to compare</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold)
        {
            return value.ThrowIfLess(threshold, () => $"{value} < {threshold}");
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold, string errorMessage)
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static int ThrowIfLess(this int value, int threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfPositive(this int value)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
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
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfPositive(this int value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfGreater(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold)
        {
            return value.ThrowIfGreater(threshold, () => $"{value} > {threshold}");
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold, string errorMessage)
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static int ThrowIfGreater(this int value, int threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessageDelegate, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualPredicate(int value, int comperand)
        {
            return value.Equals(comperand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LessThanPredicate(int value, int threshold)
        {
            return value < threshold;
        }

        #endregion

        #region ThrowIf On Long

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfZero(this long value)
        {
            return value.ThrowIfEqual(0);
        }

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfZero(this long value, string errorMessage)
        {
            return value.ThrowIfEqual(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfZero(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfEqual(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfEqual(this long value, long comperand)
        {
            return value.ThrowIfEqual(comperand, () => $"{value} == {comperand}");
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfEqual(this long value, long comperand, string errorMessage)
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/>==<paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static long ThrowIfEqual(this long value, long comperand, Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotZero(this long value)
        {
            return value.ThrowIfNotEqual(0);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotZero(this long value, string errorMessage)
        {
            return value.ThrowIfNotEqual(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is NOT 0.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotZero(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfNotEqual(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotEqual(this long value, long comperand)
        {
            return value.ThrowIfNotEqual(comperand, () => $"{value} != {comperand}");
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotEqual(this long value, long comperand, string errorMessage)
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> != <paramref name="comperand"/>.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static long ThrowIfNotEqual(this long value, long comperand, Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value)
        {
            return value.ThrowIfLess(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value, string errorMessage)
        {
            return value.ThrowIfLess(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is strictly less than 0 (-1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfNegative(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfLess(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value instance to compare</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold)
        {
            return value.ThrowIfLess(threshold, () => $"{value} < {threshold}");
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold, string errorMessage)
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static long ThrowIfLess(this long value, long threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value)
        {
            return value.ThrowIfGreater(0);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value, string errorMessage)
        {
            return value.ThrowIfGreater(0, errorMessage);
        }

        /// <summary>
        /// Throws exception when given value is strictly greater than 0 (+1 onwards).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfPositive(this long value, Func<string> errorMessageDelegate)
        {
            return value.ThrowIfGreater(0, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold)
        {
            return value.ThrowIfGreater(threshold, () => $"{value} > {threshold}");
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold, string errorMessage)
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static long ThrowIfGreater(this long value, long threshold, Func<string> errorMessageDelegate)
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessageDelegate, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualPredicate(long value, long comperand)
        {
            return value.Equals(comperand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LessThanPredicate(long value, long threshold)
        {
            return value < threshold;
        }

        #endregion

        #region ThrowIf On Generic Comparer

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand) where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand, string errorMessage) where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand, Func<string> errorMessageDelegate)
            where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer)
        {
            return EqualPredicate(value, comperand, comparer).ThrowIf(DdnDfErrorCode.ValueEqual, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer, string errorMessage)
        {
            return EqualPredicate(value, comperand, comparer).ThrowIf(DdnDfErrorCode.ValueEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueEqual"/></exception>
        public static T ThrowIfEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer,
            Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand, comparer)
                .ThrowIf(DdnDfErrorCode.ValueEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand) where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand, string errorMessage) where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand, Func<string> errorMessageDelegate)
            where T : IEquatable<T>
        {
            return EqualPredicate(value, comperand).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer)
        {
            return EqualPredicate(value, comperand, comparer).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer, string errorMessage)
        {
            return EqualPredicate(value, comperand, comparer).ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when <paramref name="value"/> and <paramref name="comperand"/> are NOT equal.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="comperand">value to compare to</param>
        /// <param name="comparer">comparer instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueNotEqual"/></exception>
        public static T ThrowIfNotEqual<T>(this T value, T comperand, IEqualityComparer<T> comparer,
            Func<string> errorMessageDelegate)
        {
            return EqualPredicate(value, comperand, comparer)
                .ThrowIfNot(DdnDfErrorCode.ValueNotEqual, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value instance to compare</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static T ThrowIfLess<T>(this T value, T threshold) where T : IComparable<T>
        {
            return LessThanPredicate(value, threshold).ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static T ThrowIfLess<T>(this T value, T threshold, string errorMessage) where T : IComparable<T>
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly less than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueLessThanThreshold"/></exception>
        public static T ThrowIfLess<T>(this T value, T threshold, Func<string> errorMessageDelegate)
            where T : IComparable<T>
        {
            return LessThanPredicate(value, threshold)
                .ThrowIf(DdnDfErrorCode.ValueLessThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static T ThrowIfGreater<T>(this T value, T threshold) where T : IComparable<T>
        {
            return LessThanPredicate(threshold, value).ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static T ThrowIfGreater<T>(this T value, T threshold, string errorMessage) where T : IComparable<T>
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is strictly greater than given threshold.
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="threshold">Threshold value of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueGreaterThanThreshold"/></exception>
        public static T ThrowIfGreater<T>(this T value, T threshold, Func<string> errorMessageDelegate)
            where T : IComparable<T>
        {
            return LessThanPredicate(threshold, value)
                .ThrowIf(DdnDfErrorCode.ValueGreaterThanThreshold, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; LowerOfTwoBound OR <paramref name="value"/> &gt; HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static T ThrowIfNotBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl) where T : IComparable<T>
        {
            return value.ThrowIfNotBounded(firstBoundExcl, secondBoundExcl,
                () => $"Either {value} < {firstBoundExcl} Or {value} > {secondBoundExcl}");
        }

        /// <summary>
        /// Throws exception when given comparable value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; LowerOfTwoBound OR <paramref name="value"/> &gt; HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static T ThrowIfNotBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl,
            string errorMessage) where T : IComparable<T>
        {
            return OutOfBoundPredicate(value, firstBoundExcl, secondBoundExcl)
                .ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is out of bound (both bound exclusive, i.e.,
        /// throws when <paramref name="value"/> &lt; LowerOfTwoBound OR <paramref name="value"/> &gt; HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueOutOfBound"/></exception>
        public static T ThrowIfNotBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl,
            Func<string> errorMessageDelegate) where T : IComparable<T>
        {
            return OutOfBoundPredicate(value, firstBoundExcl, secondBoundExcl)
                .ThrowIf(DdnDfErrorCode.ValueOutOfBound, errorMessageDelegate, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is within the bounds (both bound inclusive, i.e.,
        /// throws when LowerOfTwoBound &lt;= <paramref name="value"/> &lt;= HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static T ThrowIfBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl) where T : IComparable<T>
        {
            return value.ThrowIfBounded(firstBoundExcl, secondBoundExcl,
                () => $"{firstBoundExcl} <= {value} <= {secondBoundExcl}");
        }

        /// <summary>
        /// Throws exception when given comparable value is within the bounds (both bound inclusive, i.e.,
        /// throws when LowerOfTwoBound &lt;= <paramref name="value"/> &lt;= HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static T ThrowIfBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl,
            string errorMessage) where T : IComparable<T>
        {
            return OutOfBoundPredicate(value, firstBoundExcl, secondBoundExcl)
                .ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessage, value);
        }

        /// <summary>
        /// Throws exception when given comparable value is within the bounds (both bound inclusive, i.e.,
        /// throws when LowerOfTwoBound &lt;= <paramref name="value"/> &lt;= HigherOfTwoBound).
        /// Else <paramref name="value"/> is returned to performed method chaining.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="firstBoundExcl">first bound of comparison</param>
        /// <param name="secondBoundExcl">second bound of comparison</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.ValueInBound"/></exception>
        public static T ThrowIfBounded<T>(this T value, T firstBoundExcl, T secondBoundExcl,
            Func<string> errorMessageDelegate) where T : IComparable<T>
        {
            return OutOfBoundPredicate(value, firstBoundExcl, secondBoundExcl)
                .ThrowIfNot(DdnDfErrorCode.ValueInBound, errorMessageDelegate, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualPredicate<T>(T value, T comperand) where T : IEquatable<T>
        {
            return value.Equals(comperand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualPredicate<T>(T value, T comperand, IEqualityComparer<T> comparer)
        {
            return comparer.Equals(value, comperand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LessThanPredicate<T>(T value, T threshold) where T : IComparable<T>
        {
            return value.CompareTo(threshold) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool OutOfBoundPredicate<T>(T value, T lowValue, T highValue) where T : IComparable<T>
        {
            if (highValue.CompareTo(lowValue) < 0)
            {
                return LessThanPredicate(value, highValue) || LessThanPredicate(lowValue, value);
            }
            return LessThanPredicate(value, lowValue) || LessThanPredicate(highValue, value);
        }

        #endregion
    }
}