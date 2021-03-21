using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class MiscExtsTest
    {
        [Test]
        public void ToPpcEnumerableWithException_WorksFine_In_Absence_Of_Errors()
        {
            using (var toCancel = new CancellationTokenSource())
            {
                using (var bc = new BlockingCollection<int>())
                {
                    bc.Add(1, CancellationToken.None);
                    var obtainedValue = 0;
                    foreach (var next in bc.ToPpcEnumerableWithException(toCancel.Token, toCancel))
                    {
                        obtainedValue = next;
                        bc.CompleteAdding();
                    }

                    Assert.AreEqual(1, obtainedValue);
                    Assert.IsFalse(toCancel.IsCancellationRequested);
                }
            }
        }

        [Test]
        public void ToPpcEnumerableWithException_ThrowsError_And_Cancels_TokenSource()
        {
            using (var toCancel = new CancellationTokenSource())
            {
                using (var bc = new BlockingCollection<int>())
                {
                    bc.Add(1, CancellationToken.None);
                    var gotError = false;
                    var obtainedValue = 0;
                    try
                    {
                        foreach (var next in bc.ToPpcEnumerableWithException(toCancel.Token, toCancel))
                        {
                            obtainedValue = next;
                            using (bc)
                            {
                                //this will raise error
                            }
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        gotError = true;
                    }

                    Assert.True(gotError);
                    Assert.AreEqual(1, obtainedValue);
                    Assert.IsTrue(toCancel.IsCancellationRequested);
                }
            }
        }

        [Test]
        [TestCase(null)]
        public void HasElements_Gives_Correct_Results(ICollection nullValue)
        {
            Assert.IsFalse(nullValue.HasElements());
            Assert.IsFalse(Array.Empty<int>().HasElements());
            Assert.IsTrue((new[] {1}).HasElements());
        }

        [Test]
        public void ToPpcEnumerable_DoesNot_ThrowError_But_Cancels_TokenSource()
        {
            var errors = new List<Exception>();
            using (var toCancel = new CancellationTokenSource())
            {
                using (var bc = new BlockingCollection<int>())
                {
                    bc.Add(1, CancellationToken.None);
                    var obtainedValue = 0;
                    foreach (var next in bc.ToPpcEnumerable(toCancel.Token, toCancel, errors.Add))
                    {
                        obtainedValue = next;
                        using (bc)
                        {
                            //this will raise error
                        }
                    }

                    Assert.AreEqual(errors.Count, 1);
                    Assert.AreEqual(1, obtainedValue);
                    Assert.IsTrue(errors[0] is ObjectDisposedException);
                    Assert.IsTrue(toCancel.IsCancellationRequested);
                }
            }
        }

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