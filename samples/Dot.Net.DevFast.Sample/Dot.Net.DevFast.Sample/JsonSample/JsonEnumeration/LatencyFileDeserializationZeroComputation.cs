using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Sample.JsonSample.JsonEnumeration
{
    public static class LatencyFileDeserializationZeroComputation
    {
        public static void Run()
        {
            Console.Out.WriteLine("-------JsonEnumeration Deserialization-------");
            Console.Out.WriteLine("x64 App: " + Environment.Is64BitProcess);
            //Small Object serialization in 10M loops
            RunBoth();
        }

        private static void RunBoth()
        {
            //warm up
            var jsonFile = new FileInfo(@"C:\Temp\jsonEnumTest.json");
            //jsonFile.MeasureJsonConvert(false);
            var jsonTime = jsonFile.MeasureJsonConvert();

            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            //warm up
            var devFastJsonFile = new FileInfo(@"C:\Temp\jsonEnumDfTest.json");
            //devFastJsonFile.MeasureDevFast(false);
            var devfastTime = devFastJsonFile.MeasureDevFast();
            var dfFastness = ((int)((100 - (devfastTime / jsonTime * 100)) * 100)) / 100.0;
            Console.Out.WriteLine("DevFast " + Math.Abs(dfFastness) + (dfFastness < 0 ? " % Slower" : " % Faster"));
            Console.Out.WriteLine();
        }

        private static double MeasureJsonConvert(this FileInfo jsonFile, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            //We deserialize and loop on the array.
            var count = JsonConvert.DeserializeObject<LargeObj[]>(File.ReadAllText(jsonFile.FullName)).Count();
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("JsonConvert Total Time: " + sw.Elapsed.TotalMilliseconds);
                jsonFile.Refresh();
                Console.Out.WriteLine("FileLen: " + jsonFile.Length);
                Console.Out.WriteLine("Count: " + count);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static double MeasureDevFast(this FileInfo jsonFile, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            //We deserialize the IEnumerable and loop
            var count =
                jsonFile.CreateStream(FileMode.Open, options: FileOptions.SequentialScan, bufferSize:32*1024)
                    .FromJsonAsEnumerable<LargeObj>(new JsonSerializer(), bufferSize: 32 * 1024)
                    .Count();
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