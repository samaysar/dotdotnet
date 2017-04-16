using System;
using System.IO;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StringExt
{
    /// <summary>
    /// Extension method on UnSafe (with possible error throw) string operations
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

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance after joining filename with extension
        /// to the <paramref name="folderPath"/>.
        /// </summary>
        /// <param name="folderPath">Folder path to the file</param>
        /// <param name="filename">filename without extension</param>
        /// <param name="extension">extension without period, e.g., "txt", "json" etc</param>
        public static FileInfo ToFileInfo(this string folderPath, string filename, string extension)
        {
            return folderPath.ToFileInfo(filename + "." + extension);
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance after joining <paramref name="filenameWithExt"/>
        /// to the <paramref name="folderPath"/>.
        /// </summary>
        /// <param name="folderPath">Folder path to the file</param>
        /// <param name="filenameWithExt">file name with extensions, e.g., "abc.txt", "mydata.json" etc</param>
        public static FileInfo ToFileInfo(this string folderPath, string filenameWithExt)
        {
            return Path.Combine(folderPath, filenameWithExt).ToFileInfo();
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance from given <paramref name="fullFilePath"/>.
        /// </summary>
        /// <param name="fullFilePath">Complete path of the file</param>
        public static FileInfo ToFileInfo(this string fullFilePath)
        {
            return new FileInfo(fullFilePath);
        }
    }
}