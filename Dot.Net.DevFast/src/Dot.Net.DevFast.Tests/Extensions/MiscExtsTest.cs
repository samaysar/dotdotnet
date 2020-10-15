using System;
using System.Collections.Generic;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class MiscExtsTest
    {
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void FindMaximums_Throws_Error_For_Invalid_Arguments(int total)
        {
            var ex = Assert.Throws<DdnDfException>(() => new[] {1, 2}.FindMaximums(total));
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
            Assert.Throws<ArgumentOutOfRangeException>(() => new[] {1, 2}.FindMaximums(1, sorting: (SortOrder) (-1)));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void FindMinimums_Throws_Error_For_Invalid_Arguments(int total)
        {
            var ex = Assert.Throws<DdnDfException>(() => new[] { 1, 2 }.FindMinimums(total));
            Assert.IsTrue(ex.ErrorCode.Equals(DdnDfErrorCode.ValueLessThanThreshold));
            Assert.Throws<ArgumentOutOfRangeException>(() => new[] {1, 2}.FindMinimums(1, sorting: (SortOrder) (-1)));
        }

        [Test]
        [TestCase(SortOrder.Asc)]
        [TestCase(SortOrder.Desc)]
        [TestCase(SortOrder.None)]
        public void FindMaximums_Respects_Sort_Orders(SortOrder order)
        {
            var values = new[] {85, -56, 1, 100, 2, int.MinValue, int.MaxValue}.FindMaximums(5, sorting: order);
            var expectedResult = new[] {85, 1, 100, 2, int.MaxValue};
            CheckSortedOutput(order, expectedResult, values);
            values = new[] { int.MinValue, int.MaxValue }.FindMaximums(5, sorting: order);
            expectedResult = new[] { int.MinValue, int.MaxValue };
            CheckSortedOutput(order, expectedResult, values);
        }

        [Test]
        [TestCase(SortOrder.Asc)]
        [TestCase(SortOrder.Desc)]
        [TestCase(SortOrder.None)]
        public void FindMinimums_Respects_Sort_Orders(SortOrder order)
        {
            var values = new[] { 85, -56, 1, 100, 2, int.MinValue, int.MaxValue }.FindMinimums(5, sorting: order);
            var expectedResult = new[] { 85, 1, -56, 2, int.MinValue };
            CheckSortedOutput(order, expectedResult, values);
            values = new[] { int.MinValue, int.MaxValue }.FindMinimums(5, sorting: order);
            expectedResult = new[] { int.MinValue, int.MaxValue };
            CheckSortedOutput(order, expectedResult, values);
        }

        private static void CheckSortedOutput(SortOrder order, int[] expectedResult, IEnumerable<int> values)
        {
            if (order == SortOrder.None)
            {
                var results = new HashSet<int>(expectedResult);
                foreach (var currentValue in values)
                {
                    Assert.IsTrue(results.Contains(currentValue));
                }
            }
            else
            {
                if (order == SortOrder.Asc)
                {
                    Array.Sort(expectedResult);
                }
                else
                {
                    Array.Sort(expectedResult);
                    Array.Reverse(expectedResult);
                }

                var pos = 0;
                foreach (var currentValue in values)
                {
                    Assert.IsTrue(currentValue.Equals(expectedResult[pos]));
                    pos++;
                }
            }
        }
    }
}