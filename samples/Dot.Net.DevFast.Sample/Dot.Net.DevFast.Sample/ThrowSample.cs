using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Sample
{
    public static class ThrowSample
    {
        public static void Run()
        {
            Console.Out.WriteLine("---------------------------------------------------");
            Console.Out.WriteLine("------------- Running ThrowSample -----------------");
            Console.Out.WriteLine("---------------------------------------------------");

            IReadOnlyDictionary<int, int> dict = new Dictionary<int, int> { { 2, 10 } };
            //lookup in dictionary 
            Console.WriteLine(dict.ThrowOnMiss(2, "key 2 not found")
                //and equality on the value in chain call
                .ThrowIfNotEqual(10, () => "Value NOT 10")); // prints 10

            //checking the presence of values in list ELSE THROW
            var strgList = new List<string> { "one", "two" };
            //does NOT throw here
            Console.WriteLine(strgList.ThrowOnMiss("one").ThrowOnMiss("two").Count); //print 2

            try
            {
                //three is NOT in the list... this will throw error
                strgList.ThrowOnMiss("three");
            }
            catch (Exception e)
            {
                Console.WriteLine(e); //print error
            }
        }
    }
}