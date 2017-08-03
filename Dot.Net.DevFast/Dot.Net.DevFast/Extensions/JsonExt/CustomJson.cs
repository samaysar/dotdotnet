using System.Globalization;
using System.IO;
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
            serializer.Culture = reader.Culture;
            serializer.DateFormatString = reader.DateFormatString;
            serializer.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            serializer.FloatParseHandling = reader.FloatParseHandling;
            serializer.DateParseHandling = reader.DateParseHandling;
            serializer.MaxDepth = reader.MaxDepth;
            return serializer;
        }

        /// <summary>
        /// Creates a <seealso cref="JsonWriter"/> for given <paramref name="serializer"/> and <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="serializer">Serializer to use to populate <seealso cref="JsonWriter"/> properties</param>
        /// <param name="textWriter">target text writer</param>
        /// <param name="disposeWriter">If true, <paramref name="textWriter"/> is disposed after the serialization</param>
        public static JsonWriter AdaptedJsonWriter(this JsonSerializer serializer, TextWriter textWriter,
            bool disposeWriter = true)
        {
            return new JsonTextWriter(textWriter)
            {
                Culture = serializer.Culture,
                DateFormatHandling = serializer.DateFormatHandling,
                DateFormatString = serializer.DateFormatString,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                FloatFormatHandling = serializer.FloatFormatHandling,
                Formatting = serializer.Formatting,
                StringEscapeHandling = serializer.StringEscapeHandling,
                CloseOutput = disposeWriter
            };
        }

        /// <summary>
        /// Creates a <seealso cref="JsonReader"/> for given <paramref name="serializer"/> and <paramref name="textReader"/>.
        /// </summary>
        /// <param name="serializer">Serializer to use to populate <seealso cref="JsonReader"/> properties</param>
        /// <param name="textReader">target text reader</param>
        /// <param name="disposeReader">If true, <paramref name="textReader"/> is disposed after the deserialization</param>
        public static JsonReader AdaptedJsonReader(this JsonSerializer serializer, TextReader textReader,
            bool disposeReader = true)
        {
            return new JsonTextReader(textReader)
            {
                Culture = serializer.Culture,
                DateFormatString = serializer.DateFormatString,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DateParseHandling = serializer.DateParseHandling,
                FloatParseHandling = serializer.FloatParseHandling,
                MaxDepth = serializer.MaxDepth,
                CloseInput = disposeReader
            };
        }
    }
}