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
                //UNComment below method to RUN DevFast APIs vs in-memory buffer usage comparison
                //await ApiPerfCompare.PerfCompareNonStreamingWithStreamingAsync().ConfigureAwait(false);

                //UNcomment below method to run Random INT code with and without sugar!
                await RandomFuncNumbers.GenerateRandomIntegers().ConfigureAwait(false);
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
