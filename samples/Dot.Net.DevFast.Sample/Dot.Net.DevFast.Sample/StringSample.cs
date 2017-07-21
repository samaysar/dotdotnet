using System;
using Dot.Net.DevFast.Extensions.StringExt;

namespace Dot.Net.DevFast.Sample
{
    public static class StringSample
    {
        public static void Run()
        {
            Console.Out.WriteLine("---------------------------------------------------");
            Console.Out.WriteLine("------------- Running StringSample ----------------");
            Console.Out.WriteLine("---------------------------------------------------");

            //String => Type Parsing
            "System.Collections.Generic.List`1[[System.String, mscorlib]]".TryTo(out Type mytype);
            Console.WriteLine(mytype.FullName);//Print type full name

            //String => int parsing (.TryTo is safer n does not throws error)
            Console.WriteLine("123456".ToInt());//print 123456

            //String => decimal parsing (with some default value)
            Console.WriteLine("garbage string".ToOrDefault(58m));//prints 58
        }
    }
}