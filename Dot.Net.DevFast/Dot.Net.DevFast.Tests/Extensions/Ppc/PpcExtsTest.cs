using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class PpcExtsTest
    {
        [Test]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void List_Based_RunProducerConsumerAsync_Throws_Error_When_List_Size_Less_Than_2(int size)
        {
            var ex = Assert.ThrowsAsync<DdnDfException>(async () =>
                await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<List<object>>(1)[0], size)
                    .ConfigureAwait(false));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            ex = Assert.ThrowsAsync<DdnDfException>(async () =>
                await GetProducers().RunProducerConsumerAsync(GetConsumers<List<object>>(1)[0], size)
                    .ConfigureAwait(false));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            ex = Assert.ThrowsAsync<DdnDfException>(async () =>
                await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<List<object>>(), size)
                    .ConfigureAwait(false));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            ex = Assert.ThrowsAsync<DdnDfException>(async () =>
                await GetProducers().RunProducerConsumerAsync(GetConsumers<List<object>>(), size)
                    .ConfigureAwait(false));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
        }

        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside PpcPipeline implementation!

        [Test]
        public async Task One_To_One_RunProducerConsumerAsync_Harmonizes()
        {
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<object>(1)[0]).ConfigureAwait(false);
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<List<object>>(1)[0], 3).ConfigureAwait(false);
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<object>(1)[0], new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task One_To_Many_RunProducerConsumerAsync_Harmonizes()
        {
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<object>()).ConfigureAwait(false);
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<List<object>>(), 3).ConfigureAwait(false);
            await GetProducers(1)[0].RunProducerConsumerAsync(GetConsumers<object>(), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_One_RunProducerConsumerAsync_Harmonizes()
        {
            await GetProducers().RunProducerConsumerAsync(GetConsumers<object>(1)[0]).ConfigureAwait(false);
            await GetProducers().RunProducerConsumerAsync(GetConsumers<List<object>>(1)[0], 3).ConfigureAwait(false);
            await GetProducers().RunProducerConsumerAsync(GetConsumers<object>(1)[0], new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_Many_RunProducerConsumerAsync_Harmonizes()
        {
            await GetProducers().RunProducerConsumerAsync(GetConsumers<object>()).ConfigureAwait(false);
            await GetProducers().RunProducerConsumerAsync(GetConsumers<List<object>>(), 3).ConfigureAwait(false);
            await GetProducers().RunProducerConsumerAsync(GetConsumers<object>(), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        private static IProducer<object>[] GetProducers(int count = 2)
        {
            var producers = new IProducer<object>[count];
            for (var i = 0; i < count; i++)
            {
                producers[i] = Substitute.For<IProducer<object>>();
                producers[i].ProduceAsync(Arg.Any<IConsumerFeed<object>>(),
                    Arg.Any<CancellationToken>()).Returns(x => Task.CompletedTask);
            }
            return producers;
        }

        private static IConsumer<T>[] GetConsumers<T>(int count = 2)
        {
            var consumers = new IConsumer<T>[count];
            for (var i = 0; i < count; i++)
            {
                consumers[i] = Substitute.For<IConsumer<T>>();
                consumers[i].InitAsync().Returns(x => Task.CompletedTask);
            }
            return consumers;
        }
    }
}