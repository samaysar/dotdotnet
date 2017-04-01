using System.Globalization;
using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Etc
{
    [TestFixture]
    public class FixedValueTest
    {
        [Test]
        public void Values_Are_Consistent()
        {
            Assert.True(FixedValues.English.Equals(new CultureInfo("en-US")));
        }
    }
}