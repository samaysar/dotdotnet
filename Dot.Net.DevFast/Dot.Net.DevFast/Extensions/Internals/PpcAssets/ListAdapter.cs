using System.Threading;

namespace Dot.Net.DevFast.Extensions.Internals.PpcAssets
{
    internal sealed class ListAdapter<T> : AwaitableListAdapter<T>
    {
        internal ListAdapter(int maxListSize) : base(maxListSize, Timeout.Infinite)
        {
        }
    }
}