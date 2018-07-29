using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        // we keep internal extensions here
        internal static Func<Stream, bool, CancellationToken, Task> AddCompression(
            this Func<Stream, bool, CancellationToken, Task> pipe, 
            bool gzip,
            CompressionLevel level)
        {
            return async (s, d, t) =>
            {
                using (var compStrm = s.CreateCompressionStream(gzip, level, d))
                {
                    await pipe(compStrm, false, t).ConfigureAwait(false);
                    await compStrm.FlushAsync(t).ConfigureAwait(false);
                    await s.FlushAsync(t).ConfigureAwait(false);
                }
            };
        }

        internal static Func<Stream, bool, CancellationToken, Task> ApplyTransform(
            this Func<Stream, bool, CancellationToken, Task> pipe,
            ICryptoTransform ct)
        {
            return async (s, d, t) =>
            {
                using (var cs = s.CreateCryptoStream(ct, CryptoStreamMode.Write, d))
                {
                    await pipe(cs, false, t).ConfigureAwait(false);
                    cs.FlushFinalBlock();
                }
            };
        }
    }
}