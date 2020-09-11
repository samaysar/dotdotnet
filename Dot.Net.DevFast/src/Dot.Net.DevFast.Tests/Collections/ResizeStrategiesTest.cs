using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class ResizeStrategiesTest
    {
        [Test]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void HeapNoResizing_Strategy_Always_Returns_False(int size)
        {
            var instance = new HeapNoResizing();
            Assert.IsFalse(instance.TryComputeNewSize(size, out var newSize));
            Assert.AreEqual(newSize, default(int));
            Assert.False(instance.CanResize);
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
        [TestCase(1, int.MaxValue - 1, int.MaxValue)]
        public void StepHeapResizing_Calculates_Good_NewSize(int step, int current, int expected)
        {
            var instance = new StepHeapResizing(step);
            Assert.IsTrue(instance.TryComputeNewSize(current, out var newSize));
            Assert.AreEqual(newSize, expected);
            Assert.True(instance.CanResize);
        }

        [Test]
        [TestCase(1, int.MaxValue)]
        [TestCase(2, int.MaxValue - 1)]
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
            var instance = new PercentHeapResizing(percent);
            Assert.IsTrue(instance.TryComputeNewSize(current, out var newSize));
            Assert.AreEqual(newSize, expected);
            Assert.True(instance.CanResize);
        }

        [Test]
        [TestCase(1, int.MaxValue)]
        [TestCase(int.MaxValue, 1000)]
        public void PercentHeapResizing_Returns_False_When_Overflow_Occurs(int percent, int current)
        {
            Assert.IsFalse(new PercentHeapResizing(percent).TryComputeNewSize(current, out _));
        }
    }
}