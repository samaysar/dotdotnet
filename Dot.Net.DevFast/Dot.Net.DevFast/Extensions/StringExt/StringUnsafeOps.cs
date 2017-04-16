using System;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on UnSafe string transformation
    /// </summary>
    public static class StringUnsafeOps
    {
        /// <summary>
        /// Trims string when not null.
        /// <para>Also check <seealso cref="StringSafeOps.SafeTrimOrEmpty"/>, 
        /// <seealso cref="StringSafeOps.SafeTrimOrNull"/> and 
        /// <seealso cref="StringSafeOps.SafeTrimOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        /// <exception cref="DdnDfException">When null string is passed as input.
        /// <seealso cref="DdnDfException.ErrorCode"/> is 
        /// <seealso cref="DdnDfErrorCode.NullString"/></exception>
        public static string UnsafeTrim(this string input, params char[] trimChars)
        {
            if (ReferenceEquals(null, input))
            {
                throw new DdnDfException(DdnDfErrorCode.NullString);
            }
            return input.Trim(trimChars);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Neither validates <typeparamref name="T"/> to be enum NOR validates the existence
        /// of parsed value</para>
        /// <para>Also check <seealso cref="StringTryTo.TryToEnum{T}"/> and 
        /// <seealso cref="StringSafeOps.ToEnumSafe{T}"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <exception cref="ArgumentException">when <typeparamref name="T"/> is invalid</exception>
        public static bool ToEnumUnsafe<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return Enum.TryParse(input, ignoreCase, out value);
        }
    }
}