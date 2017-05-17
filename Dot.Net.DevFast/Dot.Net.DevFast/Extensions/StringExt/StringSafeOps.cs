using System;
using System.Diagnostics.Contracts;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on Safe (non-error throwing, except GIGO) string operations
    /// </summary>
    public static class StringSafeOps
    {
        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>If parsing is successful then calls <seealso cref="Enum.IsDefined"/>. Useful when
        /// it is not certain whether the parsed value will result in existing define enum value.
        /// (example when parsing integers back to enum coming from outside)</para>
        /// <para>Also check <seealso cref="StringUnsafeOps.ToEnumUnsafe{T}(string,out T,bool)"/> method</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        [Pure]
        public static bool ToEnumSafe<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumUnsafe(out value, ignoreCase) &&
                   Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>If parsing is successful then calls <seealso cref="Enum.IsDefined"/>. Useful when
        /// it is not certain whether the parsed value will result in existing define enum value.
        /// (example when parsing integers back to enum coming from outside)</para>
        /// <para>Also check <seealso cref="StringUnsafeOps.ToEnumUnsafe{T}(string,out T?,bool)"/> method</para>
        /// <para>Returns true when:
        /// <list type="bullet">
        /// <item><description><paramref name="input"/> is <seealso cref="string.IsNullOrWhiteSpace"/>
        /// and out <paramref name="value"/> as null.</description></item>
        /// <item><description><paramref name="input"/> is parsable to <typeparamref name="T"/> with
        /// <paramref name="value"/> as the parsed outcome among defined values.</description></item>
        /// </list></para>
        /// <para>Returns false when <paramref name="input"/> is NOT parsable to <typeparamref name="T"/>
        /// or the value does not exits among defined value, in this case <paramref name="value"/> as null.</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        [Pure]
        public static bool ToEnumSafe<T>(this string input, out T? value, bool ignoreCase = true)
            where T : struct
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.ToEnumSafe(out T parsedValue, ignoreCase)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// If value is null <seealso cref="string.Empty"/> is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.TrimUnsafe"/>,
        /// <seealso cref="TrimSafeOrNull"/> and <seealso cref="TrimSafeOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        [Pure]
        public static string TrimSafeOrEmpty(this string input, params char[] trimChars)
        {
            return input.TrimSafeOrDefault(string.Empty, trimChars);
        }

        /// <summary>
        /// If value is null, null is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.TrimUnsafe"/>,
        /// <seealso cref="TrimSafeOrEmpty"/> and <seealso cref="TrimSafeOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        [Pure]
        public static string TrimSafeOrNull(this string input, params char[] trimChars)
        {
            return input.TrimSafeOrDefault(null, trimChars);
        }

        /// <summary>
        /// If value is null <paramref name="defaultValue"/> is returned else trimmed string.
        /// <para>Also check <seealso cref="StringUnsafeOps.TrimUnsafe"/>,
        /// <seealso cref="TrimSafeOrEmpty"/> and <seealso cref="TrimSafeOrNull"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="defaultValue">default value to return when input is null.</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        [Pure]
        public static string TrimSafeOrDefault(this string input, string defaultValue, params char[] trimChars)
        {
            return ReferenceEquals(input, null) ? defaultValue : input.Trim(trimChars);
        }
    }
}