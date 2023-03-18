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
                        devDico.TryAdd(adderData[i], i);
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
                        devDico.TryRemove(p, out _);
                    }
                });
            });

            sw.Stop();
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds);
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
            Console.WriteLine("ConcurrentDictionary (using " + proc + " threads): Added " + msDico.Count + " in pairs in " + sw.ElapsedMilliseconds + " ms using " + (GC.GetTotalMemory(true) - init) + " bytes of memory");
            var iniCount = msDico.Count;
            l = new CountdownEvent(proc);
            sw = new Stopwatch();
            IDictionary<int, int> d2 = msDico;
            ll = ll.OrderBy(x => x).ToList();
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

                for (int i = start; i < stop; i++) d2.Remove(ll[i]);
            });
            sw.Stop();
            Console.WriteLine("ConcurrentDictionary (using " + proc + " threads): Removed " + (iniCount - msDico.Count) + " in pairs in " + sw.ElapsedMilliseconds + " ms");
            msDico.Clear();
        }

        private static void RunFastDico(int proc, int perT, List<int> ll)
        {
            var devDico = new FastDictionary<int, int>();
            var l = new CountdownEvent(proc);
            var sw = new Stopwatch();
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
            Console.WriteLine("FastDictionary (using " + proc + " threads): Added " + devDico.Count + " in pairs in " + sw.ElapsedMilliseconds + " ms using " + (GC.GetTotalMemory(true) - init) + " bytes of memory");
            var iniCount = devDico.Count;
            l = new CountdownEvent(proc);
            sw = new Stopwatch();
            ll = ll.OrderBy(x => x).ToList();
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

                for (int i = start; i < stop; i++) devDico.Remove(ll[i]);
            });
            sw.Stop();
            Console.WriteLine("FastDictionary (using " + proc + " threads): Removed " + (iniCount - devDico.Count) + " in pairs in " + sw.ElapsedMilliseconds + " ms");
            devDico.Clear();
        }
    }
}
