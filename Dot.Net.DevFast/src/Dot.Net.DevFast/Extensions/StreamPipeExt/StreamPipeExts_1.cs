using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        // we keep internal extensions here
        internal static Func<Stream, bool, CancellationToken, Task> AddCompression(this IStreamPipe pipe, bool gzip,
            CompressionLevel level)
        {
            return async (s, d, t) =>
            {
                using (var compStrm = s.CreateCompressionStream(gzip, level, d))
                {
                    await pipe.StreamAsync(compStrm, false, t).ConfigureAwait(false);
                    await compStrm.FlushAsync(t).ConfigureAwait(false);
                    await s.FlushAsync(t).ConfigureAwait(false);
                }
            };
        }
    }
}