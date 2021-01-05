using System;

namespace UnifiedNetwork
{
    public class Constants
    {
        public const int DISCONNECT_TIME_OUT = 5000; //ms

        public const int WAIT_CONNECT_DONE = 500; //ms
        public const int WAIT_IO_DONE = 100; //ms

        public const int MAX_NUMBER_RECEIVE_ZERO_BYTE_IN_A_ROW = 5;

        public const int DEFAULT_BUFFER_SIZE = 128 * 1024; //128 KB
        public const int DEFAULT_BUFFER_POOL_SIZE = 10;

        public const string IPv4ValidatorString = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";

        public const string DEFAULT_WEB_SOCKET_URL_PREFIX = "ws";

        public const int FLOAT_TO_INT_MULTIPLIER = 100;

        public const int DEFAULT_HEADER_SIZE = 4; //bytes
        public const int OP_CODE_SIZE = 2; //byte

        public const int PING_RECORD_HISTORY_LENGTH = 50;
        public const int DEFAULT_NUMBER_PING_SAMPLES = 10;
    }
}