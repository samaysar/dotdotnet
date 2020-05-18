using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StreamingSample
{
    public static class HoistingGenerator
    {
        public static async Task RunHoistedNUnHoistedStreams()
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("==========================================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("Running Nested Streams & Hoisted Generator").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("==========================================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            //Creating sample file
            const int totalObjects = 100;
            var file = await SampleGeneratingHelpers.GenerateRandomDataAsync(totalObjects).ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            //Deserializing with nested using block
            await Console.Out.WriteLineAsync("===> Deserializing object array with Nested using blocks <===").ConfigureAwait(false);
            var objs = GetDataFromCompressedJson<List<MyTestData>>(file);
            await Console.Out.WriteLineAsync($"Total of {objs.Count} objects deserialized from file.").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            await Console.Out.WriteLineAsync().ConfigureAwait(false);
            await Console.Out.WriteLineAsync("===> Deserializing object array with Hoisted Stream Generators <===").ConfigureAwait(false);

            //NOTE: For DEMO purpose we are not taking care of stream dispose and other optimizations we can add.
            //do NOT use this code in production instead check the https://github.com/samaysar/dotdotnet for actual implementation.
            objs = file.PullData()
                .ThenDecompress()
                .ThenDecode()
                .ThenDeserializeJson<List<MyTestData>>();
            await Console.Out.WriteLineAsync($"Total of {objs.Count} objects deserialized from file.").ConfigureAwait(false);

            Console.ReadLine();
        }

        private static T GetDataFromCompressedJson<T>(FileSystemInfo file)
        {
            //Cascading streams from outer USING to INNER using block.
            using (var fs = new FileStream(file.FullName, FileMode.Open))
            {
                using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (var tr = new StreamReader(gzip, Encoding.UTF8))
                    {
                        using (var jr = new JsonTextReader(tr))
                        {
                            return new JsonSerializer().Deserialize<T>(jr);
                        }
                    }
                }
            }
        }

        private static Func<Stream> PullData(this FileSystemInfo file)
        {
            //This stream will be consumed in the ThenDecompress method
            return () => new FileStream(file.FullName, FileMode.Open);
        }

        private static Func<Stream> ThenDecompress(this Func<Stream> prev)
        {
            //This stream will be consumed in the ThenDecode method
            return () => new GZipStream(prev(), CompressionMode.Decompress);
        }

        private static Func<StreamReader> ThenDecode(this Func<Stream> prev, Encoding enc = null)
        {
            //This READER will be consumed in the ThenDeserializeJson method
            return () => new StreamReader(prev(), enc ?? Encoding.UTF8);
        }

        private static T ThenDeserializeJson<T>(this Func<StreamReader> prev)
        {
            //Prev() returns StreamReader instance which is reading from GZipStream
            //                                     which in turn reads from FileStream
            using (var jr = new JsonTextReader(prev()))
            {
                return new JsonSerializer().Deserialize<T>(jr);
            }
        }
    }
}