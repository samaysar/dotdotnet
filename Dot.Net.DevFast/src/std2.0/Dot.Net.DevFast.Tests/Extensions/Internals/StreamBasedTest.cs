using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.Internals;
using Dot.Net.DevFast.Extensions.StringExt;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals
{
    [TestFixture]
    public class StreamBasedTest
    {
        [Test]
        public async Task Action_Based_CopyFromWithDisposeAsync_Works_Properly()
        {
            var mem = new MemoryStream();
            try
            {
                var writable = Substitute.For<Stream>();
                writable.WriteAsync(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                    .Returns(x => mem.WriteAsync(x[0] as byte[], (int) x[1], (int) x[2], (CancellationToken) x[3]));
                await writable.CopyFromWithDisposeAsync("123".Length, new UTF8Encoding(false), CancellationToken.None,
                    3, "123".CopyTo, mem).ConfigureAwait(false);

                Assert.True(Encoding.UTF8.GetString(mem.ToArray()).Equals("123"));
                writable.Received(1).Dispose();
            }
            finally
            {
                using (mem)
                {
                }
            }
        }

        [Test]
        public async Task Action_Based_CopyFromAsync_Works_Properly()
        {
            var mem = new MemoryStream();
            try
            {
                var writable = Substitute.For<Stream>();
                writable.WriteAsync(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                    .Returns(x => mem.WriteAsync(x[0] as byte[], (int)x[1], (int)x[2], (CancellationToken)x[3]));
                await writable.CopyFromAsync("123".Length, new UTF8Encoding(false), CancellationToken.None,
                    3, "123".CopyTo).ConfigureAwait(false);

                Assert.True(Encoding.UTF8.GetString(mem.ToArray()).Equals("123"));
                writable.Received(0).Dispose();
            }
            finally
            {
                using (mem)
                {
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task StringBuilder_Based_CopyToBuilderAsync_Works_Properly(bool dispose)
        {
            using (var mem = new MemoryStream())
            {
                await "123".ToStreamAsync(mem).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var sb = new StringBuilder();
                await mem.CopyToBuilderAsync(sb, CancellationToken.None, Encoding.UTF8, 10, dispose).ConfigureAwait(false);

                Assert.True(mem.CanSeek == !dispose);
                Assert.True(sb.ToString().Equals("123"));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Stream_Stream_Based_CopyFromWithDisposeAsync_Works_Properly(bool dispose)
        {
            using (var mem = new MemoryStream())
            {
                await "123".ToStreamAsync(mem).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var newbuff = new MemoryStream();

                await newbuff.CopyFromWithDisposeAsync(mem, 10, CancellationToken.None, newbuff, dispose)
                    .ConfigureAwait(false);
                Assert.True(mem.CanSeek == !dispose);
                Assert.False(newbuff.CanSeek);

                var memArr = mem.ToArray();
                var newBuffArr = newbuff.ToArray();

                Assert.True(memArr.Length == newBuffArr.Length);
                for (var i = 0; i < memArr.Length; i++)
                {
                    Assert.True(memArr[i].Equals(newBuffArr[i]));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Stream_Stream_Based_CopyToWithDisposeAsync_Works_Properly(bool dispose)
        {
            using (var mem = new MemoryStream())
            {
                await "123".ToStreamAsync(mem).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var newbuff = new MemoryStream();

                await mem.CopyToWithDisposeAsync(newbuff, 10, CancellationToken.None, dispose)
                    .ConfigureAwait(false);
                Assert.True(newbuff.CanSeek == !dispose);
                Assert.False(mem.CanSeek);

                var memArr = mem.ToArray();
                var newBuffArr = newbuff.ToArray();

                Assert.True(memArr.Length == newBuffArr.Length);
                for (var i = 0; i < memArr.Length; i++)
                {
                    Assert.True(memArr[i].Equals(newBuffArr[i]));
                }
            }
        }

        [Test]
        public async Task Stream_Array_Based_CopyFromWithDisposeAsync_Works_Properly()
        {
            using (var mem = new MemoryStream())
            {
                await "123".ToStreamAsync(mem).ConfigureAwait(false);
                var memArr = mem.ToArray();
                var newbuff = new MemoryStream();

                await newbuff.CopyFromWithDisposeAsync(memArr, 0, memArr.Length, CancellationToken.None, newbuff)
                    .ConfigureAwait(false);
                Assert.False(newbuff.CanSeek);

                var newBuffArr = newbuff.ToArray();

                Assert.True(memArr.Length == newBuffArr.Length);
                for (var i = 0; i < memArr.Length; i++)
                {
                    Assert.True(memArr[i].Equals(newBuffArr[i]));
                }
            }
        }

        [Test]
        public async Task Stream_Based_CopyToSegmentWithDisposeAsync_Works_Properly()
        {
            using (var mem = new MemoryStream())
            {
                await "123".ToStreamAsync(mem).ConfigureAwait(false);
                var memArr = mem.ToArray();
                mem.Seek(0, SeekOrigin.Begin);
                var segment = await mem.CopyToSegmentWithDisposeAsync(3, CancellationToken.None)
                    .ConfigureAwait(false);
                Assert.False(mem.CanSeek);

                var newBuffArr = segment.CreateBytes();
                Assert.True(memArr.Length == newBuffArr.Length);
                for (var i = 0; i < memArr.Length; i++)
                {
                    Assert.True(memArr[i].Equals(newBuffArr[i]));
                }
            }
        }
    }
}