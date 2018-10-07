using System.IO;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.IO;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.IO
{
    [TestFixture]
    public class PushFuncStreamTest
    {
        [Test]
        public void Ctor_Throws_Error_When_Stream_Is_Not_Writable()
        {
            using (var mem = new MemoryStream(new byte[0], false))
            {
                Assert.True(Assert.Throws<DdnDfException>(() =>
                {
                    var _ = new PushFuncStream(mem, true, CancellationToken.None);
                }).ErrorCode == DdnDfErrorCode.Unspecified);
            }
        }

        [Test]
        public void Properties_Are_Simple_Accessors()
        {
            using (var mem = new MemoryStream(new byte[0]))
            {
                var _ = new PushFuncStream(mem, true, CancellationToken.None);
                Assert.True(_.Dispose);
                Assert.True(_.Token.Equals(CancellationToken.None));
                Assert.True(ReferenceEquals(_.Writable, mem));
            }
        }
    }
}