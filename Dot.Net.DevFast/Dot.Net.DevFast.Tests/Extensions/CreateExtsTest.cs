﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class CreateExtsTest
    {
        private long _counter;
        private const string Filename = nameof(CreateExtsTest);

        [Test]
        public void CreateBytes_Creates_ByteArray_Of_The_Segment()
        {
            var segmentArr = new byte[0];
            var segment = new ArraySegment<byte>(segmentArr, 0, 0);
            var createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 0);

            segmentArr = new byte[] { 0, 255 };
            segment = new ArraySegment<byte>(segmentArr, 0, 0);
            createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 0);

            segment = new ArraySegment<byte>(segmentArr, 0, 1);
            createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 1);
            Assert.True(createdArr[0].Equals(segmentArr[0]));

            segment = new ArraySegment<byte>(segmentArr, 1, 1);
            createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 1);
            Assert.True(createdArr[0].Equals(segmentArr[1]));

            segment = new ArraySegment<byte>(segmentArr, 0, 2);
            createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 2);
            Assert.True(createdArr[0].Equals(segmentArr[0]));
            Assert.True(createdArr[1].Equals(segmentArr[1]));

            segment = new ArraySegment<byte>(segmentArr, 2, 0);
            createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 0);
        }

        [Test]
        public void CreateFileInfo_Extensions_Work_As_Expected()
        {
            var folder = FileSys.TestFolderNonExisting();
            const string ext = "json";

            var fileInfo1 = folder.CreateFileInfo(Filename, ext);
            var fileInfo2 = folder.CreateFileInfo(Filename + "." + ext);
            var fileInfo3 = Path.Combine(folder.FullName, Filename + "." + ext).ToFileInfo();

            Assert.False(fileInfo1.Exists);
            Assert.False(fileInfo2.Exists);
            Assert.False(fileInfo3.Exists);

            Assert.True(fileInfo1.FullName.Equals(fileInfo2.FullName));
            Assert.True(fileInfo1.FullName.Equals(fileInfo3.FullName));

            Assert.True(fileInfo1.Extension.TrimSafeOrNull('.').Equals(ext));
            Assert.True(fileInfo2.Extension.TrimSafeOrNull('.').Equals(ext));
            Assert.True(fileInfo3.Extension.TrimSafeOrNull('.').Equals(ext));

            Assert.NotNull(fileInfo1.Directory);
            Assert.NotNull(fileInfo2.Directory);
            Assert.NotNull(fileInfo3.Directory);

            Assert.True(fileInfo1.Directory.FullName.Equals(folder.FullName));
            Assert.True(fileInfo2.Directory.FullName.Equals(folder.FullName));
            Assert.True(fileInfo3.Directory.FullName.Equals(folder.FullName));
        }

        [Test]
        public void FileInfo_Based_CreateFileStream_Works_As_expected()
        {
            var fileInfo = Directory.GetCurrentDirectory()
                .ToDirectoryInfo()
                .CreateFileInfo(Filename + Interlocked.Increment(ref _counter), "txt");
            using (var fileStream = fileInfo.CreateStream(FileMode.Create,
                options: FileOptions.DeleteOnClose))
            {
                Assert.True(fileStream.CanRead);
                Assert.True(fileStream.CanWrite);
                Assert.False(fileStream.IsAsync);
                Assert.True(fileStream.Name.Equals(fileInfo.FullName));
            }
        }

        [Test]
        public void FileInfo_Based_CreateJsonReader_N_CreateStreamReader_Works_As_expected()
        {
            var fileInfo = Directory.GetCurrentDirectory()
                .ToDirectoryInfo()
                .CreateFileInfo(Filename + Interlocked.Increment(ref _counter), "txt");
            using (var streamReader = fileInfo.CreateStreamReader(options: FileOptions.DeleteOnClose))
            {
                Assert.True(streamReader.CurrentEncoding.WebName.Equals(Encoding.UTF8.WebName));
                Assert.True(streamReader.BaseStream is FileStream);
            }
            using (var jsonReader = fileInfo.CreateJsonReader(options: FileOptions.DeleteOnClose))
            {
                Assert.True(ReferenceEquals(jsonReader.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonReader.DateParseHandling.Equals(DateParseHandling.DateTime));
                Assert.True(jsonReader.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonReader.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonReader.FloatParseHandling.Equals(FloatParseHandling.Double));
                Assert.True(jsonReader.MaxDepth.Equals(null));
                Assert.True(jsonReader.CloseInput);
                Assert.False(jsonReader.Read());
            }
        }

        [Test]
        public void FileInfo_Based_CreateJsonWriter_NCreateStreamWriter_Works_As_expected()
        {
            var fileInfo = Directory.GetCurrentDirectory()
                .ToDirectoryInfo()
                .CreateFileInfo(Filename + Interlocked.Increment(ref _counter), "txt");
            using (var streamWriter = fileInfo.CreateStreamWriter(options: FileOptions.DeleteOnClose))
            {
                Assert.True(streamWriter.Encoding.WebName.Equals(Encoding.UTF8.WebName));
                Assert.True(streamWriter.AutoFlush);
                Assert.True(streamWriter.BaseStream is FileStream);
            }
            using (var jsonWriter = fileInfo.CreateJsonWriter(options: FileOptions.DeleteOnClose))
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonWriter.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
                Assert.True(jsonWriter.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonWriter.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
                Assert.True(jsonWriter.Formatting.Equals(Formatting.None));
                Assert.True(jsonWriter.StringEscapeHandling.Equals(StringEscapeHandling.Default));
                Assert.True(jsonWriter.CloseOutput);
                Assert.False(jsonWriter.Path.Equals(null));
            }
        }

        [Test]
        public void StringBuilder_Based_StringWriter_N_JsonTextWriter_Works_As_Expected()
        {
            var sb = new StringBuilder();
            using (var writer = sb.CreateWriter())
            {
                Assert.True(writer.Encoding.WebName.Equals(Encoding.Unicode.WebName));
                Assert.True(ReferenceEquals(writer.GetStringBuilder(), sb));
                writer.Write("testing");
            }
            Assert.True(sb.ToString().Equals("testing"));
            sb.Clear();
            using (var jsonWriter = sb.CreateJsonWriter())
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonWriter.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
                Assert.True(jsonWriter.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonWriter.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
                Assert.True(jsonWriter.Formatting.Equals(Formatting.None));
                Assert.True(jsonWriter.StringEscapeHandling.Equals(StringEscapeHandling.Default));
                Assert.True(jsonWriter.CloseOutput);
                Assert.False(jsonWriter.Path.Equals(null));
                jsonWriter.WriteStartArray();
                jsonWriter.WriteEndArray();
            }
            Assert.True(sb.ToString().Equals("[]"));
        }

        [Test]
        public void String_N_StringBuilder_Based_StringReader_N_JsonTextReader_Works_As_Expected()
        {
            var sb = new StringBuilder("testing");
            using (var reader = sb.CreateReader())
            {
                Assert.True(reader.ReadToEnd().Equals("testing"));
            }
            using (var reader = sb.ToString().CreateReader())
            {
                Assert.True(reader.ReadToEnd().Equals("testing"));
            }
            sb.Clear();
            sb.Append("[]");
            using (var jsonReader = sb.CreateJsonReader())
            {
                Assert.True(ReferenceEquals(jsonReader.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonReader.DateParseHandling.Equals(DateParseHandling.DateTime));
                Assert.True(jsonReader.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonReader.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonReader.FloatParseHandling.Equals(FloatParseHandling.Double));
                Assert.True(jsonReader.MaxDepth.Equals(null));
                Assert.True(jsonReader.CloseInput);
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.TokenType.Equals(JsonToken.StartArray));
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.TokenType.Equals(JsonToken.EndArray));
                Assert.False(jsonReader.Read());
            }
            using (var jsonReader = sb.ToString().CreateJsonReader())
            {
                Assert.True(ReferenceEquals(jsonReader.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonReader.DateParseHandling.Equals(DateParseHandling.DateTime));
                Assert.True(jsonReader.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonReader.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonReader.FloatParseHandling.Equals(FloatParseHandling.Double));
                Assert.True(jsonReader.MaxDepth.Equals(null));
                Assert.True(jsonReader.CloseInput);
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.TokenType.Equals(JsonToken.StartArray));
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.TokenType.Equals(JsonToken.EndArray));
                Assert.False(jsonReader.Read());
            }
        }

        [Test]
        public void Stream_Based_CreateReader_CreateWriter_Works_As_Expected()
        {
            using (var writer = Stream.Null.CreateWriter(Encoding.UTF32))
            {
                Assert.True(writer.Encoding.WebName.Equals(Encoding.UTF32.WebName));
                Assert.True(ReferenceEquals(writer.BaseStream, Stream.Null));
                Assert.True(writer.AutoFlush);
            }
            using (var reader = Stream.Null.CreateReader(Encoding.UTF32))
            {
                Assert.True(ReferenceEquals(reader.BaseStream, Stream.Null));
                Assert.True(reader.CurrentEncoding.WebName.Equals(Encoding.UTF32.WebName));
            }
        }

        [Test]
        public void Stream_Based_CreateJsonReader_CreateJsonWriter_Works_As_Expected()
        {
            using (var jsonReader = Stream.Null.CreateJsonReader(Encoding.UTF32))
            {
                Assert.True(ReferenceEquals(jsonReader.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonReader.DateParseHandling.Equals(DateParseHandling.DateTime));
                Assert.True(jsonReader.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonReader.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonReader.FloatParseHandling.Equals(FloatParseHandling.Double));
                Assert.True(jsonReader.MaxDepth.Equals(null));
                Assert.True(jsonReader.CloseInput);
                Assert.False(jsonReader.Read());
            }
            using (var jsonWriter = Stream.Null.CreateJsonWriter(Encoding.UTF32))
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonWriter.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
                Assert.True(jsonWriter.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonWriter.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
                Assert.True(jsonWriter.Formatting.Equals(Formatting.None));
                Assert.True(jsonWriter.StringEscapeHandling.Equals(StringEscapeHandling.Default));
                Assert.True(jsonWriter.CloseOutput);
                Assert.False(jsonWriter.Path.Equals(null));
            }
        }

        [Test]
        public void CreateJsonWriter_Holds_Serializer_Properties()
        {
            var serializer = CustomJson.Serializer();
            using (var jsonWriter = serializer.CreateJsonWriter(TextWriter.Null))
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, serializer.Culture));
                Assert.True(jsonWriter.DateFormatHandling.Equals(serializer.DateFormatHandling));
                Assert.True(jsonWriter.DateFormatString.Equals(serializer.DateFormatString));
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(serializer.DateTimeZoneHandling));
                Assert.True(jsonWriter.FloatFormatHandling.Equals(serializer.FloatFormatHandling));
                Assert.True(jsonWriter.Formatting.Equals(serializer.Formatting));
                Assert.True(jsonWriter.StringEscapeHandling.Equals(serializer.StringEscapeHandling));
                Assert.True(jsonWriter.CloseOutput);
            }
        }

        [Test]
        public void CreateJsonReader_Holds_Serializer_Properties()
        {
            var serializer = CustomJson.Serializer();
            using (var jsonReader = serializer.CreateJsonReader(TextReader.Null))
            {
                Assert.True(ReferenceEquals(jsonReader.Culture, serializer.Culture));
                Assert.True(jsonReader.DateParseHandling.Equals(serializer.DateParseHandling));
                Assert.True(jsonReader.DateFormatString.Equals(serializer.DateFormatString));
                Assert.True(jsonReader.DateTimeZoneHandling.Equals(serializer.DateTimeZoneHandling));
                Assert.True(jsonReader.FloatParseHandling.Equals(serializer.FloatParseHandling));
                Assert.True(jsonReader.MaxDepth.Equals(serializer.MaxDepth));
                Assert.True(jsonReader.CloseInput);
            }
        }
    }
}