using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StreamPipeExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamPipeExt
{
    [TestFixture]
    public class StreamPipeExtTest
    {
        [Test]
        public async Task String_Streaming_With_Base64()
        {
            const string val = "Hello Streaming Worlds!";
            var outcome = await Task.FromResult(val).Load().ThenToBase64().ThenFromBase64().AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(outcome)));
        }

        [Test]
        public async Task String_Streaming_With_Hashing()
        {
            const string val = "Hello Streaming Worlds!";
            var md5 = MD5.Create();
            var sha = SHA256.Create();
            var outcome = await Task.FromResult(new UTF8Encoding(false).GetBytes(val))
                .Load()
                .ThenComputeHash(md5)
                .ThenComputeHash(sha)
                .AndWriteBytesAsync().ConfigureAwait(false);
            Assert.False(md5.Hash.ToBase64().Equals(sha.Hash.ToBase64()));
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(outcome)));
        }
    }
}