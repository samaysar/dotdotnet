using System.Collections;
using System.Collections.Generic;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class ThrowErrorTest
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
    }
}