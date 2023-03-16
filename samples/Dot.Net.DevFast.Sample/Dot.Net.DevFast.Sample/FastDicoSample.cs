using System;
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
            var proc = Environment.ProcessorCount;
            var perT = 500000;
            var h = new HashSet<int>();
            var r = new Random();
            while (h.Count != proc * perT)
            {
                h.Add(r.Next());
            }

            var ll = h.ToList();
            var devDico = new FastDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                devDico[i] = i;
            }

            Console.WriteLine(devDico.Count);
            devDico.Clear();
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
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            devDico.Clear();

            l = new CountdownEvent(proc);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
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

            l = new CountdownEvent(proc);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
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

            var msDico = new System.Collections.Concurrent.ConcurrentDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                msDico[i] = i;
            }

            Console.WriteLine(msDico.Count);
            msDico.Clear();
            l = new CountdownEvent(proc);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
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

            l = new CountdownEvent(proc);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
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

            l = new CountdownEvent(proc);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
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
        }
    }
}
