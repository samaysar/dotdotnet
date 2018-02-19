using System.Threading;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class IdentityAdapterTest
    {
        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IProducerFeed<object>>();
            var instance = new IdentityAdapter<object>();
            instance.TryGet(feed, CancellationToken.None, out var _);
            feed.Received(1).TryGet(Arg.Any<int>(), CancellationToken.None, out _);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TryGet_Returns_The_Instance_Unaltered_With_Feed_Return_Value(bool feedValue)
        {
            var obj = new object();
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out var _).Returns(x =>
            {
                x[2] = obj;
                return feedValue;
            });
            var instance = new IdentityAdapter<object>();
            Assert.True(instance.TryGet(feed, CancellationToken.None, out var newobj).Equals(feedValue) && ReferenceEquals(newobj, obj));
        }
    }
}