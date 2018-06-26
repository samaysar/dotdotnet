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
    public class ProducerConsumerExtsTest
    {
        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside PpcPipeline implementation!

        [Test]
        public async Task One_To_One_ProducerConsumer_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.ProducerConsumer(consumer).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default).ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task One_To_Many_ProducerConsumer_Harmonizes()
        {
            var producer = Producer();
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.ProducerConsumer(consumer).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default).ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_One_ProducerConsumer_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>();
            var listConsumer = Consumer<List<object>>();

            await producer.ProducerConsumer(consumer).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default).ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task Many_To_Many_ProducerConsumer_Harmonizes()
        {
            var producer = Producer(2);
            var consumer = Consumer<object>(2);
            var listConsumer = Consumer<List<object>>(2);

            await producer.ProducerConsumer(consumer).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await producer.ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default).ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(consumer).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(listConsumer, 3, 10).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(consumer, IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerFunc(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerFunc(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await producer.ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await producer.ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10).ConfigureAwait(false);
            await ProducerFunc(producer).ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
                .ConfigureAwait(false);

            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer)).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3).ConfigureAwait(false);
            await ProducerAction(producer).ProducerConsumer(ConsumerAction(consumer), 3, 10)
                .ConfigureAwait(false);
            await ProducerAction(producer)
                .ProducerConsumer(ConsumerAction(consumer), IdentityAwaitableAdapter<object>.Default)
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