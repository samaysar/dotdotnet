using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class PpcWrappersTest
    {
        [Test]
        public async Task AsyncProducer_SimplyCalls_Ctor_Supplied_Func()
        {
            var called = 0;
            var funcSubstitute = new Func<IProducerBuffer<object>, CancellationToken, Task>((f, t) =>
                {
                    Interlocked.Increment(ref called);
                    return Task.CompletedTask;
                });
#if OLDNETUSING
            using (var producer = new AsyncProducer<object>(funcSubstitute))
#else
            var producer = new AsyncProducer<object>(funcSubstitute);
            await using (producer.ConfigureAwait(false))
#endif
            {
                Assert.True(producer.InitAsync().Equals(Task.CompletedTask));
                await producer.ProduceAsync(Substitute.For<IProducerBuffer<object>>(), CancellationToken.None);
                Assert.True(called == 1);
            }
        }

        [Test]
        public async Task AsyncConsumer_SimplyCalls_Ctor_Supplied_Func()
        {
            var called = 0;
            var funcSubstitute = new Func<object, CancellationToken, Task>((f, t) =>
            {
                Interlocked.Increment(ref called);
                return Task.CompletedTask;
            });
#if OLDNETUSING
            using (var producer = new AsyncConsumer<object>(funcSubstitute))
#else
            var producer = new AsyncConsumer<object>(funcSubstitute);
            await using (producer.ConfigureAwait(false))
#endif
            {
                Assert.True(producer.InitAsync().Equals(Task.CompletedTask));
                await producer.ConsumeAsync(null, CancellationToken.None);
                Assert.True(called == 1);
            }
        }
    }
}