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
    public static class BcLatencyFileSerializationZeroComputation
    {
        public static void Run()
        {
            Console.Out.WriteLine("-------JsonEnumeration BlockingCollection Serialization-------");
            Console.Out.WriteLine("x64 App: " + Environment.Is64BitProcess);
            //Small Object serialization in 10M loops
            Run(1024*1024*5);
        }

        private static void Run(int iteration)
        {
            Console.Out.WriteLine("Iterations: " + iteration);
            //warm up
            var devFastJsonFile = new FileInfo(@"C:\Temp\jsonBcDfTest.json");
            devFastJsonFile.MeasureDevFast(2, false);
            devFastJsonFile.MeasureDevFast(iteration);
        }

        private static double MeasureDevFast(this FileInfo jsonFile, int iteration, bool print = true)
        {
            var sw = Stopwatch.StartNew();
            //We serialize the IEnumerable
            GenerateObj(iteration, 256, out Task pop)
                .ToJsonArrayParallely(jsonFile.CreateStream(FileMode.Create, options: FileOptions.None, bufferSize: 32 * 1024),
                    new JsonSerializer(), enc: new UTF8Encoding(false), bufferSize: 32 * 1024);
            pop.Wait();
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("DevFast Total Time: " + sw.Elapsed.TotalMilliseconds);
                jsonFile.Refresh();
                Console.Out.WriteLine("FileLen: " + jsonFile.Length);
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