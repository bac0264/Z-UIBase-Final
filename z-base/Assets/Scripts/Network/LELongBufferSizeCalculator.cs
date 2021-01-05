using System;

namespace UnifiedNetwork
{
    public class LELongBufferSizeCalculator : AbstractBufferSizeCalculator
    {
        public override void WriteSizeInto(int size, ByteBuf buffer)
        {
            byte[] bytes = BitConverter.GetBytes(size);
            buffer.Data[0] = bytes[0];
            buffer.Data[1] = bytes[1];
            buffer.Data[2] = bytes[2];
            buffer.Data[3] = bytes[3];
        }

        public override int ReadSize(ByteBuf buffer)
        {
            return BitConverter.ToInt32(buffer.Data, 0);
        }
    }
}