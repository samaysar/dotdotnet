using System;
using System.IO;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal static class Others
    {
        internal static void DisposeIfRequired(this IDisposable disposable, bool dispose)
        {
            if (!dispose) return;
            using (disposable)
            {
                //to dispose
            }
        }

        internal static ArraySegment<byte> ThrowIfNoBuffer(this MemoryStream membuffer)
        {
            return membuffer.TryGetBuffer(out ArraySegment<byte> buffer)
                .ThrowIfNot(DdnDfErrorCode.UnableToGetMemoryStreamBuffer,
                    "Please check if buffer is exposable. Unable to get buffer.", buffer);
        }
    }
}