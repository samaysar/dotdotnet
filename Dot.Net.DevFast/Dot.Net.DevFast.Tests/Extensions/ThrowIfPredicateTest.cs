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
    }
}