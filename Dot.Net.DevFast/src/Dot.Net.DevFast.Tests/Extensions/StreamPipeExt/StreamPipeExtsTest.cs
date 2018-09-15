using System;
using System.Collections.Concurrent;
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
            var outcome = Task.FromResult(val).Push().ThenToBase64().AndWriteBytesAsync();
            var valBack = await outcome.Pull().ThenFromBase64().AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));

            valBack = await outcome.Result.Pull().ThenFromBase64().AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));

            //Coverage Test - PUSH
            valBack = await outcome.Push().ThenFromBase64().AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));

            //Coverage Test - PULL
            valBack = await outcome.Pull().ThenToBase64()
                .ThenFromBase64()
                .ThenFromBase64()
                .AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));

            valBack = await outcome.Result.Pull().ThenToBase64()
                .ThenFromBase64()
                .ThenFromBase64()
                .AndWriteBytesAsync()
                .ConfigureAwait(false);
            Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));
        }

        [Test]
        public async Task Encrypt_Decrypt_Harmonize()
        {
            await Encrypt_Decrypt_Harmonize<AesManaged>().ConfigureAwait(false);
            await Encrypt_Decrypt_Harmonize<DESCryptoServiceProvider>().ConfigureAwait(false);
            await Encrypt_Decrypt_Harmonize<TripleDESCryptoServiceProvider>().ConfigureAwait(false);
            await Encrypt_Decrypt_Harmonize<RijndaelManaged>().ConfigureAwait(false);
            await Encrypt_Decrypt_Harmonize<RC2CryptoServiceProvider>().ConfigureAwait(false);
#if NET472
            await Encrypt_Decrypt_Harmonize<AesCng>().ConfigureAwait(false);
            await Encrypt_Decrypt_Harmonize<TripleDESCng>().ConfigureAwait(false);
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

            md5 = MD5.Create();
            sha = SHA256.Create();
            outcome = await new UTF8Encoding(false).GetBytes(val)
                .Pull()
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
            Assert.False(val.ToString().Equals(new UTF8Encoding(false).GetString(outcome)));

            md51 = MD5.Create();
            sha1 = SHA256.Create();
            md52 = MD5.Create();
            sha2 = SHA256.Create();
            outcome = await Task.FromResult((Stream)new MemoryStream(outcome))
                .Pull()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenDecompress()
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .AndWriteBytesAsync().ConfigureAwait(false);
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));
            Assert.True(val.ToString().Equals(new UTF8Encoding(false).GetString(outcome)));
        }

        [Test]
        public async Task File_To_File_Streaming_Encryption_N_Compression()
        {
            FileInfo encryptedCompressedFile = null;
            var fileName = Guid.NewGuid().ToString("N");
            var file = AppDomain.CurrentDomain.BaseDirectory.ToDirectoryInfo().CreateFileInfo($"{fileName}.txt");
            try
            {
                File.WriteAllText(file.FullName, TestValues.BigString, new UTF8Encoding(false));
                encryptedCompressedFile = await file.Directory
                    .Push($"{fileName}.txt")
                    .ThenEncrypt<AesManaged>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt
#if NET472
                        , HashAlgorithmName.SHA512
#endif
                    )
                    .ThenCompress()
                    .AndWriteFileAsync(file.DirectoryName).ConfigureAwait(false);
                var fileRevOps = await encryptedCompressedFile.Directory
                    .Pull(encryptedCompressedFile.Name)
                    .ThenDecompress()
                    .ThenDecrypt<AesManaged>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt
#if NET472
                        , HashAlgorithmName.SHA512
#endif
                    )
                    .AndWriteFileAsync(file.DirectoryName, file.Name).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(File.ReadAllText(fileRevOps.FullName)));

                fileRevOps = await encryptedCompressedFile.Directory
                    .Pull(encryptedCompressedFile.Name)
                    .ThenDecompress()
                    .ThenDecrypt<AesManaged>(TestValues.FixedCryptoPass, TestValues.FixedCryptoSalt
#if NET472
                        , HashAlgorithmName.SHA512
#endif
                    )
                    .AndWriteFileAsync(file.Directory, file.Name).ConfigureAwait(false);
                Assert.True(TestValues.BigString.Equals(File.ReadAllText(fileRevOps.FullName)));
            }
            finally
            {
                file.Refresh();
                file.Delete();
                encryptedCompressedFile?.Refresh();
                encryptedCompressedFile?.Delete();
            }
        }

        [Test]
        public async Task Pull_To_Push_Is_Working_As_Needed()
        {
            Assert.True(new UTF8Encoding(false).GetString(await TestValues.BigString
                .ToByteSegment(new UTF8Encoding(false))
                .Pull()
                .ThenConvertToPush()
                .AndWriteBytesAsync()
                .ConfigureAwait(false)).Equals(TestValues.BigString));
        }

        [Test]
        public async Task Object_ToJson_Then_Back_Object_In_Streaming()
        {
            //ASYNC
            var obj = new TestObject();
            var postStream = await Task.FromResult(obj).PushJsonAsync()
                .ThenCompress()
                .AndWriteBytesAsync()
                .Pull()
                .ThenDecompress()
                .AndParseJsonAsync<TestObject>().ConfigureAwait(false);
            Assert.True(obj.IntProp.Equals(postStream.IntProp));
            Assert.True(obj.StrProp.Equals(postStream.StrProp));
            Assert.True(obj.BytesProp.Length.Equals(postStream.BytesProp.Length));
            for (var i = 0; i < obj.BytesProp.Length; i++)
            {
                Assert.True(obj.BytesProp[i].Equals(postStream.BytesProp[i]));
            }

            //SYNC
            obj = new TestObject();
            postStream = (await obj.PushJson()
                    .ThenCompress()
                    .AndWriteBytesAsync().ConfigureAwait(false))
                .Pull()
                .ThenDecompress()
                .AndParseJson<TestObject>();
            Assert.True(obj.IntProp.Equals(postStream.IntProp));
            Assert.True(obj.StrProp.Equals(postStream.StrProp));
            Assert.True(obj.BytesProp.Length.Equals(postStream.BytesProp.Length));
            for (var i = 0; i < obj.BytesProp.Length; i++)
            {
                Assert.True(obj.BytesProp[i].Equals(postStream.BytesProp[i]));
            }
        }

        [Test]
        public async Task Object_Array_ToJson_Then_Back_Object_In_Streaming()
        {
            var obj = new TestObject[5];
            for (var i = 0; i < obj.Length; i++)
            {
                obj[i] = new TestObject();
            }

            //ASYNC
            var coll = new BlockingCollection<TestObject>();
            var writerTask = Task.Run(async () => await obj.PushJsonArray()
                .AndWriteBytesAsync()
                .Pull()
                .AndParseJsonArrayAsync(coll).ConfigureAwait(false));
            var cnt = 0;
            foreach (var postStream in coll.GetConsumingEnumerable())
            {
                Assert.True(obj[cnt].IntProp.Equals(postStream.IntProp));
                Assert.True(obj[cnt].StrProp.Equals(postStream.StrProp));
                Assert.True(obj[cnt].BytesProp.Length.Equals(postStream.BytesProp.Length));
                for (var i = 0; i < obj[cnt].BytesProp.Length; i++)
                {
                    Assert.True(obj[cnt].BytesProp[i].Equals(postStream.BytesProp[i]));
                }
                cnt++;
            }

            await writerTask.ConfigureAwait(false);

            //SYNC
            cnt = 0;
            foreach (var postStream in (await obj.PushJson()
                    .AndWriteBytesAsync().ConfigureAwait(false))
                .Pull()
                .AndParseJsonArray<TestObject>())
            {
                Assert.True(obj[cnt].IntProp.Equals(postStream.IntProp));
                Assert.True(obj[cnt].StrProp.Equals(postStream.StrProp));
                Assert.True(obj[cnt].BytesProp.Length.Equals(postStream.BytesProp.Length));
                for (var i = 0; i < obj[cnt].BytesProp.Length; i++)
                {
                    Assert.True(obj[cnt].BytesProp[i].Equals(postStream.BytesProp[i]));
                }
                cnt++;
            }
        }

        private static async Task Encrypt_Decrypt_Harmonize<T>()
            where T : SymmetricAlgorithm, new()
        {
            //////////////// =========== PUSH

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

            //////////////// =========== PULL - No TASK

            md51 = MD5.Create();
            sha1 = SHA256.Create();
            md52 = MD5.Create();
            sha2 = SHA256.Create();
            pipeOutcome = await new ArraySegment<byte>(pipeOutcome, 0, pipeOutcome.Length).Pull()
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
            var pipeSeg = await new ArraySegment<byte>(pipeOutcome, 0, pipeOutcome.Length).Pull()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenEncrypt<T>(tins.Key, tins.IV)
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .ThenDecrypt<T>(tins.Key, tins.IV)
                .AndWriteByteSegAsync().ConfigureAwait(false);
            pipeOutcome = new MemoryStream(pipeSeg.Array, pipeSeg.Offset, pipeSeg.Count).ToArray();
            Assert.True(new UTF8Encoding(false).GetString(pipeOutcome).Equals(TestValues.BigString));
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));

            //////////////// =========== PULL - TASK

            md51 = MD5.Create();
            sha1 = SHA256.Create();
            md52 = MD5.Create();
            sha2 = SHA256.Create();
            pipeOutcome = await Task.FromResult(new ArraySegment<byte>(pipeOutcome, 0, pipeOutcome.Length)).Pull()
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
            pipeSeg = await Task.FromResult(new ArraySegment<byte>(pipeOutcome, 0, pipeOutcome.Length)).Pull()
                .ThenComputeHash(md51)
                .ThenComputeHash(sha1)
                .ThenEncrypt<T>(tins.Key, tins.IV)
                .ThenComputeHash(md52)
                .ThenComputeHash(sha2)
                .ThenDecrypt<T>(tins.Key, tins.IV)
                .AndWriteByteSegAsync().ConfigureAwait(false);
            pipeOutcome = new MemoryStream(pipeSeg.Array, pipeSeg.Offset, pipeSeg.Count).ToArray();
            Assert.True(new UTF8Encoding(false).GetString(pipeOutcome).Equals(TestValues.BigString));
            Assert.False(md51.Hash.ToBase64().Equals(md52.Hash.ToBase64()));
            Assert.False(sha1.Hash.ToBase64().Equals(sha2.Hash.ToBase64()));
        }
    }
}