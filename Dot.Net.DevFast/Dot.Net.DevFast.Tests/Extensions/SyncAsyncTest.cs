using System;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class SyncAsyncTest
    {
        [Test]
        public async Task Action_ToAsyncFunc_WithoutDelegation_Works_Asexpected()
        {
            long actTs = 0;
            // testing Action with 0 generics
            Action sync0 = () => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async0 = sync0.ToAsync(false);
            var willAwait = async0();
            var currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 1 generics
            Action<int> sync1 = (t0) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async1 = sync1.ToAsync(false);
            willAwait = async1(1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 2 generics
            Action<int, int> sync2 = (t0, t1) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async2 = sync2.ToAsync(false);
            willAwait = async2(1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 3 generics
            Action<int, int, int> sync3 = (t0, t1, t2) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async3 = sync3.ToAsync(false);
            willAwait = async3(1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 4 generics
            Action<int, int, int, int> sync4 = (t0, t1, t2, t3) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async4 = sync4.ToAsync(false);
            willAwait = async4(1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 5 generics
            Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async5 = sync5.ToAsync(false);
            willAwait = async5(1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 6 generics
            Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async6 = sync6.ToAsync(false);
            willAwait = async6(1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 7 generics
            Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async7 = sync7.ToAsync(false);
            willAwait = async7(1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 8 generics
            Action<int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async8 = sync8.ToAsync(false);
            willAwait = async8(1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 9 generics
            Action<int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async9 = sync9.ToAsync(false);
            willAwait = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 10 generics
            Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async10 = sync10.ToAsync(false);
            willAwait = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 11 generics
            Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async11 = sync11.ToAsync(false);
            willAwait = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 12 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async12 = sync12.ToAsync(false);
            willAwait = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 13 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async13 = sync13.ToAsync(false);
            willAwait = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 14 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async14 =
                sync14.ToAsync(false);
            willAwait = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 15 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async15 =
                sync15.ToAsync(false);
            willAwait = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 16 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async16 =
                sync16.ToAsync(false);
            willAwait = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            await willAwait.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);
        }
    }
}