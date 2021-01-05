using System;

namespace UnifiedNetwork
{
    public abstract class AbstractBufferSizeCalculator
    {
        public abstract void WriteSizeInto(int size, ByteBuf buffer);

        public abstract int ReadSize(ByteBuf buffer);
    }
}