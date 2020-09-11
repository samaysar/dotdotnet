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
        public void HeapNoResizing_Strategy_Always_Retruns_False(int size)
        {
            Assert.IsFalse(new HeapNoResizing().TryComputeNewSize(size, out var newSize));
            Assert.AreEqual(newSize, default(int));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void StepHeapResizing_Ctor_Throws_Error_For_Invalid_Arguments(int step)
        {
            Assert.IsTrue(Assert.Throws<DdnDfException>(() =>
            {
                var _ = new StepHeapResizing(step);
            }).ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        [TestCase(1, 0, 1)]
        [TestCase(1, 10, 11)]
        [TestCase(10, 0, 10)]
        [TestCase(10, 10, 20)]
        [TestCase(1, int.MaxValue-1, int.MaxValue)]
        public void StepHeapResizing_Calculates_Good_NewSize(int step, int current, int expected)
        {
            Assert.IsTrue(new StepHeapResizing(step).TryComputeNewSize(current, out var newSize));
            Assert.AreEqual(newSize, expected);
        }

        [Test]
        [TestCase(1, int.MaxValue)]
        [TestCase(2, int.MaxValue-1)]
        public void StepHeapResizing_Returns_False_When_Overflow_Occurs(int step, int current)
        {
            Assert.IsFalse(new StepHeapResizing(step).TryComputeNewSize(current, out _));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void PercentHeapResizing_Ctor_Throws_Error_For_Invalid_Arguments(int step)
        {
            Assert.IsTrue(Assert.Throws<DdnDfException>(() =>
            {
                var _ = new PercentHeapResizing(step);
            }).ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
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
            Assert.IsTrue(new PercentHeapResizing(percent).TryComputeNewSize(current, out var newSize));
            Assert.AreEqual(newSize, expected);
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
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
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
            var ensureCapacityMethod = instance.GetType()
                .GetMethod("EnsureCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(ensureCapacityMethod);
            ensureCapacityMethod.Invoke(instance, new object[] { }).Returns(true);
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
            var ensureCapacityMethod = instance.GetType()
                .GetMethod("EnsureCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(ensureCapacityMethod);
            ensureCapacityMethod.Invoke(instance, new object[] { }).Returns(true);
            instance.Add(1);
            instance.Add(2);
            Assert.AreEqual(instance.Pop(), 1);
            Assert.True(instance.TryPop(out var val) && val.Equals(2));
        }

        [Test]
        public void BinaryHeap_Add_N_Try_Add_Behaves_For_Empty_Heap()
        {

        }

        [Test]
        public void BinaryHeap_Compact_Behaves()
        {
            var instance = Substitute.For<BinaryHeap<int>>(2);
            Assert.AreEqual(instance.Capacity, 2);
            instance.Compact();
            Assert.AreEqual(instance.Capacity, 0);
        }
    }
}