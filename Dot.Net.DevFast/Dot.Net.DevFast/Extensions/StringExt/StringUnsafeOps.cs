using System;
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
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Does not validate the existence of parsed value. Could be useful when
        /// it is known for sure that the parsed value is among existing value.</para>
        /// <para>Also check <seealso cref="StringSafeOps.ToEnumSafe{T}(string,out T,bool)"/> method</para>
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <param name="value">parsed value</param>
        /// <param name="ignoreCase">true to ignore case, else false to consider string casing</param>
        /// <returns>True if parsing is successful else false</returns>
        /// <typeparam name="T">Enum type</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static bool ToEnumUnsafe<T>(this string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return Enum.TryParse(input, ignoreCase, out value);
        }

        /// <summary>
        /// Tries parsing <seealso cref="string"/> to <seealso cref="Enum"/> value.
        /// <para>Does not validate the existence of parsed value. Could be useful when
        /// it is known for sure that the parsed value is among existing value.</para>
        /// <para>Also check <seealso cref="StringSafeOps.ToEnumSafe{T}(string,out T?,bool)"/> method</para>
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
        public static bool ToEnumUnsafe<T>(this string input, out T? value, bool ignoreCase = true)
            where T : struct
        {
            value = null;
            if (string.IsNullOrWhiteSpace(input)) return true;
            if (!input.ToEnumUnsafe(out T parsedValue, ignoreCase)) return false;
            value = parsedValue;
            return true;
        }

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
                    $"{nameof(subPaths)} array invalid inside {nameof(ToDirectoryInfo)}", subPaths);
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
    }
}