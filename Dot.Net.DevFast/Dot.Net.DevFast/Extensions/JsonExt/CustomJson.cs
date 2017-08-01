using System.Globalization;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.JsonExt
{
    /// <summary>
    /// Defaults related to JSON Serialization
    /// </summary>
    public static class CustomJson
    {
        /// <summary>
        /// Returns a new instance of custom <seealso cref="JsonSerializer"/>.
        /// </summary>
        public static JsonSerializer Serializer()
        {
            return new JsonSerializer()
            {
                Culture = CultureInfo.CurrentCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                Formatting = Formatting.None,
                StringEscapeHandling = StringEscapeHandling.Default,
                CheckAdditionalContent = false,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                DateParseHandling = DateParseHandling.DateTime,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                FloatParseHandling = FloatParseHandling.Double,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        /// <summary>
        /// Creates <seealso cref="JsonSerializer"/> using <see cref="Serializer"/> and sets serialization related properties based on
        /// the properties of the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">writer to reference</param>
        public static JsonSerializer AdaptJsonSerializer(this JsonWriter writer)
        {
            var serializer = Serializer();
            if (writer == null) return serializer;
            serializer.Culture = writer.Culture;
            serializer.DateFormatHandling = writer.DateFormatHandling;
            serializer.DateFormatString = writer.DateFormatString;
            serializer.DateTimeZoneHandling = writer.DateTimeZoneHandling;
            serializer.FloatFormatHandling = writer.FloatFormatHandling;
            serializer.Formatting = writer.Formatting;
            serializer.StringEscapeHandling = writer.StringEscapeHandling;
            return serializer;
        }

        /// <summary>
        /// Creates <seealso cref="JsonSerializer"/> using <see cref="Serializer"/> and sets deserialization related properties based on
        /// the properties of the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">reader to reference</param>
        public static JsonSerializer AdaptJsonSerializer(this JsonReader reader)
        {
            var serializer = Serializer();
            if (reader == null) return serializer;
            serializer.Culture = reader.Culture;
            serializer.DateFormatString = reader.DateFormatString;
            serializer.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            serializer.FloatParseHandling = reader.FloatParseHandling;
            serializer.DateParseHandling = reader.DateParseHandling;
            serializer.MaxDepth = reader.MaxDepth;
            return serializer;
        }
    }
}