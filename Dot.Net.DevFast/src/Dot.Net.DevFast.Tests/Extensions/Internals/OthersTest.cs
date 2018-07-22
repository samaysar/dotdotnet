using System;
using System.IO;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals
{
    [TestFixture]
    public class OthersTest
    {
        [Test]
        public void DisposeIfRequired_Works_As_Expected()
        {
            var disposable = Substitute.For<IDisposable>();
            disposable.DisposeIfRequired(false);
            disposable.Received(0).Dispose();

            disposable.DisposeIfRequired(true);
            disposable.Received(1).Dispose();
        }

        [Test]
        public void ThrowIfNoBuffer_Throws_Error_If_Unable_ToExtract_Buffer()
        {
            using (var mem = new MemoryStream(Array.Empty<byte>(), 0, 0, false, false))
            {
                var ex = Assert.Throws<DdnDfException>(() => mem.ThrowIfNoBuffer());
                Assert.True(ex.ErrorCode == DdnDfErrorCode.UnableToGetMemoryStreamBuffer);
                Assert.True(ex.Message.Contains("Please check if buffer is exposable. Unable to get buffer."));
            }
        }

        [Test]
        public void ThrowIfNoBuffer_Returns_Buffer_Properly()
        {
            var buff = Array.Empty<byte>();
            using (var mem = new MemoryStream(buff, 0, 0, false, true))
            {
                var buffer = mem.ThrowIfNoBuffer();
                Assert.True(ReferenceEquals(buffer.Array, buff));
            }
        }
    }
}