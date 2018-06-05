using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals.PpcAssets;
using Dot.Net.DevFast.Extensions.Ppc;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class PipeExtsTest
    {
        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside PpcPipeline implementation!

        [Test]
        public async Task One_To_One_ProducerConsumer_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.Pipe(consumer).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.Pipe(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task One_To_Many_ProducerConsumer_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.Pipe(consumer).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.Pipe(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_One_ProducerConsumer_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.Pipe(consumer).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.Pipe(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_Many_ProducerConsumer_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.Pipe(consumer).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3).ConfigureAwait(false);
            await producer.Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.Pipe(consumer, new IdentityAdapter<object>()).ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(consumer).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(consumer, new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerFunc(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await producer.Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerFunc(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);

            await ProducerAction(producer).Pipe(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).Pipe(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .Pipe(ConsumerAction(consumer), new IdentityAdapter<object>())
                .ConfigureAwait(false);
        }

        internal static IProducer<object> Producer()
        {
            return Substitute.For<IProducer<object>>();
        }

        internal static IProducer<object>[] Producer(int count)
        {
            var producers = new IProducer<object>[count];
            for (var i = 0; i < count; i++) producers[i] = Producer();
            return producers;
        }

        internal static Func<IConsumerFeed<object>, CancellationToken, Task> ProducerFunc(IProducer<object> p)
        {
            return p.ProduceAsync;
        }

        internal static Action<IConsumerFeed<object>, CancellationToken> ProducerAction(IProducer<object> p)
        {
            return (f, t) => p.ProduceAsync(f, t).Wait(t);
        }

        internal static Func<IConsumerFeed<object>, CancellationToken, Task>[] ProducerFunc(IEnumerable<IProducer<object>> p)
        {
            return p.Select(ProducerFunc).ToArray();
        }

        internal static Action<IConsumerFeed<object>, CancellationToken>[] ProducerAction(IEnumerable<IProducer<object>> p)
        {
            return p.Select(ProducerAction).ToArray();
        }

        internal static IConsumer<T> Consumer<T>()
        {
            return Substitute.For<IConsumer<T>>();
        }

        internal static IConsumer<T>[] Consumer<T>(int count)
        {
            var consumers = new IConsumer<T>[count];
            for (var i = 0; i < count; i++) consumers[i] = Consumer<T>();
            return consumers;
        }

        internal static Func<T, CancellationToken, Task> ConsumerFunc<T>(IConsumer<T> c)
        {
            return c.ConsumeAsync;
        }

        internal static Action<T, CancellationToken> ConsumerAction<T>(IConsumer<T> c)
        {
            return (o, t) => c.ConsumeAsync(o, t).Wait(t);
        }

        internal static Func<T, CancellationToken, Task>[] ConsumerFunc<T>(IEnumerable<IConsumer<T>> c)
        {
            return c.Select(ConsumerFunc).ToArray();
        }

        internal static Action<T, CancellationToken>[] ConsumerAction<T>(IEnumerable<IConsumer<T>> c)
        {
            return c.Select(ConsumerAction).ToArray();
        }
    }
}