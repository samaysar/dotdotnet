namespace Dot.Net.DevFast.Etc
{
    /// <summary>
    /// Provides fixed values.
    /// </summary>
    public static class StdLookUps
    {
        /// <summary>
        /// Character used to join fileName and extension.
        /// </summary>
        public const char ExtSeparator = '.';

        /// <summary>
        /// Default Buffer size for stream related operations.
        /// </summary>
        public const int DefaultBufferSize = 1024;

        /// <summary>
        /// Default Buffer size for file stream related operations.
        /// </summary>
        public const int DefaultFileBufferSize = 4*1024;

        /// <summary>
        /// Extension for json file.
        /// </summary>
        public const string JsonFileExt = "json";

        /// <summary>
        /// Extension for zip file.
        /// </summary>
        public const string ZipFileExt = "zip";
    }
}