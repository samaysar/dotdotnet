using System;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dot.Net.DevFast.Extensions.JsonExt
{
    internal static class JsonInterns
    {
        internal static void DisposeIfRequired(this IDisposable disposable, bool dispose)
        {
            if (!dispose) return;
            using (disposable)
            {
                //to dispose
            }
        }

        internal static T FromJsonGetNext<T>(this JsonReader jsonReader, JsonSerializer serializer)
        {
            return JToken.Load(jsonReader).ToObject<T>(serializer);
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