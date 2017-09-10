using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;

namespace Dot.Net.DevFast.Sample.JsonSample.JsonReportDb
{
    public static class PpcFileStats
    {
        public static void Run()
        {
            Console.Out.WriteLine("Running Parallel Producer-Consumer on Json File");
            var jsonfile = new FileInfo(@"C:\Temp\jsonDfMysql.json");
            var collection = new BlockingCollection<Film>(256);
            var count = 0;
            decimal totalLength = 0;
            decimal totalRentalRate = 0;

            var sw = Stopwatch.StartNew();
            //single is sufficient to perform the JSON deserialization in parallel.
            var jsonTask = Task.Run(() => jsonfile.CreateStream(FileMode.Open).FromJsonArrayParallely(collection));
            //we calculate our stats
            while (collection.TryTake(out Film film, Timeout.Infinite))
            {
                count++;
                totalLength += film.Length.Value;
                totalRentalRate += film.RentalRate.Value;
            }
            jsonTask.Wait();
            sw.Stop();

            Console.Out.WriteLine("Computation Time: " + sw.Elapsed.TotalMilliseconds);
            Console.Out.WriteLine("Count: " + count);
            Console.Out.WriteLine("Avg. Rental Rate: " + (totalRentalRate/count));
            Console.Out.WriteLine("Avg. Length: " + (totalLength/count));
        }
    }
}