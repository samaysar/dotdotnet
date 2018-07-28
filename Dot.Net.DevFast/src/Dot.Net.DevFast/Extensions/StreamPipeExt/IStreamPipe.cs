using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Etc;

namespace Dot.Net.DevFast.Extensions.StreamPipeExt
{
    /// <summary>
    /// Base interface for all derived streaming implementations
    /// </summary>
    public interface IStreamPipe
    {
        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that
        /// starts writing on the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">underlying stream</param>
        /// <param name="dispose">If true, stream is dispose once operation completes.</param>
        /// <param name="token">Cancellation token to observe</param>
        Task StreamAsync(Stream stream, bool dispose = true, CancellationToken token = default(CancellationToken));
    }

    /// <summary>
    /// StreamPipe associated with a file stream.
    /// </summary>
    public interface IFilePipe
    {
        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// <para>An appropriate extension will be added to <paramref name="filename" /></para>
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary>
        /// <param name="folder">folder path where file is saved</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        Task<FileInfo> SaveAsFileAsync(string folder, string filename = null, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// <para>An appropriate extension will be added to <paramref name="filename" /></para>
        /// <para>Returns the <seealso cref="FileInfo"/> object of that the written file</para>
        /// </summary> 
        /// <param name="folder">directory information of folder where file will be created</param>
        /// <param name="filename">name of file. If not supplied a new GUID string will be used instead
        /// <para>filename should NOT contain extension</para> </param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        Task<FileInfo> SaveAsFileAsync(DirectoryInfo folder, string filename = null, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Call to this method shall bootstrap the streaming pipeline and returns the associated asynchronous task that 
        /// starts saving stream data to file.
        /// </summary>
        /// <param name="fileinfo">file info object of the file to create/rewrite.</param>
        /// <param name="fileStreamBuffer">Buffer size of the file stream</param>
        /// <param name="options">File options</param>
        /// <param name="token">Cancellation token to observe</param>
        Task SaveAsFileAsync(FileInfo fileinfo, int fileStreamBuffer = StdLookUps.DefaultFileBufferSize,
            FileOptions options = FileOptions.Asynchronous, CancellationToken token = default(CancellationToken));
    }

    /// <inheritdoc cref="IFilePipe" />
    /// <summary>
    /// Stream pipe associated with JSON data stream
    /// </summary>
    public interface IJsonPipe : IFilePipe, IStreamPipe
    {
    }

    /// <inheritdoc cref="IFilePipe" />
    /// <summary>
    /// Exposes compressed streams.
    /// </summary>
    public interface ICompressedPipe : IFilePipe, IStreamPipe
    {

    }
}