using System;
using System.Globalization;
using System.IO;
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
        /// Create new file stream for <paramref name="targetFileInfo"/>.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="mode">mode of the stream</param>
        /// <param name="access">Access permission</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        public static FileStream CreateStream(this FileInfo targetFileInfo, FileMode mode,
            FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read,
            int bufferSize = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous)
        {
            return new FileStream(targetFileInfo.FullName, mode, access, share, bufferSize, options);
        }

        /// <summary>
        /// Create new JSON reader for <paramref name="targetFileInfo"/> with custom properties.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        public static JsonTextReader CreateJsonReader(this FileInfo targetFileInfo, Encoding enc = null,
            FileShare share = FileShare.Read, int bufferSize = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, bool detectEncodingFromBom = true)
        {
            return targetFileInfo.CreateStreamReader(enc, share, bufferSize, options, detectEncodingFromBom)
                .CreateJsonReader();
        }

        /// <summary>
        /// Create new stream reader for <paramref name="targetFileInfo"/>.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        public static StreamReader CreateStreamReader(this FileInfo targetFileInfo, Encoding enc = null,
            FileShare share = FileShare.Read, int bufferSize = StdLookUps.DefaultFileBufferSize, 
            FileOptions options = FileOptions.Asynchronous, bool detectEncodingFromBom = true)
        {
            return targetFileInfo.CreateStream(FileMode.OpenOrCreate, FileAccess.Read, share, bufferSize, options)
                .CreateReader(enc, bufferSize, true, detectEncodingFromBom);
        }

        /// <summary>
        /// Create new JSON writer for <paramref name="targetFileInfo"/> with custom properties.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="appendToFile">If true, existing file is appended or new file is created. If false, existing file is
        /// truncated or new file is created.</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        public static JsonTextWriter CreateJsonWriter(this FileInfo targetFileInfo, Encoding enc = null,
            bool appendToFile = false, FileShare share = FileShare.Read, 
            int bufferSize = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous)
        {
            return targetFileInfo.CreateStreamWriter(enc, appendToFile, share, bufferSize, options).CreateJsonWriter();
        }

        /// <summary>
        /// Create new stream writer for <paramref name="targetFileInfo"/>.
        /// </summary>
        /// <param name="targetFileInfo">Target file info</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="appendToFile">If true, existing file is appended or new file is created. If false, existing file is
        /// truncated or new file is created.</param>
        /// <param name="share">File share type</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="options">File options</param>
        public static StreamWriter CreateStreamWriter(this FileInfo targetFileInfo, Encoding enc = null,
            bool appendToFile = false, FileShare share = FileShare.Read, 
            int bufferSize = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous)
        {
            return targetFileInfo.CreateStream(appendToFile ? FileMode.Append : FileMode.Create,
                FileAccess.ReadWrite, share, bufferSize, options).CreateWriter(enc, bufferSize);
        }

        #region StringBuilder based Text/Json Writer

        /// <summary>
        /// Create a JSON writer for <paramref name="stringBuilder"/> with custom properties.
        /// </summary>
        /// <param name="stringBuilder">Target string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static JsonTextWriter CreateJsonWriter(this StringBuilder stringBuilder,
            IFormatProvider formatProvider = null)
        {
            return stringBuilder.CreateWriter(formatProvider).CreateJsonWriter();
        }

        /// <summary>
        /// Create a string writer for <paramref name="stringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">Target string builder</param>
        /// <param name="formatProvider">Format provider. If null, then <seealso cref="CultureInfo.CurrentCulture"/> is used</param>
        public static StringWriter CreateWriter(this StringBuilder stringBuilder,
            IFormatProvider formatProvider = null)
        {
            return new StringWriter(stringBuilder, formatProvider ?? CultureInfo.CurrentCulture);
        }

        #endregion String/StringBuilder based Text/Json Writer

        #region String/StringBuilder based Text/Json Reader

        /// <summary>
        /// Creates a JSON reader for <paramref name="stringBuilder"/> with custom properties.
        /// </summary>
        /// <param name="stringBuilder">Target string builder</param>
        public static JsonTextReader CreateJsonReader(this StringBuilder stringBuilder)
        {
            return stringBuilder.CreateReader().CreateJsonReader();
        }

        /// <summary>
        /// Creates a string reader for <paramref name="stringBuilder"/>
        /// </summary>
        /// <param name="stringBuilder">Target string builder</param>
        public static StringReader CreateReader(this StringBuilder stringBuilder)
        {
            return stringBuilder.ToString().CreateReader();
        }

        /// <summary>
        /// Creates a JSON reader for <paramref name="textVal"/> with custom properties.
        /// </summary>
        /// <param name="textVal">Target string</param>
        public static JsonTextReader CreateJsonReader(this string textVal)
        {
            return textVal.CreateReader().CreateJsonReader();
        }

        /// <summary>
        /// Creates a string reader for <paramref name="textVal"/>
        /// </summary>
        /// <param name="textVal">Target string</param>
        public static StringReader CreateReader(this string textVal)
        {
            return new StringReader(textVal);
        }

        #endregion String/StringBuilder based Text/Json Reader

        #region JSON Reader/Writer

        /// <summary>
        /// Creates a JSON reader for <paramref name="textReader"/> with custom properties.
        /// </summary>
        /// <param name="textReader">Target text reader</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static JsonTextReader CreateJsonReader(this TextReader textReader, bool disposeReader = true)
        {
            return new JsonTextReader(textReader)
            {
                Culture = CultureInfo.CurrentCulture,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateParseHandling = DateParseHandling.DateTime,
                FloatParseHandling = FloatParseHandling.Double,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                CloseInput = true
            };
        }

        /// <summary>
        /// Creates a JSON writer for <paramref name="textWriter"/> with custom properties.
        /// </summary>
        /// <param name="textWriter">Target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static JsonTextWriter CreateJsonWriter(this TextWriter textWriter, bool disposeWriter = true)
        {
            return new JsonTextWriter(textWriter)
            {
                Culture = CultureInfo.CurrentCulture,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                Formatting = Formatting.None,
                StringEscapeHandling = StringEscapeHandling.Default,
                CloseOutput = disposeWriter
            };
        }

        #endregion JSON Reader/Writer

        #region Stream based Stream/Json Reader/Writer

        /// <summary>
        /// Create a JSON writer for <paramref name="targetStream"/> with custom properties.
        /// </summary>
        /// <param name="targetStream">target stream to write to</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="targetStream"/> is disposed after the serialization</param>
        public static JsonTextWriter CreateJsonWriter(this Stream targetStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return targetStream.CreateWriter(enc, bufferSize, disposeStream).CreateJsonWriter();
        }

        /// <summary>
        /// Create a stream writer for <paramref name="targetStream"/>.
        /// </summary>
        /// <param name="targetStream">target stream to write to</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="targetStream"/> is disposed after the serialization</param>
        public static StreamWriter CreateWriter(this Stream targetStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true)
        {
            return new StreamWriter(targetStream, enc ?? Encoding.UTF8, bufferSize, !disposeStream)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// Create a JSON reader for <paramref name="sourceStream"/> with custom properties.
        /// </summary>
        /// <param name="sourceStream">source stream to read from</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="sourceStream"/> is disposed after the deserialization</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        public static JsonTextReader CreateJsonReader(this Stream sourceStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true, bool detectEncodingFromBom = true)
        {
            return sourceStream.CreateReader(enc, bufferSize, disposeStream, detectEncodingFromBom).CreateJsonReader();
        }

        /// <summary>
        /// Create a stream reader for <paramref name="sourceStream"/>.
        /// </summary>
        /// <param name="sourceStream">source stream to read from</param>
        /// <param name="enc">Text encoding to use. If null, then <seealso cref="Encoding.UTF8"/> is used.</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeStream">If true, <paramref name="sourceStream"/> is disposed after the deserialization</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        public static StreamReader CreateReader(this Stream sourceStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true, bool detectEncodingFromBom = true)
        {
            return new StreamReader(sourceStream, enc ?? Encoding.UTF8, detectEncodingFromBom,
                bufferSize, !disposeStream);
        }

        #endregion Stream based Stream/Json Reader/Writer

        /// <summary>
        /// Creates a <seealso cref="JsonWriter"/> for given <paramref name="serializer"/> and <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="serializer">Serializer to use to populate <seealso cref="JsonWriter"/> properties</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static JsonWriter CreateJsonWriter(this JsonSerializer serializer, TextWriter textWriter,
            bool disposeWriter = true)
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

        /// <summary>
        /// Creates a <seealso cref="JsonReader"/> for given <paramref name="serializer"/> and <paramref name="textReader"/>.
        /// </summary>
        /// <param name="serializer">Serializer to use to populate <seealso cref="JsonReader"/> properties</param>
        /// <param name="textReader">target text reader</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static JsonReader CreateJsonReader(this JsonSerializer serializer, TextReader textReader,
            bool disposeReader = true)
        {
            return new JsonTextReader(textReader)
            {
                Culture = serializer.Culture,
                DateFormatString = serializer.DateFormatString,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DateParseHandling = serializer.DateParseHandling,
                FloatParseHandling = serializer.FloatParseHandling,
                MaxDepth = serializer.MaxDepth,
                CloseInput = disposeReader
            };
        }
    }
}