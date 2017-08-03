using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Data Compression related extensions.
    /// </summary>
    public static class CompressionExts
    {
        //private static async Task CompressAsync(this Stream sourceStream, Stream targetStream,
        //    bool gzipCompression = true, CompressionLevel level = CompressionLevel.Optimal,
        //    int bufferSize = StdLookUps.DefaultBufferSize,
        //    bool disposeSource = true, bool disposeTarget = true)
        //{
        //    var compressor = gzipCompression
        //        ? new GZipStream(targetStream, level, !disposeTarget)
        //        : (Stream)new DeflateStream(targetStream, level, !disposeTarget);
        //}
    }
}