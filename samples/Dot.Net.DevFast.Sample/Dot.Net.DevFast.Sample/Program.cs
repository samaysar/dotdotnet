using System;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTimeParser.Run();

            Console.Out.WriteLine();
            StringSample.Run();

            Console.Out.WriteLine();
            ThrowSample.Run();

            Console.Out.WriteLine();
            Base64Sample.Run();

            Console.Out.WriteLine();
            TransformSample.Run();

            Console.ReadLine();
        }
    }
}
