﻿using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
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

        #region Fileinfo/stream related

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

        #endregion Fileinfo/stream related

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
                CloseInput = disposeReader
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
        /// <param name="autoFlush">Auto flush</param>
        public static StreamWriter CreateWriter(this Stream targetStream, Encoding enc = null,
            int bufferSize = StdLookUps.DefaultBufferSize, bool disposeStream = true, bool autoFlush = false)
        {
            return new StreamWriter(targetStream, enc ?? Encoding.UTF8, bufferSize, !disposeStream)
            {
                AutoFlush = autoFlush
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

        #region Compression Stream Related

        /// <summary>
        /// Creates compression stream (GZip or Deflate) that would hold <paramref name="innerStream"/> 
        /// and <paramref name="level"/> with an option to dispose the <paramref name="innerStream"/> after the operation.
        /// </summary>
        /// <param name="innerStream">stream on which compressed data will be written</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="level">Compression level</param>
        /// <param name="disposeInner">If true, <paramref name="innerStream"/> is disposed after the operation.</param>
        public static Stream CreateCompressionStream(this Stream innerStream, bool gzip = true,
            CompressionLevel level = CompressionLevel.Optimal, bool disposeInner = true)
        {
            return gzip
                ? new GZipStream(innerStream, level, !disposeInner)
                : (Stream)new DeflateStream(innerStream, level, !disposeInner);
        }

        /// <summary>
        /// Creates compression stream (GZip or Deflate) that would hold <paramref name="innerStream"/> 
        /// with an option to dispose the <paramref name="innerStream"/> after the operation.
        /// </summary>
        /// <param name="innerStream">stream on which compressed data will be written</param>
        /// <param name="gzip">if true, <seealso cref="GZipStream"/> is created else <seealso cref="DeflateStream"/>
        /// is created.</param>
        /// <param name="disposeSource">If true, <paramref name="innerStream"/> is disposed after the operation.</param>
        public static Stream CreateDecompressionStream(this Stream innerStream, bool gzip = true,
            bool disposeSource = true)
        {
            return gzip
                ? new GZipStream(innerStream, CompressionMode.Decompress, !disposeSource)
                : (Stream)new DeflateStream(innerStream, CompressionMode.Decompress, !disposeSource);
        }

        #endregion Compression Stream Related

        #region CryptoStream related

        /// <summary>
        /// Create <seealso cref="CryptoStream"/> to perform operations on <paramref name="target"/>
        /// using supplied <paramref name="mode"/>.
        /// </summary>
        /// <param name="target">Target stream on which transformed data will be written
        /// or from which input data for transformation will be read (depends on <paramref name="mode"/>)</param>
        /// <param name="transform">Transform to perform</param>
        /// <param name="mode">Mode on the <paramref name="target"/></param>
        /// <param name="disposeTarget">If true, <paramref name="target"/> is disposed after the operation.</param>
        public static CryptoStream CreateCryptoStream(this Stream target, ICryptoTransform transform,
            CryptoStreamMode mode = CryptoStreamMode.Write, bool disposeTarget = false)
        {
#if !NET472
            return new CryptoStream(target.CreateWrappedStream(disposeTarget), transform, mode);
#else
            return new CryptoStream(target, transform, mode, !disposeTarget);
#endif
        }

        /// <summary>
        /// Creates an artificial wrapper around <paramref name="target"/> to control disposal
        /// (when it is not possible otherwise, e.g. <seealso cref="CryptoStream"/>).
        /// </summary>
        /// <param name="target">Target stream to wrap</param>
        /// <param name="disposeTarget">True to dispose, else false. Disposal takes effect when wrapper is disposed.</param>
        public static Stream CreateWrappedStream(this Stream target, bool disposeTarget = false)
        {
#if !NET472
            return new WrappedStream(target, disposeTarget);
#else
            return target;
#endif
        }

        #endregion CryptoStream related
    }
}