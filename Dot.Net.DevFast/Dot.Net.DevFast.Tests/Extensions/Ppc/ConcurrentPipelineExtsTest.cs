using System.Collections.Generic;
using Dot.Net.DevFast.Extensions.Ppc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class ConcurrentPipelineExtsTest
    {
        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside ConcurrentPipeline implementation!

        [Test]
        public void Single_Consumer_Based_Pipeline_Harmonizes()
        {
            using (ProducerConsumerExtsTest.Consumer<object>().ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.Consumer<List<object>>().ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.Consumer<object>().ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<object>())
                .ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<List<object>>())
                .ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<object>())
                .ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<object>())
                .ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<List<object>>())
                .ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<object>())
                .ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }
        }

        [Test]
        public void Multiple_Consumer_Based_Pipeline_Harmonizes()
        {
            using (ProducerConsumerExtsTest.Consumer<object>(2).ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.Consumer<List<object>>(2).ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.Consumer<object>(2).ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<object>(2))
                .ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<List<object>>(2))
                .ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerFunc(ProducerConsumerExtsTest.Consumer<object>(2))
                .ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<object>(2))
                .ConcurrentPipeline())
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<List<object>>(2))
                .ConcurrentPipeline(2, 0))
            {
            }

            using (ProducerConsumerExtsTest.ConsumerAction(ProducerConsumerExtsTest.Consumer<object>(2))
                .ConcurrentPipeline(new IdentityAdapter<object>()))
            {
            }
        }
    }
}