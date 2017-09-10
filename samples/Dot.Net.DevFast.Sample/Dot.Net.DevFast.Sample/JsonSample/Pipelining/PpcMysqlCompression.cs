using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using MySql.Data.MySqlClient;

namespace Dot.Net.DevFast.Sample.JsonSample.Pipelining
{
    public static class PpcMysqlCompression
    {
        public static void Run()
        {
            Console.Out.WriteLine("Running Parallel Producer-Consumer on MySql with compression");
            //var oraConnDetails = File.ReadAllText(@"c:/temp/oracleConn.txt").Split('|');
            //var connStr = new MySqlConnectionStringBuilder
            //{
            //    Server = oraConnDetails[0],
            //    UserID = oraConnDetails[1],
            //    Password = oraConnDetails[2],
            //    Pooling = true,
            //    MinimumPoolSize = 5,
            //    MaximumPoolSize = 30
            //};

            //var jsonTime = RunApproach3(connStr.ToString(), new FileInfo(@"C:\Temp\jsonA3MysqlComp.json"));

            //GC.Collect();
            //GC.WaitForFullGCApproach();
            //GC.WaitForFullGCComplete();
            //GC.WaitForPendingFinalizers();

            //GC.Collect();
            //GC.WaitForFullGCApproach();
            //GC.WaitForFullGCComplete();
            //GC.WaitForPendingFinalizers();

            //var devfastTime = RunDevFast(connStr.ToString(), new FileInfo(@"C:\Temp\jsonDfMysqlComp.json.zip"));
            //var dfFastness = ((int)((100 - (devfastTime / jsonTime * 100)) * 100)) / 100.0;
            //Console.Out.WriteLine("DevFast " + Math.Abs(dfFastness) + (dfFastness < 0 ? " % Slower" : " % Faster"));

            //GC.Collect();
            //GC.WaitForFullGCApproach();
            //GC.WaitForFullGCComplete();
            //GC.WaitForPendingFinalizers();

            //GC.Collect();
            //GC.WaitForFullGCApproach();
            //GC.WaitForFullGCComplete();
            //GC.WaitForPendingFinalizers();

            RunDevFastDeserial(new FileInfo(@"C:\Temp\jsonDfMysqlComp.json.zip"));
        }

        private static double RunApproach3(string connString, FileInfo jsonFile)
        {
            var coll = new BlockingCollection<Film>(256);
            var sw = Stopwatch.StartNew();
            //Start data fetching in Parallel.
            var fetchTask = Task.Run(() => FetchData(connString, coll));

            //Equivalent to =>
            //using (var fileHandle = CreateFileWriter(@"<Full Path Of JSON file>"))
            //{
            //    var collection = new BlockingCollection();//perhaps with some capacity
            //    var dbtask = Task.Run(() => dataAccessInstance
            //            .PopulateTransactionRecordParallel(dateOfTheTransaction, collection));
            //    //writing "["
            //    fileHandle.Write(jsonArrayStartToken);
            //    foreach (var obj in collection.GetConsumingEnumerable())
            //    {
            //        //writing json string of the object
            //        fileHandle.Write(JsonConvert.SerializeObject(obj));

            //        //writing ","
            //        fileHandle.Write(objectSeparatorToken);
            //    }
            //    //writing "]"
            //    fileHandle.Write(jsonArrayEndToken);

            //    //in case, task throw some error.
            //    await dbtask.ConfigureAwait(false);
            //}
            coll.ToJsonArrayParallely(jsonFile.CreateStream(FileMode.Create));
            fetchTask.Wait();

            //Now we do GZip compression
            //Equivalent to => FileInfo compressedFile = compressionEngine.Compress(@"<Full Path Of JSON file>");
            jsonFile.CreateStream(FileMode.Open)
                .CompressAsync((jsonFile.FullName + ".zip").ToFileInfo().CreateStream(FileMode.Create)).Wait();

            sw.Stop();
            Console.Out.WriteLine("Approach3 Total Time: " + sw.Elapsed.TotalMilliseconds);
            return sw.Elapsed.TotalMilliseconds;
        }

        private static double RunDevFast(string connString, FileInfo compressedFile)
        {
            var coll = new BlockingCollection<Film>(256);
            var sw = Stopwatch.StartNew();
            //Start data fetching in Parallel.
            var fetchTask = Task.Run(() => FetchData(connString, coll));

            //this line is sufficient to let the serialization + GZip compression run in parallel
            //while DB related code is populating the BlockingCollection in parallel
            coll.ToJsonArrayParallely(compressedFile.CreateStream(FileMode.Create).CreateCompressionStream());

            fetchTask.Wait();
            sw.Stop();
            Console.Out.WriteLine("Approach5 (DevFast) Total Time: " + sw.Elapsed.TotalMilliseconds);
            return sw.Elapsed.TotalMilliseconds;
        }

        private static void RunDevFastDeserial(FileInfo fileInfo)
        {
            var sw = Stopwatch.StartNew();
            var bc = new BlockingCollection<Film>();
            var task = Task.Run(() =>
            {
                //this line is sufficient to let the deserialization + GZip decompression run in parallel
                fileInfo.CreateStream(FileMode.Open).CreateDecompressionStream().FromJsonArrayParallely(bc);
            });
            //We make a loop to count the objects
            var count = bc.GetConsumingEnumerable().Count();
            task.Wait();
            sw.Stop();
            Console.Out.WriteLine("DevFast (Deserialization+Looping) Total Time: " + sw.Elapsed.TotalMilliseconds);
            Console.Out.WriteLine("Count: " + count);
        }

        private static void FetchData(string connString, BlockingCollection<Film> collection)
        {
            try
            {
                Parallel.For(0, 1000, new ParallelOptions {MaxDegreeOfParallelism = 3}, j =>
                {
                    var chunkLimit = new[] {0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
                    Parallel.For(1, chunkLimit.Length, i =>
                    {
                        using (var conn = new MySqlConnection(connString))
                        {
                            conn.Open();
                            using (var comm = conn.CreateCommand())
                            {
                                comm.CommandText = "SELECT * FROM sakila.film where film_id <= " + chunkLimit[i] +
                                                   " and film_id > " + chunkLimit[i - 1];
                                comm.CommandType = CommandType.Text;

                                using (var reader = comm.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        collection.Add(new Film
                                        {
                                            Length =
                                                reader.IsDBNull(reader.GetOrdinal("length"))
                                                    ? null
                                                    : (int?) reader.GetInt32("length"),
                                            Description = reader.GetString("description"),
                                            Id = reader.GetInt32("film_id"),
                                            LangId =
                                                reader.IsDBNull(reader.GetOrdinal("language_id"))
                                                    ? null
                                                    : (int?) reader.GetInt32("language_id"),
                                            OriLangId =
                                                reader.IsDBNull(reader.GetOrdinal("original_language_id"))
                                                    ? null
                                                    : (int?) reader.GetInt32("original_language_id"),
                                            Rating = reader.GetString("rating"),
                                            RentalRate =
                                                reader.IsDBNull(reader.GetOrdinal("rental_rate"))
                                                    ? null
                                                    : (int?) reader.GetInt32("rental_rate"),
                                            RentalTime =
                                                reader.IsDBNull(reader.GetOrdinal("rental_duration"))
                                                    ? null
                                                    : (int?) reader.GetInt32("rental_duration"),
                                            ReplacementCost =
                                                reader.IsDBNull(reader.GetOrdinal("replacement_cost"))
                                                    ? null
                                                    : (int?) reader.GetInt32("replacement_cost"),
                                            SpecialFeature = reader.GetString("special_features"),
                                            TimeStamp = reader.GetDateTime("last_update"),
                                            Title = reader.GetString("title"),
                                            Year = reader["release_year"].ToString()
                                        });
                                    }
                                }
                            }
                        }
                    });
                });
            }
            finally
            {
                collection.CompleteAdding();
            }
        }
    }

    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public int? LangId { get; set; }
        public int? OriLangId { get; set; }
        public int? RentalTime { get; set; }
        public decimal? RentalRate { get; set; }
        public int? Length { get; set; }
        public decimal? ReplacementCost { get; set; }
        public string Rating { get; set; }
        public string SpecialFeature { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}