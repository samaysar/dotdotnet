using System;
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
            IHeap<int> instance = new AbstractBinaryTestHeap(0, (x, y) => x < y);
            Assert.IsFalse(instance.TryAdd(1));
            var ex = Assert.Throws<DdnDfException>(() => instance.Add(1));
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.DemandUnfulfilled));
            Assert.IsTrue(ex.Message.Equals("(DemandUnfulfilled) Unable to add element in the heap."));
        }

        [Test]
        public void Add_N_Try_Add_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new AbstractBinaryTestHeap(1, (x, y) => x < y);
            Assert.True(instance.IsEmpty);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.IsFalse(instance.TryAdd(1));
            var ex = Assert.Throws<DdnDfException>(() => instance.Add(1));
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.DemandUnfulfilled));
            Assert.IsTrue(ex.Message.Equals("(DemandUnfulfilled) Unable to add element in the heap."));
            instance = new AbstractBinaryTestHeap(3, (x, y) => x < y);
            instance.Add(3);
            Assert.False(instance.IsFull);
            instance.Add(2);
            Assert.False(instance.IsFull);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.True(instance.IsFull);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void Peek_N_TryPeek_Behaves_For_Empty_Heap(int capacity)
        {
            IHeap<int> instance = Substitute.For<AbstractBinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Peek());
            Assert.False(instance.TryPeek(out _));
        }

        [Test]
        public void Peek_N_TryPeek_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new AbstractBinaryTestHeap(1, (x, y) => x < y);
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
            IHeap<int> instance = Substitute.For<AbstractBinaryHeap<int>>(capacity);
            Assert.Throws<IndexOutOfRangeException>(() => instance.Pop());
            Assert.False(instance.TryPop(out _));
        }

        [Test]
        public void Pop_N_TryPop_Behaves_For_Non_Empty_Heap()
        {
            IHeap<int> instance = new AbstractBinaryTestHeap(5, (x, y) => x < y);
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
    }
}