using System;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;

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

        /// <summary>
        /// Create a stream writer for <paramref name="streamToWrite"/>.
        /// </summary>
        /// <param name="streamToWrite">target stream</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="streamToWrite"/> is disposed after the serialization</param>
        public static StreamWriter CreateWriter(this Stream streamToWrite, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return new StreamWriter(streamToWrite, enc ?? Encoding.UTF8,
                bufferSize, !disposeStream)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// Create a string writer for <paramref name="stringBuilder"/>
        /// </summary>
        /// <param name="stringBuilder">Target string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static StringWriter CreateWriter(this StringBuilder stringBuilder,
            IFormatProvider formatProvider = null)
        {
            return new StringWriter(stringBuilder, formatProvider ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Create new file stream for <paramref name="targetFileInfo"/>.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="mode">mode of the stream</param>
        /// <param name="access">Access permission</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        public static FileStream CreateFileStream(this FileInfo targetFileInfo, FileMode mode,
            FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read,
            int bufferSize = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous)
        {
            return new FileStream(targetFileInfo.FullName, mode, access, share, bufferSize, options);
        }

        /// <summary>
        /// Creates a <seealso cref="JsonWriter"/> for given <paramref name="serializer"/> and <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="serializer">Serializer to use to populate <seealso cref="JsonWriter"/> properties</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static JsonWriter CreateJsonWriter(this JsonSerializer serializer, TextWriter textWriter,
            bool disposeWriter)
        {
            return new JsonTextWriter(textWriter)
            {
                Culture = serializer.Culture,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                FloatFormatHandling = serializer.FloatFormatHandling,
                Formatting = serializer.Formatting,
                StringEscapeHandling = serializer.StringEscapeHandling,
                CloseOutput = disposeWriter
            };
        }
    }
}