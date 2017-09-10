using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public static class EnumerableMysqlCompression
    {
        public static void Run()
        {
            Console.Out.WriteLine("Running IEnumerable on MySql with compression");
            var oraConnDetails = File.ReadAllText(@"c:/temp/oracleConn.txt").Split('|');
            var connStr = new MySqlConnectionStringBuilder
            {
                Server = oraConnDetails[0],
                UserID = oraConnDetails[1],
                Password = oraConnDetails[2],
                Pooling = true,
                MinimumPoolSize = 5,
                MaximumPoolSize = 30
            };

            var jsonTime = RunApproach2(connStr.ToString(), new FileInfo(@"C:\Temp\jsonA2MysqlComp.json"));
            var devfastTime = RunDevFast(connStr.ToString(), new FileInfo(@"C:\Temp\jsonDfA4MysqlComp.json.zip"));
            var dfFastness = ((int)((100 - (devfastTime / jsonTime * 100)) * 100)) / 100.0;
            Console.Out.WriteLine("DevFast " + Math.Abs(dfFastness) + (dfFastness < 0 ? " % Slower" : " % Faster"));

            RunDevFastDeserial(new FileInfo(@"C:\Temp\jsonDfA4MysqlComp.json.zip"));
        }

        private static double RunApproach2(string connString, FileInfo jsonFile)
        {
            var sw = Stopwatch.StartNew();

            //Equivalent to =>
            //using (var fileHandle = CreateFileWriter(@"<Full Path Of JSON file>"))
            //{
            //    //writing "["
            //    fileHandle.Write(jsonArrayStartToken);
            //    foreach (var obj in dataAccessInstance.FetchTransactionRecordAsEnumerable(dateOfTheTransaction))
            //    {
            //        //writing json string of the object
            //        fileHandle.Write(JsonConvert.SerializeObject(obj));

            //        //writing ","
            //        fileHandle.Write(objectSeparatorToken);
            //    }
            //    //writing "]"
            //    fileHandle.Write(jsonArrayEndToken);
            //}
            FetchData(connString).ToJsonArray(jsonFile.CreateStream(FileMode.Create));

            //Now we do GZip compression
            //Equivalent to => FileInfo compressedFile = compressionEngine.Compress(@"<Full Path Of JSON file>");
            jsonFile.CreateStream(FileMode.Open)
                .CompressAsync((jsonFile.FullName + ".zip").ToFileInfo().CreateStream(FileMode.Create)).Wait();

            sw.Stop();
            Console.Out.WriteLine("Approach2 Total Time: " + sw.Elapsed.TotalMilliseconds);
            return sw.Elapsed.TotalMilliseconds;
        }

        private static double RunDevFast(string connString, FileInfo compressedFile)
        {
            var sw = Stopwatch.StartNew();

            //this line is sufficient to let the serialization + GZip compression 
            FetchData(connString).ToJsonArray(compressedFile.CreateStream(FileMode.Create).CreateCompressionStream());

            sw.Stop();
            Console.Out.WriteLine("Approach4 (DevFast) Total Time: " + sw.Elapsed.TotalMilliseconds);
            return sw.Elapsed.TotalMilliseconds;
        }

        private static void RunDevFastDeserial(FileInfo fileInfo)
        {
            var sw = Stopwatch.StartNew();
            //We make a loop to count the objects
            //everything in this single line (deserialization + decompression)

            var count =
                fileInfo.CreateStream(FileMode.Open).CreateDecompressionStream().FromJsonAsEnumerable<Film>().Count();
            sw.Stop();
            Console.Out.WriteLine("DevFast (Deserialization+Looping) Total Time: " + sw.Elapsed.TotalMilliseconds);
            Console.Out.WriteLine("Count: " + count);
        }

        private static IEnumerable<Film> FetchData(string connString)
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                for (var j = 0; j < 1000; j++)
                {
                    var chunkLimit = new[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
                    for (var i = 1; i < chunkLimit.Length; i++)
                    {

                        using (var comm = conn.CreateCommand())
                        {
                            comm.CommandText = "SELECT * FROM sakila.film where film_id <= " + chunkLimit[i] +
                                               " and film_id > " + chunkLimit[i - 1];
                            comm.CommandType = CommandType.Text;

                            using (var reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    yield return new Film
                                    {
                                        Length =
                                            reader.IsDBNull(reader.GetOrdinal("length"))
                                                ? null
                                                : (int?)reader.GetInt32("length"),
                                        Description = reader.GetString("description"),
                                        Id = reader.GetInt32("film_id"),
                                        LangId =
                                            reader.IsDBNull(reader.GetOrdinal("language_id"))
                                                ? null
                                                : (int?)reader.GetInt32("language_id"),
                                        OriLangId =
                                            reader.IsDBNull(reader.GetOrdinal("original_language_id"))
                                                ? null
                                                : (int?)reader.GetInt32("original_language_id"),
                                        Rating = reader.GetString("rating"),
                                        RentalRate =
                                            reader.IsDBNull(reader.GetOrdinal("rental_rate"))
                                                ? null
                                                : (int?)reader.GetInt32("rental_rate"),
                                        RentalTime =
                                            reader.IsDBNull(reader.GetOrdinal("rental_duration"))
                                                ? null
                                                : (int?)reader.GetInt32("rental_duration"),
                                        ReplacementCost =
                                            reader.IsDBNull(reader.GetOrdinal("replacement_cost"))
                                                ? null
                                                : (int?)reader.GetInt32("replacement_cost"),
                                        SpecialFeature = reader.GetString("special_features"),
                                        TimeStamp = reader.GetDateTime("last_update"),
                                        Title = reader.GetString("title"),
                                        Year = reader["release_year"].ToString()
                                    };
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}