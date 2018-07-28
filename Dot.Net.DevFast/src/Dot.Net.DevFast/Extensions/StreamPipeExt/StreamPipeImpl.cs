using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions.JsonExt;
using Dot.Net.DevFast.Extensions.StringExt;
using Newtonsoft.Json;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    internal sealed class JsonBcPipe<T> : JsonPipe
    {
        public JsonBcPipe(BlockingCollection<T> obj, JsonSerializer js, CancellationTokenSource pcts,
            Encoding enc, int wbuff) :
            base((s, d, t) => obj.ToJsonArrayParallely(s, js, t, pcts, enc, wbuff, d))
        {
        }
    }

    internal sealed class JsonEnumeratorPipe<T> : JsonPipe
    {
        public JsonEnumeratorPipe(IEnumerable<T> obj, JsonSerializer js,
            Encoding enc, int wbuff) : base((s, d, t) => obj.ToJsonArray(s, js, t, enc, wbuff, d))
        {
        }
    }

    internal sealed class JsonObjectPipe<T> : JsonPipe
    {
        public JsonObjectPipe(T obj, JsonSerializer js, Encoding enc, int wbuff) :
            base((s, d, t) => obj.ToJson(s, js, enc, wbuff, d))
        {
        }
    }

    internal sealed class CompressedPipe : FilePipe, ICompressedPipe
    {
        public CompressedPipe(IStreamPipe src, bool gzip, CompressionLevel level) :
            base(StdLookUps.ZipFileExt, src.AddCompression(gzip, level))
        {
        }
    }

    internal abstract class JsonPipe : FilePipe, IJsonPipe
    {
        protected JsonPipe(Action<Stream, bool, CancellationToken> writerAction) : base(StdLookUps.JsonFileExt,
            writerAction)
        {
        }
    }

    internal abstract class FilePipe : StreamPipe, IFilePipe
    {
        private readonly string _fileExt;

        protected FilePipe(string fileExt, Action<Stream, bool, CancellationToken> writerAction) : base(writerAction)
        {
            _fileExt = fileExt;
        }

        protected FilePipe(string fileExt, Func<Stream, bool, CancellationToken, Task> writerFunc) : base(writerFunc)
        {
            _fileExt = fileExt;
        }

        public Task<FileInfo> SaveAsFileAsync(string folder, string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, CancellationToken token = default(CancellationToken))
        {
            return SaveAsFileAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options, token);
        }

        public async Task<FileInfo> SaveAsFileAsync(DirectoryInfo folder, string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous,
            CancellationToken token = default(CancellationToken))
        {
            var targetFile = folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"), _fileExt);
            await SaveAsFileAsync(targetFile, fileStreamBuffer, options, token).ConfigureAwait(false);
            return targetFile;
        }

        public async Task SaveAsFileAsync(FileInfo fileinfo, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, CancellationToken token = default(CancellationToken))
        {
            using (var strm = fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options))
            {
                await StreamAsync(strm, false, token).ConfigureAwait(false);
                await strm.FlushAsync(token).ConfigureAwait(false);
            }
        }
    }

    internal abstract class StreamPipe : IStreamPipe
    {
        private readonly Func<Stream, bool, CancellationToken, Task> _writerFunc;

        protected StreamPipe(Action<Stream, bool, CancellationToken> writerAction) : this(writerAction.ToAsync())
        {
        }

        protected StreamPipe(Func<Stream, bool, CancellationToken, Task> writerFunc)
        {
            _writerFunc = writerFunc;
        }

        public Task StreamAsync(Stream stream, bool dispose = true,
            CancellationToken token = default(CancellationToken))
        {
            return _writerFunc(stream, dispose, token);
        }
    }
}