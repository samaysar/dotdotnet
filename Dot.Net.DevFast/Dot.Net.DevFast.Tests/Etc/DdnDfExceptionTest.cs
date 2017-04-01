using Dot.Net.DevFast.Etc;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Etc
{
    [TestFixture]
    public class DdnDfExceptionTest
    {
        [Test]
        public void Default_Ctor_Sets_Expected_Error_Code()
        {
            var error = Assert.Throws<DdnDfException>(() =>
            {
                throw new DdnDfException();
            });
            Assert.True(error.ErrorCode == DdnDfErrorCode.Unspecified);
            Assert.True(error.Message.Equals(DdnDfErrorCode.Unspecified.ToString("G")));
        }

        [Test]
        [TestCase(DdnDfErrorCode.Unspecified)]
        [TestCase(DdnDfErrorCode.NullString)]
        public void Ctor_Sets_Error_Code_As_Message(DdnDfErrorCode errorCode)
        {
            var error = Assert.Throws<DdnDfException>(() =>
            {
                throw new DdnDfException(errorCode);
            });
            Assert.True(error.ErrorCode == errorCode);
            Assert.True(error.Message.Equals(errorCode.ToString("G")));
        }

        [Test]
        [TestCase(DdnDfErrorCode.Unspecified, "any thing")]
        [TestCase(DdnDfErrorCode.NullString, "  any error message")]
        [TestCase(DdnDfErrorCode.Unspecified, "")]
        [TestCase(DdnDfErrorCode.NullString, null)]
        public void Ctor_Concats_ErrorCode_N_Message_As_Base_Message(DdnDfErrorCode errorCode,
            string message)
        {
            var error = Assert.Throws<DdnDfException>(() =>
            {
                throw new DdnDfException(errorCode, message);
            });
            Assert.True(error.ErrorCode == errorCode);
            Assert.True(error.Message.Equals($"{errorCode:G}. {message}"));
        }

        [Test]
        [TestCase(DdnDfErrorCode.NullString, "any thing")]
        [TestCase(DdnDfErrorCode.Unspecified, "  any error message")]
        [TestCase(DdnDfErrorCode.NullString, "")]
        [TestCase(DdnDfErrorCode.Unspecified, null)]
        public void Ctor_Passes_Inner_Exception_To_Base_As_It_Is(DdnDfErrorCode errorCode,
            string message)
        {
            var inner = Assert.Throws<DdnDfException>(() =>
            {
                throw new DdnDfException(errorCode, message);
            });

            var error = Assert.Throws<DdnDfException>(() =>
            {
                throw new DdnDfException(errorCode, message, inner);
            });
            Assert.True(error.ErrorCode == errorCode);
            Assert.True(error.Message.Equals($"{errorCode:G}. {message}"));
            Assert.True(ReferenceEquals(error.InnerException, inner));
        }
    }
}