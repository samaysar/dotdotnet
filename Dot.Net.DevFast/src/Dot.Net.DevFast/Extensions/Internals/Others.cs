﻿using System;
using System.IO;
#if !NETFRAMEWORK
using System.Threading.Tasks;
#endif
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

#if !NETFRAMEWORK && !NETSTANDARD2_0
        internal static async ValueTask DisposeIfRequiredAsync(this IAsyncDisposable disposable, bool dispose)
        {
            if (dispose)
            {
                await using (disposable.ConfigureAwait(false))
                {
                    //to dispose
                }
            }
        }
#endif

        internal static ArraySegment<byte> ThrowIfNoBuffer(this MemoryStream membuffer)
        {
            return membuffer.TryGetBuffer(out var buffer)
                .ThrowIfNot(DdnDfErrorCode.UnableToGetMemoryStreamBuffer,
                    "Please check if buffer is exposable. Unable to get buffer.", buffer);
        }
    }
}