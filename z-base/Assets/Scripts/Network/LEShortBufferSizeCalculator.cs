using System;

namespace UnifiedNetwork
{
    public class LEShortBufferSizeCalculator : AbstractBufferSizeCalculator
    {
        public override void WriteSizeInto(int size, ByteBuf buffer)
        {
            byte[] bytes = BitConverter.GetBytes(size);
            buffer.Data[0] = bytes[0];
            buffer.Data[1] = bytes[1];
        }

        public override int ReadSize(ByteBuf buffer)
        {
            return BitConverter.ToUInt16(buffer.Data, 0);
        }
    }
}