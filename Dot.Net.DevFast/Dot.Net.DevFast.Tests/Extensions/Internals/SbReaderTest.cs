using System;
using System.Text;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals
{
    [TestFixture]
    public class SbReaderTest
    {
        [Test]
        public void Ctor_Throws_Error_For_Null_StringBuilder()
        {
            var err = Assert.Throws<DdnDfException>(() => Assert.Null(new SbReader(null)));
            Assert.True(err.ErrorCode == DdnDfErrorCode.NullObject);
            Assert.True(err.Message.Contains($"{nameof(StringBuilder)}"));
        }

        [Test]
        public void Dispose_Simply_Changes_Internal_State_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder("Testing");
            var sbreader = new SbReader(sb);
            using (sbreader)
            {
            }
            var err = Assert.Throws<DdnDfException>(() => Assert.Null(sbreader.Peek()));
            Assert.True(err.ErrorCode == DdnDfErrorCode.NullObject);
            Assert.True(err.Message.Contains("closed/disposed"));
            Assert.True(sb.ToString().Equals("Testing"));
        }

        [Test]
        public void Peek_And_Read_Works_As_Expected_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder("T");
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.Peek().Equals('T'));
                Assert.True(sbreader.Read().Equals('T'));
                Assert.True(sbreader.Peek().Equals(-1));
                Assert.True(sbreader.Read().Equals(-1));
            }
            Assert.True(sb.ToString().Equals("T"));
        }


        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public void Buffered_Read_Works_As_Expected_Without_Altering_StringBuilder(int buffSize)
        {
            var sb = new StringBuilder("Ttt");
            var chars = new char[buffSize];
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.Read(chars, 0, buffSize).Equals(Math.Min(3, buffSize)));
                Assert.True(chars[0].Equals('T'));
            }
            Assert.True(sb.ToString().Equals("Ttt"));
        }

        [Test]
        public void ReadToEnd_Works_As_Expected_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder("T");
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadToEnd().Equals("T"));
                Assert.True(sbreader.ReadToEnd().Equals(""));
            }
            Assert.True(sb.ToString().Equals("T"));
        }

        [Test]
        public void ReadLine_Works_As_Expected_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");
            sb.Append("Test1");
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadLine().Equals("Test"));
                Assert.True(sbreader.ReadLine().Equals("Test1"));
                Assert.Null(sbreader.ReadLine());
            }
            Assert.True(sb.ToString().StartsWith("Test"));
            Assert.True(sb.ToString().EndsWith("Test1"));
        }

        [Test]
        public void ReadLineAsync_Works_As_Expected_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");
            sb.Append("Test1");
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadLineAsync().Result.Equals("Test"));
                Assert.True(sbreader.ReadLineAsync().Result.Equals("Test1"));
                Assert.Null(sbreader.ReadLineAsync().Result);
            }
            Assert.True(sb.ToString().StartsWith("Test"));
            Assert.True(sb.ToString().EndsWith("Test1"));
        }

        [Test]
        public void ReadToEndAsync_Works_As_Expected_Without_Altering_StringBuilder()
        {
            var sb = new StringBuilder("T");
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadToEndAsync().Result.Equals("T"));
                Assert.True(sbreader.ReadToEndAsync().Result.Equals(""));
            }
            Assert.True(sb.ToString().Equals("T"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public void Buffered_ReadAsync_Works_As_Expected_Without_Altering_StringBuilder(int buffSize)
        {
            var sb = new StringBuilder("Ttt");
            var chars = new char[buffSize];
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadAsync(chars, 0, buffSize).Result.Equals(Math.Min(3, buffSize)));
                Assert.True(chars[0].Equals('T'));
            }
            Assert.True(sb.ToString().Equals("Ttt"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public void ReadBlockAsync_Works_As_Expected_Without_Altering_StringBuilder(int buffSize)
        {
            var sb = new StringBuilder("Ttt");
            var chars = new char[buffSize];
            using (var sbreader = new SbReader(sb))
            {
                Assert.True(sbreader.ReadBlockAsync(chars, 0, buffSize).Result.Equals(Math.Min(3, buffSize)));
                Assert.True(chars[0].Equals('T'));
            }
            Assert.True(sb.ToString().Equals("Ttt"));
        }
    }
}