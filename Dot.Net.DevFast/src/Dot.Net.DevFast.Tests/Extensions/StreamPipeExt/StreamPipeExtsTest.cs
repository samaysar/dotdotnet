using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StreamPipeExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
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
            var outcome = await Task.FromResult(val).Push().ThenToBase64().ThenFromBase64().AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(outcome)));
        }

        [Test]
        public async Task Push_Based_Encrypt_Decrypt_Harmonize()
        {
            await Push_Based_Encrypt_Decrypt_Harmonize<AesManaged>().ConfigureAwait(false);
            await Push_Based_Encrypt_Decrypt_Harmonize<DESCryptoServiceProvider>().ConfigureAwait(false);
            await Push_Based_Encrypt_Decrypt_Harmonize<TripleDESCryptoServiceProvider>().ConfigureAwait(false);
            await Push_Based_Encrypt_Decrypt_Harmonize<RijndaelManaged>().ConfigureAwait(false);
            await Push_Based_Encrypt_Decrypt_Harmonize<RC2CryptoServiceProvider>().ConfigureAwait(false);
#if NET472
            await Push_Based_Encrypt_Decrypt_Harmonize<AesCng>().ConfigureAwait(false);
            await Push_Based_Encrypt_Decrypt_Harmonize<TripleDESCng>().ConfigureAwait(false);
#endif
        }

        [Test]
        public async Task Byte_Streaming_With_Hashing()
        {
            const string val = "Hello Streaming Worlds!";
            var md5 = MD5.Create();
            var sha = SHA256.Create();
            var outcome = await Task.FromResult(new UTF8Encoding(false).GetBytes(val))
                .Push()
                .ThenComputeHash(md5)
                .ThenComputeHash(sha)
                .AndWriteBytesAsync().ConfigureAwait(false);
            Assert.False(md5.Hash.ToBase64().Equals(sha.Hash.ToBase64()));
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(outcome)));
        }

        [Test]
        public async Task StringBuilder_Streaming_With_Compression()
        {
            var val = new StringBuilder("Hello Streaming Worlds!");
            var md51 = MD5.Create();
            var sha1 = SHA256.Create();
            var md52 = MD5.Create();
            var sha2 = SHA256.Create();
            //var md53 = MD5.Create();
            //var sha3 = SHA256.Create();
            var outcome = await Task.FromResult(val)
                .Push()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenCompress()
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .AndWriteBytesAsync().ConfigureAwait(false);
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));
            //Assert.True(sha1.Hash.ToBase64().Equals(sha3.Hash.ToBase64()));
            //Assert.True(md51.Hash.ToBase64().Equals(md53.Hash.ToBase64()));
            Assert.False(val.ToString().Equals(new UTF8Encoding(false).GetString(outcome)));
        }

        [Test]
        public async Task File_To_File_Streaming_Encryption_N_Compression()
        {
            var fileName = Guid.NewGuid().ToString("N");
            var file = AppDomain.CurrentDomain.BaseDirectory.ToDirectoryInfo().CreateFileInfo($"{fileName}.txt");
            try
            {
                File.WriteAllText(file.FullName, TestValues.BigString, new UTF8Encoding(false));
                var encryptedCompressedFile = await file.Directory
                    .Push($"{fileName}.txt")
                    .ThenEncrypt<AesManaged>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt
#if NET472
                        , HashAlgorithmName.SHA512
#endif
                    )
                    .ThenCompress()
                    .AndWriteFileAsync(file.DirectoryName).ConfigureAwait(false);
                var unecryptedUncompressedData = await encryptedCompressedFile
                    .Pull()
                    .ThenDecompress()
                    .ThenConvertToPush()
                    .ThenDecrypt<AesManaged>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt
#if NET472
                        , HashAlgorithmName.SHA512
#endif
                    )
                    .AndWriteBytesAsync().ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(new UTF8Encoding(false).GetString(unecryptedUncompressedData)));
            }
            finally
            {
                file.Refresh();
                file.Delete();
            }
        }

        private static async Task Push_Based_Encrypt_Decrypt_Harmonize<T>()
            where T : SymmetricAlgorithm, new()
        {
            var md51 = MD5.Create();
            var sha1 = SHA256.Create();
            var md52 = MD5.Create();
            var sha2 = SHA256.Create();
            var pipeOutcome = await TestValues.BigString.Push()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenEncrypt<T>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt,
#if NET472
                    HashAlgorithmName.SHA512,
#endif
                    20)
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .ThenDecrypt<T>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt,
#if NET472
                    HashAlgorithmName.SHA512,
#endif
                    20)
                .AndWriteBytesAsync().ConfigureAwait(false);
            Assert.True(new UTF8Encoding(false).GetString(pipeOutcome).Equals(TestValues.BigString));
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));

            md51 = MD5.Create();
            sha1 = SHA256.Create();
            md52 = MD5.Create();
            sha2 = SHA256.Create();
            var tins = new T();
            tins.InitKeyNIv(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt,
#if NET472
                HashAlgorithmName.SHA512,
#endif
                20, null);
            pipeOutcome = (await Task.FromResult(TestValues.BigString).Push()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenEncrypt<T>(tins.Key, tins.IV)
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .ThenDecrypt<T>(tins.Key, tins.IV)
                .AndWriteBufferAsync(seekToOrigin: true).ConfigureAwait(false)).ToArray();
            Assert.True(new UTF8Encoding(false).GetString(pipeOutcome).Equals(TestValues.BigString));
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));
        }
    }
}