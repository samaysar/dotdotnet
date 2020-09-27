using System;
using System.Linq;
using System.Reflection;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Tests.TestHelpers;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class AbstractBinaryHeapTest
    {
        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void Ctor_Throws_Error_For_Invalid_Arguments(int capacity)
        {
            var ctorEx =
                Assert.Throws<TargetInvocationException>(() => Substitute.For<AbstractBinaryHeap<int>>(capacity))
                    .InnerException as DdnDfException;
            Assert.IsNotNull(ctorEx);
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void Properties_Are_Well_Defined(int capacity)
        {
            IHeap<int> instance = Substitute.For<AbstractBinaryHeap<int>>(capacity);
            Assert.True(instance.IsEmpty);
            Assert.AreEqual(instance.Count, 0);
            Assert.AreEqual(instance.Capacity, capacity);
            if (capacity == 0) Assert.True(instance.IsFull);
            else Assert.False(instance.IsFull);
        }

        [Test]
        public void Add_N_Try_Add_Behaves_For_Empty_Heap()
        {
            IHeap<int> instance = new TestAbstractBinaryHeap(0, (x, y) => x < y);
            Assert.IsFalse(instance.TryAdd(1));
            var ex = Assert.Throws<DdnDfException>(() => instance.Add(1));
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.DemandUnfulfilled));
            Assert.IsTrue(ex.Message.Equals("(DemandUnfulfilled) Unable to add element in the heap."));
        }

        [Test]
        public void Add_N_Try_Add_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new TestAbstractBinaryHeap(1, (x, y) => x < y);
            Assert.True(instance.IsEmpty);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.IsFalse(instance.TryAdd(1));
            var ex = Assert.Throws<DdnDfException>(() => instance.Add(1));
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.DemandUnfulfilled));
            Assert.IsTrue(ex.Message.Equals("(DemandUnfulfilled) Unable to add element in the heap."));
            instance = new TestAbstractBinaryHeap(3, (x, y) => x < y);
            instance.Add(3);
            Assert.False(instance.IsFull);
            instance.Add(2);
            Assert.False(instance.IsFull);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.True(instance.IsFull);
        }

        [Test]
        public void AddAll_Properly_Adds_All_Elements_And_Returns_The_Count()
        {
            var items = new[] {2, 4, 0, 1, 2};
            Assert.IsTrue(new TestAbstractBinaryHeap(0, (x, y) => x < y).AddAll(items).Equals(0));
            Assert.IsTrue(new TestAbstractBinaryHeap(3, (x, y) => x < y).AddAll(items).Equals(3));
            Assert.IsTrue(new TestAbstractBinaryHeap(5, (x, y) => x < y).AddAll(items).Equals(5));
            Assert.IsTrue(new TestAbstractBinaryHeap(10, (x, y) => x < y).AddAll(items).Equals(5));
        }

        [Test]
        public void PopAll_Properly_Maintains_Order_And_Sequence()
        {
            var items = new[] { 2, 4, 0, 1, 2 };
            var expected = new[] {0, 1, 2, 2, 4};
            var instance = new TestAbstractBinaryHeap(10, (x, y) => x < y);
            instance.AddAll(items);
            var poppedItems = instance.PopAll().ToList();
            Assert.IsTrue(poppedItems.Count.Equals(5));
            Assert.AreEqual(poppedItems, expected);
            poppedItems = instance.PopAll().ToList();
            Assert.IsTrue(poppedItems.Count.Equals(0));
            instance.AddAll(items);
            poppedItems = instance.PopAll().ToList();
            Assert.IsTrue(poppedItems.Count.Equals(5));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void Peek_N_TryPeek_Behaves_For_Empty_Heap(int capacity)
        {
            IHeap<int> instance = Substitute.ForPartsOf<AbstractBinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Peek());
            Assert.False(instance.TryPeek(out _));
        }

        [Test]
        public void Peek_N_TryPeek_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new TestAbstractBinaryHeap(1, (x, y) => x < y);
            instance.Add(1);
            Assert.AreEqual(instance.Peek(), 1);
            Assert.True(instance.TryPeek(out var val) && val.Equals(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void Pop_N_TryPop_Behaves_For_Empty_Heap(int capacity)
        {
            IHeap<int> instance = Substitute.ForPartsOf<AbstractBinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Pop());
            Assert.False(instance.TryPop(out _));
        }

        [Test]
        public void Pop_N_TryPop_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new TestAbstractBinaryHeap(5, (x, y) => x < y);
            instance.Add(3);
            instance.Add(2);
            instance.Add(1);
            instance.Add(4);
            instance.Add(1);
            Assert.AreEqual(instance.Pop(), 1);
            Assert.AreEqual(instance.Pop(), 1);
            Assert.AreEqual(instance.Pop(), 2);
            Assert.AreEqual(instance.Pop(), 3);
            Assert.AreEqual(instance.Pop(), 4);
            Assert.True(instance.IsEmpty);
            Assert.IsFalse(instance.TryPop(out _));
        }

        [Test]
        public void Compact_Behaves()
        {
            var instance = Substitute.For<AbstractBinaryHeap<int>>(2);
            Assert.AreEqual(instance.Capacity, 2);
            instance.Compact();
            Assert.AreEqual(instance.Capacity, 0);
        }

        [Test]
        public void GetFirstUnsafe_Throws_Error_When_Capacity_Is_Zero()
        {
            var instance = Substitute.For<AbstractBinaryHeap<int>>(0);
            Assert.Throws<IndexOutOfRangeException>(() => instance.GetFirstUnsafe());
        }

        [Test]
        public void GetFirstUnsafe_Blindly_Returns_Whatever_At_0Th_Index_Irrespective_Of_Count()
        {
            var instance = new TestAbstractBinaryHeap(5, (x, y) => x < y);
            Assert.IsTrue(instance.GetFirstUnsafe().Equals(0));
            instance.Add(0);
            Assert.IsTrue(instance.GetFirstUnsafe().Equals(0));
            instance.Add(-1);
            Assert.IsTrue(instance.GetFirstUnsafe().Equals(-1));
        }

#if NETSPAN
        [Test]
        public void GetInternalState_Exposes_Internal_Buffer()
        {
            var instance = new TestAbstractBinaryHeap(5, (x, y) => x < y);
            Assert.True(instance.GetInternalState().IsEmpty);
            instance.AddAll(new[] {0, 1});
            Assert.True(instance.GetInternalState().Length.Equals(2));
            Assert.True(instance.GetInternalState()[0].Equals(0));
            Assert.True(instance.GetInternalState()[1].Equals(1));
        }
#endif
    }
}