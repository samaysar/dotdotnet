using System.IO;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension related to file-system related operations.
    /// </summary>
    public static class FileSysExts
    {
        /// <summary>
        /// Returns a new <seealso cref="FileInfo"/> instance after combining filename and extension
        /// to <seealso cref="FileSystemInfo.FullName"/> of the <paramref name="folderInfo"/>.
        /// <para>Expect all <seealso cref="FileInfo"/> related errors.</para>
        /// </summary>
        /// <param name="folderInfo">FolderInfo to which fileInfo is associated</param>
        /// <param name="filename">filename without extension</param>
        /// <param name="extension">extension without period, e.g., "txt", "json" etc</param>
        public static FileInfo MakeFileInfo(this DirectoryInfo folderInfo, string filename, string extension)
        {
            return folderInfo.FullName.ToFileInfo(filename, extension);
        }
    }
}