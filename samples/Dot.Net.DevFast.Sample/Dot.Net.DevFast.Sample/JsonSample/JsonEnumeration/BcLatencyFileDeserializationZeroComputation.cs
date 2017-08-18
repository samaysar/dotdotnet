using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Sample.JsonSample.JsonEnumeration
{
    public static class BcLatencyFileDeserializationZeroComputation
    {
        public static void Run()
        {
            Console.Out.WriteLine("-------JsonEnumeration BlockingCollection Deserialization-------");
            Console.Out.WriteLine("x64 App: " + Environment.Is64BitProcess);
            //Small Object serialization in 10M loops
            RunDf();
        }

        private static void RunDf()
        {
            //warm up
            var devFastJsonFile = new FileInfo(@"C:\Temp\jsonBcDfTest.json");
            devFastJsonFile.MeasureDevFast();
        }

        private static double MeasureDevFast(this FileInfo jsonFile, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            var bc = new BlockingCollection<LargeObj>();
            var task = Task.Run(() =>
            {
                jsonFile.CreateStream(FileMode.Open, options: FileOptions.SequentialScan, bufferSize: 32 * 1024)
                    .FromJsonArrayParallely(bc, new JsonSerializer(), bufferSize: 32 * 1024);
            });
            //We serialize the IEnumerable
            var count = bc.GetConsumingEnumerable().Count();
            task.Wait();
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("DevFast Total Time: " + sw.Elapsed.TotalMilliseconds);
                jsonFile.Refresh();
                Console.Out.WriteLine("FileLen: " + jsonFile.Length);
                Console.Out.WriteLine("Count: " + count);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static BlockingCollection<LargeObj> GenerateObj(int iteration, int capacity, out Task population)
        {
            var bc = new BlockingCollection<LargeObj>(capacity);
            population = Task.Run(() =>
            {
                for (var i = 0; i < iteration; i++)
                {
                    bc.Add(LargeObj);
                }
                bc.CompleteAdding();
            });
            return bc;
        }

        private static readonly LargeObj LargeObj = new LargeObj
        {
            Address = "123, Json street",
            Age = 20,
            Name = "Json Born",
            City = "LDN",
            Country = "Macedonia",
            AboutMe = "A small world"
        };
    }
}