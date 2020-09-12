using Dot.Net.DevFast.Collections.Concurrent;
using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections.Concurrent
{
    [TestFixture]
    public class MinMaxHeapsTest
    {
        [Test]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        public void ConcurrentMinHeap_Ctor_Throws_Error_For_Invalid_Capacity(int capacity)
        {
            var ex = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new ConcurrentMinHeap<int>(capacity);
            });
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        public void ConcurrentMaxHeap_Ctor_Throws_Error_For_Invalid_Capacity(int capacity)
        {
            var ex = Assert.Throws<DdnDfException>(() =>
            {
                var _ = new ConcurrentMaxHeap<int>(capacity);
            });
            Assert.NotNull(ex);
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void Ctors_Passes_For_Valid_Params()
        {
            IHeap<int> instance = new ConcurrentMinHeap<int>(1);
            Assert.True(instance.IsEmpty);
            Assert.False(instance.IsFull);
            instance = new ConcurrentMaxHeap<int>(1);
            Assert.True(instance.IsEmpty);
            Assert.False(instance.IsFull);
        }
    }
}