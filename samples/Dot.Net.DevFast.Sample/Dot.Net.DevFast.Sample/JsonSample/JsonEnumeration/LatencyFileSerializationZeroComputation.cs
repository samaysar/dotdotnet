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
    public static class LatencyFileSerializationZeroComputation
    {
        public static void Run()
        {
            Console.Out.WriteLine("-------JsonEnumeration Serialization-------");
            Console.Out.WriteLine("x64 App: " + Environment.Is64BitProcess);
            //Small Object serialization in 10M loops
            Run(1024*1024*5);
        }

        private static void Run(int iteration)
        {
            Console.Out.WriteLine("Iterations: " + iteration);
            //warm up
            var jsonFile = new FileInfo(@"C:\Temp\jsonEnumTest.json");
            jsonFile.MeasureJsonConvert(2, false);
            var jsonTime = jsonFile.MeasureJsonConvert(iteration);

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
            devFastJsonFile.MeasureDevFast(2, false);
            var devfastTime = devFastJsonFile.MeasureDevFast(iteration);
            var dfFastness = ((int)((100 - (devfastTime / jsonTime * 100)) * 100)) / 100.0;
            Console.Out.WriteLine("DevFast " + Math.Abs(dfFastness) + (dfFastness < 0 ? " % Slower" : " % Faster"));
            Console.Out.WriteLine();
        }

        private static double MeasureJsonConvert(this FileInfo jsonFile, int iteration, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            //We serialize and write the string to file.
            File.WriteAllText(jsonFile.FullName, JsonConvert.SerializeObject(GenerateObj(iteration)));
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("JsonConvert Total Time: " + sw.Elapsed.TotalMilliseconds);
                jsonFile.Refresh();
                Console.Out.WriteLine("FileLen: " + jsonFile.Length);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static double MeasureDevFast(this FileInfo jsonFile, int iteration, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            //We serialize the IEnumerable
            GenerateObj(iteration)
                .ToJsonArray(jsonFile.CreateStream(FileMode.Create, options: FileOptions.None, bufferSize: 32 * 1024),
                    new JsonSerializer(), enc: new UTF8Encoding(false), bufferSize: 32 * 1024);
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("DevFast Total Time: " + sw.Elapsed.TotalMilliseconds);
                jsonFile.Refresh();
                Console.Out.WriteLine("FileLen: " + jsonFile.Length);
            }
            return sw.Elapsed.TotalMilliseconds;
        }

        private static IEnumerable<LargeObj> GenerateObj(int iteration)
        {
            for (var i = 0; i < iteration; i++)
            {
                yield return LargeObj;
            }
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