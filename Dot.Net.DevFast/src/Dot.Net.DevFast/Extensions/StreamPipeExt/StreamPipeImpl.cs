using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
        public JsonBcPipe(BlockingCollection<T> obj, JsonSerializer js, CancellationToken token,
            CancellationTokenSource pcts, Encoding enc, int wbuff) :
            base((s, d) => obj.ToJsonArrayParallely(s, js, token, pcts, enc, wbuff, d))
        {
        }
    }

    internal sealed class JsonEnumeratorPipe<T> : JsonPipe
    {
        public JsonEnumeratorPipe(IEnumerable<T> obj, JsonSerializer js, CancellationToken token,
            Encoding enc, int wbuff) : base((s, d) => obj.ToJsonArray(s, js, token, enc, wbuff, d))
        {
        }
    }

    internal sealed class JsonObjectPipe<T> : JsonPipe
    {
        public JsonObjectPipe(T obj, JsonSerializer js, Encoding enc, int wbuff) :
            base((s, d) => obj.ToJson(s, js, enc, wbuff, d))
        {
        }
    }

    internal abstract class JsonPipe : FilePipe, IJsonPipe
    {
        protected JsonPipe(Action<Stream, bool> writerAction) : base(StdLookUps.JsonFileExt, writerAction)
        {
        }
    }

    internal abstract class FilePipe : StreamPipe, IFilePipe
    {
        private readonly string _fileExt;

        protected FilePipe(string fileExt, Action<Stream, bool> writerAction) : base(writerAction)
        {
            _fileExt = fileExt;
        }

        public Task RunAsync(string folder, string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous)
        {
            return RunAsync(folder.ToDirectoryInfo(), filename, fileStreamBuffer, options);
        }

        public Task RunAsync(DirectoryInfo folder, string filename = null,
            int fileStreamBuffer = StdLookUps.DefaultFileBufferSize, FileOptions options = FileOptions.Asynchronous)
        {
            return RunAsync(folder.CreateFileInfo(filename ?? Guid.NewGuid().ToString("N"), _fileExt),
                fileStreamBuffer, options);
        }

        public Task RunAsync(FileInfo fileinfo, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous)
        {
            return RunAsync(fileinfo.CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.Read,
                fileStreamBuffer, options));
        }
    }

    internal abstract class StreamPipe : IStreamPipe
    {
        private readonly Func<Stream, bool, Task> _writerFunc;

        protected StreamPipe(Action<Stream, bool> writerAction) : this(writerAction.ToAsync())
        {
        }

        protected StreamPipe(Func<Stream, bool, Task> writerFunc)
        {
            _writerFunc = writerFunc;
        }

        public Task RunAsync(Stream stream, bool dispose = true)
        {
            return _writerFunc(stream, dispose);
        }
    }
}