using System;

namespace StreamingSample
{
    public static class Helpers
    {
        public static long GetMemoryPostGc()
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);
            return GC.GetTotalMemory(false);
        }
    }
}