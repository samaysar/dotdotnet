using System.IO;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.IO;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.IO
{
    [TestFixture]
    public class PullFuncStreamTest
    {
        [Test]
        public void Ctor_Throws_Error_When_Stream_Is_Not_Readable()
        {
            var mem = Substitute.For<Stream>();
            mem.CanRead.Returns(false);
            Assert.True(Assert.Throws<DdnDfException>(() =>
            {
                var _ = new PullFuncStream(mem, true);
            }).ErrorCode == DdnDfErrorCode.Unspecified);
        }

        [Test]
        public void Properties_Are_Simple_Accessors()
        {
            using (var mem = new MemoryStream(new byte[0]))
            {
                var _ = new PullFuncStream(mem, true);
                Assert.True(_.Dispose);
                Assert.True(ReferenceEquals(_.Readable, mem));
            }
        }
    }
}