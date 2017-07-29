using System;
using System.IO;
using System.Text;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class CreateExtsTest
    {
        [Test]
        public void CreateFileInfo_Extensions_Work_As_Expected()
        {
            const string filename = nameof(CreateExtsTest);
            var folder = FileSys.TestFolderNonExisting();
            const string ext = "json";

            var fileInfo1 = folder.CreateFileInfo(filename, ext);
            var fileInfo2 = folder.CreateFileInfo(filename + "." + ext);
            var fileInfo3 = Path.Combine(folder.FullName, filename + "." + ext).ToFileInfo();

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
        public void CreateBytes_Creates_ByteArray_Of_The_Segment()
        {
            var segmentArr = new byte[0];
            var segment = new ArraySegment<byte>(segmentArr, 0, 0);
            var createdArr = segment.CreateBytes();
            Assert.NotNull(createdArr);
            Assert.True(createdArr.Length == 0);

            segmentArr = new byte[] {0, 255};
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
        public void Stream_Based_CreateWriter_Works_As_Expected()
        {
            using (var memStream = new MemoryStream())
            {
                using (var writer = memStream.CreateWriter(Encoding.UTF32))
                {
                    Assert.True(writer.Encoding.WebName.Equals(Encoding.UTF32.WebName));
                    Assert.True(ReferenceEquals(writer.BaseStream, memStream));
                    Assert.True(writer.AutoFlush);
                }
            }
        }

        [Test]
        public void CreateReader_Works_As_Expected()
        {
            using (var memStream = new MemoryStream())
            {
                using (var reader = memStream.CreateReader(Encoding.UTF32))
                {
                    Assert.True(ReferenceEquals(reader.BaseStream, memStream));
                    Assert.True(reader.CurrentEncoding.WebName.Equals(Encoding.UTF32.WebName));
                }
            }
        }

        [Test]
        public void StringBuilder_Based_CreateWriter_Works_As_Expected()
        {
            var val = "Ticks:" + DateTime.Now.Ticks;
            var sb = new StringBuilder();
            using (var writer = sb.CreateWriter())
            {
                writer.Write(val);
            }
            Assert.True(sb.ToString().Equals(val));
        }

        [Test]
        public void FileInfo_Based_CreateStream_Works_As_expected()
        {
            var fileInfo = Directory.GetCurrentDirectory().ToDirectoryInfo().CreateFileInfo("abc", "txt");
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
            using (var jsonWriter = serializer.CreateJsonReader(TextReader.Null))
            {
                Assert.True(ReferenceEquals(jsonWriter.Culture, serializer.Culture));
                Assert.True(jsonWriter.DateParseHandling.Equals(serializer.DateParseHandling));
                Assert.True(jsonWriter.DateFormatString.Equals(serializer.DateFormatString));
                Assert.True(jsonWriter.DateTimeZoneHandling.Equals(serializer.DateTimeZoneHandling));
                Assert.True(jsonWriter.FloatParseHandling.Equals(serializer.FloatParseHandling));
                Assert.True(jsonWriter.MaxDepth.Equals(serializer.MaxDepth));
                Assert.True(jsonWriter.CloseInput);
            }
        }
    }
}