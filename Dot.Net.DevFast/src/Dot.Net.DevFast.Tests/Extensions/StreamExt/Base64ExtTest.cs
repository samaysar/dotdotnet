using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamExt
{
    [TestFixture]
    public class Base64ExtTest
    {
        #region BUFFER based extensions, useful for small strings

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
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

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public void String_Returning_FromBase64_Works_Correctly(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            var extB64 = TestValues.BigString.ToBase64(Base64FormattingOptions.InsertLineBreaks, encIns);
            var resultStr = extB64.FromBase64(encIns);
            Assert.True(resultStr.Equals(TestValues.BigString));
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public void String_Returning_FromBase64_Detects_Encoding_From_ByteMark_When_Null_Encoding_Is_Given(string enc)
        {
            using (var buff = new MemoryStream())
            {
                var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
                var dataArr = encIns.GetPreamble();
                buff.Write(dataArr, 0, dataArr.Length);
                dataArr = encIns.GetBytes(TestValues.BigString);
                buff.Write(dataArr, 0, dataArr.Length);

                var extB64 = new ArraySegment<byte>(buff.GetBuffer(), 0, (int) buff.Length).ToBase64();
                var resultStr = extB64.FromBase64(null);
                Assert.True(TestValues.BigString.Equals(resultStr));
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(350)]
        public void ByteArray_Returning_FromBase64_Works_Correctly(int arrSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var extB64 = bytes.ToBase64(Base64FormattingOptions.InsertLineBreaks);
            var returnArr = extB64.FromBase64();

            Assert.NotNull(returnArr);
            Assert.True(returnArr.Length.Equals(bytes.Length));
            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.True(returnArr[i].Equals(bytes[i]));
            }
        }

        #endregion BUFFER based extensions, useful for small strings

        #region Stream based extensions, useful for larger strings (though some string returning method would consume memory)}

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(350)]
        public async Task ByteArray_ToBase64Async_Works_As_Expected(int arrSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);
            var localB64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            string extB64;
            using (var mem = new MemoryStream())
            {
                await bytes.ToBase64Async(mem).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));

            using (var mem = new MemoryStream())
            {
                await bytes.ToBase64Async(mem, CancellationToken.None).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
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
        public async Task ByteArraySegment_ToBase64Async_Works_As_Expected(int arrSize, int segSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var segment = new ArraySegment<byte>(bytes, 0, segSize);
            var localB64 = Convert.ToBase64String(bytes, 0, segSize, Base64FormattingOptions.None);
            string extB64;
            using (var mem = new MemoryStream())
            {
                await segment.ToBase64Async(mem).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));

            using (var mem = new MemoryStream())
            {
                await segment.ToBase64Async(mem, CancellationToken.None).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task String_ToBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            string localB64;
            using (var buff = new MemoryStream())
            {
                var dataArr = encIns.GetPreamble();
                buff.Write(dataArr, 0, dataArr.Length);
                dataArr = encIns.GetBytes(TestValues.BigString);
                buff.Write(dataArr, 0, dataArr.Length);

                localB64 = new ArraySegment<byte>(buff.GetBuffer(), 0, (int) buff.Length).ToBase64();
            }
            string extB64;
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, CancellationToken.None, encoding: encIns).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task StringBuilder_ToBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            string localB64;
            using (var buff = new MemoryStream())
            {
                var dataArr = encIns.GetPreamble();
                buff.Write(dataArr, 0, dataArr.Length);
                dataArr = encIns.GetBytes(TestValues.BigString);
                buff.Write(dataArr, 0, dataArr.Length);

                localB64 = new ArraySegment<byte>(buff.GetBuffer(), 0, (int)buff.Length).ToBase64();
            }
            string extB64;
            using (var mem = new MemoryStream())
            {
                await new StringBuilder(TestValues.BigString).ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
            Assert.True(extB64.Equals(localB64));

            using (var mem = new MemoryStream())
            {
                await new StringBuilder(TestValues.BigString).ToBase64Async(mem, CancellationToken.None, encoding: encIns).ConfigureAwait(false);
                extB64 = Encoding.UTF8.GetString(mem.ToArray());
            }
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
        public async Task Stream_ToBase64Async_Works_As_Expected(int arrSize, int segSize)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var localB64 = Convert.ToBase64String(bytes, 0, segSize, Base64FormattingOptions.None);
            string extB64;
            using (var mem = new MemoryStream())
            {
                using (var input = new MemoryStream(bytes, 0, segSize))
                {
                    await input.ToBase64Async(mem).ConfigureAwait(false);
                    extB64 = Encoding.UTF8.GetString(mem.ToArray());
                }
            }
            Assert.True(extB64.Equals(localB64));

            using (var mem = new MemoryStream())
            {
                using (var input = new MemoryStream(bytes, 0, segSize))
                {
                    await input.ToBase64Async(mem, CancellationToken.None).ConfigureAwait(false);
                    extB64 = Encoding.UTF8.GetString(mem.ToArray());
                }
            }
            Assert.True(extB64.Equals(localB64));
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task Stream_FromBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                using (var output = new MemoryStream())
                {
                    await mem.FromBase64Async(output).ConfigureAwait(false);
                    output.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(output, encIns))
                    {
                        Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                    }
                }
            }

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                using (var output = new MemoryStream())
                {
                    await mem.FromBase64Async(output, CancellationToken.None).ConfigureAwait(false);
                    output.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(output, encIns))
                    {
                        Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                    }
                }
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task StringBuilder_FromBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = new StringBuilder();
                await mem.FromBase64Async(output, encoding: encIns).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(output.ToString()));
            }

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = new StringBuilder();
                await mem.FromBase64Async(output, CancellationToken.None, encoding: encIns).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(output.ToString()));
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task String_FromBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64AsStringAsync(encoding: encIns).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(output));
            }

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64AsStringAsync(CancellationToken.None, encoding: encIns).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(output));
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task ByteArray_FromBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64Async().ConfigureAwait(false);
                using (var reader = new StreamReader(new MemoryStream(output), encIns))
                {
                    Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                }
            }

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64Async(CancellationToken.None).ConfigureAwait(false);
                using (var reader = new StreamReader(new MemoryStream(output), encIns))
                {
                    Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                }
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase("utf-8")]
        [TestCase("utf-32BE")]
        [TestCase("utf-32LE")]
        [TestCase("utf-32")]
        [TestCase("utf-16BE")]
        [TestCase("utf-16LE")]
        [TestCase("utf-16")]
        [TestCase("us-ascii")]
        public async Task ByteArraySegment_FromBase64Async_Works_As_Expected(string enc)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64AsSegmentAsync().ConfigureAwait(false);
                Assert.NotNull(output.Array);
                using (var reader = new StreamReader(new MemoryStream(output.Array, output.Offset, output.Count), encIns))
                {
                    Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                }
            }

            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.ToBase64Async(mem, encoding: encIns).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var output = await mem.FromBase64AsSegmentAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.NotNull(output.Array);
                using (var reader = new StreamReader(new MemoryStream(output.Array, output.Offset, output.Count), encIns))
                {
                    Assert.True(TestValues.BigString.Equals(reader.ReadToEnd()));
                }
            }
        }

        #endregion Stream based extensions, useful for larger strings (though some string returning method would consume memory)
    }
}