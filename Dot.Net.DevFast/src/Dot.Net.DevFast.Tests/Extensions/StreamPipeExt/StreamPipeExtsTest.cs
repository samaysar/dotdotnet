using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.Ppc;
using Dot.Net.DevFast.Extensions.StreamExt;
using Dot.Net.DevFast.Extensions.StreamPipeExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StreamPipeExt
{
    [TestFixture]
    public class StreamPipeExtTest
    {
        [Test]
        public async Task Stream_Streaming_With_Base64()
        {
            const string val = "Hello Streaming Worlds!";
            using (var stream = (Stream) new MemoryStream(val.ToBytes(new UTF8Encoding(false))))
            {
                var outcome = Task.FromResult(stream).Push().ThenToBase64().AndWriteBytesAsync();
                var valBack = await outcome.Pull().ThenFromBase64().AndWriteBytesAsync()
                    .ConfigureAwait(false);
                Assert.True(val.Equals(new UTF8Encoding(false).GetString(valBack)));
            }
        }

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
            outcome = await Task.FromResult((Stream) new MemoryStream(outcome))
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
            using (var mem1 = new MemoryStream())
            {
                var obj = new TestObject();
                var byteCount = await Task.FromResult(obj)
                    .PushJsonAsync()
                    .ThenCompress()
                    .ThenConcurrentlyWriteTo(mem1, false)
                    .AndCountBytesAsync().ConfigureAwait(false);
                var postStreamBytes = Task.FromResult(mem1.ToArray());
                var postStream = await postStreamBytes
                    .Pull()
                    .ThenCountBytes(out var byteCounter)
                    .ThenDecompress()
                    .AndParseJsonAsync<TestObject>().ConfigureAwait(false);
                AssertOnValueEquality(obj, postStream);
                Assert.True((await postStreamBytes.ConfigureAwait(false)).Length.Equals((int) byteCounter.ByteCount));
                Assert.True(byteCount.Equals(byteCounter.ByteCount));
                //SYNC
                obj = new TestObject();
                postStream = (await obj.PushJson()
                        .ThenCompress()
                        .AndWriteBytesAsync().ConfigureAwait(false))
                    .Pull()
                    .ThenDecompress()
                    .AndParseJson<TestObject>();
                AssertOnValueEquality(obj, postStream);
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
                AssertOnValueEquality(obj[cnt], postStream);
                cnt++;
            }

            await writerTask.ConfigureAwait(false);

            //SYNC
            var bc = new BlockingCollection<TestObject>(new ConcurrentQueue<TestObject>(obj));
            bc.CompleteAdding();
            cnt = 0;
            foreach (var postStream in (await bc.PushJsonArray()
                    .AndWriteBytesAsync().ConfigureAwait(false))
                .Pull()
                .AndParseJsonArray<TestObject>())
            {
                AssertOnValueEquality(obj[cnt], postStream);
                cnt++;
            }
        }

        [Test]
        public async Task Pull_Writes_To_Different_Sources_Consistently()
        {
            var fileName = Guid.NewGuid().ToString("N");
            var file = AppDomain.CurrentDomain.BaseDirectory.ToDirectoryInfo().CreateFileInfo($"{fileName}.txt");
            try
            {
                using (var mem1 = new MemoryStream())
                {
                    using (var mem2 = new MemoryStream())
                    {
                        var obj = new TestObject();
                        await Task.FromResult(JsonConvert.SerializeObject(obj)
                                .ToByteSegment(new UTF8Encoding(false)))
                            .Push()
                            .ThenCountBytes(out var pushByteCounter)
                            .ThenCompress()
                            .ThenConcurrentlyWriteTo(mem1, false)
                            .ThenConcurrentlyWriteTo(mem2, false)
                            .AndExecuteAsync().ConfigureAwait(false);
                        var sourceForPull = mem1.ToArray();
                        await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteFileAsync(file).ConfigureAwait(false);
                        AssertOnValueEquality(obj,
                            JsonConvert.DeserializeObject<TestObject>(File.ReadAllText(file.FullName)));

                        Assert.True(mem1.ToArray().SequenceEqual(mem2.ToArray()));
                        var buff = await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteBufferAsync(seekToOrigin: true).ConfigureAwait(false);
                        AssertOnValueEquality(obj,
                            JsonConvert.DeserializeObject<TestObject>(
                                new UTF8Encoding(false).GetString(buff.ToArray())));

                        await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteStreamAsync(
                                file.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite),
                                disposeTarget: true)
                            .ConfigureAwait(false);
                        AssertOnValueEquality(obj,
                            JsonConvert.DeserializeObject<TestObject>(File.ReadAllText(file.FullName)));

                        var strVal = await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteStringAsync().ConfigureAwait(false);
                        AssertOnValueEquality(obj, JsonConvert.DeserializeObject<TestObject>(strVal));

                        var strBuild = await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteStringBuilderAsync().ConfigureAwait(false);
                        AssertOnValueEquality(obj, JsonConvert.DeserializeObject<TestObject>(strBuild.ToString()));

                        strBuild.Clear();
                        await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .AndWriteStringBuilderAsync(strBuild).ConfigureAwait(false);
                        AssertOnValueEquality(obj, JsonConvert.DeserializeObject<TestObject>(strBuild.ToString()));

                        await sourceForPull
                            .Pull()
                            .ThenDecompress()
                            .ThenCountBytes(out var pullByteCounter)
                            .AndExecuteAsync().ConfigureAwait(false);

                        Assert.True(pullByteCounter.ByteCount != sourceForPull.Length);
                        Assert.True(pushByteCounter.ByteCount == pullByteCounter.ByteCount);
                    }
                }
            }
            finally
            {
                file.Refresh();
                file.Delete();
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(20)]
        public async Task Ppc_Based_Json_Array_Is_Consistent(int total)
        {
            using (var mem = new MemoryStream())
            {
                var dico = new Dictionary<string, TestObject>();
                await new Action<IProducerBuffer<TestObject>, CancellationToken>((b, t) =>
                    {
                        for (var i = 0; i < total; i++)
                        {
                            var instance = new TestObject();
                            dico[instance.StrProp] = instance;
                            b.Add(instance, t);
                        }
                    }).PushJsonArray(autoFlush: true)
                    .ThenCompress().AndWriteStreamAsync(mem).ConfigureAwait(false);
                mem.Seek(0, SeekOrigin.Begin);
                var deserialList = mem.Pull(false).ThenDecompress().AndParseJsonArray<TestObject>().ToList();
                Assert.True(deserialList.Count.Equals(total));
                foreach (var o in deserialList)
                {
                    var lookedUp = dico[o.StrProp];
                    Assert.True(o.IntProp.Equals(lookedUp.IntProp));
                    Assert.True(o.BytesProp.SequenceEqual(lookedUp.BytesProp));
                }

                mem.Seek(0, SeekOrigin.Begin);
                var counter = 0;
                await mem.Pull(false).ThenDecompress().AndParseJsonArrayAsync(new Action<TestObject, CancellationToken>(
                    (o, t) =>
                    {
                        var lookedUp = dico[o.StrProp];
                        Assert.True(o.IntProp.Equals(lookedUp.IntProp));
                        Assert.True(o.BytesProp.SequenceEqual(lookedUp.BytesProp));
                        counter++;
                    })).ConfigureAwait(false);
                Assert.True(counter.Equals(total));

                mem.Seek(0, SeekOrigin.Begin);
                counter = 0;
                await mem.Pull(false).ThenDecompress().AndParseJsonArrayAsync((o, t) =>
                {
                    var lookedUp = dico[o.StrProp];
                    Assert.True(o.IntProp.Equals(lookedUp.IntProp));
                    Assert.True(o.BytesProp.SequenceEqual(lookedUp.BytesProp));
                    counter++;
                }, new IdentityAwaitableAdapter<TestObject>()).ConfigureAwait(false);
                Assert.True(counter.Equals(total));

                mem.Seek(0, SeekOrigin.Begin);
                counter = 0;
                await Task.FromResult((Stream) mem).Pull(false).ThenDecompress().AndParseJsonArrayAsync(
                    new Action<TestObject, CancellationToken>(
                        (o, t) =>
                        {
                            var lookedUp = dico[o.StrProp];
                            Assert.True(o.IntProp.Equals(lookedUp.IntProp));
                            Assert.True(o.BytesProp.SequenceEqual(lookedUp.BytesProp));
                            counter++;
                        })).ConfigureAwait(false);
                Assert.True(counter.Equals(total));

                mem.Seek(0, SeekOrigin.Begin);
                counter = 0;
                await Task.FromResult((Stream) mem).Pull(false).ThenDecompress().AndParseJsonArrayAsync((o, t) =>
                {
                    var lookedUp = dico[o.StrProp];
                    Assert.True(o.IntProp.Equals(lookedUp.IntProp));
                    Assert.True(o.BytesProp.SequenceEqual(lookedUp.BytesProp));
                    counter++;
                }, new IdentityAwaitableAdapter<TestObject>()).ConfigureAwait(false);
                Assert.True(counter.Equals(total));
            }
        }

        [Test]
        public async Task Ppc_Based_Json_Array_Streaming_Errors_Are_Aggregated()
        {
            // Testing Push JsonArray Side Exceptions
            // 1. Due to Stream Writing
            var mem = Substitute.For<Stream>();
            mem.CanWrite.Returns(true);
            mem.When(x => x.Write(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()))
                .Do(x => throw new Exception("Test"));
            mem.WriteAsync(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(x => throw new Exception("Test"));
            Assert.ThrowsAsync<AggregateException>(async () =>
                await new Action<IProducerBuffer<TestObject>, CancellationToken>((b, t) => b.Add(new TestObject(), t))
                    .PushJsonArray(autoFlush: true).AndWriteStreamAsync(mem).ConfigureAwait(false));

            // 2. Due to Producer
            mem = Substitute.For<Stream>();
            mem.CanWrite.Returns(true);
            mem.WriteAsync(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(x => Task.CompletedTask);
            Assert.ThrowsAsync<AggregateException>(async () =>
                await new Action<IProducerBuffer<TestObject>, CancellationToken>((b, t) => throw new Exception("Test"))
                    .PushJsonArray(autoFlush: true).AndWriteStreamAsync(mem).ConfigureAwait(false));

            // Testing Pull JsonArray Side Exceptions
            // 1. Due to Stream Reading
            const int total = 20;
            using (var mem2 = new MemoryStream())
            {
                await new Action<IProducerBuffer<TestObject>, CancellationToken>((b, t) =>
                    {
                        for (var i = 0; i < total; i++)
                        {
                            b.Add(new TestObject(), t);
                        }
                    }).PushJsonArray(autoFlush: true)
                    .ThenCompress().AndWriteStreamAsync(mem2).ConfigureAwait(false);
                mem2.Seek(0, SeekOrigin.Begin);
                Assert.ThrowsAsync<AggregateException>(async () =>
                    await mem2.Pull(false).ThenDecompress().AndParseJsonArrayAsync(
                        new Action<TestObject, CancellationToken>(
                            (o, t) => throw new Exception("Test"))).ConfigureAwait(false));

                mem2.Seek(0, SeekOrigin.Begin);
                Assert.ThrowsAsync<AggregateException>(async () =>
                    await mem2.Pull(false).ThenDecompress().AndParseJsonArrayAsync(
                        new Action<TestObject, CancellationToken>((o, t) => { }),
                        enc: Encoding.UTF32, detectEncodingFromBom: false).ConfigureAwait(false));
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

        private static void AssertOnValueEquality(TestObject obj, TestObject postStream)
        {
            Assert.True(obj.IntProp.Equals(postStream.IntProp));
            Assert.True(obj.StrProp.Equals(postStream.StrProp));
            Assert.True(obj.BytesProp.Length.Equals(postStream.BytesProp.Length));
            for (var i = 0; i < obj.BytesProp.Length; i++)
            {
                Assert.True(obj.BytesProp[i].Equals(postStream.BytesProp[i]));
            }
        }
    }
}