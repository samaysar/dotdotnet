using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.Internals;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using Newtonsoft.Json;
using NSubstitute;
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
                Assert.False(streamWriter.AutoFlush);
                Assert.True(streamWriter.BaseStream is FileStream);
            }
            using (var jsonWriter = fileInfo.CreateJsonWriter(options: FileOptions.DeleteOnClose))
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, CultureInfo.CurrentCulture));
                Assert.True(jsonWriter.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
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
                Assert.False(writer.AutoFlush);
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
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
                Assert.True(jsonWriter.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
                Assert.True(jsonWriter.Formatting.Equals(Formatting.None));
                Assert.True(jsonWriter.StringEscapeHandling.Equals(StringEscapeHandling.Default));
                Assert.True(jsonWriter.CloseOutput);
                Assert.False(jsonWriter.Path.Equals(null));
            }
        }

        [Test]
        public void TextReader_Based_CreateJsonReader_Is_Consistent()
        {
            var jr = new StringBuilder().CreateReader().CreateJsonReader();
            Assert.True(jr.Culture.Equals(CultureInfo.CurrentCulture));
            Assert.True(jr.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
            Assert.True(jr.DateParseHandling.Equals(DateParseHandling.DateTime));
            Assert.True(jr.FloatParseHandling.Equals(FloatParseHandling.Double));
            Assert.True(jr.CloseInput);
        }

        [Test]
        public void TextWriter_Based_CreateJsonWriter_Is_Consistent()
        {
            var jw = new StringBuilder().CreateWriter().CreateJsonWriter();
            Assert.True(jw.Culture.Equals(CultureInfo.CurrentCulture));
            Assert.True(jw.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
            Assert.True(jw.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
            Assert.True(jw.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
            Assert.True(jw.Formatting.Equals(Formatting.None));
            Assert.True(jw.StringEscapeHandling.Equals(StringEscapeHandling.Default));
            Assert.True(jw.CloseOutput);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateWrappedStream_Works_As_Expected(bool dispose)
        {
            var strm = Substitute.For<Stream>();
            using (var wrapped = strm.CreateWrappedStream(dispose))
            {
                Assert.True(wrapped is WrappedStream);
            }
            strm.Received(dispose ? 1 : 0).Dispose();
        }

        [Test]
        [TestCase(true, CryptoStreamMode.Read)]
        [TestCase(true, CryptoStreamMode.Write)]
        [TestCase(false, CryptoStreamMode.Read)]
        [TestCase(false, CryptoStreamMode.Write)]
        public void CreateCryptoStream_Works_As_Expected(bool dispose, CryptoStreamMode mode)
        {
            var strm = Substitute.For<Stream>();
            strm.CanRead.Returns(true);
            strm.CanWrite.Returns(true);
            using (var wrapped = strm.CreateCryptoStream(new FromBase64Transform(), mode, dispose))
            {
                Assert.True(wrapped.CanWrite == (mode == CryptoStreamMode.Write));
                Assert.True(wrapped.CanRead == (mode == CryptoStreamMode.Read));
            }
            strm.Received(dispose ? 1 : 0).Dispose();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateCompressionStream_Works_As_Expected(bool isgzip)
        {
            using (var stream = Stream.Null.CreateCompressionStream(isgzip))
            {
                Assert.True(stream.CanWrite);
                Assert.False(stream.CanRead);
                Assert.False(stream.CanSeek);

                Assert.True(stream.GetType() == (isgzip ? typeof(GZipStream) : typeof(DeflateStream)));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateDecompressionStream_Works_As_Expected(bool isgzip)
        {
            using (var stream = Stream.Null.CreateDecompressionStream(isgzip))
            {
                Assert.True(stream.CanRead);
                Assert.False(stream.CanWrite);
                Assert.False(stream.CanSeek);

                Assert.True(stream.GetType() == (isgzip ? typeof(GZipStream) : typeof(DeflateStream)));
            }
        }

        [Test]
        public void CreateKeyAndIv_Works_As_Expected()
        {
            var keyIv = TestValues.FixedCryptoPass.CreateKeyAndIv(TestValues.FixedCryptoSalt
#if NETHASHCRYPTO
                , HashAlgorithmName.SHA1
#endif
            );
            //This make sure that default param wont change
            Assert.True(Convert.ToBase64String(keyIv.Item1).Equals(TestValues.FixedCryptoKey));
            Assert.True(Convert.ToBase64String(keyIv.Item2).Equals(TestValues.FixedCryptoIv));

            //proof of concept if future version changes values of default params
            //lets say by mistake 10000 is changed to 1K
            keyIv = TestValues.FixedCryptoPass.CreateKeyAndIv(TestValues.FixedCryptoSalt
#if NETHASHCRYPTO
                , HashAlgorithmName.SHA1
#endif
            , 32, 16, 1000);
            //This make sure that default param wont change
            Assert.False(Convert.ToBase64String(keyIv.Item1).Equals(TestValues.FixedCryptoKey));
            Assert.False(Convert.ToBase64String(keyIv.Item2).Equals(TestValues.FixedCryptoIv));
        }
    }
}