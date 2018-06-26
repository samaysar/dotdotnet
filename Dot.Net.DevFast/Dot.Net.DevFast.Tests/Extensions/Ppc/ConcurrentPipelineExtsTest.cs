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
            using (PipeExtsTest.Consumer<object>().ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.Consumer<List<object>>().ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.Consumer<object>().ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>())
                .ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>())
                .ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>())
                .ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>())
                .ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>())
                .ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>())
                .ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }
        }

        [Test]
        public void Multiple_Consumer_Based_Pipeline_Harmonizes()
        {
            using (PipeExtsTest.Consumer<object>(2).ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.Consumer<List<object>>(2).ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.Consumer<object>(2).ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                .ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>(2))
                .ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                .ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2))
                .ConcurrentPipeline())
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>(2))
                .ConcurrentPipeline(2, 0))
            {
            }

            using (PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2))
                .ConcurrentPipeline(IdentityAwaitableAdapter<object>.Default))
            {
            }
        }
    }
}