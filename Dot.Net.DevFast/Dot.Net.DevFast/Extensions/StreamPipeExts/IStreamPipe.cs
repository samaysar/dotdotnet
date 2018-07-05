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

    /// <inheritdoc />
    /// <summary>
    /// StreamPipe associated with a file stream.
    /// </summary>
    public interface IFilePipe : IStreamPipe
    {
        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// writes on the file, immediately.
        /// </summary>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        Task RunAsync(string folder, string filename = null, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous);

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// writes on the file, immediately.
        /// </summary>
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead</param>
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
}