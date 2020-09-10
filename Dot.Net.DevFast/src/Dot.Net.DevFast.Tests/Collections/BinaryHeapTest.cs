using System;
using System.Reflection;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Etc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class BinaryHeapTest
    {
        [Test]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void HeapNoResizing_Strategy_Always_Throws_Error(int size)
        {
            Assert.Throws<InvalidOperationException>(() => new HeapNoResizing().NewSize(size))
                .Message.Equals("Heap has a fixed capacity that cannot be increased.");
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void StepHeapResizing_Ctor_Throws_Error_For_Invalid_Arguments(int step)
        {
            Assert.Throws<DdnDfException>(() =>
            {
                var _ = new StepHeapResizing(step);
            }).ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        [TestCase(1, 0, 1)]
        [TestCase(1, 10, 11)]
        [TestCase(10, 0, 10)]
        [TestCase(10, 10, 20)]
        public void StepHeapResizing_Calculates_Good_NewSize(int step, int current, int expected)
        {
            Assert.AreEqual(new StepHeapResizing(step).NewSize(current), expected);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void PercentHeapResizing_Ctor_Throws_Error_For_Invalid_Arguments(int step)
        {
            Assert.Throws<DdnDfException>(() =>
            {
                var _ = new PercentHeapResizing(step);
            }).ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        [TestCase(1, 0, 1)]
        [TestCase(1, 10, 11)]
        [TestCase(1, 1000, 1010)]
        [TestCase(10, 0, 1)]
        [TestCase(10, 10, 11)]
        [TestCase(10, 1000, 1100)]
        public void PercentHeapResizing_Calculates_Good_NewSize(int percent, int current, int expected)
        {
            Assert.AreEqual(new PercentHeapResizing(percent).NewSize(current), expected);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void BinaryHeap_Ctor_Throws_Error_For_Invalid_Arguments(int capacity)
        {
            var ctorEx = Assert.Throws<TargetInvocationException>(() =>
            {
                var _ = Substitute.For<BinaryHeap<int>>(capacity);
            }).InnerException as DdnDfException;
            Assert.IsNotNull(ctorEx);
            ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void BinaryHeap_Properties_Are_Well_Defined(int capacity)
        {
            var instance = Substitute.For<BinaryHeap<int>>(capacity);
            Assert.True(instance.IsEmpty);
            Assert.AreEqual(instance.Count, 0);
            Assert.AreEqual(instance.Capacity, capacity);
            if (capacity == 0) Assert.True(instance.IsFull);
            else Assert.False(instance.IsFull);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void BinaryHeap_Peek_N_TryPeek_Behaves_For_Empty_Heap(int capacity)
        {
            var instance = Substitute.For<BinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Peek());
            Assert.False(instance.TryPeek(out _));
        }

        [Test]
        public void BinaryHeap_Peek_N_TryPeek_Behaves_For_Non_Empty_Heap()
        {
            var instance = Substitute.For<BinaryHeap<int>>(1);
            instance.Add(1);
            Assert.AreEqual(instance.Peek(), 1);
            Assert.True(instance.TryPeek(out var val) && val.Equals(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void BinaryHeap_Pop_N_TryPop_Behaves_For_Empty_Heap(int capacity)
        {
            var instance = Substitute.For<BinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Pop());
            Assert.False(instance.TryPop(out _));
        }

        [Test]
        public void BinaryHeap_Pop_N_TryPop_Behaves_For_Non_Empty_Heap()
        {
            var instance = Substitute.For<BinaryHeap<int>>(2);
            instance.Add(1);
            instance.Add(2);
            Assert.AreEqual(instance.Pop(), 1);
            Assert.True(instance.TryPop(out var val) && val.Equals(2));
        }
    }
}