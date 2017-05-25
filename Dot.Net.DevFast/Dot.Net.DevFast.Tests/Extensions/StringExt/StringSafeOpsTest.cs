using System;
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
    }
}