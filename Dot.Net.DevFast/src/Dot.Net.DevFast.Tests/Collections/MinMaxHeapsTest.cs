using System;
using System.Reflection;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class MinMaxHeapsTest
    {
        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void MinHeap_Ctor_Throws_Error_For_Invalid_Arguments(int capacity)
        {
            var ctorEx = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new MinHeap<int>(capacity);
            });
            Assert.IsNotNull(ctorEx);
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void MinHeap_Ctor_Do_Not_Uses_Resizing_By_Default()
        {
            var instance = new MinHeap<int>(0);
            Assert.False(instance.CanResize);
        }

        [Test]
        [TestCase(10)]
        [TestCase(10000)]
        public void MinHeap_Maintains_Ascending_Sorting_Order(int count)
        {
            var instance = new MinHeap<byte>(count);
            Assert.False(instance.CanResize);
            var input = new byte[count];
            new Random().NextBytes(input);
            foreach (var val in input)
            {
                Assert.True(instance.TryAdd(val));
            }
            Assert.True(instance.IsFull);
            Array.Sort(input);
            foreach (var next in input)
            {
                Assert.IsTrue(instance.TryPop(out var val) &&
                              val.Equals(next));
            }
            Assert.True(instance.IsEmpty);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void MaxHeap_Ctor_Throws_Error_For_Invalid_Arguments(int capacity)
        {
            var ctorEx = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new MaxHeap<int>(capacity);
            });
            Assert.IsNotNull(ctorEx);
            Assert.IsTrue(ctorEx.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void MaxHeap_Ctor_Do_Not_Uses_Resizing_By_Default()
        {
            var instance = new MaxHeap<int>(0);
            Assert.False(instance.CanResize);
        }

        [Test]
        [TestCase(10)]
        [TestCase(10000)]
        public void MaxHeap_Maintains_Descending_Sorting_Order(int count)
        {
            var instance = new MaxHeap<byte>(count);
            Assert.False(instance.CanResize);
            var input = new byte[count];
            new Random().NextBytes(input);
            foreach (var val in input)
            {
                Assert.True(instance.TryAdd(val));
            }
            Assert.True(instance.IsFull);
            Array.Sort(input);
            Array.Reverse(input);
            foreach (var next in input)
            {
                Assert.IsTrue(instance.TryPop(out var val) &&
                              val.Equals(next));
            }
            Assert.True(instance.IsEmpty);
        }
    }
}