﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using NSubstitute;
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

            var errorWithTokenAction = new Action<CancellationToken>(t => throw new Exception("with token"));
            Assert.True(Assert.ThrowsAsync<Exception>(() => CreateEnumeration(errorWithTokenAction, actionCount)
                .WhenAll(concurrency)).Message.Equals("with token"));

            var errorAction = new Action(() => throw new Exception("with token"));
            Assert.True(Assert.ThrowsAsync<Exception>(() => CreateEnumeration(errorAction, actionCount)
                .WhenAll(concurrency)).Message.Equals("with token"));

            var errorCount = 0;
            var errorHandle = new Action<Exception>(e =>
            {
                Assert.True(e.Message.Equals("with token"));
                Interlocked.Increment(ref errorCount);
            });
            await CreateEnumeration(errorWithTokenAction, actionCount).WhenAll(concurrency, errorHandler: errorHandle)
                .ConfigureAwait(false);
            Assert.True(errorCount == actionCount);
            await CreateEnumeration(errorAction, actionCount).WhenAll(concurrency, errorHandler: errorHandle)
                .ConfigureAwait(false);
            Assert.True(errorCount == 2*actionCount);
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

            var errorWithTokenFunc = new Func<CancellationToken, Task>(t => throw new Exception("with token"));
            Assert.True(Assert.ThrowsAsync<Exception>(() => CreateEnumeration(errorWithTokenFunc, actionCount)
                .WhenAll(concurrency)).Message.Equals("with token"));

            var errorFunc = new Func<Task>(() => throw new Exception("with token"));
            Assert.True(Assert.ThrowsAsync<Exception>(() => CreateEnumeration(errorFunc, actionCount)
                .WhenAll(concurrency)).Message.Equals("with token"));

            var errorCount = 0;
            var errorHandle = new Action<Exception>(e =>
            {
                Assert.True(e.Message.Equals("with token"));
                Interlocked.Increment(ref errorCount);
            });
            await CreateEnumeration(errorWithTokenFunc, actionCount).WhenAll(concurrency, errorHandler: errorHandle)
                .ConfigureAwait(false);
            Assert.True(errorCount == actionCount);
            await CreateEnumeration(errorFunc, actionCount).WhenAll(concurrency, errorHandler: errorHandle)
                .ConfigureAwait(false);
            Assert.True(errorCount == 2 * actionCount);
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

        [Test]
        public async Task StartIfNeeded_Starts_The_Task_Only_If_Not_Already_Started()
        {
            var o = new object();
            var tt = new Task<object>(() => o);
            Assert.True(tt.Status == TaskStatus.Created);
            tt = tt.StartIfNeeded();
            Assert.True(tt.Status != TaskStatus.Created);
            Assert.True(ReferenceEquals(await tt.ConfigureAwait(false), o));
        }

        [Test]
        public async Task AwaitNDispose_Properly_Disposes_After_Await()
        {
            var callCount = 0;
            var disposable = Substitute.For<IDisposable>();
            var awaitable = Task.CompletedTask;
            disposable.When(x => x.Dispose()).Do(x => Interlocked.Increment(ref callCount));
            await awaitable.AwaitNDispose(disposable).ConfigureAwait(false);
            //There will be 2 calls, one we made inside task and another by method
            Assert.IsTrue(callCount.Equals(1));
#if NETASYNCDISPOSE
            callCount = 0;
            var asyncDisposable = Substitute.For<IAsyncDisposable>();
            var asyncAwaitable = Task.CompletedTask;
            asyncDisposable.DisposeAsync().Returns(x =>
            {
                Interlocked.Increment(ref callCount);
                return default;
            });
            await asyncAwaitable.AwaitNDispose(asyncDisposable).ConfigureAwait(false);
            Assert.IsTrue(callCount.Equals(1));
#endif
        }

        [Test]
        public async Task AwaitNDisposeAsync_Properly_Disposes_After_Await()
        {
            var callCount = 0;
            var disposable = Substitute.For<IDisposable>();
            var awaitable = Task.CompletedTask;
            disposable.When(x => x.Dispose()).Do(x => Interlocked.Increment(ref callCount));
            await awaitable.AwaitNDisposeAsync(disposable).ConfigureAwait(false);
            //There will be 2 calls, one we made inside task and another by method
            Assert.IsTrue(callCount.Equals(1));
#if NETASYNCDISPOSE
            callCount = 0;
            var asyncDisposable = Substitute.For<IAsyncDisposable>();
            var asyncAwaitable = Task.CompletedTask;
            asyncDisposable.DisposeAsync().Returns(x =>
            {
                Interlocked.Increment(ref callCount);
                return default;
            });
            await asyncAwaitable.AwaitNDisposeAsync(asyncDisposable).ConfigureAwait(false);
            Assert.IsTrue(callCount.Equals(1));
#endif
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