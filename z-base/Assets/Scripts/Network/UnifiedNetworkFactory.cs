using System;

namespace UnifiedNetwork
{
    public class UnifiedNetworkFactory
    {
        public static AbstractConnection CreateConnection(ConnectorOptions options)
        {
            AbstractConnection connection = null;

            switch (options.ConnectionProtocol)
            {
                case ConnectionProtocol.TCP_SOCKET:
#if !UNITY_WEBGL
                    connection = new TCPSocketConnection(options);
#endif
                    break;

                case ConnectionProtocol.WEB_SOCKET:
                    connection = new WebSocketConnection(options);
                    break;

                case ConnectionProtocol.UDP_SOCKET:
#if !UNITY_WEBGL
                    connection = new UDPSocketConnection(options);
#endif
                    break;
            }

            return connection;
        }

        public static AbstractBufferSizeCalculator CreateBufferSizeCalculator(int headerSize)
        {
            if (BitConverter.IsLittleEndian)
            {
                switch (headerSize)
                {
                    case 2:
                        return new LEShortBufferSizeCalculator();

                    case 4:
                        return new LELongBufferSizeCalculator();

                    default:
                        throw new Exception("Header size is not supported, headerSize = " + headerSize);
                }
            }
            else
            {
                switch (headerSize)
                {
                    case 2:
                        return new BEShortBufferSizeCalculator();

                    case 4:
                        return new BELongBufferSizeCalculator();

                    default:
                        throw new Exception("Header size is not supported, headerSize = " + headerSize);
                }
            }
        }

        public static AbstractBufferHelper CreateBufferHelper()
        {
            if (BitConverter.IsLittleEndian)
            {
                return new LEBufferHelper();
            }
            else
            {
                return new BEBufferHelper();
            }
        }

        public static byte[] CreatePingData(ConnectorOptions options)
        {
            switch (options.ConnectionProtocol)
            {
                case ConnectionProtocol.TCP_SOCKET:
                    switch (options.HeaderSize)
                    {
                        case 2:
                            // byte at first 2 index is prefix size
                            // byte at index 2 and 3 is opCode
                            // byte at index 4 is pingId
                            if (BitConverter.IsLittleEndian)
                            {
                                return new byte[] { 3, 0, 0, 0, 0 };
                            }
                            else
                            {
                                return new byte[] { 0, 3, 0, 0, 0 };
                            }

                        case 4:
                            // byte at first 4 index is prefix size
                            // byte at index 5 and 6 is opCode
                            // byte at index 7 is pingId
                            if (BitConverter.IsLittleEndian)
                            {
                                return new byte[] { 3, 0, 0, 0, 0, 0, 0 };
                            }
                            else
                            {
                                return new byte[] { 0, 0, 0, 3, 0, 0, 0 };
                            }

                        default:
                            throw new Exception("Header size is not supported, headerSize = " + options.HeaderSize);
                    }

                case ConnectionProtocol.UDP_SOCKET:
                    return new byte[] { 0, 0, 0 };

                case ConnectionProtocol.WEB_SOCKET:
                    return new byte[] { 0, 0, 0 };

                default:
                    return new byte[] { 0, 0, 0 };
            }
        }
    }
}