﻿using System;
using System.Runtime.CompilerServices;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension library to produce conditional error
    /// </summary>
    public static class ThrowError
    {
        /// <summary>
        /// Throws <seealso cref="DdnDfException"/> with provided code.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowDfError(this DdnDfErrorCode code)
        {
            throw new DdnDfException(code);
        }

        /// <summary>
        /// Throws <seealso cref="DdnDfException"/> with provided code and message.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <param name="message">message</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowDfError(this DdnDfErrorCode code, string message)
        {
            throw new DdnDfException(code, message);
        }

        /// <summary>
        /// Throws <seealso cref="DdnDfException"/> with provided code, message and inner exception
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        /// <exception cref="DdnDfException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowDfError(this DdnDfErrorCode code, string message, Exception innerException)
        {
            throw new DdnDfException(code, message, innerException);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="obj">Object to return if condition is false</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
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
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
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
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIf<T>(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate,
            T obj)
        {
            if (truthValue) errorCode.ThrowDfError(errorMessageDelegate());
            return obj;
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="obj">Object to return if condition is true</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
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
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
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
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNot<T>(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate,
            T obj)
        {
            return (!truthValue).ThrowIf(errorCode, errorMessageDelegate, obj);
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj) where T : class
        {
            return ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject, obj);
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
            return ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject, errorMessage, obj);
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
            return ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject, errorMessageDelegate, obj);
        }

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyArray"/></exception>
        public static T[] ThrowIfNullOrEmpty<T>(this T[] obj)
        {
            return (ReferenceEquals(obj, null) || obj.Length < 1).ThrowIf(DdnDfErrorCode.NullOrEmptyArray, obj);
        }

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyArray"/></exception>
        public static T[] ThrowIfNullOrEmpty<T>(this T[] obj, string errorMessage)
        {
            return (ReferenceEquals(obj, null) || obj.Length < 1).ThrowIf(DdnDfErrorCode.NullOrEmptyArray, errorMessage,
                obj);
        }

        /// <summary>
        /// Throws exception when provided array is either null or empty. Same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullOrEmptyArray"/></exception>
        public static T[] ThrowIfNullOrEmpty<T>(this T[] obj, Func<string> errorMessageDelegate)
        {
            return (ReferenceEquals(obj, null) || obj.Length < 1).ThrowIf(DdnDfErrorCode.NullOrEmptyArray,
                errorMessageDelegate, obj);
        }
    }
}