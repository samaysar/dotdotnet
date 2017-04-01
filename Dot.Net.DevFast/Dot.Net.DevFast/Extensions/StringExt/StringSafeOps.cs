using System;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on Safe string transformation
    /// </summary>
    public static class StringSafeOps
    {
        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Checks if <typeparamref name="T"></typeparamref> is enum</para>
        /// <para>If parsing is successful then calls <seealso cref="Enum.IsDefined"/></para>
        /// <para>Also check <seealso cref="StringTryTo.TryToEnum{T}"/> and 
        /// <seealso cref="StringUnsafeOps.ToEnumUnsafe{T}"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool ToEnumSafe<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            value = default(T);
            var enumType = typeof(T);
            return enumType.IsEnum && Enum.TryParse(input, ignoreCase, out value) &&
                   Enum.IsDefined(enumType, value);
        }

        /// <summary>
        /// If value is null or contains only whitespaces,
        /// <seealso cref="string.Empty"/> is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.UnsafeTrim"/>,
        /// <seealso cref="NullSafeTrim"/> and <seealso cref="SafeTrimOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        public static string SafeTrim(this string input, params char[] trimChars)
        {
            return input.SafeTrimOrDefault(string.Empty, trimChars);
        }

        /// <summary>
        /// If value is null or contains only whitespaces, null is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.UnsafeTrim"/>,
        /// <seealso cref="SafeTrim"/> and <seealso cref="SafeTrimOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        public static string NullSafeTrim(this string input, params char[] trimChars)
        {
            return input.SafeTrimOrDefault(null, trimChars);
        }

        /// <summary>
        /// If value is null or contains only whitespaces,
        /// <paramref name="defaultValue"/> is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.UnsafeTrim"/> and
        /// <seealso cref="SafeTrim"/> and <seealso cref="NullSafeTrim"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="defaultValue">default value to return when input is Null or whitespaces.</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        public static string SafeTrimOrDefault(this string input, string defaultValue, params char[] trimChars)
        {
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input.Trim(trimChars);
        }
    }
}