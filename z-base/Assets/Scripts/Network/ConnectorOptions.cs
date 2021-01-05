using System.Text;

public enum ConnectionState
{
    CONNECTED = 0,
    CONNECTING = 1,
    DISCONNECTED = 2,
    DISCONNECTING = 3,
}

public enum ConnectionProtocol
{
    TCP_SOCKET = 0,
    WEB_SOCKET = 1,
    UDP_SOCKET = 2,
}


public enum HostType
{
    IPv4,
    IPv6,
    DOMAIN
}

public enum LoggerMode
{
    ALL = 4,
    INFO = 3,
    WARNING = 2,
    ERROR = 1,
    NOTHING = 0,
}
namespace UnifiedNetwork
{
    public class ConnectorOptions
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public ConnectionProtocol ConnectionProtocol { get; private set; }

        public HostType HostType { get; private set; }
        public string Address { get; private set; }

        /// <summary>
        /// Tcp Socket only
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// Indicate the default pool size for buffer
        /// </summary>
        public int BufferPoolSize { get; set; }

        /// <summary>
        /// Tcp Socket only
        /// </summary>
        public int HeaderSize { get; set; }

        /// <summary>
        /// Tcp Socket only
        /// </summary>
        public bool IsPreferIPv4 { get; set; }

        /// <summary>
        /// Web Socket only, not include '/'
        /// </summary>
        public string WebSocketUrlPrefix { get; set; }

        /// <summary>
        /// Web Socket only, not include '/'
        /// </summary>
        public string WebSocketUrlPostfix { get; set; }

        /// <summary>
        /// Max message size for concatenate data from multiple receive
        /// </summary>
        public int MaxMessageSize { get; set; }

        /// <summary>
        /// Indicate number samples of ping will be used to calculate average latency
        /// </summary>
        public int NumberPingSamples { get; set; }

        public ConnectorOptions(string host, int port,
            ConnectionProtocol protocol = ConnectionProtocol.TCP_SOCKET)
        {
            Host = host;
            Port = port;

            ConnectionProtocol = protocol;

            HostType = ConnectionUtils.GetHostType(Host);
            Address = Host + ":" + Port;

            BufferSize = Constants.DEFAULT_BUFFER_SIZE;
            BufferPoolSize = Constants.DEFAULT_BUFFER_POOL_SIZE;

            HeaderSize = Constants.DEFAULT_HEADER_SIZE;

            IsPreferIPv4 = true;

            WebSocketUrlPrefix = Constants.DEFAULT_WEB_SOCKET_URL_PREFIX;
            WebSocketUrlPostfix = string.Empty;

            MaxMessageSize = Constants.DEFAULT_BUFFER_SIZE;
            NumberPingSamples = Constants.DEFAULT_NUMBER_PING_SAMPLES;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("\n");

            builder.AppendFormat("Host: {0}\n", Host);
            builder.AppendFormat("Port: {0}\n", Port);
            builder.AppendFormat("HostType: {0}\n", HostType);

            builder.AppendFormat("ConnectionProtocol: {0}\n", ConnectionProtocol);

            builder.AppendFormat("BufferSize: {0}\n", BufferSize);
            builder.AppendFormat("BufferPoolSize: {0}\n", BufferPoolSize);

            builder.AppendFormat("HeaderSize: {0}\n", HeaderSize);

            builder.AppendFormat("IsPreferIPv4: {0}\n", IsPreferIPv4);

            builder.AppendFormat("WebSocketUrlPrefix: {0}\n", WebSocketUrlPrefix);
            builder.AppendFormat("WebSocketUrlPostfix: {0}\n", WebSocketUrlPostfix);

            builder.AppendFormat("MaxMessageSize: {0}\n", MaxMessageSize);
            builder.AppendFormat("NumberPingSamples: {0}\n", NumberPingSamples);

            return builder.ToString();
        }
    }
}