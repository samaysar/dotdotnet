using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        // we keep internal extensions here
        internal static Func<Stream, bool, CancellationToken, Task> ApplyCompression(
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
                await pipe(s.CreateCryptoStream(ct, CryptoStreamMode.Write, d), true, t).ConfigureAwait(false);
            };
        }

        internal static Func<Stream, bool, CancellationToken, Task> ApplyLoad(
            this Action<int, char[], int, int> loadAction,
            int totalLen, 
            Encoding enc, 
            int bufferSize)
        {
            return async (s, d, t) =>
            {
                try
                {
                    await s.CopyFromAsync(totalLen, enc, t, bufferSize, loadAction).ConfigureAwait(false);
                }
                finally
                {
                    s.DisposeIfRequired(d);
                }
            };
        }
    }
}