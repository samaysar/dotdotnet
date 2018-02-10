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
            Action<int> sync1 = (t0) => Interlocked.Exchange(ref actTs, DateTime.Now.Ticks);
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
        public async Task Func_ToAsyncFunc_WithoutDelegation_Works_As_Expected()
        {
            //Here we want to just Assert that no matter when we await on the
            //Async Task it will always execute in line when delegation is set to false.

            long actTs = 0;
            // testing Func with 0 generics
            Func<long> sync0 = () => DateTime.Now.Ticks;
            var async0 = sync0.ToAsync(false);
            var awaitAfter = async0();
            var currentTicks = DateTime.Now.Ticks;
            Assert.True(awaitAfter.Status == TaskStatus.RanToCompletion);
            actTs = await awaitAfter.ConfigureAwait(false);
            Assert.True(currentTicks >= actTs);
            Assert.True(actTs != 0);

            actTs = 0;
            // testing Func with 1 generics
            Func<int, long> sync1 = (t0) => DateTime.Now.Ticks;
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
                Action<int> sync1 = (t0) => waitHandle.Wait();
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
                Func<int, int> sync1 = (t0) =>
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
        public async Task Action_ToAsyncFunc_WithDelegation_Cancels_Task_Well()
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
                        cts0.Cancel();
                        waitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                        Assert.True(taskValue == 100);
                        Assert.ThrowsAsync<TaskCanceledException>(() => awaitAfter);
                        taskToUtestHandle.Reset();
                        cancellationWaitHandle.Set();
                        taskToUtestHandle.Wait(CancellationToken.None);
                    }
                }
            }
        }
    }
}