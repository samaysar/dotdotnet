using System;
using System.Linq;
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
            var ex = Assert.Throws<DdnDfException>(() =>
                Assert.Null(new AwaitableListAdapter<object>(size, Timeout.Infinite)));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        public void Ctor_Throws_Error_When_Timeout_Is_Negative_And_Not_Equal_To_Infinite()
        {
            var ex = Assert.Throws<DdnDfException>(() => Assert.Null(new AwaitableListAdapter<object>(2, -10)));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void Ctor_Accepts_All_Positive_Values_For_Timeout_Including_Zero(int timeout)
        {
            Assert.NotNull(new AwaitableListAdapter<object>(2, timeout));
        }

        [Test]
        public void TryGet_Hits_TryGet_Of_ProducerFeed()
        {
            var feed = Substitute.For<IProducerFeed<object>>();
            var instance = new AwaitableListAdapter<object>(2, Timeout.Infinite);
            instance.TryGet(feed, out var outList);
            feed.Received(1).TryGet(Arg.Any<int>(), out var outobj);
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
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).ReturnsForAnyArgs(x =>
            {
                if (localFeedSize <= 0) return false;
                x[1] = obj;
                Interlocked.Decrement(ref localFeedSize);
                return true;
            });
            var instance = new AwaitableListAdapter<object>(listSize, Timeout.Infinite);
            Assert.True(instance.TryGet(feed, out var newList));
            Assert.NotNull(newList);
            Assert.True(newList.Count.Equals(Math.Min(listSize, feedSize)));
            Assert.True(newList.All(x => ReferenceEquals(x, obj)));
        }

        [Test]
        [TestCase(20)]
        [TestCase(2)]
        public void TryGet_With_Infinite_Timeout_Returns_Empty_List_When_Feed_Is_Empty(int listSize)
        {
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).ReturnsForAnyArgs(x => false);
            var instance = new AwaitableListAdapter<object>(listSize, Timeout.Infinite);
            Assert.False(instance.TryGet(feed, out var newList));
            Assert.True(newList == null || newList.Count.Equals(0));
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
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).ReturnsForAnyArgs(x =>
            {
                if (localFeedSize <= 0) return false;
                x[1] = obj;
                Interlocked.Decrement(ref localFeedSize);
                return true;
            });
            var instance = new AwaitableListAdapter<object>(listSize, timeout);
            Assert.True(instance.TryGet(feed, out var newList));
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
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).ReturnsForAnyArgs(x => false);
            var instance = new AwaitableListAdapter<object>(listSize, 0);
            Assert.False(instance.TryGet(feed, out var newList));
            Assert.True(newList == null || newList.Count.Equals(0));
        }
    }
}