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
            using (var strm = new MemoryStream())
            {
                using (new WrappedStream(strm, dispose))
                {
                }
                Assert.False(strm.CanSeek.Equals(dispose));
            }
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
    }
}