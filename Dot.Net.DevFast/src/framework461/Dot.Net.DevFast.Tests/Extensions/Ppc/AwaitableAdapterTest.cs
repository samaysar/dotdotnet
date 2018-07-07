using System.Threading;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class AwaitableAdapterTest
    {
        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            var instance = Substitute.For<AwaitableAdapter<object, object>>();
            instance.TryGet(feed, CancellationToken.None, out var _);
            feed.Received(1).TryGet(Arg.Any<int>(), CancellationToken.None, out _);
        }

        [Test]
        public void TryGet_Calls_Adapt_When_Feed_Outputs_True()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out var _).Returns(x => true);
            var instance = Substitute.For<AwaitableAdapter<object, object>>();
            instance.TryGet(feed, CancellationToken.None, out var _);
            instance.Received(1).Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public void TryGet_Does_Not_Call_Adapt_When_Feed_Outputs_False()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out var _).Returns(x => false);
            var instance = Substitute.For<AwaitableAdapter<object, object>>();
            instance.TryGet(feed, CancellationToken.None, out var _);
            instance.Received(0).Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>());
        }
    }

    [TestFixture]
    public class IdentityAwaitableAdapterTest
    {
        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            var instance = IdentityAwaitableAdapter<object>.Default;
            instance.TryGet(feed, CancellationToken.None, out var _);
            feed.Received(1).TryGet(Arg.Any<int>(), CancellationToken.None, out _);
        }

        [Test]
        public void TryGet_Returns_The_Instance_Unaltered_With_True_TruthValue()
        {
            var obj = new object();
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out var _).Returns(x =>
            {
                x[2] = obj;
                return true;
            });
            var instance = IdentityAwaitableAdapter<object>.Default;
            Assert.True(instance.TryGet(feed, CancellationToken.None, out var newobj) && ReferenceEquals(newobj, obj));
        }

        [Test]
        public void TryGet_Returns_Default_Of_Type_With_False_TruthValue()
        {
            var obj = new object();
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out var _).Returns(x =>
            {
                x[2] = obj;
                return false;
            });
            var instance = IdentityAwaitableAdapter<object>.Default;
            Assert.True(!instance.TryGet(feed, CancellationToken.None, out var newobj) && ReferenceEquals(newobj, null));
        }
    }
}