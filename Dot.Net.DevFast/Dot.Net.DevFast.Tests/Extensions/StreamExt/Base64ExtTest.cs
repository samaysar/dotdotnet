using System;
using System.Text;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamExt
{
    [TestFixture]
    public class Base64ExtTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-16")]
        [TestCase("utf-32")]
        [TestCase("us-ascii")]
        public void String_Based_ToBase64_Works_Correctly(string enc)
        {
            var extB64 = TestValues.BigString.ToBase64(Base64FormattingOptions.InsertLineBreaks,
                Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8")));
            var localB64 = Convert.ToBase64String(
                    Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8")).GetBytes(TestValues.BigString),
                    Base64FormattingOptions.InsertLineBreaks);

            Assert.True(extB64.Equals(localB64));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(350)]
        public void ByteArray_Based_ToBase64_Works_Correctly(int arrSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var extB64 = bytes.ToBase64(Base64FormattingOptions.InsertLineBreaks);
            var localB64 = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);

            Assert.True(extB64.Equals(localB64));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(350, 123)]
        [TestCase(99, 91)]
        public void ByteArraySegment_Based_ToBase64_Works_Correctly(int arrSize, int segSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var segment = new ArraySegment<byte>(bytes, 0, segSize);
            var extB64 = segment.ToBase64(Base64FormattingOptions.InsertLineBreaks);
            var localB64 = Convert.ToBase64String(bytes, 0, segSize, Base64FormattingOptions.InsertLineBreaks);

            Assert.True(extB64.Equals(localB64));
        }
    }
}