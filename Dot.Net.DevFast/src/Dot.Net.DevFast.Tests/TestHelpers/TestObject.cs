using System;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Tests.TestHelpers
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TestObject
    {
        private static readonly Random Ran = new Random();

        [JsonConstructor]
        public TestObject()
        {
            IntProp = Ran.Next();
            StrProp = Guid.NewGuid().ToString("N");
            BytesProp = new byte[8];
            Ran.NextBytes(BytesProp);
        }

        public string StrProp { get; set; }
        public int IntProp { get; set; }
        public byte[] BytesProp { get; set; }
    }
}