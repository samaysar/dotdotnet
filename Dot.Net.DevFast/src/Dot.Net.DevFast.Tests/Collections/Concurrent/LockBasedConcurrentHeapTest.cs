using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Collections.Concurrent;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections.Concurrent
{
    [TestFixture]
    public class LockBasedConcurrentHeapTest
    {
        [Test]
        [TestCase(null)]
        public void Ctor_Throws_Error_When_Heap_Instance_Is_Null(AbstractSizableBinaryHeap<int> heap)
        {
            var ex = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new LockBasedConcurrentHeap<int>(heap);
            });
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.NullObject));
            Assert.True(new LockBasedConcurrentHeap<int>(new ConcurrentMinHeap<int>(0)).IsEmpty);
        }

        [Test]
        public void IEnumerable_Is_Well_Implemented()
        {
            var instance = new LockBasedConcurrentHeap<int>(new MinHeap<int>(3))
            {
                1,
                2,
                3
            };
            Assert.IsFalse(instance.TryAdd(4));
            var results = new HashSet<int> { 1, 2, 3 };
            ((IEnumerable)instance).ForEach(x => results.Remove((int)x));
            Assert.IsTrue(results.Count == 0);
        }

        [Test]
        public void Properties_Are_Accessed_Inside_Lock()
        {
            var syncRoot = new object();
            var heap = Substitute.For<IResizableHeap<int>>();
            heap.IsEmpty.Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return true;
            });
            heap.IsFull.Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return true;
            });
            heap.Count.Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return 1;
            });
            heap.CanResize.Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return true;
            });
            heap.Capacity.Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return 1;
            });
            var instance = new LockBasedConcurrentHeap<int>(heap, syncRoot);
            Assert.True(instance.IsEmpty);
            Assert.True(instance.CanResize);
            Assert.True(instance.IsFull);
            Assert.IsTrue(instance.Count == 1);
            Assert.IsTrue(instance.Capacity == 1);
            var _ = heap.Received(1).IsFull;
            _ = heap.Received(1).IsEmpty;
            _ = heap.Received(1).CanResize;
            var __ = heap.Received(1).Count;
            __ = heap.Received(1).Capacity;
        }

        [Test]
        public void Methods_Are_Accessed_Inside_Lock()
        {
            var syncRoot = new object();
            var heap = Substitute.For<IResizableHeap<int>>();
            heap.Peek().Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return 1;
            });
            heap.TryPeek(out _).Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                x[0] = 1;
                return true;
            });
            heap.Pop().Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return 1;
            });
            heap.TryPop(out _).Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                x[0] = 2;
                return true;
            });
            heap.When(x => x.Add(1)).Do(x => Assert.IsTrue(Monitor.IsEntered(syncRoot)));
            heap.TryAdd(1).Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return true;
            });
            var addThis = new[] {1, 2};
            heap.AddAll(addThis).Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return default;
            });
            var popAllVals = new[] {1, 2, 3, 4};
            heap.PopAll().Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                return popAllVals;
            });
            heap.When(x => x.Compact()).Do(x => Assert.IsTrue(Monitor.IsEntered(syncRoot)));
            heap.When(x => x.FreezeCapacity(true)).Do(x => Assert.IsTrue(Monitor.IsEntered(syncRoot)));

            var instance = new LockBasedConcurrentHeap<int>(heap, syncRoot);
            Assert.AreEqual(instance.Peek(), 1);
            Assert.IsTrue(instance.TryPeek(out var val) && val.Equals(1));
            Assert.AreEqual(instance.Pop(), 1);
            Assert.IsTrue(instance.TryPop(out var val2) && val2.Equals(2));
            instance.Add(1);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.AreEqual(instance.AddAll(addThis), 0);
            Assert.AreEqual(instance.PopAllConsistent(), popAllVals);
            instance.Compact();
            instance.FreezeCapacity(true);
            heap.Received(1).Pop();
            heap.Received(1).TryPop(out _);
            heap.Received(1).Peek();
            heap.Received(1).TryPeek(out _);
            heap.Received(1).Add(1);
            heap.Received(1).TryAdd(1);
            heap.Received(1).Compact();
            heap.Received(1).AddAll(addThis);
            heap.Received(1).PopAll();
            heap.Received(1).FreezeCapacity(true);
            // PopAll separated as it reuses TryPop!--------------->
            heap.ClearReceivedCalls();
            var popRetVal = 0;
            heap.TryPop(out Arg.Any<int>()).Returns(x =>
            {
                Assert.IsTrue(Monitor.IsEntered(syncRoot));
                x[0] = 10;
                return Interlocked.Increment(ref popRetVal) <= 2;
            });
            var allPopped = instance.PopAll().ToArray();
            Assert.AreEqual(allPopped, new[] {10, 10});
            heap.Received(3).TryPop(out Arg.Any<int>());
            Assert.IsTrue(new LockBasedConcurrentHeap<int>(new MinHeap<int>(0), syncRoot).PopAllConsistent().Count
                .Equals(0));
        }
    }
}