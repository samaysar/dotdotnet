using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.Ppc;
using Dot.Net.DevFast.IO;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        // we keep internal extensions here
        internal static Func<PushFuncStream, Task> ApplyByteCount(this Func<PushFuncStream, Task> pipe,
            ByteCountStream bcs)
        {
            return async pfs =>
            {
                bcs.ResetWith(pfs.Writable, pfs.Dispose);
                using (bcs)
                {
                    var t = pfs.Token;
                    await pipe(new PushFuncStream(bcs, false, t)).StartIfNeeded()
                        .ConfigureAwait(false);
                    await bcs.FlushAsync(t).ConfigureAwait(false);
                }
            };
        }

        // we keep internal extensions here
        internal static Func<PushFuncStream, Task> ApplyConcurrentStream(this Func<PushFuncStream, Task> pipe,
            Stream stream,
            bool disposeStream,
            Action<Stream, Exception> errorHandler)
        {
            return async pfs =>
            {
                using (var concurrentStream = new BroadcastStream(pfs, stream, disposeStream, errorHandler))
                {
                    var t = pfs.Token;
                    await pipe(new PushFuncStream(concurrentStream, false, t)).StartIfNeeded()
                        .ConfigureAwait(false);
                    await concurrentStream.FlushAsync(t).ConfigureAwait(false);
                }
            };
        }

        internal static Func<PushFuncStream, Task> ApplyCompression(this Func<PushFuncStream, Task> pipe,
            bool gzip,
            CompressionLevel level)
        {
            return async pfs =>
            {
                var s = pfs.Writable;
                var t = pfs.Token;
                using (var compStrm = s.CreateCompressionStream(gzip, level, pfs.Dispose))
                {
                    await pipe(new PushFuncStream(compStrm, false, t)).StartIfNeeded().ConfigureAwait(false);
                    await compStrm.FlushAsync(t).ConfigureAwait(false);
                    await s.FlushAsync(t).ConfigureAwait(false);
                }
            };
        }

        internal static Func<PushFuncStream, Task> ApplyCrypto<T>(this Func<PushFuncStream, Task> pipe,
            T encAlg, bool encrypt)
            where T : SymmetricAlgorithm
        {
            return async pfs =>
            {
                using (encAlg)
                {
                    var cryptor = encrypt
                        ? encAlg.CreateEncryptor(encAlg.Key, encAlg.IV)
                        : encAlg.CreateDecryptor(encAlg.Key, encAlg.IV);
                    using (cryptor)
                    {
                        await pipe.ApplyTransform(cryptor)(pfs).ConfigureAwait(false);
                    }
                }
            };
        }

        internal static Func<PushFuncStream, Task> ApplyTransform(this Func<PushFuncStream, Task> pipe,
            ICryptoTransform ct)
        {
            return async pfs =>
            {
                await pipe(new PushFuncStream(
                        pfs.Writable.CreateCryptoStream(ct, CryptoStreamMode.Write, pfs.Dispose), true, pfs.Token))
                    .StartIfNeeded().ConfigureAwait(false);
            };
        }

        internal static Func<PushFuncStream, Task> ApplyLoad(this Action<int, char[], int, int> loadAction,
            int totalLen,
            Encoding enc,
            int bufferSize)
        {
            return async pfs =>
            {
                var s = pfs.Writable;
                try
                {
                    await s.CopyFromAsync(totalLen, enc, pfs.Token, bufferSize, loadAction).ConfigureAwait(false);
                }
                finally
                {
                    s.DisposeIfRequired(pfs.Dispose);
                }
            };
        }

        internal static Func<PushFuncStream, Task> PushJsonEnumeration<T>(this IEnumerable<T> obj,
            JsonSerializer serializer, Encoding enc, int writerBuffer, bool autoFlush)
        {
            return new Action<PushFuncStream>(pfs => obj.ToJsonArray(pfs.Writable, serializer, pfs.Token,
                enc, writerBuffer, pfs.Dispose, autoFlush)).ToAsync(false);
        }

        internal static Func<PushFuncStream, Task> PushJsonEnumeration<T>(this BlockingCollection<T> obj,
            JsonSerializer serializer, Encoding enc, int writerBuffer,
            CancellationTokenSource pcts, bool autoFlush)
        {
            return JsonEnumeration(obj, serializer, enc, writerBuffer, pcts, autoFlush).ToAsync(false);
        }

        internal static async Task PushPpcJsonEnumeration<T>(this BlockingCollection<T> bc,
            JsonSerializer serializer, Encoding enc, int writerBuffer,
            CancellationTokenSource pcts, bool autoFlush,
            CancellationToken token, PushFuncStream pfs, IProducer<T> producer,
            int ppcBufferSize)
        {
            var errList = new List<Exception>();
            var serialConsumerTask = Task.Run(
                () => JsonEnumeration(bc, serializer, enc, writerBuffer, pcts, autoFlush)(pfs), token);

            await PpcJsonEnumerationProducer(bc, producer, token, ppcBufferSize, errList, pcts)
                .ConfigureAwait(false);
            try
            {
                await serialConsumerTask.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                errList.Add(e);
            }

            if (errList.Count > 0) throw new AggregateException("Error during JSON PUSH streaming.", errList);
        }

        #region PullFuncStream TASK

        internal static Func<Task<PullFuncStream>> ApplyDecompression(this Func<Task<PullFuncStream>> pipe,
            bool gzip)
        {
            return async () =>
            {
                var data = await pipe().StartIfNeeded().ConfigureAwait(false);
                return data.ApplyDecompression(gzip);
            };
        }

        internal static Func<Task<PullFuncStream>> ApplyCrypto<T>(this Func<Task<PullFuncStream>> pipe,
            T encAlg, bool encrypt)
            where T : SymmetricAlgorithm
        {
            return async () =>
            {
                var data = await pipe().StartIfNeeded().ConfigureAwait(false);
                using (encAlg)
                {
                    var cryptor = encrypt
                        ? encAlg.CreateEncryptor(encAlg.Key, encAlg.IV)
                        : encAlg.CreateDecryptor(encAlg.Key, encAlg.IV);
                    return data.ApplyTransform(cryptor);
                }
            };
        }

        internal static Func<Task<PullFuncStream>> ApplyTransform(this Func<Task<PullFuncStream>> pipe,
            ICryptoTransform ct)
        {
            return async () =>
            {
                var data = await pipe().StartIfNeeded().ConfigureAwait(false);
                return data.ApplyTransform(ct);
            };
        }

        internal static Func<Task<PullFuncStream>> ApplyByteCount(this Func<Task<PullFuncStream>> pipe,
            ByteCountStream bcs)
        {
            return async () =>
            {
                var data = await pipe().StartIfNeeded().ConfigureAwait(false);
                return data.ApplyByteCount(bcs);
            };
        }

        #endregion PullFuncStream TASK

        #region PullFuncStream NoTASK

        internal static Func<PullFuncStream> ApplyDecompression(this Func<PullFuncStream> pipe,
            bool gzip)
        {
            return () => pipe().ApplyDecompression(gzip);
        }

        internal static Func<PullFuncStream> ApplyCrypto<T>(this Func<PullFuncStream> pipe,
            T encAlg, bool encrypt)
            where T : SymmetricAlgorithm
        {
            return () =>
            {
                using (encAlg)
                {
                    var cryptor = encrypt
                        ? encAlg.CreateEncryptor(encAlg.Key, encAlg.IV)
                        : encAlg.CreateDecryptor(encAlg.Key, encAlg.IV);
                    return pipe().ApplyTransform(cryptor);
                }
            };
        }

        internal static Func<PullFuncStream> ApplyTransform(this Func<PullFuncStream> pipe,
            ICryptoTransform ct)
        {
            return () => pipe().ApplyTransform(ct);
        }

        internal static Func<PullFuncStream> ApplyByteCount(this Func<PullFuncStream> pipe,
            ByteCountStream bcs)
        {
            return () => pipe().ApplyByteCount(bcs);
        }

        #endregion PullFuncStream NoTASK

        #region PullFuncStream PRIVATE

        private static PullFuncStream ApplyDecompression(this PullFuncStream data, bool gzip)
        {
            return new PullFuncStream(data.Readable.CreateDecompressionStream(gzip, data.Dispose), true);
        }

        private static PullFuncStream ApplyTransform(this PullFuncStream data, ICryptoTransform ct)
        {
            return new PullFuncStream(data.Readable.CreateCryptoStream(ct, CryptoStreamMode.Read, data.Dispose), true);
        }

        internal static PullFuncStream ApplyByteCount(this PullFuncStream data, ByteCountStream bcs)
        {
            bcs.ResetWith(data.Readable, data.Dispose);
            return new PullFuncStream(bcs, true);
        }

        internal static async Task ApplyPpcParseJsonArray<TJ, TC>(this PullFuncStream data,
            IConsumer<TC> consumer, 
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer,
            CancellationToken token,
            Encoding enc,
            bool detectEncodingFromBom,
            int bufferSize, 
            int ppcBuffSize)
        {
            using (var bc = new BlockingCollection<TJ>())
            {
                using (var localCts = new CancellationTokenSource())
                {
                    using (var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(token, localCts.Token))
                    {
                        await data.ApplyPpcParseJsonArray(consumer, adapter, serializer, combinedCts.Token, enc,
                            detectEncodingFromBom, bufferSize, bc, localCts, ppcBuffSize).ConfigureAwait(false);
                    }
                }
            }
        }

        internal static async Task ApplyPpcParseJsonArray<TJ, TC>(this PullFuncStream data,
            IConsumer<TC> consumer,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer,
            CancellationToken token,
            Encoding enc,
            bool detectEncodingFromBom,
            int bufferSize,
            BlockingCollection<TJ> bc,
            CancellationTokenSource localCts,
            int ppcBuffSize)
        {
            var errList = new List<Exception>();
            var ppcProducerTask = GetParseJsonArrayProducerTask(data, serializer, token, enc, detectEncodingFromBom,
                bufferSize, bc, localCts);

            await RunPpcJsonArray(bc, localCts, consumer, adapter, token, ppcBuffSize, errList)
                .ConfigureAwait(false);
            try
            {
                await ppcProducerTask.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                errList.Add(e);
            }

            if (errList.Count > 0) throw new AggregateException("Error during JSON PULL streaming.", errList);
        }

        internal static async Task RunPpcJsonArray<TJ, TC>(BlockingCollection<TJ> bc,
            CancellationTokenSource localCts,
            IConsumer<TC> consumer,
            IDataAdapter<TJ, TC> adapter,
            CancellationToken token,
            int ppcBuffSize,
            List<Exception> errList)
        {
            try
            {
                await new Action<IProducerBuffer<TJ>, CancellationToken>((pb, tkn) =>
                {
                    tkn.ThrowIfCancellationRequested();
                    while (bc.TryTake(out var outObj, Timeout.Infinite, tkn))
                    {
                        pb.Add(outObj, tkn);
                    }
                }).Pipe(consumer, adapter, token, ppcBuffSize).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                errList.Add(e);
                if (!token.IsCancellationRequested) localCts.Cancel();
            }
        }

        internal static Task GetParseJsonArrayProducerTask<TJ>(PullFuncStream data,
            JsonSerializer serializer,
            CancellationToken token,
            Encoding enc,
            bool detectEncodingFromBom,
            int bufferSize,
            BlockingCollection<TJ> bc,
            CancellationTokenSource localCts)
        {
            return Task.Run(() => data.Readable.FromJsonArrayParallely(bc, serializer, token,
                localCts, enc, bufferSize, data.Dispose, true, true, detectEncodingFromBom), CancellationToken.None);
        }

        #endregion PullFuncStream PRIVATE

        internal static T InitKeyNIv<T>(this T alg, string password, string salt,
#if NET472
            HashAlgorithmName hashName,
#endif
            int loopCnt,
            Encoding enc)
            where T : SymmetricAlgorithm
        {
            var keyIv = password.CreateKeyAndIv(salt,
#if NET472
                hashName,
#endif
                alg.KeySize / 8, alg.BlockSize / 8, loopCnt, enc);
            alg.Key = keyIv.Item1;
            alg.IV = keyIv.Item2;
            return alg;
        }

        internal static Action<PushFuncStream> JsonEnumeration<T>(BlockingCollection<T> obj,
            JsonSerializer serializer, Encoding enc, int writerBuffer,
            CancellationTokenSource pcts, bool autoFlush)
        {
            return pfs => obj.ToJsonArrayParallely(pfs.Writable, serializer,
                pfs.Token, pcts, enc, writerBuffer, pfs.Dispose, autoFlush);
        }

        internal static async Task PpcJsonEnumerationProducer<T>(BlockingCollection<T> obj, IProducer<T> producer,
            CancellationToken token, int ppcBufferSize, List<Exception> errList, CancellationTokenSource pcts)
        {
            try
            {
                await producer.Pipe((ins, tkn) => obj.Add(ins, tkn), token, ppcBufferSize)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                errList.Add(e);
                if (!token.IsCancellationRequested) pcts.Cancel();
            }
            finally
            {
                obj.CompleteAdding();
            }
        }
    }
}