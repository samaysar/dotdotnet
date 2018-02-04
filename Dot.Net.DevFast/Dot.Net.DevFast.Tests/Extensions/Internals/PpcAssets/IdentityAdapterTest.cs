using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class IdentityAdapterTest
    {
        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IProducerFeed<object>>();
            var instance = new IdentityAdapter<object>();
            instance.TryGet(feed, out var outobj);
            feed.Received(1).TryGet(Arg.Any<int>(), out outobj);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TryGet_Returns_The_Instance_Unaltered_With_Feed_Return_Value(bool feedValue)
        {
            var obj = new object();
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).Returns(x =>
            {
                x[1] = obj;
                return feedValue;
            });
            var instance = new IdentityAdapter<object>();
            Assert.True(instance.TryGet(feed, out var newobj).Equals(feedValue) && ReferenceEquals(newobj, obj));
        }
    }
}