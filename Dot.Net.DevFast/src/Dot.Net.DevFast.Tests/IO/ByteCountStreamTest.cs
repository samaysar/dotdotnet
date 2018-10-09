using System;
using System.IO;
using System.Text;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.IO;
using Dot.Net.DevFast.Tests.TestHelpers;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.IO
{
    [TestFixture]
    public class ByteCountStreamTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void LeaveOpen_Logic_Works_As_Expected(bool leaveOpen)
        {
            var strm = Substitute.For<Stream>();
            using (var _ = new ByteCountStream(strm, leaveOpen))
            {
            }
            strm.Received(leaveOpen ? 0 : 1).Dispose();
        }

        [Test]
        public void Properties_With_Null_Stream_Works_As_Expected()
        {
            var instance = new ByteCountStream();
            using (instance)
            {
                Assert.True(instance.CanRead);
                Assert.True(instance.CanWrite);
                Assert.True(instance.CanSeek);
                Assert.False(instance.CanTimeout);
                Assert.True(instance.InnerStream == Stream.Null);

                Assert.True(instance.Position == 0);
                instance.Position = 10;
                Assert.True(instance.Position == 0);

                Assert.True(instance.Length == 0);
                instance.SetLength(10);
                Assert.True(instance.Length == 0);

                Assert.True(instance.Seek(10, SeekOrigin.Begin) == 0);

                Assert.True(instance.ByteCount == 0);
                instance.Flush();
            }

            //Available after dispose
            Assert.True(instance.ByteCount == 0);
            Assert.Throws<ObjectDisposedException>(() =>
            {
                var _ = instance.CanSeek;
            });
            Assert.Null(instance.InnerStream);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void Read_Call_Properly_Estimates_The_Count(int buffSize)
        {
            using (var mem = new MemoryStream(TestValues.BigString.ToBytes()))
            {
                using (var instance = new ByteCountStream(mem, true))
                {
                    var buffer = new byte[buffSize];
                    var cnt = instance.Read(buffer, 0, buffSize);
                    Assert.True(instance.ByteCount == cnt);
                }
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void Write_Call_Properly_Estimates_The_Count(int buffSize)
        {
            using (var mem = new MemoryStream())
            {
                using (var instance = new ByteCountStream(mem, true))
                {
                    var buffer = new byte[buffSize];
                    for (var i = 0; i < buffSize; i++)
                    {
                        buffer[i] = 5;
                    }
                    instance.Write(buffer, 0, buffSize);
                    Assert.True(instance.ByteCount == mem.Length);
                }
            }
        }

        [Test]
        [TestCase(null)]
        public void Write_Call_Properly_Checks_The_Constraints_When_Stream_Is_Null(byte[] nullBuff)
        {
            using (var instance = new ByteCountStream())
            {
                //null buff
                Assert.True(Assert.Throws<DdnDfException>(() => instance.Write(nullBuff, 0, 0))
                                .ErrorCode == DdnDfErrorCode.NullObject);

                // -ve offset
                Assert.True(Assert.Throws<DdnDfException>(() => instance.Write(new byte[0], -1, 0))
                                .ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);

                // -ve count
                Assert.True(Assert.Throws<DdnDfException>(() => instance.Write(new byte[0], 0, -1))
                                .ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);

                // buff length < offset + count
                Assert.True(Assert.Throws<DdnDfException>(() => instance.Write(new byte[0], 0, 1))
                                .ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);

                //this would pass
                instance.Write(new byte[0], 0, 0);
            }
        }
    }
}