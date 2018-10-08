using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.Internals
{
    internal static class JsonBased
    {
        internal static T FromJsonGetNext<T>(this JsonReader jsonReader, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(jsonReader);
        }

        internal static bool NotAnEndArrayToken(this JsonReader jsonReader)
        {
            return (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray);
        }

        internal static bool ThrowIfTokenNotStartArray(this JsonReader jsonReader)
        {
            if (!jsonReader.Read()) return true;
            return (jsonReader.TokenType == JsonToken.StartArray).ThrowIfNot(DdnDfErrorCode.JsonIsNotAnArray,
                () =>
                    $"JSON string does not start with start array token. Found token type is {jsonReader.TokenType:G}",
                false);
        }
    }
}