using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Etc
{
    [TestFixture]
    public class ConcurrentBufferTest
    {
        [Test]
        public void Properties_Are_Consistent()
        {
            Assert.True(ConcurrentBuffer.MinSize.Equals(1));
            Assert.True(ConcurrentBuffer.StandardSize.Equals(StdLookUps.DefaultStringBuilderSize));
            Assert.True(ConcurrentBuffer.Unbounded.Equals(0));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void CreateBuffer_Throws_Error_When_BufferSize_Is_Invalid(int size)
        {
            var ex = Assert.Throws<DdnDfException>(() => ConcurrentBuffer.CreateBuffer<object>(size));
            Assert.True(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
        }

        [Test]
        public void CreateBuffer_Returns_Unbounded_BlockingCollection_For_ZeroSize()
        {
            using (var buffer = ConcurrentBuffer.CreateBuffer<object>(0))
            {
                Assert.True(buffer.BoundedCapacity == -1);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public void CreateBuffer_Returns_Bounded_BlockingCollection_As_Per_Given_Size(int size)
        {
            using (var buffer = ConcurrentBuffer.CreateBuffer<object>(size))
            {
                Assert.True(buffer.BoundedCapacity == size);
            }
        }
    }
}