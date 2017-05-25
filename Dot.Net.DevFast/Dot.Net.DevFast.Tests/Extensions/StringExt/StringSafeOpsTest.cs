using System;
using System.Globalization;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.StringExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringSafeOpsTest
    {
        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("           ", "")]
        [TestCase("   a        ", "a")]
        [TestCase("    A       ", "A")]
        [TestCase("    A     B  ", "A     B")]
        [TestCase("    AB  ", "AB")]
        [TestCase("AB  ", "AB")]
        [TestCase("    AB", "AB")]
        public void SafeTrimOrEmpty_Outcomes_Are_Consistent(string input, string expectation)
        {
            Assert.True(input.TrimSafeOrEmpty().Equals(expectation));
        }

        [Test]
        [TestCase(null, "", ' ', ',')]
        [TestCase("", "", ' ', ',')]
        [TestCase("        ,,   ", "", ' ', ',')]
        [TestCase("   ,a     ,   ", "a", ' ', ',')]
        [TestCase("    A   ,    ", "A", ' ', ',')]
        [TestCase("  , ,  A     B ,,, ", "A     B", ' ', ',', '\0')]
        [TestCase("    A,B  ", "A,B", ' ', ',')]
        [TestCase("AB  ", "AB", ' ', char.MaxValue)]
        [TestCase("    AB", "AB", ' ', char.MinValue)]
        public void SafeTrimOrEmpty_With_TrimChars_Gives_Consistent_Outcomes(string input, string expectation,
            params char[] trimChars)
        {
            Assert.True(input.TrimSafeOrEmpty(trimChars).Equals(expectation));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("           ", "")]
        [TestCase("   a        ", "a")]
        [TestCase("    A       ", "A")]
        [TestCase("    A     B  ", "A     B")]
        [TestCase("    AB  ", "AB")]
        [TestCase("AB  ", "AB")]
        [TestCase("    AB", "AB")]
        public void SafeTrimOrNull_Outcomes_Are_Consistent(string input, string expectation)
        {
            Assert.True(input.TrimSafeOrNull() == expectation);
        }

        [Test]
        [TestCase(null, null, ' ', ',')]
        [TestCase("", "", ' ', ',')]
        [TestCase("        ,,   ", "", ' ', ',')]
        [TestCase("   ,a     ,   ", "a", ' ', ',')]
        [TestCase("    A   ,    ", "A", ' ', ',')]
        [TestCase("  , ,  A     B ,,, ", "A     B", ' ', ',', '\0')]
        [TestCase("    A,B  ", "A,B", ' ', ',')]
        [TestCase("AB  ", "AB", ' ', char.MaxValue)]
        [TestCase("    AB", "AB", ' ', char.MinValue)]
        public void SafeTrimOrNull_With_TrimChars_Gives_Consistent_Outcomes(string input, string expectation,
            params char[] trimChars)
        {
            Assert.True(input.TrimSafeOrNull(trimChars) == expectation);
        }

        [Test]
        [TestCase(null, null, null)]
        [TestCase("", "", "")]
        [TestCase("           ", "", "aaaa")]
        [TestCase("   a        ", "a", "aaaa")]
        [TestCase("    A       ", "A", "aaaa")]
        [TestCase("    A     B  ", "A     B", null)]
        [TestCase("    AB  ", "AB", "QA")]
        [TestCase("AB  ", "AB", "        ")]
        [TestCase("    AB", "AB", "  A ")]
        public void SafeTrimOrDefault_Outcomes_Are_Consistent(string input, string expectation, string defaultVal)
        {
            Assert.True(input.TrimSafeOrDefault(defaultVal) == expectation);
        }

        [Test]
        [TestCase(null, "A", "A", ' ', ',')]
        [TestCase("", "", " a w ", ' ', ',')]
        [TestCase("        ,,   ", "", "aFw", ' ', ',')]
        [TestCase("   ,a     ,   ", "a", null, ' ', ',')]
        [TestCase("    A   ,    ", "A", null, ' ', ',')]
        [TestCase("  , ,  A     B ,,, ", "A     B", null, ' ', ',', '\0')]
        [TestCase("    A,B  ", "A,B", null, ' ', ',')]
        [TestCase("AB  ", "AB", null, ' ', char.MaxValue)]
        [TestCase("    AB", "AB", null, ' ', char.MinValue)]
        public void SafeTrimOrDefault_With_TrimChars_Gives_Consistent_Outcomes(string input, string expectation,
            string defaultVal, params char[] trimChars)
        {
            Assert.True(input.TrimSafeOrDefault(defaultVal, trimChars) == expectation);
        }

        [Test]
        public void ToEnumUncheckedOrDefault_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumUncheckedOrDefault("", DateTime.MinValue, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumUncheckedOrDefault("", double.MaxValue));
            Assert.Throws<ArgumentException>(() => PerformToEnumUncheckedOrDefault("", 0));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumUncheckedOrDefault_Returns_Invalid_Value_For_Invalid_Parsable_Strings(int invalidInput)
        {
            Assert.True(
                PerformToEnumUncheckedOrDefault(invalidInput.ToString(), DateTimeKind.Unspecified)
                    .Equals((DateTimeKind)invalidInput));
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
        public void ToEnumUncheckedOrDefault_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(returnValue
                ? PerformToEnumUncheckedOrDefault(input, DateTimeKind.Unspecified).Equals(expected)
                : PerformToEnumUncheckedOrDefault(input, DateTimeKind.Unspecified).Equals(DateTimeKind.Unspecified));
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
        public void ToEnumUncheckedOrDefault_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(returnValue
                ? PerformToEnumUncheckedOrDefault(input, DateTimeKind.Unspecified, false).Equals(expected)
                : PerformToEnumUncheckedOrDefault(input, DateTimeKind.Unspecified, false)
                    .Equals(DateTimeKind.Unspecified));
        }

        [Test]
        public void ToEnumOrDefault_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumOrDefault("", DateTime.MinValue, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumOrDefault("", double.MinValue));
            Assert.Throws<ArgumentException>(() => PerformToEnumOrDefault("", int.MinValue));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumOrDefault_Returns_Default_Value_For_Invalid_Parsable_Strings(int invalidInput)
        {
            Assert.True(
                PerformToEnumOrDefault(invalidInput.ToString(), DateTimeKind.Unspecified)
                    .Equals(DateTimeKind.Unspecified));
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
        public void ToEnumOrDefault_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(returnValue
                ? PerformToEnumOrDefault(input, DateTimeKind.Unspecified).Equals(expected)
                : PerformToEnumOrDefault(input, DateTimeKind.Unspecified).Equals(DateTimeKind.Unspecified));
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
        public void ToEnumOrDefault_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(returnValue
                ? PerformToEnumOrDefault(input, DateTimeKind.Unspecified, false).Equals(expected)
                : PerformToEnumOrDefault(input, DateTimeKind.Unspecified, false).Equals(DateTimeKind.Unspecified));
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
        public void ToOrDefault_For_Bool_Works_As_Expected(string input, bool expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(true) == (!returnVal || expected));
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
        public void ToOrDefault_For_Int_Works_As_Expected(string input, NumberStyles style, string provider,
            int expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(int.MaxValue, style,
                            string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                        (returnVal ? expected : int.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (long) 3, true)]
        [TestCase("-2147483648", NumberStyles.Any, null, int.MinValue, true)]
        [TestCase("2147483647", NumberStyles.Any, null, int.MaxValue, true)]
        [TestCase("-2 147 483 648", NumberStyles.Any, "fr-FR", int.MinValue, true)]
        [TestCase("2 147 483 647", NumberStyles.Any, "fr-FR", int.MaxValue, true)]
        [TestCase("-9223372036854775808", NumberStyles.Any, null, long.MinValue, true)]
        [TestCase("9223372036854775807", NumberStyles.Any, null, long.MaxValue, true)]
        [TestCase("-9 223 372 036 854 775 808", NumberStyles.Any, "fr-FR", long.MinValue, true)]
        [TestCase("9 223 372 036 854 775 807", NumberStyles.Any, "fr-FR", long.MaxValue, true)]
        public void ToOrDefault_For_Long_Works_As_Expected(string input, NumberStyles style, string provider,
            long expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(long.MaxValue, style,
                            string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                        (returnVal ? expected : long.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (byte) 3, true)]
        [TestCase("0", NumberStyles.Any, null, byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, null, byte.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, "fr-FR", byte.MaxValue, true)]
        public void ToOrDefault_For_Byte_Works_As_Expected(string input, NumberStyles style, string provider,
            byte expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(byte.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : byte.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (sbyte) 3, true)]
        [TestCase("-128", NumberStyles.Any, null, sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, null, sbyte.MaxValue, true)]
        [TestCase("-128", NumberStyles.Any, "fr-FR", sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, "fr-FR", sbyte.MaxValue, true)]
        public void ToOrDefault_For_SByte_Works_As_Expected(string input, NumberStyles style, string provider,
            sbyte expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(sbyte.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : sbyte.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (short) 3, true)]
        [TestCase("-32768", NumberStyles.Any, null, short.MinValue, true)]
        [TestCase("32767", NumberStyles.Any, null, short.MaxValue, true)]
        [TestCase("-32 768", NumberStyles.Any, "fr-FR", short.MinValue, true)]
        [TestCase("32 767", NumberStyles.Any, "fr-FR", short.MaxValue, true)]
        public void ToOrDefault_For_Short_Works_As_Expected(string input, NumberStyles style, string provider,
            short expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(short.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : short.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (ushort) 3, true)]
        [TestCase("0", NumberStyles.Any, null, ushort.MinValue, true)]
        [TestCase("65535", NumberStyles.Any, null, ushort.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ushort.MinValue, true)]
        [TestCase("65 535", NumberStyles.Any, "fr-FR", ushort.MaxValue, true)]
        public void ToOrDefault_For_UShort_Works_As_Expected(string input, NumberStyles style, string provider,
            ushort expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(ushort.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : ushort.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (uint) 3, true)]
        [TestCase("0", NumberStyles.Any, null, uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", uint.MaxValue, true)]
        public void ToOrDefault_For_UInt_Works_As_Expected(string input, NumberStyles style, string provider,
            uint expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(uint.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : uint.MaxValue));
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
        [TestCase("3.0000", NumberStyles.Any, null, (ulong) 3, true)]
        [TestCase("0", NumberStyles.Any, null, uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, null, ulong.MinValue, true)]
        [TestCase("18446744073709551615", NumberStyles.Any, null, ulong.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ulong.MinValue, true)]
        [TestCase("18 446 744 073 709 551 615", NumberStyles.Any, "fr-FR", ulong.MaxValue, true)]
        public void ToOrDefault_For_ULong_Works_As_Expected(string input, NumberStyles style, string provider,
            ulong expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(ulong.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) ==
                         (returnVal ? expected : ulong.MaxValue));
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
        [TestCase("3.0001", NumberStyles.Any, null, (float) 3.0001, true)]
        [TestCase("3.0000", NumberStyles.Any, null, (float) 3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, (float) -3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, (float) 3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", (float) -3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", (float) 3.40282, true)]
        public void ToOrDefault_For_Float_Works_As_Expected(string input, NumberStyles style, string provider,
            float expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(float.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)).Equals(
                         (returnVal ? expected : float.MaxValue)));
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
        [TestCase("3.0000", NumberStyles.Any, null, (double) 3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, -3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, 3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", -3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", 3.40282, true)]
        public void ToOrDefault_For_Double_Works_As_Expected(string input, NumberStyles style, string provider,
            double expected, bool returnVal)
        {
            Assert.True(input.ToOrDefault(double.MaxValue, style,
                             string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)).Equals(
                         (returnVal ? expected : double.MaxValue)));
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
        public void ToOrDefault_For_Decimal_When_Return_Value_Is_False(string input, NumberStyles style,
            string provider)
        {
            Assert.True(input.ToOrDefault(decimal.MinValue, style,
                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)).Equals(
                decimal.MinValue));
        }

        [Test]
        [TestCase("3.0001", NumberStyles.Any, null, 3.0001)]
        [TestCase("3.0000", NumberStyles.Any, "en-US", 3)]
        [TestCase("3.40282", NumberStyles.Any, null, 3.40282)]
        [TestCase("-3.40282", NumberStyles.Any, null, -3.40282)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", 3.40282)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", -3.40282)]
        public void ToOrDefault_For_Decimal_When_Return_Value_Is_True(string input, NumberStyles style,
            string provider, decimal expected)
        {
            Assert.True(input.ToOrDefault(decimal.MinValue, style,
                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)).Equals(
                expected));
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
        public void ToOrDefault_For_DateTime_Works_As_Expected_With_Given_Format(string input, string format,
            bool expectedReturn)
        {
            Assert.True(input.ToOrDefault(DateTime.MaxValue, format).Equals(
                         (expectedReturn ? new DateTime(2001, 01, 01, 0, 0, 0) : DateTime.MaxValue)));
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
        public void ToOrDefault_For_DateTime_Works_As_Expected_With_Format_Collection(string input,
            string formatBarSeparated, bool expectedReturn)
        {
            Assert.True(input.ToOrDefault(DateTime.MaxValue, formatBarSeparated.Split('|')).Equals(
                (expectedReturn ? new DateTime(2001, 01, 01, 0, 0, 0) : DateTime.MaxValue)));
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
        public void ToOrDefault_For_DateTime_Works_As_Expected_With_No_Format(string input, bool expectedReturn)
        {
            Assert.True(input.ToOrDefault(DateTime.MaxValue).Equals(
                (expectedReturn ? DateTime.Parse(input) : DateTime.MaxValue)));
        }

        private static T PerformToEnumUncheckedOrDefault<T>(string input, T defValue, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumUncheckedOrDefault<T>(defValue, ignoreCase);
        }

        private static T PerformToEnumOrDefault<T>(string input, T defValue, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumOrDefault<T>(defValue, ignoreCase);
        }
    }
}