using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class PipelineTest
    {
        [Test]
        public async Task Post_Dispose_Or_TearDown_Call_Accept_Throws_DisposedError()
        {
            var consumers = new IConsumer<object>[1];
            consumers[0] = Substitute.For<IConsumer<object>>();
            var instance = new Pipeline<object, object>(consumers, IdentityAwaitableAdapter<object>.Default,
                CancellationToken.None, 1);
#if NETASYNCDISPOSE
            await using (instance.ConfigureAwait(false))
#else
            using (instance)
#endif
            {
            }

            Assert.Throws<ObjectDisposedException>(() => instance.Add(new object(), CancellationToken.None));

            instance = new Pipeline<object, object>(consumers, IdentityAwaitableAdapter<object>.Default,
                CancellationToken.None, 1);
            await instance.TearDown().ConfigureAwait(false);
            Assert.Throws<ObjectDisposedException>(() => instance.Add(new object(), CancellationToken.None));
        }

        [Test]
        public async Task TryAdd_Returns_False_On_Timeout_When_Unable_To_Add_Item()
        {
            using (var handle = new ManualResetEventSlim(false))
            {
                using (var consumerHandle = new ManualResetEventSlim(false))
                {
                    var consumers = new IConsumer<object>[1];
                    consumers[0] = Substitute.For<IConsumer<object>>();
                    consumers[0].ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
                        .Returns(x =>
                        {
                            consumerHandle.Set();
                            handle.Wait();
                            return Task.CompletedTask;
                        });
                    var instance = new Pipeline<object, object>(consumers, IdentityAwaitableAdapter<object>.Default,
                        CancellationToken.None, 1);
#if NETASYNCDISPOSE
                    await using (instance.ConfigureAwait(false))
#else
                    using (instance)
#endif
                    {
                        instance.Add(new object(), CancellationToken.None); //this one will reach consumer
                        consumerHandle.Wait();
                        instance.Add(new object(), CancellationToken.None); //this one will stay in buffer
                        Assert.False(instance.TryAdd(new object(), 0, CancellationToken.None)); //this one we wont be able to add
                        handle.Set();
                    }

                    await Task.CompletedTask;
                }
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task TearDown_Returns_Normally_When_No_Data_Is_Produced_N_Consumers_Are_Disposed(int cc)
        {
            var consumers = new IConsumer<object>[cc];
            for (var i = 0; i < cc; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
            }

            var instance = new Pipeline<object, object>(consumers, IdentityAwaitableAdapter<object>.Default,
                CancellationToken.None, 1);
#if NETASYNCDISPOSE
            await using (instance.ConfigureAwait(false))
#else
            using (instance)
#endif
            {
                await instance.TearDown().ConfigureAwait(false);
                foreach (var consumer in consumers)
                {
                    await consumer.Received(1).InitAsync().ConfigureAwait(false);
#if !NETASYNCDISPOSE
                    consumer.Received(1).Dispose();
#else
                    await consumer.Received(1).DisposeAsync();
#endif
                    await consumer.Received(0).ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
                        .ConfigureAwait(false);
                }
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void When_Token_Is_Canceled_The_Consumers_Are_Disposed_Pipeline_Remains_Alive_But_Accept_Throws_OpCancelErr(int cc)
        {
            Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                var countdownHandle = new CountdownEvent(cc);
                var consumers = new IConsumer<object>[cc];
                for (var i = 0; i < cc; i++)
                {
                    consumers[i] = Substitute.For<IConsumer<object>>();
#if !NETASYNCDISPOSE
                    consumers[i].When(x => x.Dispose()).Do(x => countdownHandle.Signal());
#else
                    consumers[i].DisposeAsync().Returns(x =>
                    {
                        countdownHandle.Signal();
                        return default;
                    });
#endif
                }

                var cts = new CancellationTokenSource();
                var instance = new Pipeline<object, object>(consumers, IdentityAwaitableAdapter<object>.Default,
                    cts.Token, 1);
#if NETASYNCDISPOSE
                await using (instance.ConfigureAwait(false))
#else
                using (instance)
#endif
                {
                    cts.Cancel();
                    //we wait before checking received calls...! it means our dispose counts are correct!
                    countdownHandle.Wait(CancellationToken.None);
                    foreach (var consumer in consumers)
                    {
                        await consumer.Received(1).InitAsync().ConfigureAwait(false);
                        await consumer.Received(0).ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
                            .ConfigureAwait(false);
                    }

                    Assert.Throws<OperationCanceledException>(() => instance.Add(new object(), CancellationToken.None));
                }
            });
            //task cancel error is thrown due to TearDown call!
        }


        [Test]
        public async Task Objects_Are_Properly_Passed_To_Consumers_With_Adapter()
        {
            var consumers = new IConsumer<List<object>>[1];
            consumers[0] = Substitute.For<IConsumer<List<object>>>();

            var instance = new Pipeline<object, List<object>>(consumers,
                new IdentityAwaitableListAdapter<object>(2, 0), CancellationToken.None, 1);
#if NETASYNCDISPOSE
            await using (instance.ConfigureAwait(false))
#else
            using (instance)
#endif
            {
                Assert.True(instance.UnconsumedCount == 0);
                instance.Add(new object(), CancellationToken.None);
                Assert.True(instance.UnconsumedCount < 2);
            }

            //we check all the counts after dispose! as per documented algo... dispose waits for all
            //consumers to finish consuming objects!
            foreach (var consumer in consumers)
            {
                await consumer.Received(1).InitAsync().ConfigureAwait(false);
                await consumer.Received(1).ConsumeAsync(Arg.Any<List<object>>(), Arg.Any<CancellationToken>())
                    .ConfigureAwait(false);
#if !NETASYNCDISPOSE
                consumer.Received(1).Dispose();
#else
                await consumer.Received(1).DisposeAsync();
#endif
            }
        }
    }
}