using System;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extensions related to creation of one type of objects to another type.
    /// </summary>
    public static class ConversionExt
    {
        /// <summary>
        /// Creates the byte array of the segment.
        /// </summary>
        /// <param name="input">Input segment</param>
        public static byte[] CreateBytes(this ArraySegment<byte> input)
        {
            var size = input.Count - input.Offset;
            var retValue = new byte[size];
            Buffer.BlockCopy(input.Array, 0, retValue, input.Offset, input.Count);
            return retValue;
        }
    }
}