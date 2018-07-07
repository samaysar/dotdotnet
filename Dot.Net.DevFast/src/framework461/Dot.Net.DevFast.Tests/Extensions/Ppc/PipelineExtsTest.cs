using System.Collections.Generic;
using Dot.Net.DevFast.Extensions.Ppc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Ppc
{
    [TestFixture]
    public class PipelineExtsTest
    {
        //These tests are ONLY for coverge as ACTUAL implementation is already tested
        //inside ConcurrentPipeline implementation!

        [Test]
        public void Single_Consumer_Based_Pipeline_Harmonizes()
        {
            using (PipeExtsTest.Consumer<object>().Pipeline())
            {
            }

            using (PipeExtsTest.Consumer<List<object>>().Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.Consumer<object>().Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>())
                .Pipeline())
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>())
                .Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>())
                .Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>())
                .Pipeline())
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>())
                .Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>())
                .Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }
        }

        [Test]
        public void Multiple_Consumer_Based_Pipeline_Harmonizes()
        {
            using (PipeExtsTest.Consumer<object>(2).Pipeline())
            {
            }

            using (PipeExtsTest.Consumer<List<object>>(2).Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.Consumer<object>(2).Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                .Pipeline())
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>(2))
                .Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                .Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2))
                .Pipeline())
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>(2))
                .Pipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2))
                .Pipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }
        }
    }
}