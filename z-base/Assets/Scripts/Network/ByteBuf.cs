using System;
using System.Collections.Generic;
using System.Text;

namespace UnifiedNetwork
{
    /// <summary>
    /// Mimic Java's ByteBuffer.
    /// Heavily modified ByteBuffer (originally from Google)
    /// </summary>
    public class ByteBuf
    {
        private readonly byte[] buffer;
        public byte[] Data
        {
            get
            {
                return buffer;
            }
        }

        public int Size
        {
            get
            {
                return buffer.Length;
            }
        }

        private byte[] stringBuffer;

        public int writePos { get; private set; }
        public int readPos { get; private set; }

        private AbstractBufferHelper bufferHelper;

        public ByteBuf() :
            this(Constants.DEFAULT_BUFFER_SIZE)
        {
        }

        public ByteBuf(int size)
        {
            buffer = new byte[size];
            stringBuffer = new byte[size];

            bufferHelper = UnifiedNetworkFactory.CreateBufferHelper();

            Reset();
        }

        #region Helper methods
        /// <summary>
        /// Must use it before new read/write operations
        /// </summary>
        public void Reset()
        {
            writePos = 0;
            readPos = 0;
        }

        /// <summary>
        /// Offset writePos by numberBytes
        /// </summary>
        /// <param name="numberBytes"></param>
        public void OffsetWrite(int offsetBytes)
        {
            writePos += offsetBytes;
        }

        /// <summary>
        /// Offset readPos by numberBytes
        /// </summary>
        /// <param name="offsetBytes"></param>
        public void OffsetRead(int offsetBytes)
        {
            readPos += offsetBytes;
        }

        /// <summary>
        /// Buffer bound check
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private bool IsValidOffsetAndLength(int offset, int length)
        {
            if (offset < 0 || offset > buffer.Length - length)
            {
                UnifiedLogger.Error(Tag.BYTE_BUFFER,
                    string.Format("Invalid offset or length, Offset: {0}, Data Length: {1}, Buffer Length: {2}",
                    offset, length, buffer.Length));
                return false;
            }

            return true;
        }

        private void CheckReadPos()
        {
            if (readPos > writePos)
            {
                UnifiedLogger.Error(Tag.BYTE_BUFFER,
                    string.Format("Invalid readPos: {0} (writePos: {1})", readPos, writePos));
            }
        }
        #endregion

        #region Put methods
        public void PutSbyte(sbyte value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(sbyte)))
            {
                buffer[writePos] = (byte)value;
                writePos += sizeof(sbyte);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put sbyte: {0}", value));
            }
        }

        public void PutByte(byte value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(byte)))
            {
                buffer[writePos] = value;
                writePos += sizeof(byte);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put byte: {0}", value));
            }
        }

        public void PutShort(short value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(short)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(short), (ulong)value);
                writePos += sizeof(short);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put short: {0}", value));
            }
        }

        public void PutUshort(ushort value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(ushort)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(ushort), (ulong)value);
                writePos += sizeof(ushort);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put ushort: {0}", value));
            }
        }

        public void PutInt(int value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(int)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(int), (ulong)value);
                writePos += sizeof(int);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put int: {0}", value));
            }
        }

        public void PutUint(uint value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(uint)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(uint), (ulong)value);
                writePos += sizeof(uint);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put uint: {0}", value));
            }
        }

        public void PutLong(long value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(long)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(long), (ulong)value);
                writePos += sizeof(long);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put long: {0}", value));
            }
        }

        public void PutUlong(ulong value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(ulong)))
            {
                bufferHelper.WriteDataInto(buffer, writePos, sizeof(ulong), value);
                writePos += sizeof(ulong);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put ulong: {0}", value));
            }
        }

        public void PutFloat(float value)
        {
            if (IsValidOffsetAndLength(writePos, sizeof(float)))
            {
                int approxValue = (int)Math.Round(value * Constants.FLOAT_TO_INT_MULTIPLIER);
                PutInt(approxValue);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Put float: {0}", value));
            }
        }

        public void PutString(string value, bool isShortString = true)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                int size = Encoding.UTF8.GetBytes(value, 0, value.Length, stringBuffer, 0);
                if (isShortString)
                {
                    PutByte((byte)size);
                }
                else
                {
                    PutShort((short)size);
                }

                for (int i = 0; i < size; i++)
                {
                    PutByte(stringBuffer[i]);
                }
            }
            else
            {
                if (isShortString)
                {
                    PutByte(0);
                }
                else
                {
                    PutShort(0);
                }
            }
        }

        public void PutBool(bool value)
        {
            if (value)
            {
                PutByte(1);
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region Get methods
        public sbyte GetSbyte()
        {
            sbyte result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(sbyte)))
            {
                result = (sbyte)buffer[readPos];
                readPos += sizeof(sbyte);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get sbyte: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public byte GetByte()
        {
            byte result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(byte)))
            {
                result = (byte)buffer[readPos];
                readPos += sizeof(byte);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get byte: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public short GetShort()
        {
            short result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(short)))
            {
                result = (short)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(short));
                readPos += sizeof(short);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get short: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public ushort GetUshort()
        {
            ushort result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(ushort)))
            {
                result = (ushort)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(ushort));
                readPos += sizeof(ushort);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get ushort: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public int GetInt()
        {
            int result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(int)))
            {
                result = (int)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(int));
                readPos += sizeof(int);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get int: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public uint GetUint()
        {
            uint result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(uint)))
            {
                result = (uint)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(uint));
                readPos += sizeof(uint);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get uint: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public long GetLong()
        {
            long result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(long)))
            {
                result = (long)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(long));
                readPos += sizeof(long);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get long: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public ulong GetUlong()
        {
            ulong result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(ulong)))
            {
                result = (ulong)bufferHelper.ReadDataFrom(buffer, readPos, sizeof(ulong));
                readPos += sizeof(ulong);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get ulong: {0}", result));
                CheckReadPos();
            }

            return result;
        }

        public float GetFloat()
        {
            float result = 0;
            if (IsValidOffsetAndLength(readPos, sizeof(float)))
            {
                result = GetInt() / (float)Constants.FLOAT_TO_INT_MULTIPLIER;

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Get float: {0}", result));
            }

            return result;
        }

        public string GetString(bool isShortString = true)
        {
            int stringLength = 0;
            if (isShortString)
            {
                stringLength = GetByte();
            }
            else
            {
                stringLength = GetShort();
            }

            if (stringLength > 0 && stringLength < stringBuffer.Length)
            {
                for (int i = 0; i < stringLength; i++)
                {
                    stringBuffer[i] = GetByte();
                }

                return Encoding.UTF8.GetString(stringBuffer, 0, stringLength);
            }
            else
            {
                return string.Empty;
            }
        }

        public bool GetBool()
        {
            byte result = GetByte();

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Set methods
        public void SetSbyte(sbyte value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(sbyte)))
            {
                buffer[customWriteIndex] = (byte)value;

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set sbyte: {0}", value));
            }
        }

        public void SetByte(byte value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(byte)))
            {
                buffer[customWriteIndex] = value;

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set byte: {0}", value));
            }
        }

        public void SetShort(short value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(short)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(short), (ulong)value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set short: {0}", value));
            }
        }

        public void SetUshort(ushort value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(ushort)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(ushort), (ulong)value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set ushort: {0}", value));
            }
        }

        public void SetInt(int value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(int)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(int), (ulong)value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set int: {0}", value));
            }
        }

        public void SetUint(uint value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(uint)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(uint), (ulong)value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set uint: {0}", value));
            }
        }

        public void SetLong(long value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(long)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(long), (ulong)value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set long: {0}", value));
            }
        }

        public void SetUlong(ulong value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(ulong)))
            {
                bufferHelper.WriteDataInto(buffer, customWriteIndex, sizeof(ulong), value);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set ulong: {0}", value));
            }
        }

        public void SetFloat(float value, int customWriteIndex)
        {
            if (IsValidOffsetAndLength(customWriteIndex, sizeof(float)))
            {
                int approxValue = (int)Math.Round(value * Constants.FLOAT_TO_INT_MULTIPLIER);
                SetInt(approxValue, customWriteIndex);

                //UnifiedLogger.LogInfo(Tag.BYTE_BUFFER, string.Format("Set float: {0}", value));
            }
        }

        public void SetBool(bool value, int customWriteIndex)
        {
            if (value)
            {
                SetByte(1, customWriteIndex);
            }
            else
            {
                SetByte(0, customWriteIndex);
            }
        }
        #endregion

        #region Extensions methods

        #region Byte collections
        public int[] GetByteArray()
        {
            int length = GetByte();

            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetByte();
            }

            return result;
        }

        public void PutByteArray(int[] data)
        {
            if (data != null)
            {
                PutByte((byte)data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    PutByte((byte)data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }

        public List<int> GetByteList()
        {
            int length = GetByte();

            List<int> result = new List<int>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(GetByte());
            }

            return result;
        }

        public void PutByteList(List<int> data)
        {
            if (data != null)
            {
                PutByte((byte)data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    PutByte((byte)data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region Short collections
        public int[] GetShortArray()
        {
            int length = GetByte();

            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetShort();
            }

            return result;
        }

        public void PutShortArray(int[] data)
        {
            if (data != null)
            {
                PutByte((byte)data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    PutShort((short)data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }

        public List<int> GetShortList()
        {
            int length = GetByte();

            List<int> result = new List<int>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(GetShort());
            }

            return result;
        }

        public void PutShortList(List<int> data)
        {
            if (data != null)
            {
                PutByte((byte)data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    PutShort((short)data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region Long collections
        public long[] GetLongArray()
        {
            byte length = GetByte();

            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetLong();
            }

            return result;
        }

        public void PutLongArray(long[] data)
        {
            if (data != null)
            {
                PutByte((byte)data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    PutLong(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }

        public List<long> GetLongList()
        {
            byte length = GetByte();

            List<long> result = new List<long>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(GetLong());
            }

            return result;
        }

        public void PutLongList(List<long> data)
        {
            if (data != null)
            {
                PutByte((byte)data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    PutLong(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region Float collections
        public float[] GetFloatArray()
        {
            byte length = GetByte();

            float[] result = new float[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetFloat();
            }

            return result;
        }

        public void PutFloatArray(float[] data)
        {
            if (data != null)
            {
                PutByte((byte)data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    PutFloat(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }

        public List<float> GetFloatList()
        {
            byte length = GetByte();

            List<float> result = new List<float>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(GetFloat());
            }

            return result;
        }

        public void PutFloatList(List<float> data)
        {
            if (data != null)
            {
                PutByte((byte)data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    PutFloat(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region String collections
        public string[] GetStringArray()
        {
            byte length = GetByte();

            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GetString();
            }

            return result;
        }

        public void PutStringArray(string[] data)
        {
            if (data != null)
            {
                PutByte((byte)data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    PutString(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }

        public List<string> GetStringList()
        {
            byte length = GetByte();

            List<string> result = new List<string>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(GetString());
            }

            return result;
        }

        public void PutStringList(List<string> data)
        {
            if (data != null)
            {
                PutByte((byte)data.Count);
                for (int i = 0; i < data.Count; i++)
                {
                    PutString(data[i]);
                }
            }
            else
            {
                PutByte(0);
            }
        }
        #endregion

        #region Put by type
        public bool PutObject(Object obj)
        {
            if (obj != null)
            {
                switch (Type.GetTypeCode(obj.GetType()))
                {
                    case TypeCode.Boolean:
                        PutBool((bool)obj);
                        return true;

                    case TypeCode.SByte:
                        PutSbyte((sbyte)obj);
                        return true;

                    case TypeCode.Byte:
                        PutByte((byte)obj);
                        return true;

                    case TypeCode.Int16:
                        PutShort((short)obj);
                        return true;

                    case TypeCode.UInt16:
                        PutUshort((ushort)obj);
                        return true;

                    case TypeCode.Int32:
                        PutInt((int)obj);
                        return true;

                    case TypeCode.UInt32:
                        PutUint((uint)obj);
                        return true;

                    case TypeCode.Int64:
                        PutLong((long)obj);
                        return true;

                    case TypeCode.UInt64:
                        PutUlong((ulong)obj);
                        return true;

                    case TypeCode.Single:
                        PutFloat((float)obj);
                        return true;

                    case TypeCode.String:
                        PutString((string)obj);
                        return true;

                    case TypeCode.Object:
                        Outbound outbound = obj as Outbound;
                        if (outbound != null)
                        {
                            try
                            {
                                outbound.Serialize(this);
                                return true;
                            }
                            catch (Exception e)
                            {
                                UnifiedLogger.Error(Tag.BYTE_BUFFER, string.Format("Error serialize data: {0}", e));
                            }
                        }
                        else
                        {
                            UnifiedLogger.Error(Tag.BYTE_BUFFER,
                                string.Format("Type: {0} is not supported in this method. You have to write your own Outbound class for this!", obj.GetType()));
                        }
                        break;

                    default:
                        UnifiedLogger.Error(Tag.BYTE_BUFFER,
                            string.Format("Type: {0} is not supported in this method. You have to write your own Outbound class for this!", obj.GetType()));
                        break;
                }
            }
            else
            {
                UnifiedLogger.Error(Tag.BYTE_BUFFER, "Input object is null");
            }

            return false;
        }
        #endregion

        #endregion
    }
}