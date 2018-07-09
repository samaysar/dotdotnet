#if !NET472
using System;
using System.IO;
#if !NETSTANDARD2_0 && !NETCOREAPP2_0
using System.Runtime.Remoting;
#endif
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions.Internals;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions.Internals
{
    [TestFixture]
    public class WrappedStreamTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Dispose_Of_WrappedStream_Works_As_Expected(bool dispose)
        {
            var strm = Substitute.For<Stream>();
            using (new WrappedStream(strm, dispose))
            {
            }
            strm.Received(dispose ? 1 : 0).Dispose();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Boolean_Properties_Are_Properly_Wrapped(bool propVal)
        {
            var strm = Substitute.For<Stream>();
            strm.CanRead.Returns(propVal);
            strm.CanSeek.Returns(propVal);
            strm.CanWrite.Returns(propVal);
            strm.CanTimeout.Returns(propVal);

            using (var wrprstrm = new WrappedStream(strm, false))
            {
                Assert.True(wrprstrm.CanRead.Equals(strm.CanRead));
                Assert.True(wrprstrm.CanSeek.Equals(strm.CanSeek));
                Assert.True(wrprstrm.CanWrite.Equals(strm.CanWrite));
                Assert.True(wrprstrm.CanTimeout.Equals(strm.CanTimeout));
            }
        }

        [Test]
        [TestCase(10)]
        [TestCase(100)]
        public void Numeric_Properties_Are_Properly_Wrapped(int propVal)
        {
            var strm = Substitute.For<Stream>();
            strm.Length.Returns(propVal);
            strm.Position.Returns(propVal);
            strm.WriteTimeout.Returns(propVal);
            strm.ReadTimeout.Returns(propVal);

            using (var wrprstrm = new WrappedStream(strm, false))
            {
                Assert.True(wrprstrm.Length.Equals(strm.Length));
                Assert.True(wrprstrm.Position.Equals(strm.Position));
                Assert.True(wrprstrm.WriteTimeout.Equals(strm.WriteTimeout));
                Assert.True(wrprstrm.ReadTimeout.Equals(strm.ReadTimeout));

                wrprstrm.Position = propVal;
                strm.Received(1).Position = propVal;
                wrprstrm.WriteTimeout = propVal;
                strm.Received(1).WriteTimeout = propVal;
                wrprstrm.ReadTimeout = propVal;
                strm.Received(1).ReadTimeout = propVal;
            }
        }

        [Test]
        public void Paramless_Methods_Are_Properly_Wrapped()
        {
            var strm = Substitute.For<Stream>();
            using (var wrprstrm = new WrappedStream(strm, false))
            {
                var _ = wrprstrm.ReadByte();
                var __ = strm.Received(1).ReadByte();

                wrprstrm.Flush();
                strm.Received(1).Flush();

                var ___ = wrprstrm.GetHashCode();
                var ____ = strm.Received(1).GetHashCode();

                var _____ = wrprstrm.ToString();
                var ______ = strm.Received(1).ToString();
            }
        }

        [Test]
        public async Task Parameterized_Methods_Are_Properly_Wrapped()
        {
            var strm = Substitute.For<Stream>();
            using (var wrprstrm = new WrappedStream(strm, false))
            {
                wrprstrm.Seek(0, SeekOrigin.Current);
                strm.Received(1).Seek(0, SeekOrigin.Current);

                wrprstrm.SetLength(10);
                strm.Received(1).SetLength(10);

                var arrByte = new byte[5];
                wrprstrm.Read(arrByte, 0, arrByte.Length);
                strm.Received(1).Read(arrByte, 0, arrByte.Length);

                wrprstrm.Write(arrByte, 0, arrByte.Length);
                strm.Received(1).Write(arrByte, 0, arrByte.Length);

                var anotherStrm = new MemoryStream();
                await wrprstrm.CopyToAsync(anotherStrm, 1, CancellationToken.None).ConfigureAwait(false);
                await strm.Received(1).CopyToAsync(anotherStrm, 1, CancellationToken.None).ConfigureAwait(false);

                await wrprstrm.FlushAsync(CancellationToken.None).ConfigureAwait(false);
                await strm.Received(1).FlushAsync(CancellationToken.None).ConfigureAwait(false);

                await wrprstrm.WriteAsync(arrByte, 1, arrByte.Length, CancellationToken.None).ConfigureAwait(false);
                await strm.Received(1)
                    .WriteAsync(arrByte, 1, arrByte.Length, CancellationToken.None)
                    .ConfigureAwait(false);

                await wrprstrm.ReadAsync(arrByte, 1, arrByte.Length, CancellationToken.None).ConfigureAwait(false);
                await strm.Received(1)
                    .ReadAsync(arrByte, 1, arrByte.Length, CancellationToken.None)
                    .ConfigureAwait(false);

                var cb = new AsyncCallback(ar => { });
                var result = wrprstrm.BeginRead(arrByte, 0, arrByte.Length, cb, null);
                strm.Received(1).BeginRead(arrByte, 0, arrByte.Length, cb, null);

                wrprstrm.BeginWrite(arrByte, 0, arrByte.Length, cb, null);
                strm.Received(1).BeginWrite(arrByte, 0, arrByte.Length, cb, null);

#if FEATURE_REMOTING
                wrprstrm.CreateObjRef(null);
                strm.Received(1).CreateObjRef(null);

                wrprstrm.InitializeLifetimeService();
                strm.Received(1).InitializeLifetimeService();
#else
#if !NETSTANDARD2_0 && !NETCOREAPP2_0
                Assert.True(Assert.Throws<RemotingException>(() => wrprstrm.CreateObjRef(null))
                    .Message.Equals(WrappedStream.RemotingErrorTxt));
                Assert.True(Assert.Throws<RemotingException>(() => wrprstrm.InitializeLifetimeService())
                    .Message.Equals(WrappedStream.RemotingErrorTxt));
#endif
#endif
                wrprstrm.EndRead(result);
                strm.Received(1).EndRead(result);

                wrprstrm.EndWrite(result);
                strm.Received(1).EndWrite(result);

                wrprstrm.WriteByte(20);
                strm.Received(1).WriteByte(20);

                var _ = wrprstrm.Equals(null);
                var __ = strm.Received(1).Equals(null);
            }
        }
    }
}
#endif