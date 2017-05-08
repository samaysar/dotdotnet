using System;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension library to produce conditional error
    /// </summary>
    public static class ThrowError
    {
        /// <summary>
        /// Throws <seealso cref="DdnException{T}"/> with provided code.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <exception cref="DdnException{T}"></exception>
        public static void Throw<T>(this T code) where T : struct, IFormattable
        {
            throw new DdnException<T>(code);
        }

        /// <summary>
        /// Throws <seealso cref="DdnException{T}"/> with provided code and message.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <param name="message">message</param>
        /// <exception cref="DdnException{T}"></exception>
        public static void Throw<T>(this T code, string message) where T : struct, IFormattable
        {
            throw new DdnException<T>(code, message);
        }

        /// <summary>
        /// Throws <seealso cref="DdnException{T}"/> with provided code, message and inner exception
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        /// <exception cref="DdnException{T}"></exception>
        public static void Throw<T>(this T code, string message, Exception innerException) where T : struct, IFormattable
        {
            throw new DdnException<T>(code, message, innerException);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <typeparam name="TEx">Exception type to throw</typeparam>
        public static void ThrowIf<TEx>(this bool truthValue) where TEx : Exception, new()
        {
            if (truthValue) throw new TEx();
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIf(this bool truthValue, DdnDfErrorCode errorCode)
        {
            if (truthValue) errorCode.Throw();
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessage">Error message</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIf(this bool truthValue, DdnDfErrorCode errorCode, string errorMessage)
        {
            if (truthValue) errorCode.Throw(errorMessage);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is true.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessageDelegate">Error message delegate</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIf(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate)
        {
            if (truthValue) errorCode.Throw(errorMessageDelegate());
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <typeparam name="TEx">Exception type to throw</typeparam>
        public static void ThrowIfNot<TEx>(this bool truthValue) where TEx : Exception, new()
        {
            (!truthValue).ThrowIf<TEx>();
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIfNot(this bool truthValue, DdnDfErrorCode errorCode)
        {
            (!truthValue).ThrowIf(errorCode);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessage">Error message</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIfNot(this bool truthValue, DdnDfErrorCode errorCode, string errorMessage)
        {
            (!truthValue).ThrowIf(errorCode, errorMessage);
        }

        /// <summary>
        /// Throws exception when <paramref name="truthValue"/> is false.
        /// </summary>
        /// <param name="truthValue">truth value</param>
        /// <param name="errorCode">error code of the exception</param>
        /// <param name="errorMessageDelegate">Error message delegate</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static void ThrowIfNot(this bool truthValue, DdnDfErrorCode errorCode, Func<string> errorMessageDelegate)
        {
            (!truthValue).ThrowIf(errorCode, errorMessageDelegate);
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj) where T : class
        {
            ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject);
            return obj;
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessage">error message of the exception</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj, string errorMessage) where T : class
        {
            ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject, errorMessage);
            return obj;
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <param name="errorMessageDelegate">error message generating delegate</param>
        /// <exception cref="DdnException{T}">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj, Func<string> errorMessageDelegate) where T : class
        {
            ReferenceEquals(obj, null).ThrowIf(DdnDfErrorCode.NullObject, errorMessageDelegate);
            return obj;
        }

        /// <summary>
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <typeparam name="TEx">Exception type thats required to be thrown</typeparam>
        /// <param name="obj">instance</param>
        public static T ThrowIfNull<T, TEx>(this T obj)
            where T : class
            where TEx : Exception, new()
        {
            ReferenceEquals(obj, null).ThrowIf<TEx>();
            return obj;
        }
    }
}