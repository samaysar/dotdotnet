using System;
using System.IO;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extensions related to creation of one type of objects to another type.
    /// </summary>
    public static class CreateExts
    {
        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance (file is physically NOT created)
        /// after combining <paramref name="filename"/>.<paramref name="extension"/>
        /// to <seealso cref="FileSystemInfo.FullName"/> of the <paramref name="folderInfo"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="folderInfo">FolderInfo to which fileInfo is associated</param>
        /// <param name="filename">filename without extension</param>
        /// <param name="extension">extension without period, e.g., "txt", "json" etc</param>
        public static FileInfo CreateFileInfo(this DirectoryInfo folderInfo, string filename, string extension)
        {
            return folderInfo.FullName.ToFileInfo(filename, extension);
        }

        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance (file is physically NOT created)
        /// after combining <paramref name="filenameWithExt"/>
        /// to <seealso cref="FileSystemInfo.FullName"/> of the <paramref name="folderInfo"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="folderInfo">FolderInfo to which fileInfo is associated</param>
        /// <param name="filenameWithExt">file name with extensions, e.g., "abc.txt", "mydata.json" etc</param>
        public static FileInfo CreateFileInfo(this DirectoryInfo folderInfo, string filenameWithExt)
        {
            return folderInfo.FullName.ToFileInfo(filenameWithExt);
        }

        /// <summary>
        /// Creates the byte array of the segment.
        /// </summary>
        /// <param name="input">Input segment</param>
        public static byte[] CreateBytes(this ArraySegment<byte> input)
        {
            var retValue = new byte[input.Count];
            Buffer.BlockCopy(input.Array, input.Offset, retValue, 0, input.Count);
            return retValue;
        }
    }
}