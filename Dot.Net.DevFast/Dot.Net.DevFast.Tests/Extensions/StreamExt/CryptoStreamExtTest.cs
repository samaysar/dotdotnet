using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StringExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamExt
{
    [TestFixture]
    public class CryptoStreamExtTest
    {
        [Test]
        public async Task Rijndael_Transform_Works_As_Expected_On_ArraySegement()
        {
            const string original = "Here is some d@t@ to encrypt!";
            var segement = original.ToByteSegment();

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                rijndael.GenerateIV();

                using (var msEncrypt = new MemoryStream())
                {
                    await segement.TransformAsync(rijndael.CreateEncryptor(),
                        msEncrypt, CancellationToken.None).ConfigureAwait(false);

                    msEncrypt.Seek(0, SeekOrigin.Begin);

                    var sb = new StringBuilder();

                    await msEncrypt.TransformAsync(rijndael.CreateDecryptor(),
                        sb, CancellationToken.None).ConfigureAwait(false);

                    Assert.True(sb.ToString().Equals(original));
                }
            }
        }

        [Test]
        public async Task Rijndael_Transform_Works_As_Expected_On_ByteArray()
        {
            const string original = "Here is some d@t@ to encrypt!";
            var segement = original.ToBytes();

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                rijndael.GenerateIV();

                using (var msEncrypt = new MemoryStream())
                {
                    await segement.TransformAsync(rijndael.CreateEncryptor(),
                        msEncrypt, CancellationToken.None).ConfigureAwait(false);

                    msEncrypt.Seek(0, SeekOrigin.Begin);

                    var sb = new StringBuilder();

                    await msEncrypt.TransformAsync(rijndael.CreateDecryptor(),
                        sb, CancellationToken.None).ConfigureAwait(false);

                    Assert.True(sb.ToString().Equals(original));
                }
            }
        }

        [Test]
        public async Task Rijndael_Transform_Works_As_Expected_On_Stream()
        {
            const string original = "Here is some d@t@ to encrypt!";
            using (var inputBuff = new MemoryStream())
            {
                await original.WriteToAsync(inputBuff).ConfigureAwait(false);
                inputBuff.Seek(0, SeekOrigin.Begin);
                using (var rijndael = new RijndaelManaged())
                {
                    rijndael.GenerateKey();
                    rijndael.GenerateIV();

                    using (var msEncrypt = new MemoryStream())
                    {
                        await inputBuff.TransformAsync(rijndael.CreateEncryptor(),
                            msEncrypt, CancellationToken.None).ConfigureAwait(false);

                        msEncrypt.Seek(0, SeekOrigin.Begin);

                        var sb = new StringBuilder();

                        await msEncrypt.TransformAsync(rijndael.CreateDecryptor(),
                            sb, CancellationToken.None).ConfigureAwait(false);

                        Assert.True(sb.ToString().Equals(original));
                    }
                }
            }
        }

        [Test]
        public async Task Rijndael_Transform_Works_As_Expected_On_StringBuilder()
        {
            const string original = "Here is some d@t@ to encrypt!";

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                rijndael.GenerateIV();

                using (var msEncrypt = new MemoryStream())
                {
                    await new StringBuilder(original).TransformAsync(rijndael.CreateEncryptor(),
                        msEncrypt, CancellationToken.None).ConfigureAwait(false);

                    msEncrypt.Seek(0, SeekOrigin.Begin);

                    var sb = new StringBuilder();

                    await msEncrypt.TransformAsync(rijndael.CreateDecryptor(),
                        sb, CancellationToken.None).ConfigureAwait(false);

                    Assert.True(sb.ToString().Equals(original));
                }
            }
        }
    }
}