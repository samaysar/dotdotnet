using System.IO;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.IO
{
    /// <summary>
    /// Data structure to facilitate Pull based functional streaming,
    /// i.e., 2ndst Pipe reads from 1st, 3rd reads from 2nd and so on and so forth... /// </summary>
    public struct PullFuncStream
    {
        /// <summary>
        /// Readable stream
        /// </summary>
        public Stream Readable { get; }

        /// <summary>
        /// If true, stream is disposed at the end of streaming else left open
        /// </summary>
        public bool Dispose { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="readable">readable stream</param>
        /// <param name="dispose">true to dispose at the end of streaming else false</param>
        /// <exception cref="DdnDfException"></exception>
        public PullFuncStream(Stream readable, bool dispose)
        {
            Readable = readable.CanRead.ThrowIfNot(DdnDfErrorCode.Unspecified, "Cannot read from the stream",
                readable);
            Dispose = dispose;
        }
    }
}