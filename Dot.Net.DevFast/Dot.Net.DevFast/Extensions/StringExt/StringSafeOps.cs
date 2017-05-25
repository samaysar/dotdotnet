using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on Safe (non-error throwing, except GIGO) string operations
    /// </summary>
    public static class StringSafeOps
    {
        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Does not validate the existence of parsed value. Could be useful when
        /// it is known for sure that the parsed value is among existing value.</para>
        /// <para>Also check <see cref="ToEnumChecked{T}(string,out T,bool)"/>
        /// and <see cref="ToEnumCheckedOrDefault{T}"/> and 
        /// <see cref="ToEnumUncheckedOrDefault{T}"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static bool ToEnumUnchecked<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return Enum.TryParse(input, ignoreCase, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Does not validate the existence of parsed value. Could be useful when
        /// it is known for sure that the parsed value is among existing value.</para>
        /// <para>Also check <see cref="ToEnumChecked{T}(string,out T?,bool)"/>,
        /// <see cref="ToEnumCheckedOrDefault{T}"/> and 
        /// <see cref="ToEnumUncheckedOrDefault{T}"/> methods</para>
        /// <para>Returns true when:
        /// <list type="bullet">
        /// <item><description><paramref name="input"/> is <seealso cref="string.IsNullOrWhiteSpace"/>
        /// and out <paramref name="value"/> as null.</description></item>
        /// <item><description><paramref name="input"/> is parsable to <typeparamref name="T"/> with
        /// <paramref name="value"/> as the parsed outcome without checking whether it is defined or not.</description></item>
        /// </list></para>
        /// <para>Returns false when <paramref name="input"/> is NOT parsable to <typeparamref name="T"/>
        /// and <paramref name="value"/> as null.</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static bool ToEnumUnchecked<T>(this string input, out T? value, bool ignoreCase = true)
            where T : struct
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.ToEnumUnchecked(out T parsedValue, ignoreCase)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>If parsing is successful then calls <seealso cref="Enum.IsDefined"/>. Useful when
        /// it is not certain whether the parsed value will result in existing define enum value.
        /// (example when parsing integers back to enum coming from outside)</para>
        /// <para>Also check <see cref="ToEnumUnchecked{T}(string,out T,bool)"/>
        /// and <see cref="ToEnumCheckedOrDefault{T}"/> and 
        /// <see cref="ToEnumUncheckedOrDefault{T}"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static bool ToEnumChecked<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumUnchecked(out value, ignoreCase) &&
                   Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>If parsing is successful then calls <seealso cref="Enum.IsDefined"/>. Useful when
        /// it is not certain whether the parsed value will result in existing define enum value.
        /// (example when parsing integers back to enum coming from outside)</para>
        /// <para>Also check <see cref="ToEnumUnchecked{T}(string,out T?,bool)"/>
        /// and <see cref="ToEnumCheckedOrDefault{T}"/> and 
        /// <see cref="ToEnumUncheckedOrDefault{T}"/> methods</para>
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
        public static bool ToEnumChecked<T>(this string input, out T? value, bool ignoreCase = true)
            where T : struct
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.ToEnumChecked(out T parsedValue, ignoreCase)) return false;
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
        public static string TrimSafeOrDefault(this string input, string defaultValue, params char[] trimChars)
        {
            return ReferenceEquals(input, null) ? defaultValue : input.Trim(trimChars);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value along with
        /// <seealso cref="Enum.IsDefined"/> check. Useful when it is not certain whether the parsed value 
        /// will result in existing define enum value (example when parsing integers back to enum coming from outside).
        /// If parsing is successful then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// <para>Also check <see cref="ToEnumUnchecked{T}(string,out T,bool)"/>,
        /// <see cref="ToEnumUnchecked{T}(string,out T?,bool)"/>,
        /// <see cref="ToEnumChecked{T}(string,out T,bool)"/> and
        /// <see cref="ToEnumChecked{T}(string,out T?,bool)"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static T ToEnumCheckedOrDefault<T>(this string input, T defaultVal, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumChecked(out T value, ignoreCase) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value (NOTE: <seealso cref="Enum.IsDefined"/> check
        /// is NOT performed). Useful when it is known for sure that the parsed value is among existing value..
        /// If parsing is successful then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// <para>Also check <see cref="ToEnumUnchecked{T}(string,out T,bool)"/>,
        /// <see cref="ToEnumUnchecked{T}(string,out T?,bool)"/>,
        /// <see cref="ToEnumChecked{T}(string,out T,bool)"/> and
        /// <see cref="ToEnumChecked{T}(string,out T?,bool)"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static T ToEnumUncheckedOrDefault<T>(this string input, T defaultVal, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumUnchecked(out T value, ignoreCase) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="bool"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool ToOrDefault(this string input, bool defaultVal)
        {
            return input.TryTo(out bool value) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="int"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static int ToOrDefault(this string input, int defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out int value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="long"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static long ToOrDefault(this string input, long defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out long value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="byte"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static byte ToOrDefault(this string input, byte defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out byte value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="sbyte"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static sbyte ToOrDefault(this string input, sbyte defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out sbyte value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="short"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static short ToOrDefault(this string input, short defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out short value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ushort"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static ushort ToOrDefault(this string input, ushort defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out ushort value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="uint"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static uint ToOrDefault(this string input, uint defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out uint value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ulong"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static ulong ToOrDefault(this string input, ulong defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out ulong value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="float"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static float ToOrDefault(this string input, float defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out float value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="double"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static double ToOrDefault(this string input, double defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out double value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="decimal"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        public static decimal ToOrDefault(this string input, decimal defaultVal, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out decimal value, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// If parsing is successful then returns the parsed value else returns the 
        /// <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="format">date format string</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static DateTime ToOrDefault(this string input, DateTime defaultVal, string format,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return input.TryTo(out DateTime value, format, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing
        /// based on given set of formats. If parsing is successful then returns the parsed value 
        /// else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="formats">date format strings</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static DateTime ToOrDefault(this string input, DateTime defaultVal, string[] formats,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return input.TryTo(out DateTime value, formats, style, formatProvider) ? value : defaultVal;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value. If parsing is successful
        /// then returns the parsed value else returns the <paramref name="defaultVal"/>.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="defaultVal">Default value to return when parsing fails</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static DateTime ToOrDefault(this string input, DateTime defaultVal,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return input.TryTo(out DateTime value, style, formatProvider) ? value : defaultVal;
        }
    }
}