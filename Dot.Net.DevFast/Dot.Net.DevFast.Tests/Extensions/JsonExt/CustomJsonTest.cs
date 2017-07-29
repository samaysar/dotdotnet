using System.Globalization;
using System.IO;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.JsonExt
{
    [TestFixture]
    public class CustomJsonTest
    {
        [Test]
        public void Serializer_Returns_A_New_Instance_Everytime_With_Consistent_Properties()
        {
            var first = CustomJson.Serializer();
            Assert.False(ReferenceEquals(first, CustomJson.Serializer()));
            Assert.False(first.CheckAdditionalContent);
            Assert.True(first.Culture.Equals(CultureInfo.CurrentCulture));
            Assert.True(first.DateFormatHandling.Equals(DateFormatHandling.IsoDateFormat));
            Assert.True(first.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Utc));
            Assert.True(first.FloatFormatHandling.Equals(FloatFormatHandling.DefaultValue));
            Assert.True(first.Formatting.Equals(Formatting.None));
            Assert.True(first.StringEscapeHandling.Equals(StringEscapeHandling.Default));
            Assert.True(first.ConstructorHandling.Equals(ConstructorHandling.AllowNonPublicDefaultConstructor));
            Assert.True(first.DateParseHandling.Equals(DateParseHandling.DateTime));
            Assert.True(first.DefaultValueHandling.Equals(DefaultValueHandling.Ignore));
            Assert.True(first.FloatParseHandling.Equals(FloatParseHandling.Double));
            Assert.True(first.MissingMemberHandling.Equals(MissingMemberHandling.Ignore));
            Assert.True(first.NullValueHandling.Equals(NullValueHandling.Ignore));
            Assert.True(first.ObjectCreationHandling.Equals(ObjectCreationHandling.Auto));
            Assert.True(first.PreserveReferencesHandling.Equals(PreserveReferencesHandling.None));
            Assert.True(first.ReferenceLoopHandling.Equals(ReferenceLoopHandling.Error));
            Assert.True(first.TypeNameAssemblyFormatHandling.Equals(TypeNameAssemblyFormatHandling.Simple));
            Assert.True(first.TypeNameHandling.Equals(TypeNameHandling.Auto));
        }

        [Test]
        public void AdaptJsonSerializer_Set_Serialization_Properties_Of_Writer_To_Serializer()
        {
            using(var writer = new JsonTextWriter(TextWriter.Null)
            {
                Culture = new CultureInfo("fr-FR"),
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                FloatFormatHandling = FloatFormatHandling.Symbol,
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                CloseOutput = true
            })
            {
                var serializer = writer.AdaptJsonSerializer();
                Assert.True(serializer.Culture.Equals(new CultureInfo("fr-FR")));
                Assert.True(serializer.DateFormatHandling.Equals(DateFormatHandling.MicrosoftDateFormat));
                Assert.True(serializer.DateTimeZoneHandling.Equals(DateTimeZoneHandling.RoundtripKind));
                Assert.True(serializer.FloatFormatHandling.Equals(FloatFormatHandling.Symbol));
                Assert.True(serializer.Formatting.Equals(Formatting.Indented));
                Assert.True(serializer.StringEscapeHandling.Equals(StringEscapeHandling.EscapeNonAscii));
                Assert.True(serializer.DateFormatString.Equals("yyyy-MM-dd HH:mm:ss"));
            }
        }

        [Test]
        public void AdaptJsonSerializer_Set_Deserialization_Properties_Of_Reader_To_Serializer()
        {
            using (var writer = new JsonTextReader(TextReader.Null)
            {
                Culture = new CultureInfo("en-GB"),
                DateFormatString = "yyyy/MM/dd HH.mm.ss",
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                FloatParseHandling = FloatParseHandling.Decimal,
                MaxDepth = 100,
                CloseInput = true
            })
            {
                var serializer = writer.AdaptJsonSerializer();
                Assert.True(serializer.Culture.Equals(new CultureInfo("en-GB")));
                Assert.True(serializer.DateTimeZoneHandling.Equals(DateTimeZoneHandling.Unspecified));
                Assert.True(serializer.DateParseHandling.Equals(DateParseHandling.DateTimeOffset));
                Assert.True(serializer.FloatParseHandling.Equals(FloatParseHandling.Decimal));
                Assert.True(serializer.MaxDepth.Equals(100));
                Assert.True(serializer.DateFormatString.Equals("yyyy/MM/dd HH.mm.ss"));
            }
        }
    }
}