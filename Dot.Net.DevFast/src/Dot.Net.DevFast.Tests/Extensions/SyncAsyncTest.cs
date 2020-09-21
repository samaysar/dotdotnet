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
        public async Task Action_ToAsyncFunc_WithoutDelegation_Works_As_Expected()
        {
            //Here we want to just Assert that no matter when we await on the
            //Async Task it will always execute in line when delegation is set to false.

            long actTs = 0;
            // testing Action with 0 generics
            Action sync0 = () => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async0 = sync0.ToAsync(false);
            var awaitAfter = async0();
            var currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 1 generics
            Action<int> sync1 = t0 => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async1 = sync1.ToAsync(false);
            awaitAfter = async1(1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 2 generics
            Action<int, int> sync2 = (t0, t1) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async2 = sync2.ToAsync(false);
            awaitAfter = async2(1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 3 generics
            Action<int, int, int> sync3 = (t0, t1, t2) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async3 = sync3.ToAsync(false);
            awaitAfter = async3(1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 4 generics
            Action<int, int, int, int> sync4 = (t0, t1, t2, t3) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async4 = sync4.ToAsync(false);
            awaitAfter = async4(1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 5 generics
            Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async5 = sync5.ToAsync(false);
            awaitAfter = async5(1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 6 generics
            Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async6 = sync6.ToAsync(false);
            awaitAfter = async6(1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 7 generics
            Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async7 = sync7.ToAsync(false);
            awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 8 generics
            Action<int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async8 = sync8.ToAsync(false);
            awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 9 generics
            Action<int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async9 = sync9.ToAsync(false);
            awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 10 generics
            Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async10 = sync10.ToAsync(false);
            awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 11 generics
            Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async11 = sync11.ToAsync(false);
            awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 12 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async12 = sync12.ToAsync(false);
            awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 13 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async13 = sync13.ToAsync(false);
            awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 14 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async14 =
                sync14.ToAsync(false);
            awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 15 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async15 =
                sync15.ToAsync(false);
            awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Action with 16 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                    Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
            var async16 = sync16.ToAsync(false);
            awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);
        }

        [Test]
        public void Action_ToAsyncFunc_WithoutDelegation_Throws_Error_If_Cancellation_Is_Demanded_Before()
        {
            //Here we want to just Assert that no matter when we await on the
            //Async Task it will always execute in line when delegation is set to false.

            var cts = new CancellationTokenSource();
            cts.Cancel();
            // testing Action with 0 generics
            Action sync0 = () => { };
            var async0 = sync0.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async0().GetAwaiter().GetResult());

            // testing Action with 1 generics
            Action<int> sync1 = (t0) => { };
            var async1 = sync1.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async1(1).GetAwaiter().GetResult());

            // testing Action with 2 generics
            Action<int, int> sync2 = (t0, t1) => { };
            var async2 = sync2.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async2(1, 1).GetAwaiter().GetResult());

            // testing Action with 3 generics
            Action<int, int, int> sync3 = (t0, t1, t2) => { };
            var async3 = sync3.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async3(1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 4 generics
            Action<int, int, int, int> sync4 = (t0, t1, t2, t3) => { };
            var async4 = sync4.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async4(1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 5 generics
            Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) => { };
            var async5 = sync5.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async5(1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 6 generics
            Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) => { };
            var async6 = sync6.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async6(1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 7 generics
            Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) => { };
            var async7 = sync7.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async7(1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 8 generics
            Action<int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) => { };
            var async8 = sync8.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async8(1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 9 generics
            Action<int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) => { };
            var async9 = sync9.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async9(1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 10 generics
            Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => { };
            var async10 = sync10.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 11 generics
            Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { };
            var async11 = sync11.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 12 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { };
            var async12 = sync12.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 13 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { };
            var async13 = sync13.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 14 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { };
            var async14 = sync14.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 15 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { };
            var async15 = sync15.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 16 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { };
            var async16 = sync16.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());
        }

        [Test]
        public async Task Func_ToAsyncFunc_WithoutDelegation_Works_As_Expected()
        {
            //Here we want to just Assert that no matter when we await on the
            //Async Task it will always execute in line when delegation is set to false.

            // testing Func with 0 generics
            Func<long> sync0 = () => DateTime.Now.Ticks;
            var async0 = sync0.ToAsync(false);
            var awaitAfter = async0();
            var currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            var actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 1 generics
            Func<int, long> sync1 = t0 => DateTime.Now.Ticks;
            var async1 = sync1.ToAsync(false);
            awaitAfter = async1(1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 2 generics
            Func<int, int, long> sync2 = (t0, t1) => DateTime.Now.Ticks;
            var async2 = sync2.ToAsync(false);
            awaitAfter = async2(1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 3 generics
            Func<int, int, int, long> sync3 = (t0, t1, t2) => DateTime.Now.Ticks;
            var async3 = sync3.ToAsync(false);
            awaitAfter = async3(1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 4 generics
            Func<int, int, int, int, long> sync4 = (t0, t1, t2, t3) => DateTime.Now.Ticks;
            var async4 = sync4.ToAsync(false);
            awaitAfter = async4(1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 5 generics
            Func<int, int, int, int, int, long> sync5 = (t0, t1, t2, t3, t4) => DateTime.Now.Ticks;
            var async5 = sync5.ToAsync(false);
            awaitAfter = async5(1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 6 generics
            Func<int, int, int, int, int, int, long> sync6 = (t0, t1, t2, t3, t4, t5) => DateTime.Now.Ticks;
            var async6 = sync6.ToAsync(false);
            awaitAfter = async6(1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 7 generics
            Func<int, int, int, int, int, int, int, long> sync7 = (t0, t1, t2, t3, t4, t5, t6) => DateTime.Now.Ticks;
            var async7 = sync7.ToAsync(false);
            awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 8 generics
            Func<int, int, int, int, int, int, int, int, long> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                DateTime.Now.Ticks;
            var async8 = sync8.ToAsync(false);
            awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 9 generics
            Func<int, int, int, int, int, int, int, int, int, long> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                DateTime.Now.Ticks;
            var async9 = sync9.ToAsync(false);
            awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 10 generics
            Func<int, int, int, int, int, int, int, int, int, int, long> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => DateTime.Now.Ticks;
            var async10 = sync10.ToAsync(false);
            awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 11 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, long> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => DateTime.Now.Ticks;
            var async11 = sync11.ToAsync(false);
            awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 12 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, long> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => DateTime.Now.Ticks;
            var async12 = sync12.ToAsync(false);
            awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 13 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, long> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => DateTime.Now.Ticks;
            var async13 = sync13.ToAsync(false);
            awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 14 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, long> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => DateTime.Now.Ticks;
            var async14 = sync14.ToAsync(false);
            awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 15 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, long> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => DateTime.Now.Ticks;
            var async15 = sync15.ToAsync(false);
            awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 16 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, long> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => DateTime.Now.Ticks;
            var async16 = sync16.ToAsync(false);
            awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);
        }

        [Test]
        public void Func_ToAsyncFunc_WithoutDelegation_Throws_Error_If_Cancellation_Is_Demanded_Before()
        {
            //Here we want to just Assert that no matter when we await on the
            //Async Task it will always execute in line when delegation is set to false.

            var cts = new CancellationTokenSource();
            cts.Cancel();
            // testing Action with 0 generics
            Func<int> sync0 = () => 0;
            var async0 = sync0.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async0().GetAwaiter().GetResult());

            // testing Action with 1 generics
            Func<int, int> sync1 = (t0) => 0;
            var async1 = sync1.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async1(1).GetAwaiter().GetResult());

            // testing Action with 2 generics
            Func<int, int, int> sync2 = (t0, t1) => 0;
            var async2 = sync2.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async2(1, 1).GetAwaiter().GetResult());

            // testing Action with 3 generics
            Func<int, int, int, int> sync3 = (t0, t1, t2) => 0;
            var async3 = sync3.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async3(1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 4 generics
            Func<int, int, int, int, int> sync4 = (t0, t1, t2, t3) => 0;
            var async4 = sync4.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async4(1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 5 generics
            Func<int, int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) => 0;
            var async5 = sync5.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async5(1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 6 generics
            Func<int, int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) => 0;
            var async6 = sync6.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async6(1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 7 generics
            Func<int, int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) => 0;
            var async7 = sync7.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async7(1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 8 generics
            Func<int, int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) => 0;
            var async8 = sync8.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async8(1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 9 generics
            Func<int, int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) => 0;
            var async9 = sync9.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() => async9(1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 10 generics
            Func<int, int, int, int, int, int, int, int, int, int, int> sync10 = (t0, t1, t2, t3, t4, t5, t6, t7, t8,
                t9) => 0;
            var async10 = sync10.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 11 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int> sync11 = (t0, t1, t2, t3, t4, t5, t6, t7,
                t8, t9, t10) => 0;
            var async11 = sync11.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 12 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0;
            var async12 = sync12.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 13 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0;
            var async13 = sync13.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 14 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0;
            var async14 = sync14.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 15 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0;
            var async15 = sync15.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());

            // testing Action with 16 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0;
            var async16 = sync16.ToAsync(false, token: cts.Token);
            Assert.Throws<OperationCanceledException>(() =>
                async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1).GetAwaiter().GetResult());
        }

        [Test]
        public async Task Action_ToAsyncFunc_WithDelegation_But_WithoutCancellationToken_Works_As_Expected()
        {
            //Here we want to Assert that task is really running concurrently.
            //to do this, we will supply an unsignaled wait handle to sync action
            //await on it afterwards... status should NOT be RanToCompletion
            //here we can use wait handle and set it just before awaiting without
            //being in deadlock! coz deadlock means we are wrong!

            using (var waitHandle = new ManualResetEventSlim())
            {
                // testing Action with 0 generics
                waitHandle.Reset();
                Action sync0 = () => waitHandle.Wait();
                var async0 = sync0.ToAsync();
                var awaitAfter = async0();
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 1 generics
                waitHandle.Reset();
                Action<int> sync1 = t0 => waitHandle.Wait();
                var async1 = sync1.ToAsync();
                awaitAfter = async1(1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 2 generics
                waitHandle.Reset();
                Action<int, int> sync2 = (t0, t1) => waitHandle.Wait();
                var async2 = sync2.ToAsync();
                awaitAfter = async2(1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 3 generics
                waitHandle.Reset();
                Action<int, int, int> sync3 = (t0, t1, t2) => waitHandle.Wait();
                var async3 = sync3.ToAsync();
                awaitAfter = async3(1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 4 generics
                waitHandle.Reset();
                Action<int, int, int, int> sync4 = (t0, t1, t2, t3) => waitHandle.Wait();
                var async4 = sync4.ToAsync();
                awaitAfter = async4(1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 5 generics
                waitHandle.Reset();
                Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) => waitHandle.Wait();
                var async5 = sync5.ToAsync();
                awaitAfter = async5(1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 6 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) => waitHandle.Wait();
                var async6 = sync6.ToAsync();
                awaitAfter = async6(1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 7 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) => waitHandle.Wait();
                var async7 = sync7.ToAsync();
                awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 8 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int>
                    sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) => waitHandle.Wait();
                var async8 = sync8.ToAsync();
                awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 9 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                    waitHandle.Wait();
                var async9 = sync9.ToAsync();
                awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 10 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => waitHandle.Wait();
                var async10 = sync10.ToAsync();
                awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 11 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => waitHandle.Wait();
                var async11 = sync11.ToAsync();
                awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 12 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => waitHandle.Wait();
                var async12 = sync12.ToAsync();
                awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 13 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => waitHandle.Wait();
                var async13 = sync13.ToAsync();
                awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 14 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => waitHandle.Wait();
                var async14 = sync14.ToAsync();
                awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 15 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => waitHandle.Wait();
                var async15 = sync15.ToAsync();
                awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);

                // testing Action with 16 generics
                waitHandle.Reset();
                Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => waitHandle.Wait();
                var async16 = sync16.ToAsync();
                awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                await awaitAfter.ConfigureAwait(false);
            }
        }

        [Test]
        public async Task Func_ToAsyncFunc_WithDelegation_But_WithoutCancellationToken_Works_As_Expected()
        {
            //Here we want to Assert that task is really running concurrently.
            //to do this, we will supply an unsignaled wait handle to sync action
            //await on it afterwards... status should NOT be RanToCompletion
            //here we can use wait handle and set it just before awaiting without
            //being in deadlock! coz deadlock means we are wrong!

            using (var waitHandle = new ManualResetEventSlim())
            {
                // testing Func with 0 generics
                waitHandle.Reset();
                Func<int> sync0 = () =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async0 = sync0.ToAsync();
                var awaitAfter = async0();
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 1 generics
                waitHandle.Reset();
                Func<int, int> sync1 = t0 =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async1 = sync1.ToAsync();
                awaitAfter = async1(1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 2 generics
                waitHandle.Reset();
                Func<int, int, int> sync2 = (t0, t1) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async2 = sync2.ToAsync();
                awaitAfter = async2(1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 3 generics
                waitHandle.Reset();
                Func<int, int, int, int> sync3 = (t0, t1, t2) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async3 = sync3.ToAsync();
                awaitAfter = async3(1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 4 generics
                waitHandle.Reset();
                Func<int, int, int, int, int> sync4 = (t0, t1, t2, t3) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async4 = sync4.ToAsync();
                awaitAfter = async4(1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 5 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async5 = sync5.ToAsync();
                awaitAfter = async5(1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 6 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async6 = sync6.ToAsync();
                awaitAfter = async6(1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 7 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async7 = sync7.ToAsync();
                awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 8 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async8 = sync8.ToAsync();
                awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 9 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                {
                    waitHandle.Wait();
                    return 1;
                };
                var async9 = sync9.ToAsync();
                awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 10 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int> sync10 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async10 = sync10.ToAsync();
                awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 11 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int> sync11 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async11 = sync11.ToAsync();
                awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 12 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async12 = sync12.ToAsync();
                awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 13 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async13 = sync13.ToAsync();
                awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 14 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async14 = sync14.ToAsync();
                awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 15 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async15 = sync15.ToAsync();
                awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
                // testing Func with 16 generics
                waitHandle.Reset();
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                    (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                    {
                        waitHandle.Wait();
                        return 1;
                    };
                var async16 = sync16.ToAsync();
                awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                waitHandle.Set();
                Assert.True((await awaitAfter.ConfigureAwait(false)) == 1);
            }
        }

        [Test]
        public void Action_ToAsyncFunc_WithDelegation_Cancels_Task_Well()
        {
            //Here we want to Assert that when token is cancelled, task will throw
            //operation or Task cancellation error, but the action would continue to
            //execute as we won't observe the token in our action!!!

            using (var waitHandle = new ManualResetEventSlim())
            {
                using (var taskToUtestHandle = new ManualResetEventSlim())
                {
                    using (var cancellationWaitHandle = new ManualResetEventSlim())
                    {
                        // testing Action with 0 generics
                        var taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts0 = new CancellationTokenSource();
                        Action sync0 = () =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async0 = sync0.ToAsync(token: cts0.Token);
                        var awaitAfter = async0();
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts0.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 1 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts1 = new CancellationTokenSource();
                        Action<int> sync1 = t0 =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async1 = sync1.ToAsync(token: cts1.Token);
                        awaitAfter = async1(1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts1.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 2 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts2 = new CancellationTokenSource();
                        Action<int, int> sync2 = (t0, t1) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async2 = sync2.ToAsync(token: cts2.Token);
                        awaitAfter = async2(1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts2.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 3 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts3 = new CancellationTokenSource();
                        Action<int, int, int> sync3 = (t0, t1, t2) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async3 = sync3.ToAsync(token: cts3.Token);
                        awaitAfter = async3(1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts3.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 4 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts4 = new CancellationTokenSource();
                        Action<int, int, int, int> sync4 = (t0, t1, t2, t3) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async4 = sync4.ToAsync(token: cts4.Token);
                        awaitAfter = async4(1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts4.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 5 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts5 = new CancellationTokenSource();
                        Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async5 = sync5.ToAsync(token: cts5.Token);
                        awaitAfter = async5(1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts5.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 6 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts6 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async6 = sync6.ToAsync(token: cts6.Token);
                        awaitAfter = async6(1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts6.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 7 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts7 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async7 = sync7.ToAsync(token: cts7.Token);
                        awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts7.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 8 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts8 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            taskToUtestHandle.Set();
                        };
                        var async8 = sync8.ToAsync(token: cts8.Token);
                        awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts8.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 9 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts9 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int> sync9 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async9 = sync9.ToAsync(token: cts9.Token);
                        awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts9.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 10 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts10 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async10 = sync10.ToAsync(token: cts10.Token);
                        awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts10.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 11 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts11 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async11 = sync11.ToAsync(token: cts11.Token);
                        awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts11.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 12 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts12 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async12 = sync12.ToAsync(token: cts12.Token);
                        awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts12.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 13 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts13 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async13 = sync13.ToAsync(token: cts13.Token);
                        awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts13.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 14 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts14 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async14 = sync14.ToAsync(token: cts14.Token);
                        awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts14.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 15 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts15 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async15 = sync15.ToAsync(token: cts15.Token);
                        awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts15.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);

                        // testing Action with 16 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts16 = new CancellationTokenSource();
                        Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                taskToUtestHandle.Set();
                            };
                        var async16 = sync16.ToAsync(token: cts16.Token);
                        awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts16.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                    }
                }
            }
        }

        [Test]
        public void Func_ToAsyncFunc_WithDelegation_Cancels_Task_Well()
        {
            //Here we want to Assert that when token is cancelled, task will throw
            //operation or Task cancellation error, but the func would continue to
            //execute as we won't observe the token in our action!!!

            using (var waitHandle = new ManualResetEventSlim())
            {
                using (var taskToUtestHandle = new ManualResetEventSlim())
                {
                    using (var cancellationWaitHandle = new ManualResetEventSlim())
                    {
                        // testing Func with 0 generics
                        var taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts0 = new CancellationTokenSource();
                        Func<int> sync0 = () =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async0 = sync0.ToAsync(token: cts0.Token);
                        var awaitAfter = async0();
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts0.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 1 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts1 = new CancellationTokenSource();
                        Func<int, int> sync1 = t0 =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async1 = sync1.ToAsync(token: cts1.Token);
                        awaitAfter = async1(1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts1.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 2 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts2 = new CancellationTokenSource();
                        Func<int, int, int> sync2 = (t0, t1) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async2 = sync2.ToAsync(token: cts2.Token);
                        awaitAfter = async2(1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts2.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 3 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts3 = new CancellationTokenSource();
                        Func<int, int, int, int> sync3 = (t0, t1, t2) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async3 = sync3.ToAsync(token: cts3.Token);
                        awaitAfter = async3(1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts3.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 4 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts4 = new CancellationTokenSource();
                        Func<int, int, int, int, int> sync4 = (t0, t1, t2, t3) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async4 = sync4.ToAsync(token: cts4.Token);
                        awaitAfter = async4(1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts4.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 5 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts5 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async5 = sync5.ToAsync(token: cts5.Token);
                        awaitAfter = async5(1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts5.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 6 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts6 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async6 = sync6.ToAsync(token: cts6.Token);
                        awaitAfter = async6(1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts6.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 7 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts7 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async7 = sync7.ToAsync(token: cts7.Token);
                        awaitAfter = async7(1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts7.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 8 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts8 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                        {
                            waitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 100);
                            taskToUtestHandle.Set();
                            cancellationWaitHandle.Wait(CancellationToken.None);
                            Interlocked.Exchange(ref taskValue, 1);
                            taskToUtestHandle.Set();
                            return 1;
                        };
                        var async8 = sync8.ToAsync(token: cts8.Token);
                        awaitAfter = async8(1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts8.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 9 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts9 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int> sync9 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async9 = sync9.ToAsync(token: cts9.Token);
                        awaitAfter = async9(1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts9.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 10 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts10 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int> sync10 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async10 = sync10.ToAsync(token: cts10.Token);
                        awaitAfter = async10(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts10.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 11 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts11 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int> sync11 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async11 = sync11.ToAsync(token: cts11.Token);
                        awaitAfter = async11(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts11.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 12 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts12 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async12 = sync12.ToAsync(token: cts12.Token);
                        awaitAfter = async12(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts12.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 13 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts13 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async13 = sync13.ToAsync(token: cts13.Token);
                        awaitAfter = async13(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts13.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 14 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts14 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async14 = sync14.ToAsync(token: cts14.Token);
                        awaitAfter = async14(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts14.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 15 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts15 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                            (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async15 = sync15.ToAsync(token: cts15.Token);
                        awaitAfter = async15(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts15.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);

                        // testing Func with 16 generics
                        taskValue = 0;
                        waitHandle.Reset();
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Reset();
                        var cts16 = new CancellationTokenSource();
                        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16
                            = (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
                            {
                                waitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 100);
                                taskToUtestHandle.Set();
                                cancellationWaitHandle.Wait(CancellationToken.None);
                                Interlocked.Exchange(ref taskValue, 1);
                                taskToUtestHandle.Set();
                                return 1;
                            };
                        var async16 = sync16.ToAsync(token: cts16.Token);
                        awaitAfter = async16(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
                        Assert.True(awaitAfter.Status != TaskStatus.RanToCompletion);
                        Assert.True(taskValue == 0);
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        cts16.Cancel();
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 1);
                    }
                }
            }
        }

        [Test]
        public void Action_ToAsyncFunc_WithDelegation_Throws_Action_Exception()
        {
            //Here we want to Assert that when action throws an exception, 
            //task throws the exact same exception without tinkering with it!!!

            const string erroText = "My custom error";
            var cts = new CancellationTokenSource();
            var customException = new Exception(erroText);
            // testing Action with 0 generics
            Action sync0 = () => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync0.ToAsync(token: cts.Token)()).Message
                .Equals(erroText));

            // testing Action with 1 generics
            Action<int> sync1 = t0 => throw customException;
            Assert.True(
                Assert.ThrowsAsync<Exception>(() => sync1.ToAsync(token: cts.Token)(1)).Message.Equals(erroText));

            // testing Action with 2 generics
            Action<int, int> sync2 = (t0, t1) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync2.ToAsync(token: cts.Token)(1, 1)).Message
                .Equals(erroText));

            // testing Action with 3 generics
            Action<int, int, int> sync3 = (t0, t1, t2) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync3.ToAsync(token: cts.Token)(1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 4 generics
            Action<int, int, int, int> sync4 = (t0, t1, t2, t3) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync4.ToAsync(token: cts.Token)(1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 5 generics
            Action<int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync5.ToAsync(token: cts.Token)(1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 6 generics
            Action<int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync6.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 7 generics
            Action<int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync7.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Action with 8 generics
            Action<int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync8.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Action with 9 generics
            Action<int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync9.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Action with 10 generics
            Action<int, int, int, int, int, int, int, int, int, int> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync10.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 11 generics
            Action<int, int, int, int, int, int, int, int, int, int, int> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync11.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 12 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync12.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Action with 13 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync13.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Action with 14 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(
                    () => sync14.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 15 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() =>
                    sync15.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Action with 16 generics
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() =>
                    sync16.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));
        }

        [Test]
        public void Func_ToAsyncFunc_WithDelegation_Throws_Func_Exception()
        {
            //Here we want to Assert that when action throws an exception, 
            //task throws the exact same exception without tinkering with it!!!

            const string erroText = "My custom error";
            var cts = new CancellationTokenSource();
            var customException = new Exception(erroText);
            // testing Func with 0 generics
            Func<int> sync0 = () => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync0.ToAsync(token: cts.Token)()).Message
                .Equals(erroText));

            // testing Func with 1 generics
            Func<int, int> sync1 = t0 => throw customException;
            Assert.True(
                Assert.ThrowsAsync<Exception>(() => sync1.ToAsync(token: cts.Token)(1)).Message.Equals(erroText));

            // testing Func with 2 generics
            Func<int, int, int> sync2 = (t0, t1) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync2.ToAsync(token: cts.Token)(1, 1)).Message
                .Equals(erroText));

            // testing Func with 3 generics
            Func<int, int, int, int> sync3 = (t0, t1, t2) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync3.ToAsync(token: cts.Token)(1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 4 generics
            Func<int, int, int, int, int> sync4 = (t0, t1, t2, t3) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync4.ToAsync(token: cts.Token)(1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 5 generics
            Func<int, int, int, int, int, int> sync5 = (t0, t1, t2, t3, t4) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync5.ToAsync(token: cts.Token)(1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 6 generics
            Func<int, int, int, int, int, int, int> sync6 = (t0, t1, t2, t3, t4, t5) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync6.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 7 generics
            Func<int, int, int, int, int, int, int, int> sync7 = (t0, t1, t2, t3, t4, t5, t6) => throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync7.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Func with 8 generics
            Func<int, int, int, int, int, int, int, int, int> sync8 = (t0, t1, t2, t3, t4, t5, t6, t7) =>
                throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync8.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Func with 9 generics
            Func<int, int, int, int, int, int, int, int, int, int> sync9 = (t0, t1, t2, t3, t4, t5, t6, t7, t8) =>
                throw customException;
            Assert.True(Assert.ThrowsAsync<Exception>(() => sync9.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Func with 10 generics
            Func<int, int, int, int, int, int, int, int, int, int, int> sync10 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync10.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 11 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int> sync11 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync11.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 12 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> sync12 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync12.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Func with 13 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync13 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() => sync13.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1))
                .Message.Equals(erroText));

            // testing Func with 14 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync14 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(
                    () => sync14.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 15 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync15 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() =>
                    sync15.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));

            // testing Func with 16 generics
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> sync16 =
                (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => throw customException;
            Assert.True(Assert
                .ThrowsAsync<Exception>(() =>
                    sync16.ToAsync(token: cts.Token)(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)).Message
                .Equals(erroText));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Sync_Lambdas_Runs_Error_Wrapped_Perfectly(bool withError)
        {
            var errorCnt = 0;
            void ErrorHandler(Exception e)
            {
                errorCnt++;
                Assert.True(e.Message.Equals("Test"));
            }

            var actionToRun = withError ? () => throw new Exception("Test") : new Action(() => { });
            actionToRun.ExecuteErrorWrapped(ErrorHandler);
            Assert.True(errorCnt.Equals(withError ? 1 : 0));

            int ErrorHandler2(Exception e)
            {
                errorCnt++;
                Assert.True(e.Message.Equals("Test"));
                return 0;
            }

            var funcToRun = withError ? () => throw new Exception("Test") : new Func<int>(() => 0);
            Assert.True(funcToRun.ExecuteErrorWrapped(ErrorHandler2).Equals(0));
            Assert.True(errorCnt.Equals(withError ? 2 : 0));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Async_Lambdas_Runs_Error_Wrapped_Perfectly(bool withError)
        {
            var errorCnt = 0;
            var actionToRun = withError ? () => throw new Exception("Test") : new Func<Task>(() => Task.CompletedTask);
            await actionToRun.ExecuteErrorWrappedAsync(e =>
            {
                errorCnt++;
                Assert.True(e.Message.Equals("Test"));
            }).ConfigureAwait(false);
            Assert.True(errorCnt.Equals(withError ? 1 : 0));

            int ErrorHandler2(Exception e)
            {
                errorCnt++;
                Assert.True(e.Message.Equals("Test"));
                return 0;
            }

            var funcToRun = withError ? () => throw new Exception("Test") : new Func<Task<int>>(() => Task.FromResult(0));
            Assert.True((await funcToRun.ExecuteErrorWrappedAsync(ErrorHandler2).ConfigureAwait(false)).Equals(0));
            Assert.True(errorCnt.Equals(withError ? 2 : 0));
        }
    }
}