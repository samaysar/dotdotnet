using System;
using System.Globalization;
using System.IO;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.Tests.TestHelpers;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringUnsafeTest
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
            var filename = nameof(StringUnsafeTest);
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

        [Test]
        public void ToEnumUnchecked_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumUnchecked("", out DateTime wrongType, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumUnchecked("", out double wrongType));
            Assert.Throws<ArgumentException>(() => PerformToEnumUnchecked("", out int wrongType));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumUnchecked_Returns_Value_For_Invalid_Parsable_Strings(int invalidInput)
        {
            PerformToEnumUnchecked(invalidInput.ToString(), out DateTimeKind value);
            Assert.True(value.Equals((DateTimeKind) invalidInput));
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
        public void ToEnumUnchecked_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            if (returnValue)
            {
                PerformToEnumUnchecked(input, out DateTimeKind value);
                Assert.True(value.Equals(expected));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => PerformToEnumUnchecked(input, out DateTimeKind value));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{typeof(DateTimeKind)}"));
            }
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
        public void ToEnumUnchecked_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            if (returnValue)
            {
                PerformToEnumUnchecked(input, out DateTimeKind value, false);
                Assert.True(value.Equals(expected));
            }
            else
            {
                var ex =
                    Assert.Throws<DdnDfException>(() => PerformToEnumUnchecked(input, out DateTimeKind value, false));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{typeof(DateTimeKind)}"));
            }
        }

        [Test]
        public void ToEnum_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnum("", out DateTime wrongType, false));
            Assert.Throws<ArgumentException>(() => PerformToEnum("", out double wrongType));
            Assert.Throws<ArgumentException>(() => PerformToEnum("", out int wrongType));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnum_ThrowsError_For_Invalid_Parsable_Strings(int invalidInput)
        {
            var ex = Assert.Throws<DdnDfException>(() => PerformToEnum(invalidInput.ToString(), out DateTimeKind value));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
            Assert.True(ex.Message.Contains($" {invalidInput} "));
            Assert.True(ex.Message.Contains($"{typeof(DateTimeKind)}"));
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
        public void ToEnum_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            if (returnValue)
            {
                PerformToEnum(input, out DateTimeKind value);
                Assert.True(value.Equals(expected));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => PerformToEnum(input, out DateTimeKind value));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{typeof(DateTimeKind)}"));
            }
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
        public void ToEnum_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            if (returnValue)
            {
                PerformToEnum(input, out DateTimeKind value, false);
                Assert.True(value.Equals(expected));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => PerformToEnum(input, out DateTimeKind value, false));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{typeof(DateTimeKind)}"));
            }
        }

        [Test]
        [TestCase("true", true, true)]
        [TestCase("false", false, true)]
        [TestCase("  true   ", true, true)]
        [TestCase(" false  ", false, true)]
        [TestCase("  TRUE   ", true, true)]
        [TestCase(" FALSE  ", false, true)]
        [TestCase("  TRuE   ", true, true)]
        [TestCase(" FAlsE  ", false, true)]
        [TestCase("notbool", default(bool), false)]
        [TestCase("boolean", default(bool), false)]
        [TestCase(" invalid_bool ", default(bool), false)]
        [TestCase("       ", default(bool), false)]
        [TestCase("", default(bool), false)]
        [TestCase(null, default(bool), false)]
        public void ToBool_Works_As_Expected(string input, bool expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToBool() == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToBool());
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Boolean)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(int), false)]
        [TestCase("", NumberStyles.Any, null, default(int), false)]
        [TestCase(null, NumberStyles.Any, null, default(int), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(int), false)]
        [TestCase("a2", NumberStyles.Any, null, default(int), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(int), false)]
        [TestCase("3_", NumberStyles.Any, null, default(int), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(int), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(int), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(int), false)]
        [TestCase("3.0000", NumberStyles.Any, null, 3, true)]
        [TestCase("-2147483648", NumberStyles.Any, null, int.MinValue, true)]
        [TestCase("2147483647", NumberStyles.Any, null, int.MaxValue, true)]
        [TestCase("-2 147 483 648", NumberStyles.Any, "fr-FR", int.MinValue, true)]
        [TestCase("2 147 483 647", NumberStyles.Any, "fr-FR", int.MaxValue, true)]
        public void ToInt_Works_As_Expected(string input, NumberStyles style, string provider,
            int expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToInt(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToInt(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Int32)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(long), false)]
        [TestCase("", NumberStyles.Any, null, default(long), false)]
        [TestCase(null, NumberStyles.Any, null, default(long), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(long), false)]
        [TestCase("a2", NumberStyles.Any, null, default(long), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(long), false)]
        [TestCase("3_", NumberStyles.Any, null, default(long), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(long), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(long), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(long), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (long)3, true)]
        [TestCase("-2147483648", NumberStyles.Any, null, int.MinValue, true)]
        [TestCase("2147483647", NumberStyles.Any, null, int.MaxValue, true)]
        [TestCase("-2 147 483 648", NumberStyles.Any, "fr-FR", int.MinValue, true)]
        [TestCase("2 147 483 647", NumberStyles.Any, "fr-FR", int.MaxValue, true)]
        [TestCase("-9223372036854775808", NumberStyles.Any, null, long.MinValue, true)]
        [TestCase("9223372036854775807", NumberStyles.Any, null, long.MaxValue, true)]
        [TestCase("-9 223 372 036 854 775 808", NumberStyles.Any, "fr-FR", long.MinValue, true)]
        [TestCase("9 223 372 036 854 775 807", NumberStyles.Any, "fr-FR", long.MaxValue, true)]
        public void ToLong_Works_As_Expected(string input, NumberStyles style, string provider,
            long expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToLong(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToLong(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Int64)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(byte), false)]
        [TestCase("", NumberStyles.Any, null, default(byte), false)]
        [TestCase(null, NumberStyles.Any, null, default(byte), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(byte), false)]
        [TestCase("a2", NumberStyles.Any, null, default(byte), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(byte), false)]
        [TestCase("3_", NumberStyles.Any, null, default(byte), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(byte), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(byte), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(byte), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (byte)3, true)]
        [TestCase("0", NumberStyles.Any, null, byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, null, byte.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, "fr-FR", byte.MaxValue, true)]
        public void ToByte_Works_As_Expected(string input, NumberStyles style, string provider,
            byte expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToByte(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToByte(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Byte)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase(null, NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("a2", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("3_", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(sbyte), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(sbyte), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(sbyte), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (sbyte)3, true)]
        [TestCase("-128", NumberStyles.Any, null, sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, null, sbyte.MaxValue, true)]
        [TestCase("-128", NumberStyles.Any, "fr-FR", sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, "fr-FR", sbyte.MaxValue, true)]
        public void ToSByte_Works_As_Expected(string input, NumberStyles style, string provider,
            sbyte expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToSByte(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToSByte(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(SByte)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(short), false)]
        [TestCase("", NumberStyles.Any, null, default(short), false)]
        [TestCase(null, NumberStyles.Any, null, default(short), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(short), false)]
        [TestCase("a2", NumberStyles.Any, null, default(short), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(short), false)]
        [TestCase("3_", NumberStyles.Any, null, default(short), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(short), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(short), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(short), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (short)3, true)]
        [TestCase("-32768", NumberStyles.Any, null, short.MinValue, true)]
        [TestCase("32767", NumberStyles.Any, null, short.MaxValue, true)]
        [TestCase("-32 768", NumberStyles.Any, "fr-FR", short.MinValue, true)]
        [TestCase("32 767", NumberStyles.Any, "fr-FR", short.MaxValue, true)]
        public void ToShort_Works_As_Expected(string input, NumberStyles style, string provider,
           short expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToShort(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToShort(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Int16)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("", NumberStyles.Any, null, default(ushort), false)]
        [TestCase(null, NumberStyles.Any, null, default(ushort), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("a2", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("3_", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(ushort), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(ushort), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(ushort), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (ushort)3, true)]
        [TestCase("0", NumberStyles.Any, null, ushort.MinValue, true)]
        [TestCase("65535", NumberStyles.Any, null, ushort.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ushort.MinValue, true)]
        [TestCase("65 535", NumberStyles.Any, "fr-FR", ushort.MaxValue, true)]
        public void ToUShort_Works_As_Expected(string input, NumberStyles style, string provider,
            ushort expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToUShort(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToUShort(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(UInt16)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(uint), false)]
        [TestCase("", NumberStyles.Any, null, default(uint), false)]
        [TestCase(null, NumberStyles.Any, null, default(uint), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(uint), false)]
        [TestCase("a2", NumberStyles.Any, null, default(uint), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(uint), false)]
        [TestCase("3_", NumberStyles.Any, null, default(uint), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(uint), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(uint), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(uint), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (uint)3, true)]
        [TestCase("0", NumberStyles.Any, null, uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", uint.MaxValue, true)]
        public void ToUInt_Works_As_Expected(string input, NumberStyles style, string provider,
            uint expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToUInt(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToUInt(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(UInt32)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("", NumberStyles.Any, null, default(ulong), false)]
        [TestCase(null, NumberStyles.Any, null, default(ulong), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("a2", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("3_", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("3.0001", NumberStyles.Any, null, default(ulong), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(ulong), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(ulong), false)]
        [TestCase("3.0000", NumberStyles.Any, null, (ulong)3, true)]
        [TestCase("0", NumberStyles.Any, null, uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, null, ulong.MinValue, true)]
        [TestCase("18446744073709551615", NumberStyles.Any, null, ulong.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ulong.MinValue, true)]
        [TestCase("18 446 744 073 709 551 615", NumberStyles.Any, "fr-FR", ulong.MaxValue, true)]
        public void ToULong_Works_As_Expected(string input, NumberStyles style, string provider,
            ulong expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToULong(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            == expected);
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToULong(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(UInt64)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(float), false)]
        [TestCase("", NumberStyles.Any, null, default(float), false)]
        [TestCase(null, NumberStyles.Any, null, default(float), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(float), false)]
        [TestCase("a2", NumberStyles.Any, null, default(float), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(float), false)]
        [TestCase("3_", NumberStyles.Any, null, default(float), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(float), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(float), false)]
        [TestCase("3.0001", NumberStyles.Any, null, (float)3.0001, true)]
        [TestCase("3.0000", NumberStyles.Any, null, (float)3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, (float)-3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, (float)3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", (float)-3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", (float)3.40282, true)]
        public void ToFloat_Works_As_Expected(string input, NumberStyles style, string provider,
            float expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToFloat(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            .Equals(expected));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToFloat(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Single)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, default(double), false)]
        [TestCase("", NumberStyles.Any, null, default(double), false)]
        [TestCase(null, NumberStyles.Any, null, default(double), false)]
        [TestCase("    a3   ", NumberStyles.Any, null, default(double), false)]
        [TestCase("a2", NumberStyles.Any, null, default(double), false)]
        [TestCase("3a123", NumberStyles.Any, null, default(double), false)]
        [TestCase("3_", NumberStyles.Any, null, default(double), false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, default(double), false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, default(double), false)]
        [TestCase("3.0001", NumberStyles.Any, null, 3.0001, true)]
        [TestCase("3.0000", NumberStyles.Any, null, (double)3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, -3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, 3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", -3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", 3.40282, true)]
        public void ToDouble_Works_As_Expected(string input, NumberStyles style, string provider,
            double expected, bool returnVal)
        {
            if (returnVal)
            {
                Assert.True(input.ToDouble(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                            .Equals(expected));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToDouble(style,
                                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(Double)}"));
            }
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null)]
        [TestCase("", NumberStyles.Any, "fr-FR")]
        [TestCase(null, NumberStyles.Any, null)]
        [TestCase("    a3   ", NumberStyles.Any, null)]
        [TestCase("a2", NumberStyles.Any, "fr-FR")]
        [TestCase("3a123", NumberStyles.Any, null)]
        [TestCase("3_", NumberStyles.Any, "fr-FR")]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, "fr-FR")]
        public void ToDecimal_When_Return_Value_Is_False(string input, NumberStyles style,
            string provider)
        {
            var ex = Assert.Throws<DdnDfException>(() => input.ToDecimal(style,
                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
            Assert.True(ex.Message.Contains($" {input} "));
            Assert.True(ex.Message.Contains($"{nameof(Decimal)}"));
        }

        [Test]
        [TestCase("3.0001", NumberStyles.Any, null, 3.0001)]
        [TestCase("3.0000", NumberStyles.Any, "en-US", 3)]
        [TestCase("3.40282", NumberStyles.Any, null, 3.40282)]
        [TestCase("-3.40282", NumberStyles.Any, null, -3.40282)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", 3.40282)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", -3.40282)]
        public void ToDecimal_When_Return_Value_Is_True(string input, NumberStyles style,
            string provider, decimal expected)
        {
            Assert.True(input.ToDecimal(style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider))
                .Equals(expected));
        }

        [Test]
        [TestCase("2001-01-01 00:00:00", "yyyy-MM-dd hh:mm:ss", true)]
        [TestCase("2001-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", true)]
        [TestCase("2001-01-01 25:00:00", "yyyy-MM-dd HH:mm:ss", false)]
        [TestCase("2001-01-01 15:00:00", "yyyy-MM-dd hh:mm:ss", false)]
        [TestCase("01-01-2001 00:00:00", "MM-dd-yyyy hh:mm:ss", true)]
        [TestCase("01-01-2001 00:00:00", "MM-dd-yyyy HH:mm:ss", true)]
        [TestCase("01-01-2001 25:00:00", "MM-dd-yyyy HH:mm:ss", false)]
        [TestCase("01-01-2001 15:00:00", "MM-dd-yyyy hh:mm:ss", false)]
        public void ToDateTime_Works_As_Expected_With_Given_Format(string input, string format,
            bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.ToDateTime(format).Equals(new DateTime(2001, 01, 01, 0, 0, 0)));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToDateTime(format));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(DateTime)}"));
            }
        }

        [Test]
        [TestCase("2001-01-01 00:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd hh:mm:ss", true)]
        [TestCase("2001-01-01 00:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd HH:mm:ss", true)]
        [TestCase("2001-01-01 25:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd HH:mm:ss", false)]
        [TestCase("2001-01-01 15:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd hh:mm:ss", false)]
        [TestCase("01-01-2001 00:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy hh:mm:ss", true)]
        [TestCase("01-01-2001 00:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy HH:mm:ss", true)]
        [TestCase("01-01-2001 25:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy HH:mm:ss", false)]
        [TestCase("01-01-2001 15:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy hh:mm:ss", false)]
        public void ToDateTime_Works_As_Expected_With_Format_Collection(string input,
            string formatBarSeparated, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.ToDateTime(formatBarSeparated.Split('|')).Equals(new DateTime(2001, 01, 01, 0, 0, 0)));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToDateTime(formatBarSeparated.Split('|')));
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(DateTime)}"));
            }
        }

        [Test]
        [TestCase("2001-01-01 00:00:00", true)]
        [TestCase("2001-01-01 00:00:00", true)]
        [TestCase("2001-01-01 25:00:00", false)]
        [TestCase("2001-01-01 15:00:00", true)]
        [TestCase("01-01-2001 00:00:00", true)]
        [TestCase("01-01-2001 60:60:60", false)]
        [TestCase("01-01-2001 25:00:00", false)]
        [TestCase("12-31-2001 23:59:59", true)]
        public void ToDateTime_Works_As_Expected_With_No_Format(string input, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.ToDateTime().Equals(DateTime.Parse(input)));
            }
            else
            {
                var ex = Assert.Throws<DdnDfException>(() => input.ToDateTime());
                Assert.True(ex.ErrorCode == DdnDfErrorCode.StringParsingFailed);
                Assert.True(ex.Message.Contains($" {input} "));
                Assert.True(ex.Message.Contains($"{nameof(DateTime)}"));
            }
        }
        
        private static void PerformToEnumUnchecked<T>(string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            value = input.ToEnumUnchecked<T>(ignoreCase);
        }

        private static void PerformToEnum<T>(string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            value = input.ToEnum<T>(ignoreCase);
        }
    }
}