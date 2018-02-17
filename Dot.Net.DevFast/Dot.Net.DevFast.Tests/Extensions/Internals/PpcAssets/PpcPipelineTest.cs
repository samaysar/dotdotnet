using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals.PpcAssets
{
    [TestFixture]
    public class PpcPipelineTest
    {
        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        public async Task RunPpcAsync_Returns_Normally_When_No_Data_Is_Produced(int pc, int cc)
        {
            var producers = new IProducer<object>[pc];
            for (var i = 0; i < pc; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
            }
            var consumers = new IConsumer<object>[cc];
            for (var i = 0; i < cc; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
            }
            await PpcPipeline<object, object>.RunPpcAsync(CancellationToken.None, ConcurrentBuffer.MinSize,
                new IdentityAdapter<object>(), producers, consumers).ConfigureAwait(false);
            foreach (var consumer in consumers)
            {
                consumer.Received(1).Dispose();
                await consumer.Received(0)
                    .ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
                    .ConfigureAwait(false);
            }
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        public void RunPpcAsync_Wrappes_All_Exceptions_And_Destroys_Ppc(int pcount, int ccount)
        {
            //to make all producer consumers to throw error almost at same time!
            var countHandle = new CountdownEvent(pcount + ccount);
            var waitHandle = new ManualResetEventSlim(false);
            var producers = new IProducer<object>[pcount];
            for (var i = 0; i < pcount; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
                producers[i].ProduceAsync(Arg.Any<IConsumerFeed<object>>(),
                    Arg.Any<CancellationToken>()).Returns(x =>
                {
                    countHandle.Signal();
                    waitHandle.Wait();
                    throw new Exception("Testing");
                });
            }

            var consumers = new IConsumer<object>[ccount];
            for (var i = 0; i < ccount; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
                consumers[i].InitAsync().Returns(x =>
                {
                    countHandle.Signal();
                    waitHandle.Wait();
                    throw new Exception("Testing");
                });
            }

            var ppcTask = Task.Run(() => PpcPipeline<object, object>.RunPpcAsync(CancellationToken.None,
                ConcurrentBuffer.MinSize, new IdentityAdapter<object>(), producers, consumers));
            countHandle.Wait();
            waitHandle.Set();
            var ex = Assert.Throws<AggregateException>(() => ppcTask.Wait());
            Assert.True(ex.InnerExceptions[0].Message.Equals("Testing"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task RunPpcAsync_Forwards_Items_To_Consumer(int ccount)
        {
            //to make all consumers to recieve an item!
            var obj = new object();
            var countHandle = new CountdownEvent(ccount);
            var waitHandle = new ManualResetEventSlim(false);
            var producers = new IProducer<object>[1];
            producers[0] = Substitute.For<IProducer<object>>();
            producers[0].ProduceAsync(Arg.Any<IConsumerFeed<object>>(),
                Arg.Any<CancellationToken>()).Returns(x =>
            {
                for (var i = 0; i < ccount; i++)
                {
                    ((IConsumerFeed<object>) x[0]).Add(obj);
                }
                return Task.CompletedTask;
            });

            var consumers = new IConsumer<object>[ccount];
            for (var i = 0; i < ccount; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
                consumers[i].InitAsync().Returns(x => Task.CompletedTask);
                consumers[i].ConsumeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(x =>
                {
                    Assert.True(ReferenceEquals(x[0], obj));
                    countHandle.Signal();
                    waitHandle.Wait();
                    return Task.CompletedTask;
                });
            }

            var ppcTask = Task.Run(() => PpcPipeline<object, object>.RunPpcAsync(CancellationToken.None,
                ConcurrentBuffer.MinSize, new IdentityAdapter<object>(), producers, consumers));
            countHandle.Wait();
            waitHandle.Set();
            await ppcTask.ConfigureAwait(false);
        }
    }
}