using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Etc
{
    [TestFixture]
    public class StdLookUpsTest
    {
        [Test]
        public void Values_Are_Consistent()
        {
            Assert.True(StdLookUps.ExtSeparator.Equals('.'));
            Assert.True(StdLookUps.DefaultBufferSize.Equals(1024));
        }
    }
}