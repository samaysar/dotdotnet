using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task Single_Consumer_Based_Pipeline_Harmonizes()
        {
#if !NETFRAMEWORK
            await 
#endif
            using (
                PipeExtsTest.Consumer<object>().Pipeline()
#if !NETFRAMEWORK
            .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.Consumer<List<object>>().Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.Consumer<object>().Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>()).Pipeline()
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>()).Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>())
                        .Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>()).Pipeline()
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>()).Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>())
                        .Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

            await Task.CompletedTask;
        }

        [Test]
        public async Task Multiple_Consumer_Based_Pipeline_Harmonizes()
        {
#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.Consumer<object>(2).Pipeline()
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.Consumer<List<object>>(2).Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.Consumer<object>(2).Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                        .Pipeline()
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<List<object>>(2))
                        .Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerFunc(PipeExtsTest.Consumer<object>(2))
                        .Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2)).Pipeline()
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<List<object>>(2)).Pipeline(2, 0)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

#if !NETFRAMEWORK
            await
#endif
                using (
                    PipeExtsTest.ConsumerAction(PipeExtsTest.Consumer<object>(2))
                        .Pipeline(IdentityAwaitableAdapter<object>.Default)
#if !NETFRAMEWORK
                        .ConfigureAwait(false)
#endif
                )
            {
            }

            await Task.CompletedTask;
        }
    }
}