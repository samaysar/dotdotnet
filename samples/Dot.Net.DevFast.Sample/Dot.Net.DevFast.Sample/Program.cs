using System;
using Dot.Net.DevFast.Sample.JsonSample;
using Dot.Net.DevFast.Sample.JsonSample.FromExt;
using Dot.Net.DevFast.Sample.JsonSample.JsonEnumeration;
using Dot.Net.DevFast.Sample.JsonSample.ToExt;

namespace Dot.Net.DevFast.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //DateTimeParser.Run();

            //Console.Out.WriteLine();
            //StringSample.Run();

            //Console.Out.WriteLine();
            //ThrowSample.Run();

            //Console.Out.WriteLine();
            //Base64Sample.Run();

            //Console.Out.WriteLine();
            //TransformSample.Run();

            //Console.Clear();
            //JsonConvertLatency.Run();

            //Console.Clear();
            //JsonConvertStreamLatency.Run();

            //Console.Clear();
            //JsonConvertMemStreamLatency.Run();

            //Console.Clear();
            //JsonConvertDeLatency.Run();

            //Console.Clear();
            //JsonConvertStreamDeLatency.Run();

            //Console.Clear();
            //JsonConvertMemStreamDeLatency.Run();

            //Console.Clear();
            LatencyFileSerializationZeroComputation.Run();

            Console.ReadLine();
        }
    }
}