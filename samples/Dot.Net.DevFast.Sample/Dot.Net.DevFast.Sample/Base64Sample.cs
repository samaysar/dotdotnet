using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.StreamExt;

namespace Dot.Net.DevFast.Sample
{
    public static class Base64Sample
    {
        public static void Run()
        {
            Console.Out.WriteLine("---------------------------------------------------");
            Console.Out.WriteLine("------------- Running Base64Sample ----------------");
            Console.Out.WriteLine("---------------------------------------------------");

            var byteArr = new byte[10];
            var rndm = new Random();
            rndm.NextBytes(byteArr);
            Base64SampleAsync(byteArr).Wait();
        }

        private static async Task Base64SampleAsync(byte[] byteArr)
        {
            using (var inputStream = new MemoryStream(byteArr))
            {
                using (var outputStream = new MemoryStream())
                {
                    await inputStream.ToBase64Async(outputStream).ConfigureAwait(false);
                    //prints same Base64 string as above
                    Console.WriteLine(Encoding.UTF8.GetString(outputStream.ToArray()));
                    outputStream.Seek(0, SeekOrigin.Begin);
                    //converting back
                    Console.WriteLine((await outputStream.FromBase64Async()
                        .ConfigureAwait(false)).Length == 10);//prints true
                }
            }
        }
    }
}