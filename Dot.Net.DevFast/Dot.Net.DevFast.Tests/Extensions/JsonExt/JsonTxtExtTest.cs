using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const long Number = 123456789;

        private static readonly Data ObjData = new Data
        {
            Kind = DateTimeKind.Local,
            By = byte.MaxValue,
            Conv = Number,
            Bytes = new byte[] {byte.MinValue, byte.MaxValue, 0},
            Dec = 123456789m,
            Name = "testing",
            Ts = new DateTime(2000, 1, 1, 1, 1, 1),
            More = new Data
            {
                Kind = DateTimeKind.Local,
                By = byte.MaxValue,
                Conv = Number,
                Bytes = new byte[] {byte.MinValue, byte.MaxValue, 0},
                Dec = 123456789m,
                Name = "testing",
                Ts = new DateTime(2000, 1, 1, 1, 1, 1)
            }
        };

        private static readonly string[] StrArr = new[] {"t", "1", "two"};
        private static readonly Data[] DataObjArr = new[] {ObjData, ObjData};

        private static void AssertOnObjData(IList<string> dup)
        {
            Assert.True(dup.Count == StrArr.Length);
            for (var i = 0; i < dup.Count; i++)
            {
                Assert.True(dup[i].Equals(StrArr[i]));
            }
        }

        private static void AssertOnObjData(IEnumerable<Data> dup)
        {
            foreach (var data in dup)
            {
                AssertOnObjData(data);
            }
        }

        private static void AssertOnObjData(Data dup)
        {
            Assert.True(ObjData.By.Equals(dup.By));
            Assert.True(ObjData.Kind.Equals(dup.Kind));
            Assert.True(ObjData.Conv.Equals(dup.Conv));
            Assert.True(ObjData.Bytes[0].Equals(dup.Bytes[0]));
            Assert.True(ObjData.Bytes[1].Equals(dup.Bytes[1]));
            Assert.True(ObjData.Bytes[2].Equals(dup.Bytes[2]));
            Assert.True(ObjData.Dec.Equals(dup.Dec));
            Assert.True(ObjData.Name.Equals(dup.Name));
            Assert.True(ObjData.Ts.Equals(dup.Ts));

            Assert.True(ObjData.More.By.Equals(dup.More.By));
            Assert.True(ObjData.More.Kind.Equals(dup.More.Kind));
            Assert.True(ObjData.More.Conv.Equals(dup.More.Conv));
            Assert.True(ObjData.More.Bytes[0].Equals(dup.More.Bytes[0]));
            Assert.True(ObjData.More.Bytes[1].Equals(dup.More.Bytes[1]));
            Assert.True(ObjData.More.Bytes[2].Equals(dup.More.Bytes[2]));
            Assert.True(ObjData.More.Dec.Equals(dup.More.Dec));
            Assert.True(ObjData.More.Name.Equals(dup.More.Name));
            Assert.True(ObjData.More.Ts.Equals(dup.More.Ts));
        }

        [Test]
        public void JsonReader_Based_ToJson_FromJson_Harmonize()
        {
            var sb = new StringBuilder();
            Number.ToJson(sb.CreateJsonWriter());
            Assert.True(Number == sb.CreateJsonReader().FromJson<long>());

            sb.Clear();
            ObjData.ToJson(sb.CreateJsonWriter());
            AssertOnObjData(sb.CreateJsonReader().FromJson<Data>());

            sb.Clear();
            DataObjArr.ToJson(sb.CreateJsonWriter());
            AssertOnObjData(sb.CreateJsonReader().FromJson<Data[]>());

            sb.Clear();
            StrArr.ToJson(sb.CreateJsonWriter());
            AssertOnObjData(sb.CreateJsonReader().FromJson<string[]>());
        }

        [Test]
        public void JsonReader_Based_ToJsonArray_FromJsonAsEnumerable_Harmonize()
        {
            var sb = new StringBuilder();
            DataObjArr.ToJsonArray(sb.CreateJsonWriter());
            foreach (var data in sb.CreateJsonReader().FromJsonAsEnumerable<Data>())
            {
                AssertOnObjData(data);
            }

            sb.Clear();
            StrArr.ToJsonArray(sb.CreateJsonWriter());
            var count = 0;
            foreach (var data in sb.CreateJsonReader().FromJsonAsEnumerable<string>())
            {
                Assert.True(data.Equals(StrArr[count++]));
            }

            sb.Clear();
            Array.Empty<string>().ToJsonArray(CustomJson.Serializer(), sb.CreateJsonWriter());
            Assert.True(sb.ToString().Equals("[]"));
            Assert.False(sb.CreateJsonReader()
                .FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
            sb.Clear();
            Assert.False(sb.CreateJsonReader()
                .FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
        }

        [Test]
        public void TextReader_Based_ToJson_FromJson_Harmonize()
        {
            var sb = new StringBuilder();
            Number.ToJson(sb.CreateWriter());
            Assert.True(Number == sb.CreateReader().FromJson<long>());

            sb.Clear();
            ObjData.ToJson(sb.CreateWriter());
            AssertOnObjData(sb.CreateReader().FromJson<Data>());

            sb.Clear();
            DataObjArr.ToJson(sb.CreateWriter());
            AssertOnObjData(sb.CreateReader().FromJson<Data[]>());

            sb.Clear();
            StrArr.ToJson(sb.CreateWriter());
            AssertOnObjData(sb.CreateReader().FromJson<string[]>());
        }

        [Test]
        public void TextReader_Based_ToJsonArray_FromJsonAsEnumerable_Harmonize()
        {
            var sb = new StringBuilder();
            DataObjArr.ToJsonArray(sb.CreateWriter());
            foreach (var data in sb.CreateReader().FromJsonAsEnumerable<Data>())
            {
                AssertOnObjData(data);
            }

            sb.Clear();
            StrArr.ToJsonArray(sb.CreateWriter());
            var count = 0;
            foreach (var data in sb.CreateReader().FromJsonAsEnumerable<string>())
            {
                Assert.True(data.Equals(StrArr[count++]));
            }

            sb.Clear();
            Array.Empty<string>().ToJsonArray(CustomJson.Serializer(), sb.CreateWriter());
            Assert.True(sb.ToString().Equals("[]"));
            Assert.False(sb.CreateReader()
                .FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
            sb.Clear();
            Assert.False(sb.CreateReader()
                .FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
        }

        [Test]
        public void Stream_Based_ToJson_FromJson_Harmonize()
        {
            var sb = new MemoryStream();
            Number.ToJson(sb, disposeStream:false);
            sb.Seek(0, SeekOrigin.Begin);
            Assert.True(Number == sb.FromJson<long>());

            sb = new MemoryStream();
            ObjData.ToJson(sb, disposeStream: false);
            sb.Seek(0, SeekOrigin.Begin);
            AssertOnObjData(sb.FromJson<Data>());

            sb = new MemoryStream();
            DataObjArr.ToJson(sb, disposeStream: false);
            sb.Seek(0, SeekOrigin.Begin);
            AssertOnObjData(sb.FromJson<Data[]>());

            sb = new MemoryStream();
            StrArr.ToJson(sb, disposeStream: false);
            sb.Seek(0, SeekOrigin.Begin);
            AssertOnObjData(sb.FromJson<string[]>());
        }

        [Test]
        public void Stream_Based_ToJsonArray_FromJsonAsEnumerable_Harmonize()
        {
            var sb = new MemoryStream();
            DataObjArr.ToJsonArray(sb, disposeStream: false);
            sb.Seek(0, SeekOrigin.Begin);
            foreach (var data in sb.FromJsonAsEnumerable<Data>())
            {
                AssertOnObjData(data);
            }

            sb = new MemoryStream();
            StrArr.ToJsonArray(sb, disposeStream: false);
            var count = 0;
            foreach (var data in sb.FromJsonAsEnumerable<string>())
            {
                Assert.True(data.Equals(StrArr[count++]));
            }

            sb = new MemoryStream();
            Array.Empty<string>().ToJsonArray(CustomJson.Serializer(), sb, disposeStream:false);
            Assert.False(sb.FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
            sb = new MemoryStream();
            Assert.False(sb.FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
        }

        [Test]
        public void String_Based_ToJson_FromJson_Harmonize()
        {
            Assert.True(Number == Number.ToJson().FromJson<long>());
            AssertOnObjData(ObjData.ToJson().FromJson<Data>());
            AssertOnObjData(DataObjArr.ToJson().FromJson<Data[]>());
            AssertOnObjData(StrArr.ToJson().FromJson<string[]>());
        }

        [Test]
        public void String_Based_ToJsonArray_FromJsonAsEnumerable_Harmonize()
        {
            foreach (var data in DataObjArr.ToJsonArray().FromJsonAsEnumerable<Data>())
            {
                AssertOnObjData(data);
            }
            var count = 0;
            foreach (var data in StrArr.ToJsonArray().FromJsonAsEnumerable<string>())
            {
                Assert.True(data.Equals(StrArr[count++]));
            }
            Assert.False(Array.Empty<string>().ToJsonArray(CustomJson.Serializer())
                .FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
            Assert.False("".FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
        }

        [Test]
        public void StringBuilder_Based_ToJson_FromJson_Harmonize()
        {
            var sb = new StringBuilder();
            Number.ToJson(sb);
            Assert.True(Number == sb.FromJson<long>());

            sb.Clear();
            ObjData.ToJson(sb);
            AssertOnObjData(sb.FromJson<Data>());

            sb.Clear();
            DataObjArr.ToJson(sb);
            AssertOnObjData(sb.FromJson<Data[]>());

            sb.Clear();
            StrArr.ToJson(sb);
            AssertOnObjData(sb.FromJson<string[]>());
        }

        [Test]
        public void StringBuilder_Based_ToJsonArray_FromJsonAsEnumerable_Harmonize()
        {
            var sb = new StringBuilder();
            DataObjArr.ToJsonArray(sb);
            foreach (var data in sb.FromJsonAsEnumerable<Data>())
            {
                AssertOnObjData(data);
            }

            sb.Clear();
            StrArr.ToJsonArray(sb);
            var count = 0;
            foreach (var data in sb.FromJsonAsEnumerable<string>())
            {
                Assert.True(data.Equals(StrArr[count++]));
            }

            sb.Clear();
            Array.Empty<string>().ToJsonArray(CustomJson.Serializer(), sb);
            Assert.True(sb.ToString().Equals("[]"));
            Assert.False(sb.FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
            sb.Clear();
            Assert.False(sb.FromJsonAsEnumerable<string>(CustomJson.Serializer()).GetEnumerator().MoveNext());
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