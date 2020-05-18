using System;
using Dot.Net.DevFast.Extensions.StringExt;
using Microsoft.Owin.Hosting;

namespace StreamingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = args[0].ToInt();
            using (var app = WebApp.Start<Bootstrapper>($"http://localhost:{port}"))
            {
                Console.WriteLine($"Server is started on port:{port}");
                Console.WriteLine("ENTER TO EXIT");
                Console.ReadLine();
            }
        }
    }
}
