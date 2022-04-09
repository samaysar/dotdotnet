using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Generic Exception class for Dot.Net libraries.
    /// <para>All libraries must throw this exception for known error cases.</para>
    /// <typeparam name="T">Normally should be ENUM type explaining the cause behind the exception</typeparam>
    /// </summary>
    [Serializable]
    public abstract class DdnException<T> : DdnException where T: struct
    {
        /// <summary>
        /// Gets the error code associated.
        /// </summary>
        public T ErrorCode { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        protected DdnException(T errorCode) : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        protected DdnException(T errorCode, string message) : base(errorCode.ToString(), message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="errorCode">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        protected DdnException(T errorCode, string message, Exception inner)
            : base(errorCode.ToString(), message, inner)
        {
            ErrorCode = errorCode;
        }

        /// <inheritdoc/>
        protected DdnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
#if !NET5_0_OR_GREATER
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    /// <summary>
    /// Non generic base exception class for Dot.Net libraries.
    /// </summary>
    [Serializable]
    public abstract class DdnException : Exception
    {
        /// <summary>
        /// Gets the error reason string.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="reason">Associated Error reason</param>
        protected DdnException(string reason) : base(reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="reason">Associated Error code</param>
        /// <param name="message">message text</param>
        protected DdnException(string reason, string message) : base($"({reason}) {message}")
        {
            Reason = reason;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="reason">Associated Error code</param>
        /// <param name="message">message text</param>
        /// <param name="inner">Inner exception</param>
        protected DdnException(string reason, string message, Exception inner)
            : base($"({reason}) {message}", inner)
        {
            Reason = reason;
        }

        /// <inheritdoc/>
        protected DdnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
#if !NET5_0_OR_GREATER
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.ThrowIfNull($"{nameof(SerializationInfo)} object is null").AddValue("ErrorReason", Reason);
            base.GetObjectData(info, context);
        }
    }
}