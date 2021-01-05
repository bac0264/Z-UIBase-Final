using System;

namespace UnifiedNetwork
{
    // little endian
    public class LEBufferHelper : AbstractBufferHelper
    {
        public override void WriteDataInto(byte[] output, int offset, int count, ulong data)
        {
            for (int i = 0; i < count; i++)
            {
                output[offset + i] = (byte)(data >> (i << 3));
            }
        }

        public override ulong ReadDataFrom(byte[] input, int offset, int count)
        {
            ulong result = 0;
            for (int i = 0; i < count; i++)
            {
                result |= (ulong)input[offset + i] << (i << 3);
            }

            return result;
        }
    }
}