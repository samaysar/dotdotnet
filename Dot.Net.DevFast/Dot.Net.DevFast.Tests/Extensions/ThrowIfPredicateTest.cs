using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class ThrowIfPredicateTest
    {
        [Test]
        [TestCase(DdnDfErrorCode.Unspecified)]
        [TestCase(DdnDfErrorCode.NullOrEmptyCollection)]
        public void ThrowIf_And_ThrowIfNot_Works_As_Expected(DdnDfErrorCode errorCode)
        {
            var obj = new object();
            var ex = Assert.Throws<DdnDfException>(
                () => true.ThrowIf(errorCode, "test message", obj));
            Assert.True(ex.ErrorCode == errorCode);
            Assert.True(ex.Message.Contains("test message"));
            Assert.True(ReferenceEquals(obj, false.ThrowIf(errorCode, "test message", obj)));

            ex = Assert.Throws<DdnDfException>(
                () => false.ThrowIfNot(errorCode, "test message", obj));
            Assert.True(ex.ErrorCode == errorCode);
            Assert.True(ex.Message.Contains("test message"));
            Assert.True(ReferenceEquals(obj, false.ThrowIf(errorCode, "test message", obj)));

            ex = Assert.Throws<DdnDfException>(
                () => false.ThrowIfNot(errorCode, () => "test message", obj));
            Assert.True(ex.ErrorCode == errorCode);
            Assert.True(ex.Message.Contains("test message"));
            Assert.True(ReferenceEquals(obj, false.ThrowIf(errorCode, "test message", obj)));

            ex = Assert.Throws<DdnDfException>(
                () => false.ThrowIfNot(errorCode, obj));
            Assert.True(ex.ErrorCode == errorCode);
            Assert.True(!ex.Message.Contains("test message"));
            Assert.True(ReferenceEquals(obj, false.ThrowIf(errorCode, "test message", obj)));
        }

        [Test]
        [TestCase(null)]
        public void ThrowIfNull_ThrowsError_When_Object_Is_Null(string nullStr)
        {
            var ex = Assert.Throws<DdnDfException>(() => nullStr.ThrowIfNull("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullObject);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => nullStr.ThrowIfNull());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullObject);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => nullStr.ThrowIfNull(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullObject);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase("")]
        [TestCase("      ")]
        [TestCase("a")]
        public void ThrowIfNull_Returns_The_Object_For_Chaining_When_Not_Null(string val)
        {
            Assert.True(ReferenceEquals(val.ThrowIfNull("test message"), val));
            Assert.True(ReferenceEquals(val.ThrowIfNull(), val));
            Assert.True(ReferenceEquals(val.ThrowIfNull(() => "some error message"), val));
        }

        [Test]
        [TestCase(null)]
        public void ThrowIfNullOrEmpty_ThrowsError_When_Array_Is_NullOrEmpty(ICollection nullArr)
        {
            var ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(ex.Message.Contains("some error message"));

            nullArr = new string[0];
            ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(ex.Message.Contains("test message"));

            nullArr = new List<string>();
            ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(!ex.Message.Contains("test message"));

            nullArr = new Dictionary<string, string>();
            ex = Assert.Throws<DdnDfException>(() => nullArr.ThrowIfNullOrEmpty(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.NullOrEmptyCollection);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        public void ThrowIfNullOrEmpty_Returns_The_Array_For_Chaining_When_Not_NullOrEmpty()
        {
            ICollection val = new List<string> {"something"};
            Assert.True(ReferenceEquals(val.ThrowIfNullOrEmpty("test message"), val));

            val = new string[1];
            Assert.True(ReferenceEquals(val.ThrowIfNullOrEmpty(), val));

            val = new Dictionary<int, int> {{1, 1}};
            Assert.True(ReferenceEquals(val.ThrowIfNullOrEmpty(() => "some error message"), val));
        }

        [Test]
        public void ThrowOnMiss_On_Collection_ThrowsError_When_Value_Is_Not_In_The_Collection()
        {
            ICollection<int> coll = new HashSet<int> {1, 2, 3, 4, 5, 6};
            var ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(ex.Message.Contains("some error message"));

            coll = new List<int> {1, 2, 3, 4, 5, 6};
            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueNotInCollection);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        public void ThrowOnMiss_On_Collection_Returns_The_Collection_For_Chaining_When_Value_Is_Found()
        {
            ICollection<int> coll = new HashSet<int> {1, 2, 3, 4, 5, 6};
            Assert.True(ReferenceEquals(coll.ThrowOnMiss(1, "test message"), coll));

            coll = new List<int> {1, 2, 3, 4, 5, 6};
            Assert.True(ReferenceEquals(coll.ThrowOnMiss(1), coll));

            coll = new[] {1, 2, 3, 4, 5, 6};
            Assert.True(ReferenceEquals(coll.ThrowOnMiss(1, () => "test message"), coll));
        }

        [Test]
        public void ThrowOnMiss_On_Dictionary_ThrowsError_When_Lookup_Fails()
        {
            IReadOnlyDictionary<int, int> coll = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2}
            };
            var ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(ex.Message.Contains("some error message"));

            coll = new ConcurrentDictionary<int, int>(coll);
            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => coll.ThrowOnMiss(0, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.KeyNotFound);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        public void ThrowOnMiss_On_Dictionary_Returns_The_Value_For_Chaining_When_Lookup_Passes()
        {
            IReadOnlyDictionary<int, int> coll = new Dictionary<int, int>
            {
                {1, 1},
                {2, 2}
            };
            Assert.True(Equals(coll.ThrowOnMiss(1, "test message"), 1));

            coll = new ConcurrentDictionary<int, int>(coll);
            Assert.True(Equals(coll.ThrowOnMiss(1), 1));
            Assert.True(Equals(coll.ThrowOnMiss(1, () => "test message"), 1));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        public void ThrowIfNegative_ThrowsError_If_Value_Is_Strictly_Less_Than_Zero(int value)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        [TestCase(long.MinValue)]
        public void ThrowIfNegative_ThrowsError_If_Value_Is_Strictly_Less_Than_Zero(long value)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNegative(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void ThrowIfNegative_Returns_The_Value_When_It_Is_Zero_Or_Positive(int value)
        {
            Assert.True(value.ThrowIfNegative("test message").Equals(value));
            Assert.True(value.ThrowIfNegative().Equals(value));
            Assert.True(value.ThrowIfNegative(() => "some error message").Equals(value));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        [TestCase(long.MaxValue)]
        public void ThrowIfNegative_Returns_The_Value_When_It_Is_Zero_Or_Positive(long value)
        {
            Assert.True(value.ThrowIfNegative("test message").Equals(value));
            Assert.True(value.ThrowIfNegative().Equals(value));
            Assert.True(value.ThrowIfNegative(() => "some error message").Equals(value));
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(-10, -9)]
        [TestCase(int.MinValue, -10)]
        public void ThrowIfLess_ThrowsError_If_Value_Is_Strictly_Less_Than_Threshold(int value, int threshold)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(-10, -9)]
        [TestCase(int.MinValue, -10)]
        [TestCase(long.MinValue, int.MinValue)]
        public void ThrowIfLess_ThrowsError_If_Value_Is_Strictly_Less_Than_Threshold(long value, long threshold)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfLess(threshold, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueLessThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(-1, -2)]
        [TestCase(0, 0)]
        [TestCase(-10, -11)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void ThrowIfLess_Returns_The_Value_It_Is_Greater_Or_Equal_To_Threshold(int value, int threshold)
        {
            Assert.True(value.ThrowIfLess(threshold, "test message").Equals(value));
            Assert.True(value.ThrowIfLess(threshold).Equals(value));
            Assert.True(value.ThrowIfLess(threshold, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(-1, -2)]
        [TestCase(0, 0)]
        [TestCase(-10, -11)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(long.MinValue, long.MinValue)]
        [TestCase(long.MaxValue, long.MinValue)]
        [TestCase(long.MaxValue, int.MinValue)]
        [TestCase(long.MaxValue, int.MaxValue)]
        [TestCase(long.MaxValue, long.MaxValue)]
        public void ThrowIfLess_Returns_The_Value_It_Is_Greater_Or_Equal_To_Threshold(long value, long threshold)
        {
            Assert.True(value.ThrowIfLess(threshold, "test message").Equals(value));
            Assert.True(value.ThrowIfLess(threshold).Equals(value));
            Assert.True(value.ThrowIfLess(threshold, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void ThrowIfPositive_ThrowsError_If_Value_Is_Strictly_Greater_Than_Zero(int value)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        [TestCase(long.MaxValue)]
        public void ThrowIfPositive_ThrowsError_If_Value_Is_Strictly_Greater_Than_Zero(long value)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive("test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive());
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfPositive(() => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        public void ThrowIfPositive_Returns_The_Value_When_It_Is_Zero_Or_Negative(int value)
        {
            Assert.True(value.ThrowIfPositive("test message").Equals(value));
            Assert.True(value.ThrowIfPositive().Equals(value));
            Assert.True(value.ThrowIfPositive(() => "some error message").Equals(value));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(int.MinValue)]
        [TestCase(long.MinValue)]
        public void ThrowIfPositive_Returns_The_Value_When_It_Is_Zero_Or_Negative(long value)
        {
            Assert.True(value.ThrowIfPositive("test message").Equals(value));
            Assert.True(value.ThrowIfPositive().Equals(value));
            Assert.True(value.ThrowIfPositive(() => "some error message").Equals(value));
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(10, 9)]
        [TestCase(int.MaxValue, 10)]
        public void ThrowIfGreater_ThrowsError_If_Value_Is_Strictly_Greater_Than_Threshold(int value, int threshold)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(10, 9)]
        [TestCase(int.MaxValue, 10)]
        [TestCase(long.MaxValue, int.MaxValue)]
        public void ThrowIfGreater_ThrowsError_If_Value_Is_Strictly_Greater_Than_Threshold(long value, long threshold)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfGreater(threshold, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueGreaterThanThreshold);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(-1, 1)]
        [TestCase(0, 0)]
        [TestCase(-10, -9)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void ThrowIfGreater_Returns_The_Value_It_Is_Less_Or_Equal_To_Threshold(int value, int threshold)
        {
            Assert.True(value.ThrowIfGreater(threshold, "test message").Equals(value));
            Assert.True(value.ThrowIfGreater(threshold).Equals(value));
            Assert.True(value.ThrowIfGreater(threshold, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(-1, 1)]
        [TestCase(0, 0)]
        [TestCase(-10, -1)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(long.MinValue, long.MinValue)]
        [TestCase(long.MinValue, long.MaxValue)]
        [TestCase(long.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, long.MaxValue)]
        [TestCase(long.MaxValue, long.MaxValue)]
        public void ThrowIfGreater_Returns_The_Value_It_Is_Less_Or_Equal_To_Threshold(long value, long threshold)
        {
            Assert.True(value.ThrowIfGreater(threshold, "test message").Equals(value));
            Assert.True(value.ThrowIfGreater(threshold).Equals(value));
            Assert.True(value.ThrowIfGreater(threshold, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(1, 0, 0)]
        [TestCase(10, 9, -10)]
        [TestCase(-10, -9, -1)]
        [TestCase(-10, -11, -12)]
        [TestCase(int.MaxValue, 10, int.MaxValue - 1)]
        [TestCase(int.MinValue, int.MinValue + 1, int.MaxValue)]
        public void ThrowIfNotBounded_ThrowsError_If_Value_Is_Strictly_Outside_Of_Given_Bounds(int value, int bound1,
            int bound2)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(1, 0, 0)]
        [TestCase(10, 9, -10)]
        [TestCase(-10, -9, -1)]
        [TestCase(-10, -11, -12)]
        [TestCase(int.MaxValue, 10, int.MaxValue - 1)]
        [TestCase(int.MinValue, int.MinValue + 1, int.MaxValue)]
        [TestCase(long.MaxValue, int.MinValue, int.MaxValue)]
        [TestCase(long.MinValue, int.MaxValue, int.MinValue)]
        [TestCase(long.MaxValue, 10, long.MaxValue - 1)]
        [TestCase(long.MinValue, long.MinValue + 1, long.MaxValue)]
        public void ThrowIfNotBounded_ThrowsError_If_Value_Is_Strictly_Outside_Of_Given_Bounds(long value, long bound1,
            long bound2)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfNotBounded(bound1, bound2, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueOutOfBound);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 1, 1)]
        [TestCase(10, -9, 10)]
        [TestCase(-10, 9, -19)]
        [TestCase(-10, -11, 12)]
        [TestCase(int.MaxValue-1, 10, int.MaxValue)]
        [TestCase(int.MinValue+1, int.MinValue, int.MaxValue)]
        public void ThrowIfNotBounded_Returns_The_Value_If_It_Is_On_Or_Inside_The_Bounds(int value, int bound1,
            int bound2)
        {
            Assert.True(value.ThrowIfNotBounded(bound1, bound2, "test message").Equals(value));
            Assert.True(value.ThrowIfNotBounded(bound1, bound2).Equals(value));
            Assert.True(value.ThrowIfNotBounded(bound1, bound2, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 1, 1)]
        [TestCase(10, 20, -10)]
        [TestCase(int.MaxValue-1, 10, int.MaxValue)]
        [TestCase(int.MinValue+1, int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, long.MinValue, long.MaxValue)]
        [TestCase(int.MinValue, long.MaxValue, long.MinValue)]
        [TestCase(long.MaxValue, 10, long.MaxValue)]
        [TestCase(long.MinValue, long.MinValue, long.MaxValue)]
        public void ThrowIfNotBounded_Returns_The_Value_If_It_Is_On_Or_Inside_The_Bounds(long value, long bound1,
            long bound2)
        {
            Assert.True(value.ThrowIfNotBounded(bound1, bound2, "test message").Equals(value));
            Assert.True(value.ThrowIfNotBounded(bound1, bound2).Equals(value));
            Assert.True(value.ThrowIfNotBounded(bound1, bound2, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 1, 1)]
        [TestCase(10, -9, 10)]
        [TestCase(-10, 9, -19)]
        [TestCase(-10, -11, 12)]
        [TestCase(int.MaxValue - 1, 10, int.MaxValue)]
        [TestCase(int.MinValue + 1, int.MinValue, int.MaxValue)]
        public void ThrowIfBounded_ThrowsError_If_Value_Is_On_Or_Inside_Of_Given_Bounds(int value, int bound1,
            int bound2)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 1, -1)]
        [TestCase(1, 1, 1)]
        [TestCase(10, 20, -10)]
        [TestCase(int.MaxValue - 1, 10, int.MaxValue)]
        [TestCase(int.MinValue + 1, int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, long.MinValue, long.MaxValue)]
        [TestCase(int.MinValue, long.MaxValue, long.MinValue)]
        [TestCase(long.MaxValue, 10, long.MaxValue)]
        [TestCase(long.MinValue, long.MinValue, long.MaxValue)]
        public void ThrowIfBounded_ThrowsError_If_Value_Is_On_Or_Inside_Of_Given_Bounds(long value, long bound1,
            long bound2)
        {
            var ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2, "test message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(!ex.Message.Contains("test message"));

            ex = Assert.Throws<DdnDfException>(() => value.ThrowIfBounded(bound1, bound2, () => "some error message"));
            Assert.True(ex.ErrorCode == DdnDfErrorCode.ValueInBound);
            Assert.True(ex.Message.Contains("some error message"));
        }

        [Test]
        [TestCase(1, 0, 0)]
        [TestCase(10, 9, -10)]
        [TestCase(-10, -9, -1)]
        [TestCase(-10, -11, -12)]
        [TestCase(int.MaxValue, 10, int.MaxValue - 1)]
        [TestCase(int.MinValue, int.MinValue + 1, int.MaxValue)]
        public void ThrowIfBounded_Returns_The_Value_If_It_Is_Strictly_Outside_The_Bounds(int value, int bound1,
            int bound2)
        {
            Assert.True(value.ThrowIfBounded(bound1, bound2, "test message").Equals(value));
            Assert.True(value.ThrowIfBounded(bound1, bound2).Equals(value));
            Assert.True(value.ThrowIfBounded(bound1, bound2, () => "some error message").Equals(value));
        }

        [Test]
        [TestCase(1, 0, 0)]
        [TestCase(10, 9, -10)]
        [TestCase(-10, -9, -1)]
        [TestCase(-10, -11, -12)]
        [TestCase(int.MaxValue, 10, int.MaxValue - 1)]
        [TestCase(int.MinValue, int.MinValue + 1, int.MaxValue)]
        [TestCase(long.MaxValue, int.MinValue, int.MaxValue)]
        [TestCase(long.MinValue, int.MaxValue, int.MinValue)]
        [TestCase(long.MaxValue, 10, long.MaxValue - 1)]
        [TestCase(long.MinValue, long.MinValue + 1, long.MaxValue)]
        public void ThrowIfBounded_Returns_The_Value_If_It_Strictly_Outside_The_Bounds(long value, long bound1,
            long bound2)
        {
            Assert.True(value.ThrowIfBounded(bound1, bound2, "test message").Equals(value));
            Assert.True(value.ThrowIfBounded(bound1, bound2).Equals(value));
            Assert.True(value.ThrowIfBounded(bound1, bound2, () => "some error message").Equals(value));
        }
    }
}