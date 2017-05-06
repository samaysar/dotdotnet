namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Error codes associated with <seealso cref="DdnDfException"/>.
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
        /// When supplied array is a null instance.
        /// </summary>
        NullArray,

        /// <summary>
        /// When supplied array is has no element.
        /// </summary>
        EmptyArray,

        /// <summary>
        /// When supplied object instance is null.
        /// </summary>
        NullObject
    }
}