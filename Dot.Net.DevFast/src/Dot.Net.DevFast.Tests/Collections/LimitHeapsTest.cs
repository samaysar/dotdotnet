using System;
using System.Collections.Generic;
using System.Reflection;
using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Tests.TestHelpers;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class LimitHeapsTest
    {
        [Test]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void AbstractLimitHeap_Ctor_Throws_Error_For_Invalid_Argument(int size)
        {
            var ex = Assert.Throws<TargetInvocationException>(() =>
            {
                var _ = Substitute.For<AbstractLimitHeap<int>>(size);
            }).InnerException as DdnDfException;
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void AbstractLimitHeap_TryAdd_Works_Correctly()
        {
            var instance = new TestAbstractLimitHeap(1);
            Assert.IsTrue(instance.TryAdd(1));
            Assert.IsTrue(instance.TryAdd(0));
            Assert.IsTrue(instance.TryAdd(0));
            Assert.IsTrue(instance.TryAdd(-1));

            instance = new TestAbstractLimitHeap(1);
            Assert.IsFalse(instance.TryAdd(1, out _));
            Assert.IsTrue(instance.TryAdd(0, out var i) && i.Equals(1));
            Assert.IsFalse(instance.TryAdd(0, out _));
            Assert.IsTrue(instance.TryAdd(-1, out i) && i.Equals(0));
        }

        [Test]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void MinLimitHeap_Ctor_Throws_Error_For_Invalid_Argument(int size)
        {
            var ex = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new MinLimitHeap<int>(size);
            });
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        [TestCase(1, 10)]
        [TestCase(5, 10)]
        [TestCase(10, 10)]
        [TestCase(10, 5)]
        [TestCase(10, 1)]
        [TestCase(10, 100)]
        public void MinLimitHeap_Works_Correctly(int heapSize, int itemCount)
        {
            var items = new string[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                items[i] = Guid.NewGuid().ToString("N");
            }

            var heap = new MinLimitHeap<string>(heapSize);
            Assert.AreEqual(heap.AddAll(items), itemCount);
            Array.Sort(items);
            var minItems = new HashSet<string>();
            var itemCounter = 0;
            for (var i = 0; i < Math.Min(heapSize, itemCount); i++)
            {
                minItems.Add(items[itemCounter++]);
            }

            itemCounter = 0;
            foreach (var current in heap.PopAll())
            {
                Assert.IsTrue(minItems.Contains(current));
                itemCounter++;
            }

            Assert.AreEqual(itemCounter, Math.Min(heapSize, itemCount));
        }

        [Test]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void MaxLimitHeap_Ctor_Throws_Error_For_Invalid_Argument(int size)
        {
            var ex = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new MaxLimitHeap<int>(size);
            });
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        [TestCase(1, 10)]
        [TestCase(5, 10)]
        [TestCase(10, 10)]
        [TestCase(10, 5)]
        [TestCase(10, 1)]
        [TestCase(10, 100)]
        public void MaxLimitHeap_Works_Correctly(int heapSize, int itemCount)
        {
            var items = new string[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                items[i] = Guid.NewGuid().ToString("N");
            }

            var heap = new MaxLimitHeap<string>(heapSize);
            Assert.AreEqual(heap.AddAll(items), itemCount);
            Array.Sort(items);
            Array.Reverse(items);
            var minItems = new HashSet<string>();
            var itemCounter = 0;
            for (var i = 0; i < Math.Min(heapSize, itemCount); i++)
            {
                minItems.Add(items[itemCounter++]);
            }

            itemCounter = 0;
            foreach (var current in heap.PopAll())
            {
                Assert.IsTrue(minItems.Contains(current));
                itemCounter++;
            }

            Assert.AreEqual(itemCounter, Math.Min(heapSize, itemCount));
        }
    }
}