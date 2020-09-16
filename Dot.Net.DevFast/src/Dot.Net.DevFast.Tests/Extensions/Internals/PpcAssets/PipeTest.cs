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
    public class PipeTest
    {
        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        public async Task Execute_Returns_Normally_When_No_Data_Is_Produced(int pc, int cc)
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
            await Pipe<object, object>.Execute(CancellationToken.None, ConcurrentBuffer.MinSize,
                IdentityAwaitableAdapter<object>.Default, producers, consumers).ConfigureAwait(false);
            foreach (var consumer in consumers)
            {
#if OLDNETUSING
                consumer.Received(1).Dispose();
#else
                await consumer.Received(1).DisposeAsync();
#endif
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
        public void Execute_Throws_OpCancelError_And_Destroys_Ppc_When_Token_Is_Cancelled(int pcount, int ccount)
        {
            //to make all producer consumers to throw error almost at same time!
            var producers = new IProducer<object>[pcount];
            for (var i = 0; i < pcount; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
            }

            var consumers = new IConsumer<object>[ccount];
            for (var i = 0; i < ccount; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
            }
            var cts = new CancellationTokenSource();
            cts.Cancel();
            Assert.ThrowsAsync<OperationCanceledException>(() => Pipe<object, object>.Execute(cts.Token,
                ConcurrentBuffer.MinSize, IdentityAwaitableAdapter<object>.Default, producers, consumers));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        public void Execute_Wrappes_All_Producer_Exceptions_And_Destroys_Ppc(int pcount, int ccount)
        {
            //to make all producer consumers to throw error almost at same time!
            var producers = new IProducer<object>[pcount];
            for (var i = 0; i < pcount; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
                producers[i].InitAsync().Returns(x => throw new Exception("Testing"));
            }

            var consumers = new IConsumer<object>[ccount];
            for (var i = 0; i < ccount; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
            }

            var ppcTask = Task.Run(() => Pipe<object, object>.Execute(CancellationToken.None,
                ConcurrentBuffer.MinSize, IdentityAwaitableAdapter<object>.Default, producers, consumers));
            Assert.True(Assert.ThrowsAsync<Exception>(() => ppcTask).Message.Equals("Testing"));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        public void Execute_Wrappes_All_Consumer_Exceptions_And_Destroys_Ppc(int pcount, int ccount)
        {
            //to make all producer consumers to throw error almost at same time!
            var producers = new IProducer<object>[pcount];
            for (var i = 0; i < pcount; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
            }

            var consumers = new IConsumer<object>[ccount];
            for (var i = 0; i < ccount; i++)
            {
                consumers[i] = Substitute.For<IConsumer<object>>();
                consumers[i].InitAsync().Returns(x => throw new Exception("Testing"));
            }

            var ppcTask = Task.Run(() => Pipe<object, object>.Execute(CancellationToken.None,
                ConcurrentBuffer.MinSize, IdentityAwaitableAdapter<object>.Default, producers, consumers));
            Assert.True(Assert.ThrowsAsync<Exception>(() => ppcTask).Message.Equals("Testing"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Execute_Forwards_Items_To_Consumer(int ccount)
        {
            //to make all consumers to recieve an item!
            var obj = new object();
            var countHandle = new CountdownEvent(ccount);
            var waitHandle = new ManualResetEventSlim(false);
            var producers = new IProducer<object>[1];
            producers[0] = Substitute.For<IProducer<object>>();
            producers[0].ProduceAsync(Arg.Any<IProducerBuffer<object>>(),
                Arg.Any<CancellationToken>()).Returns(x =>
            {
                for (var i = 0; i < ccount; i++)
                {
                    ((IProducerBuffer<object>) x[0]).Add(obj, CancellationToken.None);
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

            var ppcTask = Task.Run(() => Pipe<object, object>.Execute(CancellationToken.None,
                ConcurrentBuffer.MinSize, IdentityAwaitableAdapter<object>.Default, producers, consumers));
            countHandle.Wait();
            waitHandle.Set();
            await ppcTask.ConfigureAwait(false);
        }
    }
}