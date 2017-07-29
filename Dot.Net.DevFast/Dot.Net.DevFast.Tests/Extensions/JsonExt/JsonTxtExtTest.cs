using System;
using System.IO;
using System.Text;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.JsonExt
{
    [TestFixture]
    public class JsonTxtExtTest
    {
        [Test]
        public void Tt()
        {

            var dt = new Data
            {
                Kind = DateTimeKind.Utc,
                By = 14,
                Bytes = new byte[] {15, 55, 44},
                Conv = 1024,
                Dec = 10.25684m,
                Name = "sar",
                Ts = DateTime.Now,
                More = new Data
                {
                    Kind = DateTimeKind.Utc,
                    By = 14,
                    Bytes = new byte[] {15, 55, 44},
                    Conv = 1024,
                    Dec = 10.25684m,
                    Name = "sar",
                    Ts = DateTime.Now
                }
            };
            var dtArr = new object[] {dt, dt, "sar", 1.24m};
            var st = dtArr.ToJsonAsync().Result;
            Console.Out.WriteLine(st);
            var jr = new JsonTextReader(new StringReader(st));
            foreach (var val in st.FromJsonAsEnumerable<object>())
            {
                Console.Out.WriteLine();
                Console.Out.WriteLine(val.ToJsonAsync().Result);
            }
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