using System.Collections.Generic;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;

namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Extensions for PPC patterns.
    /// </summary>
    public static partial class PpcExts
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <typeparam name="TC"></typeparam>
        /// <param name="consumers"></param>
        /// <param name="adapter"></param>
        /// <param name="token"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static IConcurrentPipeline<TP> ConcurrentPipeline<TP, TC>(this IReadOnlyList<IConsumer<TC>> consumers,
            IDataAdapter<TP, TC> adapter, CancellationToken token = default(CancellationToken),
            int bufferSize = ConcurrentBuffer.StandardSize)
        {
            return new ConcurrentPipeline<TP, TC>(consumers, adapter, token, bufferSize);
        }
    }
}