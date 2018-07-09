using System.IO;
using System.Threading;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Tests.TestHelpers
{
#pragma warning disable 420
    internal static class FileSys
    {
        private static volatile int _counter;
        private static readonly DirectoryInfo BaseDi;

        static FileSys()
        {
            _counter = 0;
            var currentDi = Directory.GetCurrentDirectory();
            BaseDi = currentDi.ToDirectoryInfo(new[] {"DV" + nameof(FileSys)});
            BaseDi.Refresh();
            if (BaseDi.Exists)
            {
                BaseDi.Delete(true);
            }
            BaseDi.Refresh();
            BaseDi.Create();
            BaseDi.Refresh();
        }

        public static string BaseTestFolder => BaseDi.FullName;

        internal static DirectoryInfo NewTestFolder(bool makeEmpty = true)
        {
            var di = BaseDi.FullName
                .ToDirectoryInfo(new[] {"TH" + Interlocked.Increment(ref _counter)});
            di.Refresh();
            if (di.Exists)
            {
                if (!makeEmpty) return di;
                di.Delete(true);
                di.Create();
                di.Refresh();
            }
            else
            {
                di.Create();
                di.Refresh();
            }
            return di;
        }

        internal static DirectoryInfo TestFolderNonExisting()
        {
            var di = BaseDi.FullName
                .ToDirectoryInfo(new[] { "TH" + Interlocked.Increment(ref _counter) });
            di.Refresh();
            if (di.Exists)
            {
                di.Delete(true);
                di.Refresh();
            }
            return di;
        }
    }
#pragma warning restore 420
}