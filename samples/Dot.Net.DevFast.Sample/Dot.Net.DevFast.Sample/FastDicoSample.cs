using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Collections.Concurrent;

namespace Dot.Net.DevFast.Sample
{
    internal static class FastDicoSample
    {
        public static void Run()
        {
            RunAddStats();
            Console.WriteLine("-----------------------");
            RunConcurrencyConsistency();
        }

        private static void RunConcurrencyConsistency()
        {
            var proc = Environment.ProcessorCount;
            var perT = 500000;
            var h = new HashSet<int>();
            var r = new Random();
            while (h.Count != proc * perT)
            {
                h.Add(r.Next());
            }

            var adderData = h.ToList();
            var removerData1 = h.OrderByDescending(x => x).Skip(2500000).ToList();
            var removerData2 = h.OrderBy(x => x).Skip(2500000).ToList();

            var devDico = new FastDictionary<int, int>();
            var l = new CountdownEvent(proc + 2);
            var count = 0;
            var sw = Stopwatch.StartNew();
            Parallel.Invoke(() =>
            {
                Parallel.For(0, proc, ii =>
                {
                    var start = ii * perT;
                    var stop = start + perT;
                    if (!l.Signal())
                    {
                        l.Wait();
                    }

                    for (int i = start; i < stop; i++)
                    {
                        if (devDico.TryAdd(adderData[i], i))
                        {
                            Interlocked.Increment(ref count);
                        }
                    }
                });
            }, () =>
            {
                Parallel.For(0, 2, ii =>
                {
                    var data = ii == 0 ? removerData1 : removerData2;
                    if (!l.Signal())
                    {
                        l.Wait();
                    }

                    foreach (var p in data)
                    {
                        if (devDico.TryRemove(p, out _))
                        {
                            Interlocked.Decrement(ref count);
                        }
                    }
                });
            });

            sw.Stop();
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds + ", " + count);
            devDico.Clear();
        }

        private static void RunAddStats()
        {
            var proc = Environment.ProcessorCount;
            var perT = 500000;
            var h = new HashSet<int>();
            var r = new Random();
            while (h.Count != proc * perT)
            {
                h.Add(r.Next());
            }

            var ll = h.ToList();

            var msDico = new ConcurrentDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                msDico[i] = i;
            }

            Console.WriteLine(msDico.Count);
            msDico.Clear();

            RunMsDico(proc, perT, ll);
            RunMsDico(proc, perT, ll);
            RunMsDico(proc, perT, ll);

            var devDico = new FastDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                devDico[i] = i;
            }

            Console.WriteLine(devDico.Count);
            devDico.Clear();

            RunFastDico(proc, perT, ll);
            RunFastDico(proc, perT, ll);
            RunFastDico(proc, perT, ll);            
        }

        private static void RunMsDico(int proc, int perT, List<int> ll)
        {
            var msDico = new ConcurrentDictionary<int, int>();
            var l = new CountdownEvent(proc);
            var sw = Stopwatch.StartNew();
            var init = GC.GetTotalMemory(true);
            Parallel.For(0, proc, ii =>
            {
                var start = ii * perT;
                var stop = start + perT;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) msDico[ll[i]] = i;
            });

            sw.Stop();
            Console.WriteLine(msDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            msDico.Clear();
        }

        private static void RunFastDico(int proc, int perT, List<int> ll)
        {
            var devDico = new FastDictionary<int, int>();
            CountdownEvent l;
            Stopwatch sw;
            l = new CountdownEvent(proc);
            sw = new Stopwatch();
            var init = GC.GetTotalMemory(true);
            Parallel.For(0, proc, ii =>
            {
                var start = ii * perT;
                var stop = start + perT;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) devDico[ll[i]] = i;
            });

            sw.Stop();
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            devDico.Clear();
        }
    }
}
