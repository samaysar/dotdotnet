namespace Dot.Net.DevFast.Extensions.Ppc
{
    /// <summary>
    /// Data provider interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataFeed<T>
    {
        /// <summary>
        /// Provides the data instance and returns true. False is returned
        /// when no more data is available and never be available in future.
        /// </summary>
        /// <param name="data">data instance, if any</param>
        bool TryGet(out T data);
    }
}