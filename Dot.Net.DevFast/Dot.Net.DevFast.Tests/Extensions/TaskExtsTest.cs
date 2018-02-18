using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class TaskExtsTest
    {
        [Test]
        [TestCase(1,1)]
        [TestCase(10,1)]
        [TestCase(10,2)]
        [TestCase(10,10)]
        [TestCase(1,10)]
        public async Task EnumerableAction_Based_WhenAll_Method_Harmonizes(int actionCount, int concurrency)
        {
            var count = 0;
            var action = new Action(() => Interlocked.Increment(ref count));
            await CreateEnumeration(action, actionCount).WhenAll(concurrency).ConfigureAwait(false);
            Assert.True(count == actionCount);

            count = 0;
            var tokenAction = new Action<CancellationToken>(t => Interlocked.Increment(ref count));
            await CreateEnumeration(tokenAction, actionCount).WhenAll(concurrency).ConfigureAwait(false);
            Assert.True(count == actionCount);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(10, 1)]
        [TestCase(10, 2)]
        [TestCase(10, 10)]
        [TestCase(1, 10)]
        public async Task EnumerableFunc_Based_WhenAll_Method_Harmonizes(int actionCount, int concurrency)
        {
            var count = 0;
            var func = new Func<Task>(() =>
            {
                Interlocked.Increment(ref count);
                return Task.CompletedTask;
            });
            await CreateEnumeration(func, actionCount).WhenAll(concurrency).ConfigureAwait(false);
            Assert.True(count == actionCount);

            count = 0;
            var tokenFunc = new Func<CancellationToken, Task>(t =>
                {
                    return new Task(() => Interlocked.Increment(ref count));
                });
            await CreateEnumeration(tokenFunc, actionCount).WhenAll(concurrency).ConfigureAwait(false);
            Assert.True(count == actionCount);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task Repeatation_Based_WhenAll_Methods_Harmonizes(int repeatCount)
        {
            //0 based indexing... so i-1 instead of i+1
            var cumsum = (repeatCount * (repeatCount - 1)) / 2;
            var count = 0;
            var func = new Func<int, CancellationToken, Task>((i,t) =>
                {
                    return new Task(() => Interlocked.Add(ref count, i));
                });
            await func.WhenAll(repeatCount).ConfigureAwait(false);
            Assert.True(count == cumsum);

            count = 0;
            var action = new Action<int, CancellationToken>((i, t) => Interlocked.Add(ref count, i));
            await action.WhenAll(repeatCount).ConfigureAwait(false);
            Assert.True(count == cumsum);
        }

        private static IEnumerable<T> CreateEnumeration<T>(T obj, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return obj;
            }
        }
    }
}