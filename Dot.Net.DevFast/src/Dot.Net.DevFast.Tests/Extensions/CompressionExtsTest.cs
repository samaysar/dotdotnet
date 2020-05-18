using System;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class CompressionExtsTest
    {
        [Test]
        [TestCase(0, 0, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(2, 0, true)]
        [TestCase(2, 1, true)]
        [TestCase(2, 2, true)]
        [TestCase(350, 123, true)]
        [TestCase(99, 91, true)]
        [TestCase(0, 0, false)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, false)]
        [TestCase(2, 0, false)]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, false)]
        [TestCase(350, 123, false)]
        [TestCase(99, 91, false)]
        public async Task Segment_Based_CompressAsync_DecompressAsSegmentAsync_Harmonize(int arrSize, int segSize,
            bool gzip)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            var segment = new ArraySegment<byte>(bytes, 0, segSize);
            using (var mem = new MemoryStream())
            {
                await segment.CompressAsync(mem, gzip).ConfigureAwait(false);
                var compressedData = mem.ToArray();
                if (segSize == 0)
                {
#if !NETSTANDARD2_0 && !NETCOREAPP2_0
                    Assert.True(compressedData.Length == 0);
#else
                    Assert.False(compressedData.Length == 0);
#endif
                }
                else
                {
                    var invalidCompress = compressedData.Length == segSize;
                    Assert.NotNull(segment.Array);
                    for (var i = 0; i < Math.Min(segSize, compressedData.Length); i++)
                    {
                        if (invalidCompress)
                        {
                            invalidCompress = segment.Array[i] == compressedData[i];
                        }
                    }

                    Assert.False(invalidCompress);
                }

                mem.Seek(0, SeekOrigin.Begin);
                var uncompressed = await mem.DecompressAsSegmentAsync(gzip).ConfigureAwait(false);
                Assert.True(segment.Count == uncompressed.Count);
                Assert.NotNull(segment.Array);
                Assert.NotNull(uncompressed.Array);
                for (var i = 0; i < uncompressed.Count; i++)
                {
                    Assert.True(segment.Array[i].Equals(uncompressed.Array[i]));
                }
            }
        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(350, true)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(350, false)]
        public async Task ByteArray_Based_CompressAsync_DecompressAsync_Harmonize(int arrSize, bool gzip)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            using (var mem = new MemoryStream())
            {
                await bytes.CompressAsync(mem, gzip).ConfigureAwait(false);
                var compressedData = mem.ToArray();
                if (arrSize == 0)
                {
#if !NETSTANDARD2_0 && !NETCOREAPP2_0
                    Assert.True(compressedData.Length == 0);
#else
                    Assert.False(compressedData.Length == 0);
#endif
                }
                else
                {
                    var invalidCompress = compressedData.Length == arrSize;
                    for (var i = 0; i < Math.Min(arrSize, compressedData.Length); i++)
                    {
                        if (invalidCompress)
                        {
                            invalidCompress = bytes[i] == compressedData[i];
                        }
                    }
                    Assert.False(invalidCompress);
                }
                mem.Seek(0, SeekOrigin.Begin);
                var uncompressed = await mem.DecompressAsync(gzip).ConfigureAwait(false);
                Assert.True(arrSize == uncompressed.Length);
                for (var i = 0; i < uncompressed.Length; i++)
                {
                    Assert.True(bytes[i].Equals(uncompressed[i]));
                }
            }
        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(350, true)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(350, false)]
        public async Task Stream_Stream_Based_CompressAsync_DecompressAsync_Harmonize(int arrSize, bool gzip)
        {
            var randm = new Random();
            var bytes = new byte[arrSize];
            randm.NextBytes(bytes);

            using (var input = new MemoryStream(bytes, false))
            {
                using (var mem = new MemoryStream())
                {
                    await input.CompressAsync(mem, gzip).ConfigureAwait(false);
                    var compressedData = mem.ToArray();
                    if (arrSize == 0)
                    {
                        Assert.True(compressedData.Length >= 0);
                    }
                    else
                    {
                        var invalidCompress = compressedData.Length == arrSize;
                        for (var i = 0; i < Math.Min(arrSize, compressedData.Length); i++)
                        {
                            if (invalidCompress)
                            {
                                invalidCompress = bytes[i] == compressedData[i];
                            }
                        }
                        Assert.False(invalidCompress);
                    }
                    mem.Seek(0, SeekOrigin.Begin);
                    using (var output = new MemoryStream())
                    {
                        await mem.DecompressAsync(output, gzip).ConfigureAwait(false);
                        var uncompressed = output.ToArray();
                        Assert.True(arrSize == uncompressed.Length);
                        for (var i = 0; i < uncompressed.Length; i++)
                        {
                            Assert.True(bytes[i].Equals(uncompressed[i]));
                        }
                    }
                }
            }
        }

        [Test]
        [TestCase(null, true)]
        [TestCase("utf-8", true)]
        [TestCase("utf-7", true)]
        [TestCase("utf-32BE", true)]
        [TestCase("utf-32LE", true)]
        [TestCase("utf-32", true)]
        [TestCase("utf-16BE", true)]
        [TestCase("utf-16LE", true)]
        [TestCase("utf-16", true)]
        [TestCase("us-ascii", true)]
        [TestCase(null, false)]
        [TestCase("utf-8", false)]
        [TestCase("utf-7", false)]
        [TestCase("utf-32BE", false)]
        [TestCase("utf-32LE", false)]
        [TestCase("utf-32", false)]
        [TestCase("utf-16BE", false)]
        [TestCase("utf-16LE", false)]
        [TestCase("utf-16", false)]
        [TestCase("us-ascii", false)]
        public async Task StringBuilder_Based_CompressAsync_DecompressAsync_Harmonize(string enc, bool gzip)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            var sb = new StringBuilder(TestValues.BigString);
            using (var mem = new MemoryStream())
            {
                await sb.CompressAsync(mem, gzip, enc: encIns).ConfigureAwait(false);
                Assert.False(encIns.GetString(mem.ToArray()).Equals(TestValues.BigString));
                mem.Seek(0, SeekOrigin.Begin);
                sb.Clear();
                await mem.DecompressAsync(sb, gzip, enc: encIns).ConfigureAwait(false);
                Assert.True(sb.ToString().Equals(TestValues.BigString));
            }
        }

        [Test]
        [TestCase(null, true)]
        [TestCase("utf-8", true)]
        [TestCase("utf-7", true)]
        [TestCase("utf-32BE", true)]
        [TestCase("utf-32LE", true)]
        [TestCase("utf-32", true)]
        [TestCase("utf-16BE", true)]
        [TestCase("utf-16LE", true)]
        [TestCase("utf-16", true)]
        [TestCase("us-ascii", true)]
        [TestCase(null, false)]
        [TestCase("utf-8", false)]
        [TestCase("utf-7", false)]
        [TestCase("utf-32BE", false)]
        [TestCase("utf-32LE", false)]
        [TestCase("utf-32", false)]
        [TestCase("utf-16BE", false)]
        [TestCase("utf-16LE", false)]
        [TestCase("utf-16", false)]
        [TestCase("us-ascii", false)]
        public async Task String_Based_CompressAsync_DecompressAsStringAsync_Harmonize(string enc, bool gzip)
        {
            var encIns = Encoding.GetEncoding(enc.TrimSafeOrDefault("utf-8"));
            using (var mem = new MemoryStream())
            {
                await TestValues.BigString.CompressAsync(mem, gzip, enc: encIns).ConfigureAwait(false);
                Assert.False(encIns.GetString(mem.ToArray()).Equals(TestValues.BigString));
                mem.Seek(0, SeekOrigin.Begin);
                Assert.True(
                    (await mem.DecompressAsStringAsync(gzip, enc: encIns).ConfigureAwait(false)).Equals(
                        TestValues.BigString));
            }
        }
    }
}