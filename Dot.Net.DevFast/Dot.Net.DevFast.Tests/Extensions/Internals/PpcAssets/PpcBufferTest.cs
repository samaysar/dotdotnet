using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class PpcBufferTest
    {
        [Test]
        public void TryGet_Throws_Error_When_Cancellation_Is_Demanded()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();
                using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, cts.Token))
                {
                    Assert.Throws<OperationCanceledException>(() => instance.TryGet(Timeout.Infinite, out var data));
                }
            }
        }

        [Test]
        public void Add_Throws_Error_When_Cancellation_Is_Demanded()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();
                using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, cts.Token))
                {
                    Assert.Throws<OperationCanceledException>(() => instance.Add(new object()));
                }
            }
        }

        [Test]
        public void TryGet_Returns_False_After_Close()
        {
            using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None))
            {
                instance.Close();
                Assert.False(instance.TryGet(Timeout.Infinite, out var data));
            }
        }

        [Test]
        public void Add_Throws_Error_When_After_Close()
        {
            using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None))
            {
                instance.Close();
                Assert.Throws<InvalidOperationException>(() => instance.Add(new object()));
            }
        }

        [Test]
        public void TryGet_Throws_Error_After_Dispose()
        {
            using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None))
            {
                instance.Dispose();
                Assert.Throws<NullReferenceException>(() => instance.TryGet(Timeout.Infinite, out var obj));
            }
        }

        [Test]
        public void Add_Throws_Error_When_After_Dispose()
        {
            using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None))
            {
                instance.Dispose();
                Assert.Throws<NullReferenceException>(() => instance.Add(new object()));
            }
        }

        [Test]
        public void TryGet_And_Add_Harmonize()
        {
            using (var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None))
            {
                var obj = new object();
                instance.Add(obj);
                Assert.True(instance.TryGet(Timeout.Infinite, out var newObj) && ReferenceEquals(newObj, obj));
                instance.Close();
            }
        }

        [Test]
        public async Task TryGet_Blocks_The_Call_If_Close_Not_Called_Returns_Added_Element_If_Available()
        {
            var instance = new PpcBuffer<object>(ConcurrentBuffer.Unbounded, CancellationToken.None);
            var obj = new object();
            object outObj = null;
            var tryGetTask = Task.Run(() => instance.TryGet(Timeout.Infinite, out outObj));
            Assert.True(tryGetTask.Status != TaskStatus.RanToCompletion);
            instance.Add(obj);
            Assert.True(await tryGetTask.ConfigureAwait(false) && ReferenceEquals(outObj, obj));
            tryGetTask = Task.Run(() => instance.TryGet(Timeout.Infinite, out outObj));
            Assert.True(tryGetTask.Status != TaskStatus.RanToCompletion);
            instance.Close();
            Assert.False(await tryGetTask.ConfigureAwait(false));
            Assert.True(instance.Finished);
            using (instance)
            {
                //to dispose.
            }
        }
    }
}