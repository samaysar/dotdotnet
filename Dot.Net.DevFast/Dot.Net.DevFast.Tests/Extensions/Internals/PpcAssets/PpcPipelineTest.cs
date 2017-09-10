using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class PpcPipelineTest
    {
        //        [Test]
        //        public async Task Distribute_Forwards_Items_To_Consumer()
        //        {
        //            using (var instance = new IdentityDistributor<object>(CancellationToken.None, 0))
        //            {
        //                instance.Distribute(new object());
        //                var consumer = Substitute.For<IConsumer<object>>();
        //                await instance.RunPpcAsync(new IProducer<object>[0], consumer).ConfigureAwait(false);
        //                await consumer.Received(1)
        //                    .ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
        //                    .ConfigureAwait(false);
        //            }
        //        }

        //        [Test]
        //        [TestCase(1, 1)]
        //        [TestCase(1, 10)]
        //        [TestCase(10, 10)]
        //        [TestCase(10, 1)]
        //        public async Task RunPpcAsync_Completes_Ppc_Without_Error_Even_When_Producer_Do_Not_Produce(int pcount, int ccount)
        //        {
        //            //to make all producer consumers to throw error almost at same time!
        //            var countHandle = new CountdownEvent(pcount + ccount);
        //            var waitHandle = new ManualResetEventSlim(false);
        //            var producers = new IProducer<object>[pcount];
        //            for (var i = 0; i < pcount; i++)
        //            {
        //                producers[i] = Substitute.For<IProducer<object>>();
        //                producers[i].ProduceAsync(Arg.Any<IDistributor<object>>(),
        //                    Arg.Any<CancellationToken>()).Returns(x =>
        //                    {
        //                        countHandle.Signal();
        //                        waitHandle.Wait();
        //                        return Task.CompletedTask;
        //                    });
        //            }

        //            var consumers = new IConsumer<object>[ccount];
        //            for (var i = 0; i < ccount; i++)
        //            {
        //                consumers[i] = Substitute.For<IConsumer<object>>();
        //                consumers[i].InitAsync().Returns(x =>
        //                {
        //                    countHandle.Signal();
        //                    waitHandle.Wait();
        //                    return Task.CompletedTask;
        //                });
        //            }

        //            using (var instance = new IdentityDistributor<object>(CancellationToken.None, 0))
        //            {
        //                var ppcTask = instance.RunPpcAsync(producers, consumers);
        //                countHandle.Wait();
        //                waitHandle.Set();
        //                await ppcTask.ConfigureAwait(false);
        //                foreach (var consumer in consumers)
        //                {
        //                    consumer.Received(1).Dispose();
        //                    await consumer.Received(0)
        //                        .ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
        //                        .ConfigureAwait(false);
        //                }
        //            }
        //        }

        //        [Test]
        //        [TestCase(1, 1)]
        //        [TestCase(1, 10)]
        //        [TestCase(10, 10)]
        //        [TestCase(10, 1)]
        //        public void RunPpcAsync_Wrappes_All_Exceptions_And_Destroys_Ppc(int pcount, int ccount)
        //        {
        //            //to make all producer consumers to throw error almost at same time!
        //            var countHandle = new CountdownEvent(pcount + ccount);
        //            var waitHandle = new ManualResetEventSlim(false);
        //            var producers = new IProducer<object>[pcount];
        //            for (var i = 0; i < pcount; i++)
        //            {
        //                producers[i] = Substitute.For<IProducer<object>>();
        //                producers[i].ProduceAsync(Arg.Any<IDistributor<object>>(),
        //                    Arg.Any<CancellationToken>()).Returns(x =>
        //                {
        //                    countHandle.Signal();
        //                    waitHandle.Wait();
        //                    throw new Exception("Testing");
        //                });
        //            }

        //            var consumers = new IConsumer<object>[ccount];
        //            for (var i = 0; i < ccount; i++)
        //            {
        //                consumers[i] = Substitute.For<IConsumer<object>>();
        //                consumers[i].InitAsync().Returns(x =>
        //                {
        //                    countHandle.Signal();
        //                    waitHandle.Wait();
        //                    throw new Exception("Testing");
        //                });
        //            }

        //            using (var instance = new IdentityDistributor<object>(CancellationToken.None, 0))
        //            {
        //                var ppcTask = instance.RunPpcAsync(producers, consumers);
        //                countHandle.Wait();
        //                waitHandle.Set();
        //                var err = Assert.Throws<AggregateException>(() => ppcTask.Wait());
        //                //one from Producer task and another from consumer task
        //                Assert.True(err.InnerExceptions.Count.Equals(2));
        //            }
        //        }

    }
}