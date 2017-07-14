using System;
using System.IO;
using Dot.Net.DevFast.Extensions.StreamExt;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamExt
{
    [TestFixture]
    public class WrappedStreamTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Dispose_Of_WrappedStream_Works_As_Expected(bool dispose)
        {
            var strm = Substitute.For<Stream>();
            using (new WrappedStream(strm, dispose))
            {
            }
            strm.Received(dispose ? 1 : 0).Dispose();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Boolean_Properties_Are_Properly_Wrapped(bool propVal)
        {
            var strm = Substitute.For<Stream>();
            strm.CanRead.Returns(propVal);
            strm.CanSeek.Returns(propVal);
            strm.CanWrite.Returns(propVal);
            strm.CanTimeout.Returns(propVal);

            using (var wrprstrm = new WrappedStream(strm, false))
            {
                Assert.True(wrprstrm.CanRead.Equals(strm.CanRead));
                Assert.True(wrprstrm.CanSeek.Equals(strm.CanSeek));
                Assert.True(wrprstrm.CanWrite.Equals(strm.CanWrite));
                Assert.True(wrprstrm.CanTimeout.Equals(strm.CanTimeout));
            }
        }

        [Test]
        [TestCase(10)]
        [TestCase(100)]
        public void Numeric_Properties_Are_Properly_Wrapped(int propVal)
        {
            var strm = Substitute.For<Stream>();
            strm.Length.Returns(propVal);
            strm.Position.Returns(propVal);
            strm.WriteTimeout.Returns(propVal);
            strm.ReadTimeout.Returns(propVal);

            using (var wrprstrm = new WrappedStream(strm, false))
            {
                Assert.True(wrprstrm.Length.Equals(strm.Length));
                Assert.True(wrprstrm.Position.Equals(strm.Position));
                Assert.True(wrprstrm.WriteTimeout.Equals(strm.WriteTimeout));
                Assert.True(wrprstrm.ReadTimeout.Equals(strm.ReadTimeout));

                wrprstrm.Position = propVal;
                strm.Received(1).Position = propVal;
                wrprstrm.WriteTimeout = propVal;
                strm.Received(1).WriteTimeout = propVal;
                wrprstrm.ReadTimeout = propVal;
                strm.Received(1).ReadTimeout = propVal;
            }
        }

        [Test]
        public void Paramless_Methods_Are_Properly_Wrapped()
        {
            var strm = Substitute.For<Stream>();
            using (var wrprstrm = new WrappedStream(strm, false))
            {
                var code = wrprstrm.ReadByte();
                code = strm.Received(1).ReadByte();

                wrprstrm.Flush();
                strm.Received(1).Flush();

                code = wrprstrm.GetHashCode();
                code = strm.Received(1).GetHashCode();

                var str = wrprstrm.ToString();
                str = strm.Received(1).ToString();
            }
        }
    }
}