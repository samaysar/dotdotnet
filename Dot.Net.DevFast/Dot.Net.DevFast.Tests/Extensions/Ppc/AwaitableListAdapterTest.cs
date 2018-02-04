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
            var ex = Assert.Throws<DdnDfException>(() => Assert.Null(new AwaitableListAdapter<object>(size, Timeout.Infinite)));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
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
        public void TryGet_Returns_NonEmpty_List_When_Feed_Has_Elements(int listSize, int feedSize)
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
        public void TryGet_Returns_Empty_List_When_Feed_Is_Empty(int listSize)
        {
            var feed = Substitute.For<IProducerFeed<object>>();
            feed.TryGet(Arg.Any<int>(), out var outObj).ReturnsForAnyArgs(x => false);
            var instance = new AwaitableListAdapter<object>(listSize, Timeout.Infinite);
            Assert.False(instance.TryGet(feed, out var newList));
            Assert.True(newList == null || newList.Count.Equals(0));
        }
    }
}