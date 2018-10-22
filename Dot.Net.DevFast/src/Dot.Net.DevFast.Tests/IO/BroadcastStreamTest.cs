using System;
using System.IO;
using System.Threading;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.IO;
using NSubstitute;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.IO
{
    [TestFixture]
    public class BroadcastStreamTest
    {
        [Test]
        public void Ctor_Throws_Error_If_Stream_Is_Not_Writable_Or_Pfs_Is_Default()
        {
            Assert.True(Assert.Throws<DdnDfException>(() =>
            {
                var nonWritable = Substitute.For<Stream>();
                nonWritable.CanWrite.Returns(false);
                var _ = new BroadcastStream(new PushFuncStream(Stream.Null, false, CancellationToken.None),
                    nonWritable, true);
            }).ErrorCode == DdnDfErrorCode.Unspecified);

            Assert.True(Assert.Throws<DdnDfException>(() =>
            {
                var nonWritable = Substitute.For<Stream>();
                nonWritable.CanWrite.Returns(true);
                var _ = new BroadcastStream(default(PushFuncStream), nonWritable, true);
            }).ErrorCode == DdnDfErrorCode.NullObject);
        }

        [Test]
        public void Flush_Flushes_Both_Underlying_Streams()
        {
            var stm1 = Substitute.For<Stream>();
            var stm2 = Substitute.For<Stream>();
            stm1.CanWrite.Returns(true);
            stm2.CanWrite.Returns(true);
            using (var instance =
                new BroadcastStream(new PushFuncStream(stm1, true, CancellationToken.None), stm2, true))
            {
                instance.Flush();
                stm1.Received(1).FlushAsync();
                stm2.Received(1).FlushAsync();
            }
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(true, true)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void Dispose_Works_As_Expected(bool disposePfs, bool disposeStrm)
        {
            var stm1 = Substitute.For<Stream>();
            var stm2 = Substitute.For<Stream>();
            stm1.CanWrite.Returns(true);
            stm2.CanWrite.Returns(true);
            using (var _ = new BroadcastStream(new PushFuncStream(stm1, disposePfs, CancellationToken.None),
                stm2, disposeStrm))
            {
            }
            stm1.Received(disposePfs ? 1 : 0).Dispose();
            stm2.Received(disposeStrm ? 1 : 0).Dispose();
        }

        [Test]
        public void Default_Properties_Are_Consistent_So_Are_Methods()
        {
            var stm1 = Substitute.For<Stream>();
            var stm2 = Substitute.For<Stream>();
            stm1.CanWrite.Returns(true);
            stm2.CanWrite.Returns(true);
            using (var instance =
                new BroadcastStream(new PushFuncStream(stm1, true, CancellationToken.None), stm2, true))
            {
                Assert.True(instance.CanWrite);
                Assert.False(instance.CanSeek);
                Assert.False(instance.CanRead);

                Assert.True(instance.Length == 0);
                instance.SetLength(10);
                Assert.True(instance.Length == 0);

                Assert.True(instance.Position == 0);
                instance.Position = 10;
                Assert.True(instance.Position == 0);

                Assert.Throws<NotImplementedException>(() => instance.Seek(10, SeekOrigin.Begin));
                Assert.Throws<NotImplementedException>(() => instance.Read(new byte[0], 0, 0 ));
            }
        }

        [Test]
        public void Write_Method_Invoke_Write_On_Both_Underlying_Streams()
        {
            var stm1 = Substitute.For<Stream>();
            var stm2 = Substitute.For<Stream>();
            stm1.CanWrite.Returns(true);
            stm2.CanWrite.Returns(true);
            using (var instance =
                new BroadcastStream(new PushFuncStream(stm1, true, CancellationToken.None), stm2, true))
            {
                var bytes = new byte[0];
                instance.Write(bytes, 0, 0);
                stm1.Received(1).WriteAsync(bytes, 0, 0, Arg.Any<CancellationToken>());
                stm2.Received(1).WriteAsync(bytes, 0, 0, Arg.Any<CancellationToken>());
            }
        }
    }
}