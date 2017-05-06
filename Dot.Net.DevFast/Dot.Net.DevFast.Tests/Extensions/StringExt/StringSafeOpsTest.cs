using System;
using Dot.Net.DevFast.Extensions.StringExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringSafeOpsTest
    {
        [Test]
        public void ToEnumSafe_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("", out DateTime wrongType, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("", out double wrongType));
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("", out int wrongType));
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumSafe_Returns_False_For_Invalid_Parsable_Strings(int invalidInput)
        {
            Assert.False(PerformToEnumSafe(invalidInput.ToString(), out DateTimeKind value));
            Assert.True(value.Equals((DateTimeKind)invalidInput));
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
        public void ToEnumSafe_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(PerformToEnumSafe(input, out DateTimeKind value) == returnValue);
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
        public void ToEnumSafe_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind expected, bool returnValue)
        {
            Assert.True(PerformToEnumSafe(input, out DateTimeKind value, false) == returnValue);
            Assert.True(value.Equals(expected));
        }

        [Test]
        public void ToEnumSafe_For_NullableEnum_ThrowsError_For_Invalid_Cases()
        {
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("anything", out DateTime? wrongType, false));
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("anything", out double? wrongType));
            Assert.Throws<ArgumentException>(() => PerformToEnumSafe("anything", out int? wrongType));
        }

        [Test]
        public void ToEnumSafe_For_NullableEnum_Always_Returns_True_With_Null_OutValue_When_String_IsNoWS()
        {
            Assert.True(PerformToEnumSafe("", out DateTime? wrongDt, false) && wrongDt == null);
            Assert.True(PerformToEnumSafe("", out double? wrongDob) && wrongDob == null);
            Assert.True(PerformToEnumSafe("", out int? wrongInt) && wrongInt == null);
            Assert.True(PerformToEnumSafe("", out DateTimeKind? goodEnumType) && goodEnumType == null);
        }

        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void ToEnumSafe_For_NullableEnum_Returns_False_For_Invalid_Parsable_Strings(int invalidInput)
        {
            Assert.False(PerformToEnumSafe(invalidInput.ToString(), out DateTimeKind? value));
            Assert.True(value == null);
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
        [TestCase("anything", null, false)]
        [TestCase("   invalidString   ", null, false)]
        [TestCase("         ", null, true)]
        [TestCase("", null, true)]
        [TestCase(null, null, true)]
        public void ToEnumSafe_For_NullableEnum_Returns_Consistent_Results_When_IgnoreCase_Is_True(string input,
            DateTimeKind? expected, bool returnValue)
        {
            Assert.True(PerformToEnumSafe(input, out DateTimeKind? value) == returnValue);
            Assert.True(value.Equals(expected));
        }

        [Test]
        [TestCase("Unspecified", DateTimeKind.Unspecified, true)]
        [TestCase("Local", DateTimeKind.Local, true)]
        [TestCase("Utc", DateTimeKind.Utc, true)]
        [TestCase("    Unspecified     ", DateTimeKind.Unspecified, true)]
        [TestCase("    Local  ", DateTimeKind.Local, true)]
        [TestCase("  Utc   ", DateTimeKind.Utc, true)]
        [TestCase("unspecified", null, false)]
        [TestCase("local", null, false)]
        [TestCase("utc", null, false)]
        [TestCase("  unspecified  ", null, false)]
        [TestCase("   local   ", null, false)]
        [TestCase("   utc   ", null, false)]
        [TestCase("anything", null, false)]
        [TestCase("   invalidString   ", null, false)]
        [TestCase("         ", null, true)]
        [TestCase("", null, true)]
        [TestCase(null, null, true)]
        public void ToEnumSafe_For_NullableEnum_Returns_Consistent_Results_When_IgnoreCase_Is_False(string input,
            DateTimeKind? expected, bool returnValue)
        {
            Assert.True(PerformToEnumSafe(input, out DateTimeKind? value, false) == returnValue);
            Assert.True(value.Equals(expected));
        }

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
        [TestCase(null, "A","A", ' ', ',')]
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

        private static bool PerformToEnumSafe<T>(string input, out T value, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumSafe(out value, ignoreCase);
        }

        private static bool PerformToEnumSafe<T>(string input, out T? value, bool ignoreCase = true)
            where T : struct
        {
            return input.ToEnumSafe(out value, ignoreCase);
        }
    }
}