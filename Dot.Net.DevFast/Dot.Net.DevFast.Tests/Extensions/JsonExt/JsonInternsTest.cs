using System;
using System.IO;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.JsonExt
{
    [TestFixture]
    public class JsonInternsTest
    {
        [Test]
        public void DisposeIfRequired_Works_As_Expected()
        {
            var disposable = Substitute.For<IDisposable>();
            disposable.DisposeIfRequired(false);
            disposable.Received(0).Dispose();

            disposable.DisposeIfRequired(true);
            disposable.Received(1).Dispose();
        }

        [Test]
        public void FromJsonGetNext_Works_As_Expected()
        {
            var data = "20.45";
            using (var jsonReader = data.CreateJsonReader())
            {
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.FromJsonGetNext<double>(CustomJson.Serializer()).Equals(20.45));
            }
            data = @"{""Name"":""Testing""}";
            using (var jsonReader = data.CreateJsonReader())
            {
                Assert.True(jsonReader.Read());
                Assert.True(jsonReader.FromJsonGetNext<TestClass>(CustomJson.Serializer()).Name.Equals("Testing"));
            }
        }

        [Test]
        public void NotAnEndArrayToken_Works_As_Expected()
        {
            const string data = "[20.45]";
            using (var jsonReader = data.CreateJsonReader())
            {
                Assert.True(jsonReader.NotAnEndArrayToken());
                Assert.True(jsonReader.NotAnEndArrayToken());
                Assert.False(jsonReader.NotAnEndArrayToken());
            }
        }

        [Test]
        public void ThrowIfTokenNotStartArray_Throws_Error_If_Current_Token_Is_Not_StartArray()
        {
            const string data = @"{""Name"":""Testing""}";
            using (var jsonReader = data.CreateJsonReader())
            {
                var err = Assert.Throws<DdnDfException>(() => jsonReader.ThrowIfTokenNotStartArray());
                Assert.True(err.ErrorCode.Equals(DdnDfErrorCode.JsonIsNotAnArray));
                Assert.True(err.Message.Contains(JsonToken.StartObject.ToString("G")));
            }
        }

        [Test]
        public void ThrowIfTokenNotStartArray_Returns_True_If_Read_Is_Unsuccessful()
        {
            const string data = @"";
            using (var jsonReader = data.CreateJsonReader())
            {
                Assert.True(jsonReader.ThrowIfTokenNotStartArray());
            }
        }

        [Test]
        public void ThrowIfTokenNotStartArray_Returns_False_If_Read_Is_Successful_At_StartArray()
        {
            const string data = @"[]";
            using (var jsonReader = data.CreateJsonReader())
            {
                Assert.False(jsonReader.ThrowIfTokenNotStartArray());
            }
        }

        private class TestClass
        {
            public string Name { get; set; }
        }
    }
}