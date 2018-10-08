using System.IO;
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
            
        }
    }
}