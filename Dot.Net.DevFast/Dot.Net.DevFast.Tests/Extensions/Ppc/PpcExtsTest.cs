using System;
using System.Collections.Generic;
using System.Linq;
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
        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside PpcPipeline implementation!

        [Test]
        public async Task One_To_One_RunProducerConsumerAsync_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task One_To_Many_RunProducerConsumerAsync_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_One_RunProducerConsumerAsync_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_Many_RunProducerConsumerAsync_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(consumer).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).RunProducerConsumerAsync(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .RunProducerConsumerAsync(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        private static IProducer<object> Producer()
        {
            return Substitute.For<IProducer<object>>();
        }

        private static IProducer<object>[] Producer(int count)
        {
            var producers = new IProducer<object>[count];
            for (var i = 0; i < count; i++) producers[i] = Producer();
            return producers;
        }

        private static Func<IConsumerFeed<object>, CancellationToken, Task> ProducerFunc(IProducer<object> p)
        {
            return p.ProduceAsync;
        }

        private static Action<IConsumerFeed<object>, CancellationToken> ProducerAction(IProducer<object> p)
        {
            return (f, t) => p.ProduceAsync(f, t).Wait(t);
        }

        private static Func<IConsumerFeed<object>, CancellationToken, Task>[] ProducerFunc(IEnumerable<IProducer<object>> p)
        {
            return p.Select(ProducerFunc).ToArray();
        }

        private static Action<IConsumerFeed<object>, CancellationToken>[] ProducerAction(IEnumerable<IProducer<object>> p)
        {
            return p.Select(ProducerAction).ToArray();
        }

        private static IConsumer<T> Consumer<T>()
        {
            return Substitute.For<IConsumer<T>>();
        }

        private static IConsumer<T>[] Consumer<T>(int count)
        {
            var consumers = new IConsumer<T>[count];
            for (var i = 0; i < count; i++) consumers[i] = Consumer<T>();
            return consumers;
        }

        private static Func<T, CancellationToken, Task> ConsumerFunc<T>(IConsumer<T> c)
        {
            return c.ConsumeAsync;
        }

        private static Action<T, CancellationToken> ConsumerAction<T>(IConsumer<T> c)
        {
            return (o, t) => c.ConsumeAsync(o, t).Wait(t);
        }

        private static Func<T, CancellationToken, Task>[] ConsumerFunc<T>(IEnumerable<IConsumer<T>> c)
        {
            return c.Select(ConsumerFunc).ToArray();
        }

        private static Action<T, CancellationToken>[] ConsumerAction<T>(IEnumerable<IConsumer<T>> c)
        {
            return c.Select(ConsumerAction).ToArray();
        }
    }
}