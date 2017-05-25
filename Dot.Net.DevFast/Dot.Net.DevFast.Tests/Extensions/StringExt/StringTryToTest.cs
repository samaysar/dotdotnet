using System;
using System.Globalization;
using Dot.Net.DevFast.Extensions.StringExt;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.StringExt
{
    [TestFixture]
    public class StringTryToTest
    {
        [Test]
        [TestCase("       ", default(Type), false)]
        [TestCase("", default(Type), false)]
        [TestCase(null, default(Type), false)]
        [TestCase("stringtrytotest", default(Type), false)]
        [TestCase("STRINGTRYTOTEST", default(Type), false)]
        [TestCase("dot.net.DevFast.Tests.Extensions.stringext.stringtrytotest, dot.net.DevFast.Tests",
            typeof(StringTryToTest), true)]
        [TestCase("DOT.NET.DEVFAST.TESTS.EXTENSIONS.STRINGEXT.STRINGTRYTOTEST, DOT.NET.DevFast.TESTS",
            typeof(StringTryToTest), true)]
        [TestCase("Dot.Net.DevFast.Tests.Extensions.StringExt.StringTryToTest, Dot.Net.DevFast.Tests",
            typeof(StringTryToTest), true)]
        public void TryTo_Type_Works_As_Expected_When_IgnoreCase_Is_True(string input,
            Type expected, bool returnVal)
        {
            Assert.True(input.TryTo(out Type outcome) == returnVal);
            Assert.True(ReferenceEquals(outcome, expected));
        }

        [Test]
        [TestCase("       ", default(Type), false)]
        [TestCase("", default(Type), false)]
        [TestCase(null, default(Type), false)]
        [TestCase("stringtrytotest", default(Type), false)]
        [TestCase("STRINGTRYTOTEST", default(Type), false)]
        [TestCase("dot.net.DevFast.Tests.Extensions.stringext.stringtrytotest, dot.net.DevFast.tests",
            default(Type), false)]
        [TestCase("DOT.NET.DevFast.Tests.Extensions.STRINGEXT.STRINGTRYTOTEST, DOT.NET.DevFast.TESTS",
            default(Type), false)]
        [TestCase("Dot.Net.DevFast.Tests.Extensions.StringExt.StringTryToTest, Dot.Net.DevFast.Tests",
            typeof(StringTryToTest), true)]
        public void TryTo_Type_Works_As_Expected_When_IgnoreCase_Is_False(string input,
            Type expected, bool returnVal)
        {
            Assert.True(input.TryTo(out Type outcome, false) == returnVal);
            Assert.True(ReferenceEquals(outcome, expected));
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
        public void TryTo_Bool_Works_As_Expected(string input, bool expected, bool returnVal)
        {
            Assert.True(input.TryTo(out bool outcome) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Int_Works_As_Expected(string input, NumberStyles style, string provider,
            int expected, bool returnVal)
        {
            Assert.True(input.TryTo(out int outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Long_Works_As_Expected(string input, NumberStyles style, string provider,
            long expected, bool returnVal)
        {
            Assert.True(input.TryTo(out long outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Byte_Works_As_Expected(string input, NumberStyles style, string provider,
            byte expected, bool returnVal)
        {
            Assert.True(input.TryTo(out byte outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_SByte_Works_As_Expected(string input, NumberStyles style, string provider,
            sbyte expected, bool returnVal)
        {
            Assert.True(input.TryTo(out sbyte outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Short_Works_As_Expected(string input, NumberStyles style, string provider,
            short expected, bool returnVal)
        {
            Assert.True(input.TryTo(out short outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_UShort_Works_As_Expected(string input, NumberStyles style, string provider,
            ushort expected, bool returnVal)
        {
            Assert.True(input.TryTo(out ushort outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_UInt_Works_As_Expected(string input, NumberStyles style, string provider,
            uint expected, bool returnVal)
        {
            Assert.True(input.TryTo(out uint outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_ULong_Works_As_Expected(string input, NumberStyles style, string provider,
            ulong expected, bool returnVal)
        {
            Assert.True(input.TryTo(out ulong outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Float_Works_As_Expected(string input, NumberStyles style, string provider,
            float expected, bool returnVal)
        {
            Assert.True(input.TryTo(out float outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Double_Works_As_Expected(string input, NumberStyles style, string provider,
            double expected, bool returnVal)
        {
            Assert.True(input.TryTo(out double outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
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
        public void TryTo_Decimal_When_Return_Value_Is_False(string input, NumberStyles style, 
            string provider)
        {
            Assert.False(input.TryTo(out decimal outcome, style,
                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)));
            Assert.True(outcome.Equals(default(decimal)));
        }

        [Test]
        public void TryTo_Decimal_When_Return_Value_Is_True()
        {
            Assert.True("3.0001".TryTo(out decimal outcome));
            Assert.True(outcome.Equals(3.0001m));

            Assert.True("3.0000".TryTo(out outcome));
            Assert.True(outcome.Equals(3.0000m));

            Assert.True("3.40282".TryTo(out outcome));
            Assert.True(outcome.Equals(3.40282m));

            Assert.True("-3.40282".TryTo(out outcome));
            Assert.True(outcome.Equals(-3.40282m));

            Assert.True("3,40282".TryTo(out outcome, NumberStyles.Any, new CultureInfo("fr-FR")));
            Assert.True(outcome.Equals(3.40282m));

            Assert.True("-3,40282".TryTo(out outcome, NumberStyles.Any, new CultureInfo("fr-FR")));
            Assert.True(outcome.Equals(-3.40282m));
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
        public void TryTo_DateTime_Works_As_Expected_With_Given_Format(string input, string format,
            bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime ts, format));
                Assert.True(new DateTime(2001,01,01,0,0,0).Equals(ts));
            }
            else
            {
                Assert.False(input.TryTo(out DateTime ts, format));
                Assert.True(default(DateTime).Equals(ts));
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
        public void TryTo_DateTime_Works_As_Expected_With_Format_Collection(string input, 
            string formatBarSeparated, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime ts, formatBarSeparated.Split('|')));
                Assert.True(new DateTime(2001, 01, 01, 0, 0, 0).Equals(ts));
            }
            else
            {
                Assert.False(input.TryTo(out DateTime ts, formatBarSeparated.Split('|')));
                Assert.True(default(DateTime).Equals(ts));
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
        public void TryTo_DateTime_Works_As_Expected_With_No_Format(string input, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime ts));
                Assert.True(DateTime.Parse(input).Equals(ts));
            }
            else
            {
                Assert.False(input.TryTo(out DateTime ts));
                Assert.True(default(DateTime).Equals(ts));
            }
        }

        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        [Test]
        [TestCase("true", true, true)]
        [TestCase("false", false, true)]
        [TestCase("  true   ", true, true)]
        [TestCase(" false  ", false, true)]
        [TestCase("  TRUE   ", true, true)]
        [TestCase(" FALSE  ", false, true)]
        [TestCase("  TRuE   ", true, true)]
        [TestCase(" FAlsE  ", false, true)]
        [TestCase("notbool", null, false)]
        [TestCase("boolean", null, false)]
        [TestCase(" invalid_bool ", null, false)]
        [TestCase("       ", null, true)]
        [TestCase("", null, true)]
        [TestCase(null, null, true)]
        public void TryTo_Nullable_Bool_Works_As_Expected(string input, bool? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out bool? outcome) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, 3, true)]
        [TestCase("-2147483648", NumberStyles.Any, null, int.MinValue, true)]
        [TestCase("2147483647", NumberStyles.Any, null, int.MaxValue, true)]
        [TestCase("-2 147 483 648", NumberStyles.Any, "fr-FR", int.MinValue, true)]
        [TestCase("2 147 483 647", NumberStyles.Any, "fr-FR", int.MaxValue, true)]
        public void TryTo_Nullable_Int_Works_As_Expected(string input, NumberStyles style, string provider,
            int? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out int? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (long)3, true)]
        [TestCase("-2147483648", NumberStyles.Any, null, (long)int.MinValue, true)]
        [TestCase("2147483647", NumberStyles.Any, null, (long)int.MaxValue, true)]
        [TestCase("-2 147 483 648", NumberStyles.Any, "fr-FR", (long)int.MinValue, true)]
        [TestCase("2 147 483 647", NumberStyles.Any, "fr-FR", (long)int.MaxValue, true)]
        [TestCase("-9223372036854775808", NumberStyles.Any, null, long.MinValue, true)]
        [TestCase("9223372036854775807", NumberStyles.Any, null, long.MaxValue, true)]
        [TestCase("-9 223 372 036 854 775 808", NumberStyles.Any, "fr-FR", long.MinValue, true)]
        [TestCase("9 223 372 036 854 775 807", NumberStyles.Any, "fr-FR", long.MaxValue, true)]
        public void TryTo_Nullable_Long_Works_As_Expected(string input, NumberStyles style, string provider,
            long? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out long? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (byte)3, true)]
        [TestCase("0", NumberStyles.Any, null, byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, null, byte.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", byte.MinValue, true)]
        [TestCase("255", NumberStyles.Any, "fr-FR", byte.MaxValue, true)]
        public void TryTo_Nullable_Byte_Works_As_Expected(string input, NumberStyles style, string provider,
            byte? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out byte? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (sbyte)3, true)]
        [TestCase("-128", NumberStyles.Any, null, sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, null, sbyte.MaxValue, true)]
        [TestCase("-128", NumberStyles.Any, "fr-FR", sbyte.MinValue, true)]
        [TestCase("127", NumberStyles.Any, "fr-FR", sbyte.MaxValue, true)]
        public void TryTo_Nullable_SByte_Works_As_Expected(string input, NumberStyles style, string provider,
            sbyte? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out sbyte? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (short)3, true)]
        [TestCase("-32768", NumberStyles.Any, null, short.MinValue, true)]
        [TestCase("32767", NumberStyles.Any, null, short.MaxValue, true)]
        [TestCase("-32 768", NumberStyles.Any, "fr-FR", short.MinValue, true)]
        [TestCase("32 767", NumberStyles.Any, "fr-FR", short.MaxValue, true)]
        public void TryTo_Nullable_Short_Works_As_Expected(string input, NumberStyles style, string provider,
            short? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out short? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (ushort)3, true)]
        [TestCase("0", NumberStyles.Any, null, ushort.MinValue, true)]
        [TestCase("65535", NumberStyles.Any, null, ushort.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ushort.MinValue, true)]
        [TestCase("65 535", NumberStyles.Any, "fr-FR", ushort.MaxValue, true)]
        public void TryTo_Nullable_UShort_Works_As_Expected(string input, NumberStyles style, string provider,
            ushort? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out ushort? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (uint)3, true)]
        [TestCase("0", NumberStyles.Any, null, uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", uint.MaxValue, true)]
        public void TryTo_Nullable_UInt_Works_As_Expected(string input, NumberStyles style, string provider,
            uint? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out uint? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0000", NumberStyles.Any, null, (ulong)3, true)]
        [TestCase("0", NumberStyles.Any, null, (ulong)uint.MinValue, true)]
        [TestCase("4294967295", NumberStyles.Any, null, (ulong)uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", (ulong)uint.MinValue, true)]
        [TestCase("4 294 967 295", NumberStyles.Any, "fr-FR", (ulong)uint.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, null, ulong.MinValue, true)]
        [TestCase("18446744073709551615", NumberStyles.Any, null, ulong.MaxValue, true)]
        [TestCase("0", NumberStyles.Any, "fr-FR", ulong.MinValue, true)]
        [TestCase("18 446 744 073 709 551 615", NumberStyles.Any, "fr-FR", ulong.MaxValue, true)]
        public void TryTo_Nullable_ULong_Works_As_Expected(string input, NumberStyles style, string provider,
            ulong? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out ulong? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, (float)3.0001, true)]
        [TestCase("3.0000", NumberStyles.Any, null, (float)3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, (float)-3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, (float)3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", (float)-3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", (float)3.40282, true)]
        public void TryTo_Nullable_Float_Works_As_Expected(string input, NumberStyles style, string provider,
            float? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out float? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome.Equals(expected));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, null, true)]
        [TestCase("", NumberStyles.Any, null, null, true)]
        [TestCase(null, NumberStyles.Any, null, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, null, false)]
        [TestCase("a2", NumberStyles.Any, null, null, false)]
        [TestCase("3a123", NumberStyles.Any, null, null, false)]
        [TestCase("3_", NumberStyles.Any, null, null, false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, null, null, false)]
        [TestCase("3.0001", NumberStyles.Any, null, 3.0001, true)]
        [TestCase("3.0000", NumberStyles.Any, null, (double)3, true)]
        [TestCase("-3.40282", NumberStyles.Any, null, -3.40282, true)]
        [TestCase("3.40282", NumberStyles.Any, null, 3.40282, true)]
        [TestCase("-3,40282", NumberStyles.Any, "fr-FR", -3.40282, true)]
        [TestCase("3,40282", NumberStyles.Any, "fr-FR", 3.40282, true)]
        public void TryTo_Nullable_Double_Works_As_Expected(string input, NumberStyles style, string provider,
            double? expected, bool returnVal)
        {
            Assert.True(input.TryTo(out double? outcome, style,
                    string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(returnVal ? outcome.Equals(expected) : (outcome == null));
        }

        [Test]
        [TestCase("       ", NumberStyles.Any, null, true)]
        [TestCase("", NumberStyles.Any, "fr-FR", true)]
        [TestCase(null, NumberStyles.Any, null, true)]
        [TestCase("    a3   ", NumberStyles.Any, null, false)]
        [TestCase("a2", NumberStyles.Any, "fr-FR", false)]
        [TestCase("3a123", NumberStyles.Any, null, false)]
        [TestCase("3_", NumberStyles.Any, "fr-FR", false)]
        [TestCase("3    ", NumberStyles.AllowLeadingWhite, null, false)]
        [TestCase("   3", NumberStyles.AllowTrailingWhite, "fr-FR", false)]
        public void TryTo_Nullable_Decimal_When_Return_Value_Is_False(string input, NumberStyles style,
            string provider, bool returnVal)
        {
            Assert.True(input.TryTo(out decimal? outcome, style,
                string.IsNullOrWhiteSpace(provider) ? null : new CultureInfo(provider)) == returnVal);
            Assert.True(outcome == null);
        }

        [Test]
        public void TryTo_Nullable_Decimal_When_Return_Value_Is_True_And_String_Is_Valid()
        {
            Assert.True("3.0001".TryTo(out decimal? outcome));
            Assert.True(outcome.Equals(3.0001m));

            Assert.True("3.0000".TryTo(out outcome));
            Assert.True(outcome.Equals(3.0000m));

            Assert.True("3.40282".TryTo(out outcome));
            Assert.True(outcome.Equals(3.40282m));

            Assert.True("-3.40282".TryTo(out outcome));
            Assert.True(outcome.Equals(-3.40282m));

            Assert.True("3,40282".TryTo(out outcome, NumberStyles.Any, new CultureInfo("fr-FR")));
            Assert.True(outcome.Equals(3.40282m));

            Assert.True("-3,40282".TryTo(out outcome, NumberStyles.Any, new CultureInfo("fr-FR")));
            Assert.True(outcome.Equals(-3.40282m));
        }

        [Test]
        [TestCase(null, "does not matter", true)]
        [TestCase("          ", "does not matter", true)]
        [TestCase("", "does not matter", true)]
        [TestCase("2001-01-01 00:00:00", "yyyy-MM-dd hh:mm:ss", true)]
        [TestCase("2001-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", true)]
        [TestCase("2001-01-01 25:00:00", "yyyy-MM-dd HH:mm:ss", false)]
        [TestCase("2001-01-01 15:00:00", "yyyy-MM-dd hh:mm:ss", false)]
        [TestCase("01-01-2001 00:00:00", "MM-dd-yyyy hh:mm:ss", true)]
        [TestCase("01-01-2001 00:00:00", "MM-dd-yyyy HH:mm:ss", true)]
        [TestCase("01-01-2001 25:00:00", "MM-dd-yyyy HH:mm:ss", false)]
        [TestCase("01-01-2001 15:00:00", "MM-dd-yyyy hh:mm:ss", false)]
        public void TryTo_Nullable_DateTime_Works_As_Expected_With_Given_Format(string input, string format,
            bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime? ts, format));
                if (string.IsNullOrWhiteSpace(input))
                {
                    Assert.True(ts == null);
                }
                else
                {
                    Assert.True(new DateTime(2001, 01, 01, 0, 0, 0).Equals(ts.Value));
                }
            }
            else
            {
                Assert.False(input.TryTo(out DateTime? ts, format));
                Assert.True(ts == null);
            }
        }

        [Test]
        [TestCase(null, "does not matter", true)]
        [TestCase("          ", "does not matter", true)]
        [TestCase("", "does not matter", true)]
        [TestCase("2001-01-01 00:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd hh:mm:ss", true)]
        [TestCase("2001-01-01 00:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd HH:mm:ss", true)]
        [TestCase("2001-01-01 25:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd HH:mm:ss", false)]
        [TestCase("2001-01-01 15:00:00", "MM-dd-yyyy hh:mm:ss|yyyy-MM-dd hh:mm:ss", false)]
        [TestCase("01-01-2001 00:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy hh:mm:ss", true)]
        [TestCase("01-01-2001 00:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy HH:mm:ss", true)]
        [TestCase("01-01-2001 25:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy HH:mm:ss", false)]
        [TestCase("01-01-2001 15:00:00", "yyyy-MM-dd hh:mm:ss|MM-dd-yyyy hh:mm:ss", false)]
        public void TryTo_Nullable_DateTime_Works_As_Expected_With_Format_Collection(string input,
            string formatBarSeparated, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime? ts, formatBarSeparated.Split('|')));
                if (string.IsNullOrWhiteSpace(input))
                {
                    Assert.True(ts == null);
                }
                else
                {
                    Assert.True(new DateTime(2001, 01, 01, 0, 0, 0).Equals(ts));
                }
            }
            else
            {
                Assert.False(input.TryTo(out DateTime? ts, formatBarSeparated.Split('|')));
                Assert.True(ts == null);
            }
        }

        [Test]
        [TestCase("", true)]
        [TestCase("         ", true)]
        [TestCase(null, true)]
        [TestCase("2001-01-01 00:00:00", true)]
        [TestCase("2001-01-01 00:00:00", true)]
        [TestCase("2001-01-01 25:00:00", false)]
        [TestCase("2001-01-01 15:00:00", true)]
        [TestCase("01-01-2001 00:00:00", true)]
        [TestCase("01-01-2001 60:60:60", false)]
        [TestCase("01-01-2001 25:00:00", false)]
        [TestCase("12-31-2001 23:59:59", true)]
        public void TryTo_Nullable_DateTime_Works_As_Expected_With_No_Format(string input, bool expectedReturn)
        {
            if (expectedReturn)
            {
                Assert.True(input.TryTo(out DateTime? ts));
                if (string.IsNullOrWhiteSpace(input))
                {
                    Assert.True(ts == null);
                }
                else
                {
                    Assert.True(DateTime.Parse(input).Equals(ts));
                }
            }
            else
            {
                Assert.False(input.TryTo(out DateTime? ts));
                Assert.True(ts == null);
            }
        }
    }
}