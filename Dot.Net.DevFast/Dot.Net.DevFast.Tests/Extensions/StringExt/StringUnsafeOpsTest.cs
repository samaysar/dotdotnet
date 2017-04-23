using System;
using System.IO;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringUnsafeOpsTest
    {
#pragma warning disable 420

        [Test]
        [TestCase(null)]
        public void UnsafeTrim_Throws_Error_When_Input_Is_Null(string input)
        {
            Assert.True(Assert.Throws<DdnDfException>(() =>
                            input.UnsafeTrim()).ErrorCode == DdnDfErrorCode.NullString);
        }

        [Test]
        [TestCase(null, char.MinValue, char.MaxValue)]
        public void UnsafeTrim_With_TrimChars_Throws_Error_When_Input_Is_Null(string input,
            params char[] trimChars)
        {
            Assert.True(Assert.Throws<DdnDfException>(() =>
                            input.UnsafeTrim(trimChars)).ErrorCode == DdnDfErrorCode.NullString);
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
            Assert.True(input.UnsafeTrim().Equals(expectation));
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
            Assert.True(input.UnsafeTrim(trimChars).Equals(expectation));
        }

        [Test]
        public void ToEnumUnsafe_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumUnsafe("", out DateTime wrongType, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumUnsafe("", out double wrongType));
            Assert.Throws<ArgumentException>(() => PerformToEnumUnsafe("", out int wrongType));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumUnsafe_Returns_True_For_Invalid_Parsable_Strings(int invalidInput)
        {
            Assert.True(PerformToEnumUnsafe(invalidInput.ToString(), out DateTimeKind value) &&
                        value.Equals((DateTimeKind)invalidInput));
        }

        [Test]
        [TestCase("Unspecified", DateTimeKind.Unspecified, true)]
        [TestCase("Local", DateTimeKind.Local, true)]
        [TestCase("Utc", DateTimeKind.Utc, true)]
        [TestCase("unspecified", DateTimeKind.Unspecified, true)]
        [TestCase("local", DateTimeKind.Local, true)]
        [TestCase("utc", DateTimeKind.Utc, true)]
        [TestCase("  unspecified  ", DateTimeKind.Unspecified, true)]
        [TestCase("   local   ", DateTimeKind.Local, true)]
        [TestCase("   utc   ", DateTimeKind.Utc, true)]
        [TestCase("anything", default(DateTimeKind), false)]
        [TestCase("   invalidString   ", default(DateTimeKind), false)]
        [TestCase("", default(DateTimeKind), false)]
        [TestCase(null, default(DateTimeKind), false)]
        public void ToEnumUnsafe_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(PerformToEnumUnsafe(input, out DateTimeKind value) == returnValue);
            Assert.True(value.Equals(expected));
        }

        [Test]
        [TestCase("Unspecified", DateTimeKind.Unspecified, true)]
        [TestCase("Local", DateTimeKind.Local, true)]
        [TestCase("Utc", DateTimeKind.Utc, true)]
        [TestCase("    Unspecified     ", DateTimeKind.Unspecified, true)]
        [TestCase("    Local  ", DateTimeKind.Local, true)]
        [TestCase("  Utc   ", DateTimeKind.Utc, true)]
        [TestCase("unspecified", default(DateTimeKind), false)]
        [TestCase("local", default(DateTimeKind), false)]
        [TestCase("utc", default(DateTimeKind), false)]
        [TestCase("  unspecified  ", default(DateTimeKind), false)]
        [TestCase("   local   ", default(DateTimeKind), false)]
        [TestCase("   utc   ", default(DateTimeKind), false)]
        [TestCase("anything", default(DateTimeKind), false)]
        [TestCase("   invalidString   ", default(DateTimeKind), false)]
        [TestCase("", default(DateTimeKind), false)]
        [TestCase(null, default(DateTimeKind), false)]
        public void ToEnumUnsafe_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(PerformToEnumUnsafe(input, out DateTimeKind value, false) == returnValue);
            Assert.True(value.Equals(expected));
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

            Assert.True(fileInfo1.Extension.SafeTrimOrNull('.').Equals(ext));
            Assert.True(fileInfo2.Extension.SafeTrimOrNull('.').Equals(ext));
            Assert.True(fileInfo3.Extension.SafeTrimOrNull('.').Equals(ext));

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
                    .ErrorCode == DdnDfErrorCode.NullArray);
            }
            else
            {
                Assert.True(Assert.Throws<DdnDfException>(() => string.Empty.ToDirectoryInfo(new string[0]))
                    .ErrorCode == DdnDfErrorCode.EmptyArray);
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

        private static bool PerformToEnumUnsafe<T>(string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumUnsafe(out value, ignoreCase);
        }
#pragma warning restore 420
    }
}