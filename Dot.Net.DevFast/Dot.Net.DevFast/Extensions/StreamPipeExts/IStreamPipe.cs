using System.IO;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StreamPipeExts
{
    /// <summary>
    /// Base interface for all derived streaming implementations
    /// </summary>
    public interface IStreamPipe
    {
        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that
        /// writes on the <paramref name="stream"/>, immediately.
        /// </summary>
        /// <param name="stream">underlying stream</param>
        /// <param name="dispose">If true, stream is dispose once operation completes.</param>
        Task RunAsync(Stream stream, bool dispose = true);
    }

    /// <summary>
    /// StreamPipe associated with a file stream.
    /// </summary>
    public interface IFilePipe
    {
        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// writes on the file, immediately.
        /// <para><paramref name="filename" /> should NOT contain extension</para>
        /// </summary>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        Task RunAsync(string folder, string filename = null, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous);

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// writes on the file, immediately.
        /// <para><paramref name="filename" /> should NOT contain extension</para>
        /// </summary> 
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        Task RunAsync(DirectoryInfo folder, string filename = null, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous);

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// writes on the file, immediately.
        /// </summary>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        Task RunAsync(FileInfo fileinfo, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous);
    }

    /// <inheritdoc cref="IFilePipe" />
    /// <summary>
    /// Stream pipe associated with JSON data stream
    /// </summary>
    public interface IJsonPipe : IFilePipe, IStreamPipe
    {
    }
}