using System;

namespace UnifiedNetwork
{
    // big endian
    public class BELongBufferSizeCalculator : AbstractBufferSizeCalculator
    {
        public override void WriteSizeInto(int size, ByteBuf buffer)
        {
            byte[] bytes = BitConverter.GetBytes(size);
            buffer.Data[0] = bytes[3];
            buffer.Data[1] = bytes[2];
            buffer.Data[2] = bytes[1];
            buffer.Data[3] = bytes[0];
        }

        public override int ReadSize(ByteBuf buffer)
        {
            byte[] reverse = new byte[] { buffer.Data[3], buffer.Data[2], buffer.Data[1], buffer.Data[0] };
            return BitConverter.ToInt32(reverse, 0);
        }
    }
}