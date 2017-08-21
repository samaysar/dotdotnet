using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using Dot.Net.DevFast.Extensions.JsonExt;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Sample.JsonSample.JsonReportDb
{
    public static class PpcMysql
    {
        public static void Run()
        {
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
            var coll = new BlockingCollection<Film>(256);

            var sw = Stopwatch.StartNew();

            //Start data fetching in Parallel.
            var fetchTask = Task.Run(() => FetchData(connStr.ToString(), coll));

            //Single line to serialize the BlockingCollection to the file.
            //Serialization would terminate as soon as collection.CompleteAdding(); is called
            //fetching and serialization runs in parallel.
            var serialTask = Task.Run(() =>
                coll.ToJsonArrayParallely(new FileInfo(@"C:\Temp\jsonDfMysql.json").CreateStream(FileMode.Create))
            );
            
            //We wait both tasks to finish.
            Task.WaitAll(serialTask, fetchTask);

            sw.Stop();
            Console.Out.WriteLine("Total Time: " + sw.Elapsed.TotalMilliseconds);
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