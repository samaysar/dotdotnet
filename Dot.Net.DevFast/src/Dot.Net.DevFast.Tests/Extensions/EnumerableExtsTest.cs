using System;
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

            public override bool Equals(object obj)
            {
                return Equals(obj as ValueHolder);
            }

            public bool Equals(ValueHolder obj)
            {
                if(obj == null) return false;
                return obj.Val == this.Val;
            }

            public override int GetHashCode()
            {
                return Val.GetHashCode();
            }
        }

        [Test]
        public void EqualsItemWise_Returns_False_When_Either_Collection_Is_Null_Or_Not_Of_Same_Length_Or_Items_Differs()
        {
            Assert.IsFalse(Array.Empty<object>().EqualsItemWise(null));
            Assert.IsFalse(((List<object>)null).EqualsItemWise(Array.Empty<object>()));
            Assert.IsFalse(((List<object>)null).EqualsItemWise(null));
            Assert.IsFalse(new List<int>{1}.EqualsItemWise(Array.Empty<int>()));
            Assert.IsFalse(new List<int> { 2 }.EqualsItemWise(new[] { 1 }));
            Assert.IsFalse(new List<int> { 2, 2, 1 }.EqualsItemWise(new[] { 1, 1, 2 }));
            Assert.IsFalse(new List<int> { 2, 2, 1 }.EqualsItemWise(new[] { 1, 2, 2 }, sameItemOrder: true));
        }

        [Test]
        public void EqualsItemWise_Returns_True_Only_When_Both_Length_And_Items_Are_Same()
        {
            Assert.IsTrue(Array.Empty<object>().EqualsItemWise(Array.Empty<object>()));
            Assert.IsTrue(new List<int> { 1 }.EqualsItemWise(new[] { 1 }));
            Assert.IsTrue(
                new List<ValueHolder> { new() { Val = 1 } }.EqualsItemWise(new[]
                    { new ValueHolder { Val = 1 } }));
            Assert.IsTrue(new List<int> { 2, 2, 1 }.EqualsItemWise(new[] { 1, 2, 2 }));
            Assert.IsTrue(new List<int> { 2, 2, 1 }.EqualsItemWise(new[] { 2, 2, 1 }, sameItemOrder: true));
        }

        [Test]
        public void ToOneToManyDictionary_Works_Well()
        {
            var l = new[] { new ValueHolder { Val = 1 } };
            var dico = l.ToOneToManyDictionary(x => x.Val);
            Assert.IsTrue(dico.Count == 1);
            Assert.IsTrue(dico.TryGetValue(1, out var value) && value.EqualsItemWise(l));
            Assert.IsTrue(dico.TryGetValue(1, out var value1) &&
                          value1.EqualsItemWise(new[] { new ValueHolder { Val = 1 } }));

            var dico1 = l.ToOneToManyDictionary(x => x.Val, x => x.Val);
            Assert.IsTrue(dico1.Count == 1);
            Assert.IsTrue(dico1.TryGetValue(1, out var value2) && value2.EqualsItemWise(new[] { 1 }));

            l = new[] { new ValueHolder { Val = 1 }, new ValueHolder { Val = 1 } };
            dico = l.ToOneToManyDictionary(x => x.Val);
            Assert.IsTrue(dico.Count == 1);
            Assert.IsTrue(dico.TryGetValue(1, out var value3) && value3.Count == 2 && value3.EqualsItemWise(l));
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