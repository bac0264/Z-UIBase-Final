using System;

namespace UnifiedNetwork
{
    public abstract class AbstractBufferHelper
    {
        public abstract void WriteDataInto(byte[] output, int offset, int count, ulong data);

        public abstract ulong ReadDataFrom(byte[] output, int offset, int count);
    }
}