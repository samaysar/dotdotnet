using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Collections.Concurrent;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections.Concurrent
{
    [TestFixture]
    public class ConcurrentDictionaryTest
    {
        [Test]
        public void Perf_Test()
        {
            var devDico = new ConcurrentDictionary<int, int>();
            for (int i = 0; i < 10; i++)
            {
                devDico[i] = i;
            }

            Console.WriteLine(devDico.Count);
            devDico.Clear();
            var l = new CountdownEvent(8);
            var sw = new Stopwatch();
            var init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) devDico[i] = i;
            });

            sw.Stop();
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            devDico.Clear();

            l = new CountdownEvent(8); 
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) devDico[i] = i;
            });

            sw.Stop();
            Console.WriteLine(devDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            devDico.Clear();

            l = new CountdownEvent(8);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) devDico[i] = i;
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
            l = new CountdownEvent(8);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) msDico[i] = i;
            });

            sw.Stop();
            Console.WriteLine(msDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            msDico.Clear();

            l = new CountdownEvent(8);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) msDico[i] = i;
            });

            sw.Stop();
            Console.WriteLine(msDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
            msDico.Clear();

            l = new CountdownEvent(8);
            sw = Stopwatch.StartNew();
            init = GC.GetTotalMemory(true);
            Parallel.For(0, 8, ii =>
            {
                var start = ii * 500000;
                var stop = start + 500000;
                if (!l.Signal())
                {
                    l.Wait();
                }
                else
                {
                    sw.Restart();
                }

                for (int i = start; i < stop; i++) msDico[i] = i;
            });

            sw.Stop();
            Console.WriteLine(msDico.Count + ", " + sw.ElapsedMilliseconds + ", " + (GC.GetTotalMemory(true) - init));
        }
    }
}