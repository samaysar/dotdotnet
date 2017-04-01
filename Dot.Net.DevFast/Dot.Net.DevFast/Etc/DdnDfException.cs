using System;

namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Exception class for Dot.Net Extension class.
    /// <para>The whole library must throw this exception for known error cases.</para>
    /// </summary>
    public sealed class DdnDfException : Exception
    {
        /// <summary>
        /// Gets the error code associated.
        /// </summary>
        public DdnDfErrorCode ErrorCode { get; }

        /// <summary>
        /// Default Ctor.
        /// <para>Sets <see cref="ErrorCode"/> to <seealso cref="DdnDfErrorCode.Unspecified"/></para>
        /// </summary>
        public DdnDfException() : this(DdnDfErrorCode.Unspecified)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        public DdnDfException(DdnDfErrorCode errorCode) : base(errorCode.ToString("G"))
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        public DdnDfException(DdnDfErrorCode errorCode, string message) : base($"{errorCode:G}. {message}")
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        public DdnDfException(DdnDfErrorCode errorCode, string message, Exception inner)
            : base($"{errorCode:G}. {message}", inner)
        {
            ErrorCode = errorCode;
        }
    }
}