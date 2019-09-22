using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StreamPipeExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;

namespace StreamingSample
{
    public class MyTestData
    {
        public string SomeLongString { get; set; } = @"I am a very very long string.";
        public double SomeDecimal { get; set; }
        public byte[] ByteArray { get; set; }
    }

    public static class SampleGeneratingHelpers
    {
        public static async Task<FileInfo> GenerateRandomDataAsync(int totalObjects)
        {
            var sw = Stopwatch.StartNew();
            var folder = AppDomain.CurrentDomain.BaseDirectory.ToDirectoryInfo(new[] { "TestData" }, true);
            var fileToWrite = folder.CreateFileInfo("StreamingSample-PerfComp.json.zip");
            await Console.Out.WriteLineAsync($"Generating Random file data: {fileToWrite.FullName}").ConfigureAwait(false);
            await GenerateTestObjects(totalObjects).PushJsonArray(new JsonSerializer
                {
                    Formatting = Formatting.Indented
                }).ThenCompress().AndWriteFileAsync(fileToWrite).ConfigureAwait(false);
            await Console.Out.WriteLineAsync($"Random Data gen successful. Time:{sw.ElapsedMilliseconds} ms").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);
            return fileToWrite;
        }

        private static IEnumerable<MyTestData> GenerateTestObjects(int total)
        {
            var rand = new Random();
            for (var i = 0; i < total; i++)
            {
                var doub = rand.NextDouble();
                var byteArr = new byte[i + 1];
                rand.NextBytes(byteArr);
                yield return new MyTestData
                {
                    SomeDecimal = doub,
                    ByteArray = byteArr
                };
            }
        }
    }
}