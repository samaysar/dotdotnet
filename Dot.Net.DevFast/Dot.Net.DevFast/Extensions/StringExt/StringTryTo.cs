using System;
using System.Globalization;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension methods to parse string to different types.
    /// </summary>
    public static class StringTryTo
    {
        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Validates <typeparamref name="T"/> is enum but does NOT
        /// check if parsed value <seealso cref="Enum.IsDefined"/></para>
        /// <para>Also check <seealso cref="StringUnsafeOps.ToEnumUnsafe{T}"/> and 
        /// <seealso cref="StringSafeOps.ToEnumSafe{T}"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryToEnum<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            value = default(T);
            return typeof(T).IsEnum && Enum.TryParse(input, ignoreCase, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="bool"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out bool value)
        {
            return bool.TryParse(input, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Type"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore string casing else false to consider casing</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out Type value, bool ignoreCase = true)
        {
            value = default(Type);
            return !ReferenceEquals(null, input) &&
                   !ReferenceEquals((value = Type.GetType(input, false, ignoreCase)), null);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="int"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out int value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return int.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="long"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out long value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return long.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="byte"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out byte value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return byte.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="sbyte"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out sbyte value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return sbyte.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="short"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out short value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return short.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ushort"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out ushort value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return ushort.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="uint"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out uint value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return uint.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ulong"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out ulong value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return ulong.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="float"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out float value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return float.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="double"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out double value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return double.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="decimal"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out decimal value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return decimal.TryParse(input, style, formatProvider ?? FixedValues.English, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="format">date format string</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out DateTime value, string format,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return DateTime.TryParseExact(input, format, formatProvider ?? FixedValues.English, style, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="formats">date format strings</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, string[] formats, out DateTime value,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return DateTime.TryParseExact(input, formats, formatProvider ?? FixedValues.English, style, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out DateTime value,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return DateTime.TryParse(input, formatProvider ?? FixedValues.English, style, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="bool"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out bool? value)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out bool parsedValue)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="int"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out int? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out int parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="long"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out long? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out long parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="byte"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out byte? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out byte parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="sbyte"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out sbyte? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out sbyte parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="short"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out short? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out short parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ushort"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out ushort? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out ushort parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="uint"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out uint? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out uint parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ulong"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out ulong? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out ulong parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="float"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out float? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out float parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="double"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out double? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out double parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="decimal"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out decimal? value, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out decimal parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="format">date format string</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out DateTime? value, string format,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out DateTime parsedValue, format, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="formats">date format strings</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, string[] formats, out DateTime? value,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(formats, out DateTime parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        public static bool TryTo(this string input, out DateTime? value,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.TryTo(out DateTime parsedValue, style, formatProvider)) return false;
            value = parsedValue;
            return true;
        }
    }
}