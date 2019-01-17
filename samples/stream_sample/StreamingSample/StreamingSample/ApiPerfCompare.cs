using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StreamPipeExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;

namespace StreamingSample
{
    public static class ApiPerfCompare
    {
        public static async Task PerfCompareNonStreamingWithStreamingAsync()
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("========================================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("Running Perf Comparison of Streaming API").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("========================================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);
            const int totalObjects = 10000;
            var file = await GenerateRandomDataAsync(totalObjects).ConfigureAwait(false);
            await RunWithoutStreamingApiAsync(file, totalObjects).ConfigureAwait(false);
            await RunWithStreamingApiAsync(file, totalObjects).ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);
            await Console.Out.WriteLineAsync("============================================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("Done... please enter to run next simulation.").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("============================================").ConfigureAwait(false);
            Console.ReadLine();
        }

        private static async Task RunWithoutStreamingApiAsync(FileInfo file, int totalObjects, bool iswarmup = true)
        {
            long beforeMem = 0;
            long afterMem = 0;
            if (iswarmup) beforeMem = Helpers.GetMemoryPostGc() >> 10;
            var sw = Stopwatch.StartNew();
            var byteData = File.ReadAllBytes(file.FullName);
            var unzippedData = new MemoryStream();
            using (var unzipper = new GZipStream(new MemoryStream(byteData), CompressionMode.Decompress, false))
            {
                await unzipper.CopyToAsync(unzippedData).ConfigureAwait(false);
            }

            var deserializedJson =
                JsonConvert.DeserializeObject<List<MyTestData>>(new UTF8Encoding().GetString(unzippedData.ToArray()));
            sw.Stop();
            if (iswarmup) afterMem = Helpers.GetMemoryPostGc() >> 10;
            Thread.MemoryBarrier();
            if (deserializedJson.Count != totalObjects)
            {
                throw new Exception($"Expected {totalObjects} objects. Got only {deserializedJson.Count}");
            }

            if (iswarmup)
            {
                await Console.Out.WriteLineAsync($"NonAPI Memory => Before: {beforeMem} KB, After: {afterMem} KB, " +
                                                 $"Diff: {afterMem - beforeMem} KB")
                    .ConfigureAwait(false);
                await RunWithoutStreamingApiAsync(file, totalObjects, false).ConfigureAwait(false);
            }
            else
            {
                await Console.Out.WriteLineAsync($"NonAPI Total Time:{sw.ElapsedMilliseconds} ms")
                    .ConfigureAwait(false);
            }
        }

        private static async Task RunWithStreamingApiAsync(FileInfo file, int totalObjects, bool iswarmup = true)
        {
            long beforeMem = 0;
            long afterMem = 0;
            beforeMem = Helpers.GetMemoryPostGc() >> 10;
            var sw = Stopwatch.StartNew();
            var count = 0;
            if (iswarmup)
            {
                //In fact it is useless to convert it to list... coz actually it is a real
                //on the fly IEnumerable... so it consumes memory required for object
                var deserializedJson = file.Pull().ThenDecompress().AndParseJsonArray<MyTestData>().ToList();
                sw.Stop();
                afterMem = Helpers.GetMemoryPostGc() >> 10;
                Thread.MemoryBarrier();
                count = deserializedJson.Count;
            }
            else
            {
                var deserializedJson = file.Pull().ThenDecompress().AndParseJsonArray<MyTestData>();
                afterMem = Helpers.GetMemoryPostGc() >> 10;
                Thread.MemoryBarrier();
                count = deserializedJson.Count();
                sw.Stop();
            }
            if (count != totalObjects)
            {
                throw new Exception($"Expected {totalObjects} objects. Got only {count}");
            }

            if (iswarmup)
            {
                Console.Out.WriteLine();
                await Console.Out.WriteLineAsync($"StreamingAPI Memory (Forcing to LIST) => Before: {beforeMem} KB, After: {afterMem} KB, " +
                                                 $"Diff: {afterMem - beforeMem} KB")
                    .ConfigureAwait(false);
                await Console.Out.WriteLineAsync($"StreamingAPI (LIST) Total Time:{sw.ElapsedMilliseconds} ms")
                    .ConfigureAwait(false);
                await RunWithStreamingApiAsync(file, totalObjects, false).ConfigureAwait(false);
            }
            else
            {
                Console.Out.WriteLine();
                await Console.Out.WriteLineAsync($"StreamingAPI Memory (using IEnumerable) => Before: {beforeMem} KB, After: {afterMem} KB, " +
                                                 $"Diff: {afterMem - beforeMem} KB")
                    .ConfigureAwait(false);
                await Console.Out.WriteLineAsync($"StreamingAPI (IENUMERABLE) Total Time:{sw.ElapsedMilliseconds} ms")
                    .ConfigureAwait(false);
            }
        }

        private static async Task<FileInfo> GenerateRandomDataAsync(int totalObjects)
        {
            var sw = Stopwatch.StartNew();
            var folder = AppDomain.CurrentDomain.BaseDirectory.ToDirectoryInfo(new[] { "TestData" }, true);
            var fileToWrite = folder.CreateFileInfo("StreamingSample-PerfComp.json.zip");
            await Console.Out.WriteLineAsync($"Generating Random file data: {fileToWrite.FullName}").ConfigureAwait(false);
            await GenerateTestObjects(totalObjects).PushJsonArray(new JsonSerializer
                {
                    Formatting = Formatting.Indented
                }).ThenCompress().AndWriteFileAsync(fileToWrite)
                .ConfigureAwait(false);
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
                var byteArr = new byte[i+1];
                rand.NextBytes(byteArr);
                yield return new MyTestData
                {
                    SomeDecimal = doub,
                    ByteArray = byteArr
                };
            }
        }
    }

    public class MyTestData
    {
        public string SomeLongString { get; set; } = @"I am a very very long string.";
        public double SomeDecimal { get; set; }
        public byte[] ByteArray { get; set; }
    }
}