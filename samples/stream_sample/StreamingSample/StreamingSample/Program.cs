using System;
using System.Threading.Tasks;

namespace StreamingSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await ApiPerfCompare.PerfCompareNonStreamingWithStreamingAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString()).ConfigureAwait(false);
                Console.ReadLine();
            }
            Console.Clear();
            await Console.Out.WriteLineAsync("====================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("Done! Enter to quit.").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("====================").ConfigureAwait(false);
            Console.ReadLine();
        }
    }
}
