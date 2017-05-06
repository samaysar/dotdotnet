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
        /// Throws exception when provided object is null else same object instance is returned
        /// to performed method chaining.
        /// </summary>
        /// <typeparam name="T">Any reference type</typeparam>
        /// <param name="obj">instance</param>
        /// <exception cref="DdnDfException">Error code as <seealso cref="DdnDfErrorCode.NullObject"/></exception>
        public static T ThrowIfNull<T>(this T obj) where T : class
        {
            if (ReferenceEquals(obj, null))
            {
                DdnDfErrorCode.NullObject.Throw();
            }
            return obj;
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
            if (ReferenceEquals(obj, null))
            {
                DdnDfErrorCode.NullObject.Throw(errorMessage);
            }
            return obj;
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
            if (ReferenceEquals(obj, null))
            {
                DdnDfErrorCode.NullObject.Throw(errorMessageDelegate());
            }
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
            if (ReferenceEquals(obj, null))
            {
                throw new TEx();
            }
            return obj;
        }

        /// <summary>
        /// Throws <seealso cref="DdnDfException"/> with provided code.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <exception cref="DdnDfException"></exception>
        public static void Throw(this DdnDfErrorCode code)
        {
            throw new DdnDfException(code);
        }

        /// <summary>
        /// Throws <seealso cref="DdnDfException"/> with provided code and message.
        /// </summary>
        /// <param name="code">Code of the exception</param>
        /// <param name="message">message</param>
        /// <exception cref="DdnDfException"></exception>
        public static void Throw(this DdnDfErrorCode code, string message)
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
        public static void Throw(this DdnDfErrorCode code, string message, Exception innerException)
        {
            throw new DdnDfException(code, message, innerException);
        }
    }
}