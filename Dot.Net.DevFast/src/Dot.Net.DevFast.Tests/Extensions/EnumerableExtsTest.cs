using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using NUnit.Framework;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtsTest
    {
        private class ValueHolder
        {
            public int Val{ get; set; }
        }

        [Test]
        public void IEnumerable_ForEach_Applies_Given_Lambda()
        {
            var results = new HashSet<int> { 2, 4, 6 };
            IEnumerable<ValueHolder> instance = new List<ValueHolder>
            {
                new ValueHolder { Val = 1 },
                new ValueHolder { Val = 2 },
                new ValueHolder { Val = 3 }
            };
            instance.ForEach(x => x.Val *= 2);
            instance.ForEach(x => Assert.True(results.Contains(x.Val)));
            IEnumerable instance2 = new List<ValueHolder>
            {
                new ValueHolder { Val = 1 },
                new ValueHolder { Val = 2 },
                new ValueHolder { Val = 3 }
            };
            instance2.ForEach(x => ((ValueHolder)x).Val *= 2);
            instance2.ForEach(x => Assert.True(results.Contains(((ValueHolder)x).Val)));

            instance = new List<ValueHolder>
            {
                new ValueHolder { Val = 1 },
                new ValueHolder { Val = 2 },
                new ValueHolder { Val = 3 }
            };
            instance.ForEach((x, _) => x.Val *= 2);
            instance.ForEach(x => Assert.True(results.Contains(x.Val)));
            instance2 = new List<ValueHolder>
            {
                new ValueHolder { Val = 1 },
                new ValueHolder { Val = 2 },
                new ValueHolder { Val = 3 }
            };
            instance2.ForEach((x, _) => ((ValueHolder)x).Val *= 2);
            instance2.ForEach(x => Assert.True(results.Contains(((ValueHolder)x).Val)));
        }

        [Test]
        public async Task IEnumerable_ForEachAsync_Applies_Given_Lambda()
        {
            var called = 0;
            var instance = new List<ValueHolder>
            {
                new ValueHolder { Val = 1 },
                new ValueHolder { Val = 2 },
                new ValueHolder { Val = 3 }
            };
            await instance.ForEachAsync((x, __) =>
            {
                called += x.Val;
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.AreEqual(6, called);
        }

#if !NETFRAMEWORK && !NETCOREAPP2_2
        private async IAsyncEnumerable<ValueHolder> GenerateAsyncEnumerable()
        {
            await Task.CompletedTask;
            yield return new ValueHolder { Val = 1 };
            yield return new ValueHolder { Val = 2 };
            yield return new ValueHolder { Val = 3 };
        }

        [Test]
        public async Task IAsyncEnumerable_ForEachAsync_Applies_Given_Lambda()
        {
            var called = 0;
            await GenerateAsyncEnumerable().ForEachAsync((x, __) =>
            {
                called += x.Val;
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            Assert.AreEqual(6, called);

            called = 0; 
            await GenerateAsyncEnumerable().ForEachAsync((x, __) => called += x.Val).ConfigureAwait(false);
            Assert.AreEqual(6, called);
        }

        [Test]
        public async Task IAsyncEnumerable_Where_Select_ToList_AsyncS_Works()
        {
            var data = await GenerateAsyncEnumerable()
                .WhereAsync((x, __) => x.Val % 2 == 0)
                .SelectAsync((x, __) => x.Val)
                .ToListAsync()
                .ConfigureAwait(false);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(2, data[0]);

            data = await GenerateAsyncEnumerable()
                .WhereAsync((x, __) => Task.FromResult(x.Val % 2 == 0))
                .SelectAsync((x, __) => Task.FromResult(x.Val))
                .ToListAsync()
                .ConfigureAwait(false);
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(2, data[0]);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void IAsyncEnumerable_ToListAsync_Throws_Error_If_Limit_Is_Zero_Or_Less(int limit)
        {
            Assert.ThrowsAsync<DdnDfException>(async () => await GenerateAsyncEnumerable().ToListAsync(limit).ConfigureAwait(false));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void IAsyncEnumerable_ToListAsync_Throws_Error_If_Limit_Is_Breached(int limit)
        {
            var ex = Assert.ThrowsAsync<DdnDfException>(async () =>
                await GenerateAsyncEnumerable().ToListAsync(limit).ConfigureAwait(false));
            Assert.NotNull(ex);
            Assert.AreEqual(ex.Reason, DdnDfErrorCode.OverAllocationDemanded.ToString());
        }

        [Test]
        public async Task IAsyncEnumerable_ToListAsync_Returns_All_Elements_When_Limit_Is_Respected()
        {
            var data = await GenerateAsyncEnumerable().ToListAsync(int.MaxValue).ConfigureAwait(false);
            Assert.AreEqual(3, data.Count);
            Assert.AreEqual(1, data[0].Val);
            Assert.AreEqual(2, data[1].Val);
            Assert.AreEqual(3, data[2].Val);
        }
#endif
    }
}