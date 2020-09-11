using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class AwaitableListAdapterTest
    {
        [Test]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void Ctor_Throws_Error_When_List_Size_Less_Than_2(int size)
        {
            var ex = Assert.Throws<TargetInvocationException>(() =>
                Assert.Null(Substitute.For<AwaitableListAdapter<object, object>>(size, Timeout.Infinite))).InnerException;
            Assert.True(ex is DdnDfException);
            Assert.True(((DdnDfException)ex).ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        public void Ctor_Throws_Error_When_Timeout_Is_Negative_And_Not_Equal_To_Infinite()
        {
            var ex = Assert.Throws<TargetInvocationException>(() => Assert.Null(Substitute.For<AwaitableListAdapter<object, object>>(2, -10))).InnerException;
            Assert.True(ex is DdnDfException);
            Assert.True(((DdnDfException)ex).ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void Ctor_Accepts_All_Positive_Values_For_Timeout_Including_Zero(int timeout)
        {
            Assert.NotNull(Substitute.For<AwaitableListAdapter<object, object>>(2, timeout));
        }

        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(2, Timeout.Infinite);
            instance.TryGet(feed, CancellationToken.None, out _);
            feed.Received(1).TryGet(Arg.Any<int>(), CancellationToken.None, out _);
        }

        [Test]
        [TestCase(2)]
        [TestCase(10)]
        public void TryGet_Calls_Adapt_When_Feed_Outputs_True(int listSize)
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).Returns(x => true);
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(listSize, Timeout.Infinite);
            instance.TryGet(feed, CancellationToken.None, out _);
            instance.Received(listSize).Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public void TryGet_Does_Not_Call_Adapt_When_Feed_Outputs_False()
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).Returns(x => false);
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(2, Timeout.Infinite);
            instance.TryGet(feed, CancellationToken.None, out _);
            instance.Received(0).Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>());
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(20, 3)]
        [TestCase(20, 23)]
        public void TryGet_With_Infinite_Timeout_Returns_NonEmpty_List_When_Feed_Has_Elements(int listSize,
            int feedSize)
        {
            var localFeedSize = feedSize;
            var obj = new object();
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).ReturnsForAnyArgs(x =>
            {
                if (localFeedSize <= 0) return false;
                x[2] = obj;
                Interlocked.Decrement(ref localFeedSize);
                return true;
            });
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(listSize, Timeout.Infinite);
            instance.Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(x => x[0]);
            Assert.True(instance.TryGet(feed, CancellationToken.None, out var newList));
            Assert.NotNull(newList);
            Assert.True(newList.Count.Equals(Math.Min(listSize, feedSize)));
            Assert.True(newList.All(x => ReferenceEquals(x, obj)));
        }

        [Test]
        [TestCase(20)]
        [TestCase(2)]
        public void TryGet_With_Infinite_Timeout_Returns_Default_Of_List_When_Feed_Is_Empty(int listSize)
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).ReturnsForAnyArgs(x => false);
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(listSize, Timeout.Infinite);
            instance.Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(x => x[0]);
            Assert.False(instance.TryGet(feed, CancellationToken.None, out var newList));
            Assert.Null(newList);
        }

        [Test]
        [TestCase(2, 1,0)]
        [TestCase(2, 2, 0)]
        [TestCase(2, 3, 0)]
        [TestCase(20, 3, 0)]
        [TestCase(20, 23, 0)]
        [TestCase(2, 1, 10)]
        [TestCase(2, 2, 10)]
        [TestCase(2, 3, 10)]
        [TestCase(20, 3, 10)]
        [TestCase(20, 23, 10)]
        public void TryGet_With_Finite_Timeout_Returns_NonEmpty_List_When_Feed_Has_Elements(int listSize,
            int feedSize, int timeout)
        {
            var localFeedSize = feedSize;
            var obj = new object();
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).ReturnsForAnyArgs(x =>
            {
                if (localFeedSize <= 0) return false;
                x[2] = obj;
                Interlocked.Decrement(ref localFeedSize);
                return true;
            });
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(listSize, timeout);
            instance.Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(x => x[0]);
            Assert.True(instance.TryGet(feed, CancellationToken.None, out var newList));
            Assert.NotNull(newList);
            Assert.True(newList.Count.Equals(Math.Min(listSize, feedSize)));
            Assert.True(newList.All(x => ReferenceEquals(x, obj)));
        }

        [Test]
        [TestCase(20, 0)]
        [TestCase(2, 0)]
        [TestCase(20, 10)]
        [TestCase(2, 10)]
        public void TryGet_With_Finite_Timeout_Returns_Empty_List_When_Feed_Is_Empty(int listSize, int timeout)
        {
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).ReturnsForAnyArgs(x => false);
            var instance = Substitute.For<AwaitableListAdapter<object, object>>(listSize, 0);
            instance.Adapt(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(x => x[0]);
            Assert.False(instance.TryGet(feed, CancellationToken.None, out var newList));
            Assert.True(newList == null || newList.Count.Equals(0));
        }
    }

    [TestFixture]
    public class IdentityAwaitableListAdapterTest
    {
        [Test]
        public void Adapt_Returns_The_Object_As_It_Is()
        {
            var obj = new object();
            var feed = Substitute.For<IConsumerBuffer<object>>();
            feed.TryGet(Arg.Any<int>(), CancellationToken.None, out _).ReturnsForAnyArgs(x =>
            {
                x[2] = obj;
                return true;
            }); var instance = new IdentityAwaitableListAdapter<object>(2, Timeout.Infinite);
            instance.TryGet(feed, CancellationToken.None, out var chunk);
            Assert.True(chunk.Count.Equals(2));
            Assert.True(chunk.All(x => ReferenceEquals(x, obj)));
        }
    }
}