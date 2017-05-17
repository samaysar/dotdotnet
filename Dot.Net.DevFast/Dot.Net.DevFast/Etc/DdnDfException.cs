using System;

namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Enum Error codes associated with <seealso cref="DdnDfException"/>
    /// for DevFast project.
    /// </summary>
    public enum DdnDfErrorCode
    {
        /// <summary>
        /// Unspecified code, when the exception is created by using 
        /// its default Ctor.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// String instance is null
        /// </summary>
        NullString,

        /// <summary>
        /// When the supplied collection is null or empty.
        /// </summary>
        NullOrEmptyCollection,

        ///// <summary>
        ///// When supplied array is a null instance.
        ///// </summary>
        //NullArray,

        ///// <summary>
        ///// When supplied array is has no element.
        ///// </summary>
        //EmptyArray,

        /// <summary>
        /// When supplied object instance is null.
        /// </summary>
        NullObject
    }

    /// <summary>
    /// Exceptions used inside DevFast library.
    /// </summary>
    public sealed class DdnDfException : DdnException<DdnDfErrorCode>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        public DdnDfException(DdnDfErrorCode errorCode) : base(errorCode)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        public DdnDfException(DdnDfErrorCode errorCode, string message) : base(errorCode, message)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        public DdnDfException(DdnDfErrorCode errorCode, string message, Exception inner) : base(errorCode, message, inner)
        {
        }
    }
}