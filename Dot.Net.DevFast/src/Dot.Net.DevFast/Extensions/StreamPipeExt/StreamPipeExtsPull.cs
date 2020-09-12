﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.Internals;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.Ppc;
using Dot.Net.DevFast.Extensions.StringExt;
using Dot.Net.DevFast.IO;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    public static partial class StreamPipeExts
    {
        #region Various Pull

        /// <summary>
        /// Pulls underlying data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="folder">Parent folder of the file</param>
        /// <param name="filename">An existing readable file's name with extension</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PullFuncStream> Pull(this DirectoryInfo folder,
            string filename,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return folder.CreateFileInfo(filename).Pull(fileStreamBuffer, options);
        }

        /// <summary>
        /// Pulls underlying data of the given file as byte stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="fileinfo">Fileinfo instance of an existing readable file</param>
        /// <param name="fileStreamBuffer">Buffer size to use</param>
        /// <param name="options">File options</param>
        public static Func<PullFuncStream> Pull(this FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan)
        {
            return fileinfo.CreateStream(FileMode.Open, FileAccess.Read, FileShare.Read, fileStreamBuffer, options)
                .Pull();
        }

        /// <summary>
        /// Pulls bytes from given array and returns a new pipe for chaining.
        /// Supplied <paramref name="byteTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteTask">Task returning Source byte array. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<byte[]> byteTask)
        {
            return async () =>
            {
                var bytes = await byteTask.StartIfNeeded().ConfigureAwait(false);
                return bytes.Pull()();
            };
        }

        /// <summary>
        /// Pulls bytes from given array and returns a new pipe for chaining.
        /// </summary>
        /// <param name="bytes">Source byte array</param>
        public static Func<PullFuncStream> Pull(this byte[] bytes)
        {
            return new ArraySegment<byte>(bytes, 0, bytes.Length).Pull();
        }

        /// <summary>
        /// Pulls bytes from given byte segment and returns a new pipe for chaining.
        /// Supplied <paramref name="byteSegTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="byteSegTask">task returning Source array segment. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<ArraySegment<byte>> byteSegTask)
        {
            return async () =>
            {
                var byteSeg = await byteSegTask.StartIfNeeded().ConfigureAwait(false);
                return byteSeg.Pull()();
            };
        }

        /// <summary>
        /// Pulls bytes from given byte segment and returns a new pipe for chaining.
        /// </summary>
        /// <param name="byteSeg">Source byte array</param>
        public static Func<PullFuncStream> Pull(this ArraySegment<byte> byteSeg)
        {
            return new MemoryStream(byteSeg.Array, byteSeg.Offset, byteSeg.Count, false, false).Pull();
        }

        /// <summary>
        /// Pulls bytes from given source stream and returns a new pipe for chaining.
        /// Supplied <paramref name="streamTask"/> is awaited during bootstrapping (NOT during chaining) 
        /// </summary>
        /// <param name="streamTask">Task returning Source data stream. If the task is just created,
        ///  it will be started during bootstrapping.</param>
        /// <param name="disposeSourceStream">If true, source stream is disposed</param>
        public static Func<Task<PullFuncStream>> Pull(this Task<Stream> streamTask,
            bool disposeSourceStream = true)
        {
            return async () =>
            {
                var stream = await streamTask.StartIfNeeded().ConfigureAwait(false);
                return stream.Pull(disposeSourceStream)();
            };
        }

        /// <summary>
        /// Pulls the underlying data from the stream and returns a new pipe for chaining.
        /// </summary>
        /// <param name="stream">A readable stream</param>
        /// <param name="disposeSource">If true, stream is disposed at the end of streaming else left open</param>
        public static Func<PullFuncStream> Pull(this Stream stream,
            bool disposeSource = true)
        {
            return () => new PullFuncStream(stream, disposeSource);
        }

        #endregion Various Pull

        #region Then Clauses NoTASK

        /// <summary>
        /// Counts the number of bytes observed during pull based streaming (exposed through 
        /// <seealso cref="IByteCounter"/>.<seealso cref="IByteCounter.ByteCount"/>) 
        /// and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="IByteCounter.ByteCount"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="byteCounter"/>.ByteCount immediately
        /// after this call will not provide the correct count.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="byteCounter">out param which exposes <seealso cref="IByteCounter.ByteCount"/>) property.</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        public static Func<PullFuncStream> ThenCountBytes(this Func<PullFuncStream> src,
            out IByteCounter byteCounter,
            bool include = true)
        {
            var bcs = new ByteCountStream();
            byteCounter = bcs;
            return src.ThenApply(s => s.ApplyByteCount(bcs), include);
        }

        /// <summary>
        /// Applies decompression on the data of given functional Stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="pullSrc">Current pipe of the PUSH pipeline</param>
        /// <param name="include">If true is passed, decompression is performed else ignored</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        public static Func<PullFuncStream> ThenDecompress(this Func<PullFuncStream> pullSrc,
            bool gzip = true,
            bool include = true)
        {
            return pullSrc.ThenApply(p => p.ApplyDecompression(gzip), include);
        }

        /// <summary>
        /// Computes the hash on the data of the given functional stream pipe and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        /// <param name="include">If true is passed, hash is computed else ignored</param>
        public static Func<PullFuncStream> ThenComputeHash(this Func<PullFuncStream> src,
            HashAlgorithm ha,
            bool include = true)
        {
            return src.ThenTransform(ha, include);
        }

        /// <summary>
        /// Converts the data, of the given functional stream pipe to equivalent Base64
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, ToBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenToBase64(this Func<PullFuncStream> src,
            bool include = true)
        {
            return src.ThenTransform(new ToBase64Transform(), include);
        }

        /// <summary>
        /// Decodes the Base64 data, of the given functional stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="mode">Base64 transform mode</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenFromBase64(this Func<PullFuncStream> src,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool include = true)
        {
            return src.ThenTransform(new FromBase64Transform(mode), include);
        }

#if NET472
        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<PullFuncStream> ThenEncrypt<T>(this Func<PullFuncStream> src,
            string password,
            string salt,
#if NET472
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NET472
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), true), include);
        }

        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenEncrypt<T>(this Func<PullFuncStream> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, true), include);
        }

#if NET472
        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
/// <summary>
/// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
/// and returns a new pipe for chaining.
/// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
/// using plain text password and salt string.</para>
/// </summary>
/// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
/// <param name="src">Current pipe of the pipeline</param>
/// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="loopCnt">Loop count</param>
/// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
/// <param name="cipher">Cipher mode to use</param>
/// <param name="padding">Padding mode to use</param>
/// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<PullFuncStream> ThenDecrypt<T>(this Func<PullFuncStream> src,
            string password,
            string salt,
#if NET472
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NET472
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), false), include);
        }

        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<PullFuncStream> ThenDecrypt<T>(this Func<PullFuncStream> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, false), include);
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given functional stream pipe
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<PullFuncStream> ThenTransform(this Func<PullFuncStream> src,
            ICryptoTransform transformation,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyTransform(transformation), include);
        }

        /// <summary>
        /// Appends the given arbitrary custom functional stream pipe (i.e. <paramref name="applyFunc"/>) to the pipeline
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="applyFunc">Yet another custom functional stream pipe</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<PullFuncStream> ThenApply(this Func<PullFuncStream> src,
            Func<Func<PullFuncStream>, Func<PullFuncStream>> applyFunc,
            bool include = true)
        {
            return include ? applyFunc(src) : src;
        }

        #endregion Then Clauses NoTASK

        #region Then Clauses TASK

        /// <summary>
        /// Counts the number of bytes observed during pull based streaming (exposed through 
        /// <seealso cref="IByteCounter"/>.<seealso cref="IByteCounter.ByteCount"/>) 
        /// and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="IByteCounter.ByteCount"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="byteCounter"/>.ByteCount immediately
        /// after this call will not provide the correct count.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="byteCounter">out param which exposes <seealso cref="IByteCounter.ByteCount"/>) property.</param>
        /// <param name="include">If true is passed, compression is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenCountBytes(this Func<Task<PullFuncStream>> src,
            out IByteCounter byteCounter,
            bool include = true)
        {
            var bcs = new ByteCountStream();
            byteCounter = bcs;
            return src.ThenApply(s => s.ApplyByteCount(bcs), include);
        }

        /// <summary>
        /// Applies decompression on the data of given functional Stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="pullSrc">Current pipe of the PUSH pipeline</param>
        /// <param name="include">If true is passed, decompression is performed else ignored</param>
        /// <param name="gzip">If true, <seealso cref="GZipStream"/> is used else 
        /// <seealso cref="DeflateStream"/> is used</param>
        public static Func<Task<PullFuncStream>> ThenDecompress(this Func<Task<PullFuncStream>> pullSrc,
            bool gzip = true,
            bool include = true)
        {
            return pullSrc.ThenApply(p => p.ApplyDecompression(gzip), include);
        }

        /// <summary>
        /// Computes the hash on the data of the given functional stream pipe and returns a new pipe for chaining.
        /// <para>IMPORTANT: Access <seealso cref="HashAlgorithm.Hash"/> ONLY AFTER the full
        /// piepline is bootstrapped and processed, i.e., calling <paramref name="ha"/>.Hash immediately
        /// after this call will not provide the correct hash.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="ha">Instance of crypto hash algorithm</param>
        /// <param name="include">If true is passed, hash is computed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenComputeHash(this Func<Task<PullFuncStream>> src,
            HashAlgorithm ha,
            bool include = true)
        {
            return src.ThenTransform(ha, include);
        }

        /// <summary>
        /// Converts the data, of the given functional stream pipe to equivalent Base64
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="include">If true is passed, ToBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenToBase64(this Func<Task<PullFuncStream>> src,
            bool include = true)
        {
            return src.ThenTransform(new ToBase64Transform(), include);
        }

        /// <summary>
        /// Decodes the Base64 data, of the given functional stream pipe and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="mode">Base64 transform mode</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenFromBase64(this Func<Task<PullFuncStream>> src,
            FromBase64TransformMode mode = FromBase64TransformMode.IgnoreWhiteSpaces,
            bool include = true)
        {
            return src.ThenTransform(new FromBase64Transform(mode), include);
        }

#if NET472
        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
/// <summary>
/// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
/// and returns a new pipe for chaining.
/// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
/// using plain text password and salt string.</para>
/// </summary>
/// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
/// <param name="src">Current pipe of the pipeline</param>
/// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="loopCnt">Loop count</param>
/// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
/// <param name="cipher">Cipher mode to use</param>
/// <param name="padding">Padding mode to use</param>
/// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<Task<PullFuncStream>> ThenEncrypt<T>(this Func<Task<PullFuncStream>> src,
            string password,
            string salt,
#if NET472
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NET472
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), true), include);
        }

        /// <summary>
        /// Encrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenEncrypt<T>(this Func<Task<PullFuncStream>> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, true), include);
        }

#if NET472
        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
        /// using plain text password and salt string.</para>
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
        /// <param name="hashName">Hash algorithm to use</param>
        /// <param name="loopCnt">Loop count</param>
        /// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#else
/// <summary>
/// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
/// and returns a new pipe for chaining.
/// <para>NOTE:You may use <seealso cref="CreateExts.CreateKeyAndIv"/> extension method to create IV and KEY byte arrays
/// using plain text password and salt string.</para>
/// </summary>
/// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
/// <param name="src">Current pipe of the pipeline</param>
/// <param name="password">password for key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="salt">Salt string to use during key/IV generation (see <seealso cref="Rfc2898DeriveBytes"/>)</param>
/// <param name="loopCnt">Loop count</param>
/// <param name="enc">Encoding to use to convert password and salt to bytes. If not provided, UTF8Encoding(false) is used</param>
/// <param name="cipher">Cipher mode to use</param>
/// <param name="padding">Padding mode to use</param>
/// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
#endif
        public static Func<Task<PullFuncStream>> ThenDecrypt<T>(this Func<Task<PullFuncStream>> src,
            string password,
            string salt,
#if NET472
            HashAlgorithmName hashName,
#endif
            int loopCnt = 10000,
            Encoding enc = null,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            var encAlg = new T
            {
                Mode = cipher,
                Padding = padding
            };
            return src.ThenApply(s => s.ApplyCrypto(encAlg.InitKeyNIv(password, salt,
#if NET472
                hashName,
#endif
                loopCnt, enc ?? new UTF8Encoding(false)), false), include);
        }

        /// <summary>
        /// Decrypts the underlying data, of the given functional stream pipe based on give <seealso cref="SymmetricAlgorithm"/>,
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <typeparam name="T">Type of <seealso cref="SymmetricAlgorithm"/> to apply</typeparam>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="key">key byte array</param>
        /// <param name="iv">iv byte array</param>
        /// <param name="cipher">Cipher mode to use</param>
        /// <param name="padding">Padding mode to use</param>
        /// <param name="include">If true is passed, FromBase64 conversion is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenDecrypt<T>(this Func<Task<PullFuncStream>> src,
            byte[] key,
            byte[] iv,
            CipherMode cipher = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7,
            bool include = true)
            where T : SymmetricAlgorithm, new()
        {
            return src.ThenApply(s => s.ApplyCrypto(new T
            {
                Mode = cipher,
                Padding = padding,
                Key = key,
                IV = iv
            }, false), include);
        }

        /// <summary>
        /// Applies the given crypto transformation to the data of the given functional stream pipe
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="transformation">Crypto Transformation to apply</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenTransform(this Func<Task<PullFuncStream>> src,
            ICryptoTransform transformation,
            bool include = true)
        {
            return src.ThenApply(s => s.ApplyTransform(transformation), include);
        }

        /// <summary>
        /// Appends the given arbitrary custom functional stream pipe (i.e. <paramref name="applyFunc"/>) to the pipeline
        /// and returns a new pipe for chaining.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="applyFunc">Yet another custom functional stream pipe</param>
        /// <param name="include">If true is passed, CryptoTransform is performed else ignored</param>
        public static Func<Task<PullFuncStream>> ThenApply(this Func<Task<PullFuncStream>> src,
            Func<Func<Task<PullFuncStream>>, Func<Task<PullFuncStream>>> applyFunc,
            bool include = true)
        {
            return include ? applyFunc(src) : src;
        }

        #endregion Then Clauses TASK

        #region ConvertorToPush

        /// <summary>
        /// Converts the PULL pipeline to PUSH pipeline and returns it for chaining.
        /// </summary>
        /// <param name="src">Current PULL source pipe</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Func<PushFuncStream, Task> ThenConvertToPush(this Func<PullFuncStream> src,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return src.ToAsync(false).ThenConvertToPush(bufferSize);
        }

        /// <summary>
        /// Converts the PULL pipeline to PUSH pipeline and returns it for chaining.
        /// </summary>
        /// <param name="src">Current PULL source pipe</param>
        /// <param name="bufferSize">Buffer size</param>
        public static Func<PushFuncStream, Task> ThenConvertToPush(this Func<Task<PullFuncStream>> src,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return async pfs =>
            {
                await src.AndWriteStreamAsync(pfs.Writable, bufferSize, pfs.Dispose, pfs.Token)
                    .ConfigureAwait(false);
            };
        }

        #endregion ConvertorToPush

        #region Finalization NoTASK

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PullFuncStream> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            return await src.ToAsync(false).AndWriteFileAsync(folder, filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<PullFuncStream> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            return await src.ToAsync(false).AndWriteFileAsync(folder, filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteFileAsync(this Func<PullFuncStream> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            await src.ToAsync(false).AndWriteFileAsync(fileinfo, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<byte[]> AndWriteBytesAsync(this Func<PullFuncStream> src,
            CancellationToken token = default,
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteBytesAsync(token, estimatedSize, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<ArraySegment<byte>> AndWriteByteSegAsync(this Func<PullFuncStream> src,
            CancellationToken token = default,
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteByteSegAsync(token, estimatedSize, bufferSize)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        /// <param name="initialSize">Initial Memory buffer Size</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<MemoryStream> AndWriteBufferAsync(this Func<PullFuncStream> src,
            CancellationToken token = default,
            bool seekToOrigin = false,
            int initialSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return await src.ToAsync(false).AndWriteBufferAsync(token, seekToOrigin, initialSize, bufferSize)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline.
        /// <para>NOTE: Calling this function will result in running the streaming pipeline, but, you won't receive
        /// anything in the end. Normally, usage of this function is to avoid use of <seealso cref="MemoryStream"/>
        /// to reduce runtime memory pressure, and at the same time counting bytes, calculate crypto-hashes etc.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndExecuteAsync(this Func<PullFuncStream> src,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            await src.ToAsync(false).AndExecuteAsync(bufferSize, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStreamAsync(this Func<PullFuncStream> src,
            Stream writableTarget,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeTarget = false,
            CancellationToken token = default)
        {
            await src.ToAsync(false).AndWriteStreamAsync(writableTarget, bufferSize, disposeTarget, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as string.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of the string (optimization on resizing ops)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<string> AndWriteStringAsync(this Func<PullFuncStream> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            return await src.ToAsync(false)
                .AndWriteStringAsync(initialSize, enc, detectEncodingFromBom, bufferSize, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as newly created <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of StringBuilder (optimization on array resizing)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<StringBuilder> AndWriteStringBuilderAsync(this Func<PullFuncStream> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            return await src.ToAsync(false)
                .AndWriteStringBuilderAsync(initialSize, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false);

        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="sbToAppend">String builder to append data</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStringBuilderAsync(this Func<PullFuncStream> src,
            StringBuilder sbToAppend,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            await src.ToAsync(false)
                .AndWriteStringBuilderAsync(sbToAppend, enc, detectEncodingFromBom, bufferSize, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and pulls data throw the pipeline 
        /// in order to obtain deserialized object based on the Json text.
        /// </summary>
        /// <typeparam name="T">Type of the object from deserialization</typeparam>
        /// <param name="src"></param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static T AndParseJson<T>(this Func<PullFuncStream> src,
            JsonSerializer serializer = null,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var data = src();
            return data.Readable.FromJson<T>(serializer, enc ?? new UTF8Encoding(false),
                bufferSize, data.Dispose, detectEncodingFromBom);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and pulls data throw the pipeline 
        /// in order to achieve enumeration on json based deserialized objects.
        /// </summary>
        /// <typeparam name="T">Type of the object from deserialization</typeparam>
        /// <param name="src"></param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="objectBufferSize">Size of the intermediate object buffer in terms of number (count) of deserialized objects. NOTE: <see cref="ConcurrentBuffer.Unbounded"/> is a special number to create unbounded buffer.</param>
        /// <exception cref="DdnDfException">When given size is negative</exception>
        public static IEnumerable<T> AndParseJsonArray<T>(this Func<PullFuncStream> src,
            JsonSerializer serializer = null,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default,
            int objectBufferSize = ConcurrentBuffer.MinSize)
        {
            var data = src();
            return data.Readable.FromJsonAsEnumerable<T>(serializer, token, enc ?? new UTF8Encoding(false),
                bufferSize, data.Dispose, detectEncodingFromBom, objectBufferSize);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerAction">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<PullFuncStream> src,
            Action<T, CancellationToken> consumerAction,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerAction.ToAsync(false), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerFunc">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<PullFuncStream> src,
            Func<T, CancellationToken, Task> consumerFunc,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerFunc.ToConsumer(), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer instance to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<PullFuncStream> src,
            IConsumer<T> consumer,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumer, new IdentityAwaitableAdapter<T>(), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<PullFuncStream> src,
            Action<TC, CancellationToken> consumer,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumer.ToAsync(false), adapter, serializer, ppcBufferSize, token,
                enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<PullFuncStream> src,
            Func<TC, CancellationToken, Task> consumer,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumer.ToConsumer(), adapter, serializer, ppcBufferSize, token,
                enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer instance to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<PullFuncStream> src,
            IConsumer<TC> consumer,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var data = src();
            await data.ApplyPpcParseJsonArray(consumer, adapter, serializer, token, enc ?? new UTF8Encoding(false),
                detectEncodingFromBom, bufferSize, ppcBufferSize).ConfigureAwait(false);
        }

        #endregion Finalization NoTASK

        #region Finalization TASK

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            string folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            return await src.AndWriteFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead (without extension)</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<FileInfo> AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            DirectoryInfo folder,
            string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"));
            await src.AndWriteFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
            return targetFile;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline to the file.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteFileAsync(this Func<Task<PullFuncStream>> src,
            FileInfo fileinfo,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.SequentialScan,
            CancellationToken token = default)
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await src.AndWriteStreamAsync(strm, fileStreamBuffer, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<byte[]> AndWriteBytesAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default,
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await src.AndWriteByteSegAsync(token, estimatedSize, bufferSize).ConfigureAwait(false))
                .CreateBytes();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the results as byte array.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="estimatedSize">Intial guess for the size of the byte array (optimization on resizing operation).</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<ArraySegment<byte>> AndWriteByteSegAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default,
            int estimatedSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            return (await src.AndWriteBufferAsync(token, false, estimatedSize, bufferSize).ConfigureAwait(false))
                .ThrowIfNoBuffer();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns the contents as newly created <seealso cref="MemoryStream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="seekToOrigin">If true, Seek with <seealso cref="SeekOrigin.Begin"/> is performed else not.</param>
        /// <param name="initialSize">Initial Memory buffer Size</param>
        /// <param name="bufferSize">Buffer size for data-pull (copying) ops</param>
        public static async Task<MemoryStream> AndWriteBufferAsync(this Func<Task<PullFuncStream>> src,
            CancellationToken token = default,
            bool seekToOrigin = false,
            int initialSize = StdLookUps.DefaultBufferSize,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var ms = new MemoryStream(initialSize);
            await src.AndWriteStreamAsync(ms, bufferSize, false, token).ConfigureAwait(false);
            if (seekToOrigin) ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pushes data throw the pipeline.
        /// <para>NOTE: Calling this function will result in running the streaming pipeline, but, you won't receive
        /// anything in the end. Normally, usage of this function is to avoid use of <seealso cref="MemoryStream"/>
        /// to reduce runtime memory pressure, and at the same time counting bytes, calculate crypto-hashes etc.</para>
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndExecuteAsync(this Func<Task<PullFuncStream>> src,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            try
            {
                var buffer = new byte[bufferSize];
                while (await data.Readable.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false) != 0)
                {
                    //reading until drained!
                }
            }
            finally
            {
                data.Readable.DisposeIfRequired(data.Dispose);
            }
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="Stream"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="writableTarget">Target stream to write on</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="disposeTarget">If true, target stream is disposed else left open.</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStreamAsync(this Func<Task<PullFuncStream>> src,
            Stream writableTarget,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool disposeTarget = false,
            CancellationToken token = default)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            await data.Readable.CopyToAsync(writableTarget, bufferSize, token, data.Dispose, disposeTarget)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as string.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of the string (optimization on resizing ops)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<string> AndWriteStringAsync(this Func<Task<PullFuncStream>> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            return (await src.AndWriteStringBuilderAsync(initialSize, enc ?? new UTF8Encoding(false),
                    detectEncodingFromBom, bufferSize, token).ConfigureAwait(false)).ToString();
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and returns results as newly created <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="initialSize">Initial guess of the size of StringBuilder (optimization on array resizing)</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task<StringBuilder> AndWriteStringBuilderAsync(this Func<Task<PullFuncStream>> src,
            int initialSize = StdLookUps.DefaultStringBuilderSize,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            var sb = new StringBuilder(initialSize);
            await src.AndWriteStringBuilderAsync(sb, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize,
                    token).ConfigureAwait(false);
            return sb;
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and appends the contents to the given <seealso cref="StringBuilder"/>.
        /// </summary>
        /// <param name="src">Current pipe of the pipeline</param>
        /// <param name="sbToAppend">String builder to append data</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="token">Cancellation token to observe</param>
        public static async Task AndWriteStringBuilderAsync(this Func<Task<PullFuncStream>> src,
            StringBuilder sbToAppend,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            CancellationToken token = default)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            await data.Readable.CopyToBuilderAsync(sbToAppend, token, enc ?? new UTF8Encoding(false), bufferSize,
                data.Dispose, detectEncodingFromBom).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object based on the Json text.
        /// </summary>
        /// <typeparam name="T">Type of the object from deserialization</typeparam>
        /// <param name="src"></param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task<T> AndParseJsonAsync<T>(this Func<Task<PullFuncStream>> src,
            JsonSerializer serializer = null,
            Encoding enc = null, 
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            return data.Readable.FromJson<T>(serializer, enc ?? new UTF8Encoding(false),
                bufferSize, data.Dispose, detectEncodingFromBom);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object based on the Json text.
        /// </summary>
        /// <typeparam name="T">Type of the object from deserialization</typeparam>
        /// <param name="src"></param>
        /// <param name="target">target blocking collection to populate deserialied JSON data objects</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="observedTokenSource">When developing parallel prooducer-consumer pattern and if you wish you
        /// cancel the consumer (other producer), in case of error produced during json deserialization,
        /// pass the source of cancellation token which consumer (other producer) is observing.</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        /// <param name="closeTarget"><para>When this is the ONLY call that populates the <paramref name="target"/>
        /// keep it true so that when operation finishes the collection is automatically closed for adding so that consumer 
        /// shall not remain blocked infinitely. If setting false, then you must explicitly call 
        /// <seealso cref="BlockingCollection{T}.CompleteAdding"/> for the obvious reason.</para>
        /// <para>When this call is one among multiple producers populating the same <paramref name="target"/>,
        /// it is MANDATORY to set this to false and you must explicitly call <seealso cref="BlockingCollection{T}.CompleteAdding"/>
        /// when all producers finished populating the collection, otherwise weird things may happen.</para></param>
        /// <param name="forceCloseWhenError">if true, when any error occurs closes the collection for any additional 
        /// adding irrespective of <paramref name="closeTarget"/> setting. When false, <paramref name="closeTarget"/>
        /// setting takes precedence.</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<Task<PullFuncStream>> src,
            BlockingCollection<T> target, 
            JsonSerializer serializer = null,
            CancellationToken token = default,
            CancellationTokenSource observedTokenSource = null,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize,
            bool closeTarget = true, 
            bool forceCloseWhenError = true)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            data.Readable.FromJsonArrayParallely(target, serializer, token, observedTokenSource,
                enc ?? new UTF8Encoding(false), bufferSize, data.Dispose, closeTarget, forceCloseWhenError,
                detectEncodingFromBom);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerAction">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<Task<PullFuncStream>> src,
            Action<T, CancellationToken> consumerAction,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerAction.ToAsync(false), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerFunc">Consumer async lambda to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<Task<PullFuncStream>> src,
            Func<T, CancellationToken, Task> consumerFunc,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerFunc.ToConsumer(), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="T">Type of the object for deserialization and consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer instance to form the PPC-Pipe</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<T>(this Func<Task<PullFuncStream>> src,
            IConsumer<T> consumer,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumer, new IdentityAwaitableAdapter<T>(), serializer, ppcBufferSize,
                token, enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerAction">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<Task<PullFuncStream>> src,
            Action<TC, CancellationToken> consumerAction,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerAction.ToAsync(false), adapter, serializer, ppcBufferSize, token,
                enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumerFunc">Consumer lambda to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<Task<PullFuncStream>> src,
            Func<TC, CancellationToken, Task> consumerFunc,
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            await src.AndParseJsonArrayAsync(consumerFunc.ToConsumer(), adapter, serializer, ppcBufferSize, token,
                enc ?? new UTF8Encoding(false), detectEncodingFromBom, bufferSize).ConfigureAwait(false);
        }

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// pulls data throw the pipeline and deserializes the object and feeds to consumer after adapting those
        /// using the given adapter.
        /// </summary>
        /// <typeparam name="TJ">Type of the object for deserialization</typeparam>
        /// <typeparam name="TC">Type of the object for consumer</typeparam>
        /// <param name="src"></param>
        /// <param name="consumer">Consumer instance to form the PPC-Pipe</param>
        /// <param name="adapter">Adapter to convert deserialized objects to consumable type</param>
        /// <param name="serializer">if not provided, JsonSerializer with default values
        /// (see also <seealso cref="CustomJson.Serializer()"/>) will be used.</param>
        /// <param name="ppcBufferSize">Max. number of produced item to hold in intermediary buffer.</param>
        /// <param name="token">Cancellation token to observe</param>
        /// <param name="enc">Encoding to use while writing the file. 
        /// If not supplied, by default <seealso cref="Encoding.UTF8"/>
        /// (withOUT the utf-8 identifier, i.e. new UTF8Encoding(false)) will be used</param>
        /// <param name="detectEncodingFromBom">If true, an attempt to detect encoding from BOM (byte order mark) is made</param>
        /// <param name="bufferSize">Buffer size</param>
        public static async Task AndParseJsonArrayAsync<TJ, TC>(this Func<Task<PullFuncStream>> src,
            IConsumer<TC> consumer, 
            IDataAdapter<TJ, TC> adapter,
            JsonSerializer serializer = null,
            int ppcBufferSize = ConcurrentBuffer.StandardSize,
            CancellationToken token = default,
            Encoding enc = null,
            bool detectEncodingFromBom = true,
            int bufferSize = StdLookUps.DefaultBufferSize)
        {
            var data = await src().StartIfNeeded().ConfigureAwait(false);
            await data.ApplyPpcParseJsonArray(consumer, adapter, serializer, token, enc ?? new UTF8Encoding(false),
                detectEncodingFromBom, bufferSize, ppcBufferSize).ConfigureAwait(false);
        }

        #endregion Finalization TASK
    }
}