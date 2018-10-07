using System.IO;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.IO
{
    /// <summary>
    /// Data structure to facilitate Push based functional streaming,
    /// i.e., 1st Pipe writes on 2nd that writes on 3rd and so on and so forth...
    /// </summary>
    public struct PushFuncStream
    {
        /// <summary>
        /// Writable stream
        /// </summary>
        public Stream Writable { get; }

        /// <summary>
        /// If true, stream is disposed at the end of streaming else left open
        /// </summary>
        public bool Dispose { get; }

        /// <summary>
        /// Associated Cancellation token
        /// </summary>
        public CancellationToken Token { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="writable">writable stream</param>
        /// <param name="dispose">true to dispose at the end of streaming else false</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <exception cref="DdnDfException"></exception>
        public PushFuncStream(Stream writable, bool dispose, CancellationToken token)
        {
            Writable = writable.CanWrite.ThrowIfNot(DdnDfErrorCode.Unspecified, "Cannot write on the stream",
                writable);
            Dispose = dispose;
            Token = token;
        }
    }
}