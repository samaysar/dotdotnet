using System.IO;
using Dot.Net.DevFast.Extensions.StringExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.TestHelpers
{
    public static class DevFastFileSys
    {
        private static volatile int _counter;
        private static readonly DirectoryInfo _baseDi;

        static DevFastFileSys()
        {
            _counter = 0;
            var currentDi = Directory.GetCurrentDirectory();
            _baseDi = currentDi.ToDirectoryInfo(new[] {nameof(DevFastFileSys)});
            _baseDi.Refresh();
            if (_baseDi.Exists)
            {
                _baseDi.Delete(true);
            }
            _baseDi.Refresh();
            Assert.False(_baseDi.Exists);
            _baseDi.Create();
            _baseDi.Refresh();
            Assert.True(_baseDi.Exists);
        }
    }
}