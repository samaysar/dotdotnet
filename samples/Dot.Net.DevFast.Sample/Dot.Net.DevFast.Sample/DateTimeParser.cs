using System;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Sample
{
    public static class DateTimeParser
    {
        public static void Run()
        {
            Console.Out.WriteLine("---------------------------------------------------");
            Console.Out.WriteLine("------------- Running DateTimeParser --------------");
            Console.Out.WriteLine("---------------------------------------------------");
            //Lets parse integer string value wrongly to Datetime... if parsing fails 
            //then we want Jan 12, 2000 as default value
            Console.Out.WriteLine(ParseDateTimeOrDefault("some string"));
            Console.Out.WriteLine("some string".ToOrDefault(new DateTime(2000, 1, 12)));
        }

        public static DateTime ParseDateTimeOrDefault(string possibleDatetimeString)
        {
            if (possibleDatetimeString == null) return new DateTime(2000, 1, 12);
            DateTime parsedTs;
            if (DateTime.TryParse(possibleDatetimeString, out parsedTs))
            {
                return parsedTs;
            }
            return new DateTime(2000, 1, 12);
        }
    }
}