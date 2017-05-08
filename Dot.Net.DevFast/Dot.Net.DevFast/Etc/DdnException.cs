using System;

namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Generic Exception class for Dot.Net libraries.
    /// <para>All libraries must throw this exception for known error cases.</para>
    /// <typeparam name="T">Normally should be ENUM type explaining the cause behind the exception</typeparam>
    /// </summary>
    public sealed class DdnException<T> : DdnException where T: struct, IFormattable
    {
        /// <summary>
        /// Gets the error code associated.
        /// </summary>
        public T ErrorCode { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        public DdnException(T errorCode) : base($"{errorCode:G}")
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        public DdnException(T errorCode, string message) : base($"{errorCode:G}", message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        public DdnException(T errorCode, string message, Exception inner)
            : base($"{errorCode:G}", message, inner)
        {
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Non generic base exception class for Dot.Net libraries.
    /// </summary>
    public class DdnException : Exception
    {
        /// <summary>
        /// Gets the error reason string.
        /// </summary>
        public string ErrorReason { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorReason">Associated Error reason</param>
        public DdnException(string errorReason) : base(errorReason)
        {
            ErrorReason = errorReason;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorReason">Associated Error code</param>
        /// <param name="message">message text</param>
        public DdnException(string errorReason, string message) : base($"{errorReason}. {message}")
        {
            ErrorReason = errorReason;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorReason">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        public DdnException(string errorReason, string message, Exception inner)
            : base($"{errorReason}. {message}", inner)
        {
            ErrorReason = errorReason;
        }
    }
}