using System;
using System.IO;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringUnsafeOpsTest
    {
        [Test]
        [TestCase(null)]
        public void UnsafeTrim_Throws_Error_When_Input_Is_Null(string input)
        {
            Assert.True(Assert.Throws<DdnDfException>(() =>
                            input.TrimUnsafe()).ErrorCode == DdnDfErrorCode.NullString);
        }

        [Test]
        [TestCase(null, char.MinValue, char.MaxValue)]
        public void UnsafeTrim_With_TrimChars_Throws_Error_When_Input_Is_Null(string input,
            params char[] trimChars)
        {
            Assert.True(Assert.Throws<DdnDfException>(() =>
                            input.TrimUnsafe(trimChars)).ErrorCode == DdnDfErrorCode.NullString);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("           ", "")]
        [TestCase("   a        ", "a")]
        [TestCase("    A       ", "A")]
        [TestCase("    A     B  ", "A     B")]
        [TestCase("    AB  ", "AB")]
        [TestCase("AB  ", "AB")]
        [TestCase("    AB", "AB")]
        public void UnsafeTrim_Outcomes_Are_Consistent(string input, string expectation)
        {
            Assert.True(input.TrimUnsafe().Equals(expectation));
        }

        [Test]
        [TestCase("", "", ' ', ',')]
        [TestCase("        ,,   ", "", ' ', ',')]
        [TestCase("   ,a     ,   ", "a", ' ', ',')]
        [TestCase("    A   ,    ", "A", ' ', ',')]
        [TestCase("  , ,  A     B ,,, ", "A     B", ' ', ',', '\0')]
        [TestCase("    A,B  ", "A,B", ' ', ',')]
        [TestCase("AB  ", "AB", ' ', char.MaxValue)]
        [TestCase("    AB", "AB", ' ', char.MinValue)]
        public void UnsafeTrim_With_TrimChars_Gives_Consistent_Outcomes(string input, string expectation,
            params char[] trimChars)
        {
            Assert.True(input.TrimUnsafe(trimChars).Equals(expectation));
        }

        [Test]
        public void ToFileInfo_Works_As_Expected()
        {
            var filename = nameof(StringUnsafeOpsTest);
            var folder = FileSys.TestFolderNonExisting().FullName;
            const string ext = "json";

            var fileInfo1 = folder.ToFileInfo(filename, ext);
            var fileInfo2 = folder.ToFileInfo(filename + "." + ext);
            var fileInfo3 = Path.Combine(folder, filename + "." + ext).ToFileInfo();

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

            Assert.True(fileInfo1.Directory.FullName.Equals(folder));
            Assert.True(fileInfo2.Directory.FullName.Equals(folder));
            Assert.True(fileInfo3.Directory.FullName.Equals(folder));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ToDirectoryInfo_Throws_Error_When_SubPath_Array_Is_NullOrEmpty(bool useNull)
        {
            if (useNull)
            {
                Assert.True(Assert.Throws<DdnDfException>(() => string.Empty.ToDirectoryInfo(null))
                                .ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            }
            else
            {
                Assert.True(Assert.Throws<DdnDfException>(() => string.Empty.ToDirectoryInfo(new string[0]))
                                .ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            }
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ToDirectoryInfo_With_Given_SubPath_Works_As_Expected(bool create)
        {
            var directory = FileSys.TestFolderNonExisting();
            var withSubPathDi = directory.FullName.ToDirectoryInfo(new[] {"SubPathTest1"}, create);
            directory.Refresh();

            Assert.True(withSubPathDi.Exists == create);
            Assert.True(directory.Exists == create);
            Assert.True(withSubPathDi.FullName.Equals(Path.Combine(directory.FullName, "SubPathTest1")));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ToDirectoryInfo_With_FullPath_Works_As_Expected(bool create)
        {
            var directory = FileSys.TestFolderNonExisting();
            var withSubPathDi = directory.FullName.ToDirectoryInfo(create);
            directory.Refresh();

            Assert.True(withSubPathDi.Exists == create);
            Assert.True(directory.Exists == create);
            Assert.True(withSubPathDi.FullName.Equals(directory.FullName));
        }
    }
}