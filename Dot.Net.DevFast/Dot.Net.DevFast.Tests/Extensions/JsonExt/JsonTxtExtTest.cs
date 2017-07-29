using System;
using System.IO;
using System.Text;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.JsonExt
{
    [TestFixture]
    public class JsonTxtExtTest
    {
        [Test]
        public void ToJson_FromJson_Harmonize()
        {
            const long data = 1254578;
            var sb = new StringBuilder();
            data.ToJson(CustomJson.Serializer().CreateJsonWriter(sb.CreateWriter()));
            //CustomJson.Serializer().CreateJsonReader(sb.cre)
        }
    }

    public class Data
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public DateTimeKind Kind { get; set; }
        public DateTime Ts { get; set; }
        public long Conv { get; set; }
        public byte By { get; set; }
        public decimal Dec { get; set; }
        public Data More { get; set; }
    }
}