using System;

namespace UnifiedNetwork
{
    public class BEShortBufferSizeCalculator : AbstractBufferSizeCalculator
    {
        public override void WriteSizeInto(int size, ByteBuf buffer)
        {
            byte[] bytes = BitConverter.GetBytes(size);
            buffer.Data[0] = bytes[1];
            buffer.Data[1] = bytes[0];
        }

        public override int ReadSize(ByteBuf buffer)
        {
            byte[] reverse = new byte[] { buffer.Data[1], buffer.Data[0] };
            return BitConverter.ToUInt16(reverse, 0);
        }
    }
}