using System;
using System.Diagnostics;
using System.Text;
using Dot.Net.DevFast.Extensions.JsonExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Sample.JsonSample
{
    public static class JsonConvertLatency
    {
        private static readonly Employee Employee = new Employee
        {
            Address = "House No. Json, Json street, Json Square, Json Town, 123456 JSON",
            Age = 20,
            Name = "Mr Json Born"
        };

        public static void Run(int iteration)
        {
            Console.Out.WriteLine("-----------------------------------------------------");
            Console.Out.WriteLine("-------------------JSON.NET (Single)-----------------");
            Console.Out.WriteLine("-----------------------------------------------------");
            //warm up
            Employee.MeasureJsonConvert(1, false);
            Employee.MeasureJsonConvert(iteration*1000);

            Console.Out.WriteLine("-----------------------------------------------------");
            Console.Out.WriteLine("-------------------DEV FAST (Single)-----------------");
            Console.Out.WriteLine("-----------------------------------------------------");
            //warm up
            Employee.MeasureDevFast(1, false);
            Employee.MeasureDevFast(iteration * 1000);

            var employees = new Employee[1000];
            for (var i = 0; i < 1000; i++)
            {
                employees[i] = Employee;
            }
            Console.Out.WriteLine("----------------------------------------------------");
            Console.Out.WriteLine("-------------------JSON.NET (Array)-----------------");
            Console.Out.WriteLine("----------------------------------------------------");
            //warm up
            employees.MeasureJsonConvert(1, false);
            employees.MeasureJsonConvert(iteration);

            Console.Out.WriteLine("----------------------------------------------------");
            Console.Out.WriteLine("-------------------DEV FAST (Array)-----------------");
            Console.Out.WriteLine("----------------------------------------------------");
            //warm up
            employees.MeasureDevFast(1, false);
            employees.MeasureDevFast(iteration);
        }

        private static void MeasureJsonConvert<T>(this T val, int iteration, bool print = true)
        {
            var json = string.Empty;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iteration; i++)
            {
                json = JsonConvert.SerializeObject(val);
            }
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("Total Time: " + sw.Elapsed.TotalMilliseconds + " for " + iteration +
                                      " iterations");
                Console.Out.WriteLine("JSON:" + Environment.NewLine + json.Substring(0, 10));
            }
        }

        private static void MeasureDevFast<T>(this T val, int iteration, bool print = true)
        {
            var json = string.Empty;
            var sb = new StringBuilder(256);
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iteration; i++)
            {
                val.ToJson(sb);
                json = sb.ToString();
                sb.Clear();
            }
            sw.Stop();
            if (print)
            {
                Console.Out.WriteLine("Total Time: " + sw.Elapsed.TotalMilliseconds + " for " + iteration +
                                      " iterations");
                Console.Out.WriteLine("JSON:" + Environment.NewLine + json.Substring(0, 10));
            }
        }
    }
}