using System.Reflection;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class AbstractSizableBinaryHeapTest
    {
        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void AbstractSizableBinaryHeap_Ctor_Throws_Error_For_Invalid_Capacity(int capacity)
        {
            var ctorEx = Assert.Throws<TargetInvocationException>(() =>
            {
                var _ = Substitute.For<AbstractSizableBinaryHeap<int>>(capacity);
            }).InnerException as DdnDfException;
            Assert.IsNotNull(ctorEx);
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void AbstractSizableBinaryHeap_Ctor_Throws_Error_For_Missing_Strategy()
        {
            var ctorEx = Assert.Throws<TargetInvocationException>(() =>
            {
                var _ = Substitute.For<AbstractSizableBinaryHeap<int>>(0, null);
            }).InnerException as DdnDfException;
            Assert.IsNotNull(ctorEx);
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.NullObject));
        }

        [Test]
        public void AbstractSizableBinaryHeap_Ctor_Properly_Sets_Properties()
        {
            IResizeStrategy strategy = new HeapNoResizing();
            var instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0);
            Assert.IsTrue(instance.CanResize.Equals(strategy.CanResize));
            strategy = new HeapNoResizing();
            instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0, strategy);
            Assert.IsTrue(instance.CanResize.Equals(strategy.CanResize));
            strategy = new StepHeapResizing(1);
            instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0, strategy);
            Assert.IsTrue(instance.CanResize.Equals(strategy.CanResize));
            strategy = new PercentHeapResizing(1);
            instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0, strategy);
            Assert.IsTrue(instance.CanResize.Equals(strategy.CanResize));
        }

        [Test]
        public void AbstractSizableBinaryHeap_FreezeCapacity_Behaves()
        {
            foreach (var strategy in new IResizeStrategy[]
            {
                new HeapNoResizing(),
                new StepHeapResizing(1),
                new PercentHeapResizing(1)
            })
            {
                foreach (var compact in new[] {true, false})
                {
                    var instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0, strategy);
                    Assert.IsTrue(instance.CanResize.Equals(strategy.CanResize));
                    instance.FreezeCapacity(compact);
                    Assert.False(instance.CanResize);
                    instance.Received(compact ? 1 : 0).Compact();
                }
            }
        }

        [Test]
        public void AbstractSizableBinaryHeap_UseStrategies_Properly()
        {
            var strategy = Substitute.For<IResizeStrategy>();
            strategy.TryComputeNewSize(Arg.Any<int>(), out _)
                .Returns(x => {
                    x[1] = 0;
                    return false;
                });
            var instance = Substitute.For<AbstractSizableBinaryHeap<int>>(0,
                strategy);
            Assert.True(instance.IsFull);
            Assert.IsFalse(instance.TryAdd(1));
            strategy.Received(1).TryComputeNewSize(0, out _);

            strategy = new StepHeapResizing(1);
            instance = Substitute.For<AbstractSizableBinaryHeap<int>>(1, strategy);
            Assert.AreEqual(instance.Count, 0);
            Assert.False(instance.IsFull);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.True(instance.IsFull);
            Assert.AreEqual(instance.Count, 1);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.True(instance.IsFull);
            Assert.AreEqual(instance.Count, 2);
            instance.FreezeCapacity();
            Assert.IsFalse(instance.TryAdd(1));
            Assert.True(instance.IsFull);
            Assert.AreEqual(instance.Count, 2);
        }
    }
}