using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on UnSafe (possible exception or invalid results) string operations
    /// </summary>
    public static class StringUnsafeOps
    {
        /// <summary>
        /// Trims string when not null else throws error (another way to avoid <seealso cref="NullReferenceException"/>)
        /// <para>Also check <seealso cref="StringSafeOps.TrimSafeOrEmpty"/>, 
        /// <seealso cref="StringSafeOps.TrimSafeOrNull"/> and 
        /// <seealso cref="StringSafeOps.TrimSafeOrDefault"/></para>
        /// </summary>
        /// <param name="input">Value to trim safe</param>
        /// <param name="trimChars">optional. when not given any char set,
        /// whitespaces will be removed</param>
        /// <exception cref="DdnDfException">When null string is passed as input
        /// (refer <seealso cref="DdnDfErrorCode.NullString"/>)</exception>
        public static string TrimUnsafe(this string input, params char[] trimChars)
        {
            return ReferenceEquals(null, input).ThrowIf(DdnDfErrorCode.NullString, "cannot trim", input).Trim(trimChars);
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance after joining filename with extension
        /// to the <paramref name="folderPath"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="folderPath">Folder path to the file</param>
        /// <param name="filename">filename without extension</param>
        /// <param name="extension">extension without period, e.g., "txt", "json" etc</param>
        public static FileInfo ToFileInfo(this string folderPath, string filename, string extension)
        {
            return folderPath.ToFileInfo(filename + StdLookUps.ExtSeparator + extension);
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance after joining <paramref name="filenameWithExt"/>
        /// to the <paramref name="folderPath"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="folderPath">Folder path to the file</param>
        /// <param name="filenameWithExt">file name with extensions, e.g., "abc.txt", "mydata.json" etc</param>
        public static FileInfo ToFileInfo(this string folderPath, string filenameWithExt)
        {
            return Path.Combine(folderPath, filenameWithExt).ToFileInfo();
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance from given <paramref name="fullFilePath"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="fullFilePath">Complete path of the file</param>
        public static FileInfo ToFileInfo(this string fullFilePath)
        {
            return new FileInfo(fullFilePath);
        }

        /// <summary>
        /// Returns a new <seealso cref="DirectoryInfo"/> instance from combined path using
        /// <paramref name="basePath"/> and <paramref name="subPaths"/>.
        /// <para>Expect all <seealso cref="DirectoryInfo"/> related errors.</para>
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="subPaths">individual path components</param>
        /// <param name="create">if true <seealso cref="Directory.CreateDirectory(string)"/> will be called</param>
        /// <exception cref="DdnDfException">When null array is passed as input
        /// (refer  <seealso cref="DdnDfErrorCode.NullOrEmptyCollection"/>)</exception>
        public static DirectoryInfo ToDirectoryInfo(this string basePath, string[] subPaths, bool create = false)
        {
            subPaths = (ReferenceEquals(null, subPaths) || subPaths.Length == 0)
                .ThrowIf(DdnDfErrorCode.NullOrEmptyCollection,
                    () => $"{nameof(subPaths)} array invalid inside {nameof(ToDirectoryInfo)}", subPaths);
            var paths = new string[1 + subPaths.Length];
            paths[0] = basePath;
            Array.Copy(subPaths, 0, paths, 1, subPaths.Length);
            return Path.Combine(paths).ToDirectoryInfo(create);
        }

        /// <summary>
        /// Returns a new <seealso cref="DirectoryInfo"/> instance from given <paramref name="fullPath"/>.
        /// <para>Expect all <seealso cref="DirectoryInfo"/> related errors.</para>
        /// </summary>
        /// <param name="fullPath">Full path to the directory</param>
        /// <param name="create">if true <seealso cref="Directory.CreateDirectory(string)"/> will be called</param>
        public static DirectoryInfo ToDirectoryInfo(this string fullPath, bool create = false)
        {
            return create ? Directory.CreateDirectory(fullPath) : new DirectoryInfo(fullPath);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value along with
        /// <seealso cref="Enum.IsDefined"/> check. Useful when it is not certain whether the parsed value 
        /// will result in existing define enum value (example when parsing integers back to enum coming from outside).
        /// If parsing is successful then returns the parsed value else throws an exception.
        /// <para>Also check <seealso cref="StringTryTo.TryToEnumUnchecked{T}(string,out T,bool)"/>,
        /// <seealso cref="StringTryTo.TryToEnumUnchecked{T}(string,out T?,bool)"/>,
        /// <seealso cref="StringTryTo.TryToEnum{T}(string,out T,bool)"/> and
        /// <seealso cref="StringTryTo.TryToEnum{T}(string,out T?,bool)"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static T ToEnum<T>(this string input, bool ignoreCase = true)
            where T : struct
        {
            return input.TryToEnum(out T value, ignoreCase).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {typeof(T)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value (NOTE: <seealso cref="Enum.IsDefined"/> check
        /// is NOT performed). Useful when it is known for sure that the parsed value is among existing value..
        /// If parsing is successful then returns the parsed value else throws an exception.
        /// <para>Also check <seealso cref="StringTryTo.TryToEnumUnchecked{T}(string,out T,bool)"/>,
        /// <seealso cref="StringTryTo.TryToEnumUnchecked{T}(string,out T?,bool)"/>,
        /// <seealso cref="StringTryTo.TryToEnum{T}(string,out T,bool)"/> and
        /// <seealso cref="StringTryTo.TryToEnum{T}(string,out T?,bool)"/> methods</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static T ToEnumUnchecked<T>(this string input, bool ignoreCase = true)
            where T : struct
        {
            return input.TryToEnumUnchecked(out T value, ignoreCase).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {typeof(T)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="bool"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static bool ToBool(this string input)
        {
            return input.TryTo(out bool value).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Boolean)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="int"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static int ToInt(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out int value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Int32)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="long"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static long ToLong(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out long value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Int64)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="byte"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static byte ToByte(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out byte value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Byte)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="sbyte"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static sbyte ToSByte(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out sbyte value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(SByte)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="short"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static short ToShort(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out short value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Int16)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ushort"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static ushort ToUShort(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out ushort value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(UInt16)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="uint"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static uint ToUInt(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out uint value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(UInt32)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="ulong"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static ulong ToULong(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out ulong value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(UInt64)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="float"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static float ToFloat(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out float value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Single)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="double"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static double ToDouble(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out double value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Double)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="decimal"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="style">style to use during parsing</param>
        /// <param name="formatProvider">format provider</param>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static decimal ToDecimal(this string input, NumberStyles style = NumberStyles.Any,
            IFormatProvider formatProvider = null)
        {
            return input.TryTo(out decimal value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(Decimal)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing.
        /// If parsing is successful then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="format">date format string</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static DateTime ToDateTime(this string input, string format,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return
                input.TryTo(out DateTime value, format, style, formatProvider)
                    .ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                        () => $"Unable to parse {input} to {nameof(DateTime)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value using exact parsing
        /// based on given set of formats. If parsing is successful then returns the parsed value 
        /// else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="formats">date format strings</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static DateTime ToDateTime(this string input, string[] formats,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return
                input.TryTo(out DateTime value, formats, style, formatProvider)
                    .ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                        () => $"Unable to parse {input} to {nameof(DateTime)}.", value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="DateTime"/> value. If parsing is successful
        /// then returns the parsed value else throws an exception.
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="formatProvider">format provider</param>
        /// <param name="style">datetime style</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <exception cref="DdnDfException">with <seealso cref="DdnDfErrorCode.StringParsingFailed"/></exception>
        public static DateTime ToDateTime(this string input,
            DateTimeStyles style = DateTimeStyles.AssumeLocal, IFormatProvider formatProvider = null)
        {
            return input.TryTo(out DateTime value, style, formatProvider).ThrowIfNot(DdnDfErrorCode.StringParsingFailed,
                () => $"Unable to parse {input} to {nameof(DateTime)}.", value);
        }
    }
}