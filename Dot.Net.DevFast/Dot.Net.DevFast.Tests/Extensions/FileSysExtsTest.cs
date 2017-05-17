using System.IO;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class FileSysExtsTest
    {
        [Test]
        public void CreateFileInfo_Extensions_Work_As_Expected()
        {
            const string filename = nameof(FileSysExtsTest);
            var folder = FileSys.TestFolderNonExisting();
            const string ext = "json";

            var fileInfo1 = folder.CreateFileInfo(filename, ext);
            var fileInfo2 = folder.CreateFileInfo(filename + "." + ext);
            var fileInfo3 = Path.Combine(folder.FullName, filename + "." + ext).ToFileInfo();

            Assert.False(fileInfo1.Exists);
            Assert.False(fileInfo2.Exists);
            Assert.False(fileInfo3.Exists);

            Assert.True(fileInfo1.FullName.Equals(fileInfo2.FullName));
            Assert.True(fileInfo1.FullName.Equals(fileInfo3.FullName));

            Assert.True(fileInfo1.Extension.TrimSafeOrNull('.').Equals(ext));
            Assert.True(fileInfo2.Extension.TrimSafeOrNull('.').Equals(ext));
            Assert.True(fileInfo3.Extension.TrimSafeOrNull('.').Equals(ext));

            Assert.NotNull(fileInfo1.Directory);
            Assert.NotNull(fileInfo2.Directory);
            Assert.NotNull(fileInfo3.Directory);

            Assert.True(fileInfo1.Directory.FullName.Equals(folder.FullName));
            Assert.True(fileInfo2.Directory.FullName.Equals(folder.FullName));
            Assert.True(fileInfo3.Directory.FullName.Equals(folder.FullName));
        }
    }
}