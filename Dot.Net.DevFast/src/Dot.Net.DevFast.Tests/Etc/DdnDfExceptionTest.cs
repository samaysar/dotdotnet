using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Dot.Net.DevFast.Etc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Etc
{
    [TestFixture]
    public class DdnDfExceptionTest
    {
        [Test]
        [TestCase(DdnDfErrorCode.Unspecified)]
        [TestCase(DdnDfErrorCode.NullString)]
        public void Ctor_Sets_Error_Code_As_Message(DdnDfErrorCode errorCode)
        {
            var error = Assert.Throws<DdnDfException>(() => throw new DdnDfException(errorCode));
            Assert.True(error.ErrorCode.Equals(errorCode));
            Assert.True(error.Reason.Equals(errorCode.ToString()));
            Assert.True(error.Message.Equals(errorCode.ToString()));
        }

        [Test]
        [TestCase(DdnDfErrorCode.Unspecified, "any thing")]
        [TestCase(DdnDfErrorCode.NullString, "  any error message")]
        [TestCase(DdnDfErrorCode.Unspecified, "")]
        [TestCase(DdnDfErrorCode.NullString, null)]
        public void Ctor_Concats_ErrorCode_N_Message_As_Base_Message(DdnDfErrorCode errorCode,
            string message)
        {
            var error = Assert.Throws<DdnDfException>(() => throw new DdnDfException(errorCode, message));
            Assert.True(error.ErrorCode.Equals(errorCode));
            Assert.True(error.Reason.Equals(errorCode.ToString()));
            Assert.True(error.Message.Equals($"({errorCode}) {message}"));
        }

        [Test]
        [TestCase(DdnDfErrorCode.NullString, "any thing")]
        [TestCase(DdnDfErrorCode.Unspecified, "  any error message")]
        [TestCase(DdnDfErrorCode.NullString, "")]
        [TestCase(DdnDfErrorCode.Unspecified, null)]
        public void Ctor_Passes_Inner_Exception_To_Base_As_It_Is(DdnDfErrorCode errorCode,
            string message)
        {
            var inner = Assert.Throws<DdnDfException>(() => throw new DdnDfException(errorCode, message));

            var error = Assert.Throws<DdnDfException>(() => throw new DdnDfException(errorCode, message, inner));
            Assert.True(error.ErrorCode.Equals(errorCode));
            Assert.True(error.Reason.Equals(errorCode.ToString()));
            Assert.True(error.Message.Equals($"({errorCode}) {message}"));
            Assert.True(ReferenceEquals(error.InnerException, inner));
        }

        [Test]
        public void GetObjectData_Properly_Inserts_ErrorReason()
        {
            var sinfo = new SerializationInfo(typeof(DdnDfException), new FormatterConverter());
            var sctxt = new StreamingContext();
            var ddndfEx = new DdnDfException(DdnDfErrorCode.JsonIsNotAnArray);

            ddndfEx.GetObjectData(sinfo, sctxt);
            var copy = new DdnDfException(sinfo, sctxt);
            Assert.AreEqual(copy.Reason, DdnDfErrorCode.JsonIsNotAnArray.ToString());
        }
    }
}