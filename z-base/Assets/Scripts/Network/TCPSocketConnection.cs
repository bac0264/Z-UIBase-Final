#if !UNITY_WEBGL
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UnifiedNetwork
{
    /// <summary>
    /// Stream-based TCP Socket implementation
    /// </summary>
    public class TCPSocketConnection : AbstractConnection
    {
        private Socket tcpSocket;

        private IPEndPoint ipEndPoint;
        private IPAddress[] addressList;
        private int currentAddressIndex;

        private byte[] receiveBuffer;
        private Thread receiverThread;
        private int numberReceiveZeroByteInARow;

        private SocketAsyncEventArgs connectArgs;
        private AsyncCallback sendCallback;

        private byte[] pingData;

        public TCPSocketConnection(ConnectorOptions options)
            : base(options)
        {
            receiveBuffer = new byte[options.BufferSize];
            currentAddressIndex = -1;

            sendCallback = new AsyncCallback(OnSend);
            receiverThread = new Thread(Receive);

            pingData = UnifiedNetworkFactory.CreatePingData(options);
        }

        #region Init
        public override bool Initialize()
        {
            try
            {
                if (options.HostType == HostType.DOMAIN)
                {
                    addressList = ConnectionUtils.GetAddressList(options.Host);
                }

                // Instantiates the endpoint
                ipEndPoint = ConnectionUtils.GetIPEndPoint(options.Host, options.Port, options.IsPreferIPv4);
                UnifiedLogger.Info(Tag.TCP_SOCKET, string.Format("Address Family of {0} is {1}, IP Address is {2}",
                    GetAddress(), ipEndPoint.AddressFamily, ipEndPoint.Address));

                PrepareTcpSocket();
                return true;
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error initialize socket to {0} : {1}", GetAddress(), e));
                return false;
            }
        }

        void PrepareTcpSocket()
        {
            switch (ipEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    break;

                case AddressFamily.InterNetworkV6:
                    tcpSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                    break;

                default:
                    tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    UnifiedLogger.Warn(Tag.TCP_SOCKET, string.Format("Address Family of {0} is {1}: may not be supported. Auto fallback to IPv4 address family",
                        GetAddress(), ipEndPoint.AddressFamily));
                    break;
            }

            tcpSocket.NoDelay = true;

            tcpSocket.SendBufferSize = options.BufferSize;
            tcpSocket.ReceiveBufferSize = options.BufferSize;

            tcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = ipEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);

            UnifiedLogger.Info(Tag.TCP_SOCKET, string.Format("Initialize socket to {0} : SUCCESS", GetAddress()));
        }
        #endregion

        #region Connection Control
        public override void Connect()
        {
            TriggerConnectionEvent(ConnectionState.CONNECTING);

            try
            {
                //Value of true mean the I/O operation is pending (wait for executing async)
                //Value of false mean the I/O operation completed synchronously, need invoke callback manually
                if (tcpSocket.ConnectAsync(connectArgs) == false)
                {
                    OnConnect(null, connectArgs);
                }
            }
            catch (SocketException e)
            {
                HandleConnectError(e.SocketErrorCode);
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error connecting to {0} : {1}", GetAddress(), e));
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }

        public override void Disconnect()
        {
            TriggerConnectionEvent(ConnectionState.DISCONNECTING);

            try
            {
                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close(Constants.DISCONNECT_TIME_OUT);
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error disconnect to {0} : {1}", GetAddress(), e));
            }
            finally
            {
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }

        public override bool IsConnected()
        {
            return state == ConnectionState.CONNECTED;
        }

        public override IPEndPoint GetConnectedIPEndPoint()
        {
            return ipEndPoint;
        }
        #endregion

        #region Transport Control
        public override void Ping(byte pingId)
        {
            pingData[pingData.Length - 1] = pingId;
            Send(pingData, pingData.Length);
        }

        public override void Send(byte[] data, int bytesNeedWrite)
        {
            try
            {
                UnifiedLogger.Trace(Tag.TCP_SOCKET, string.Format("Sending {0} bytes to {1}", bytesNeedWrite, GetAddress()));
                tcpSocket.BeginSend(data, 0, bytesNeedWrite, SocketFlags.None, sendCallback, null);
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error sending data to {0} : {1}", GetAddress(), e));
            }
        }

        private void Receive()
        {
            try
            {
                while (IsConnected())
                {
                    UnifiedLogger.Trace(Tag.TCP_SOCKET, string.Format("Waiting data from {0}", GetAddress()));
                    int bytesReceived = tcpSocket.Receive(receiveBuffer);

                    if (bytesReceived > 0)
                    {
                        numberReceiveZeroByteInARow = 0;

                        UnifiedLogger.Trace(Tag.TCP_SOCKET, string.Format("Received {0} bytes from {1}", bytesReceived, GetAddress()));
                        TriggerDataReceivedEvent(receiveBuffer, bytesReceived);
                    }
                    else
                    {
                        numberReceiveZeroByteInARow++;

                        UnifiedLogger.Warn(Tag.TCP_SOCKET, string.Format("Received 0 bytes {0} times in a row", numberReceiveZeroByteInARow));
                        if (numberReceiveZeroByteInARow >= Constants.MAX_NUMBER_RECEIVE_ZERO_BYTE_IN_A_ROW)
                        {
                            UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Exceeded {0} times receive 0 byte in a row", Constants.MAX_NUMBER_RECEIVE_ZERO_BYTE_IN_A_ROW));
                            TriggerConnectionEvent(ConnectionState.DISCONNECTED);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error receiving data from {0} : {1}" + e, GetAddress(), e));
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }
        #endregion

        #region Callback
        private void OnConnect(object sender, SocketAsyncEventArgs connectArgs)
        {
            try
            {
                if (connectArgs.SocketError == SocketError.Success)
                {
                    UnifiedLogger.Info(Tag.TCP_SOCKET, string.Format("Connect to {0} : SUCCESS", GetAddress()));
                    TriggerConnectionEvent(ConnectionState.CONNECTED);
                    StartReceiverThread();
                }
                else if (connectArgs.SocketError == SocketError.IsConnected)
                {
                    UnifiedLogger.Warn(Tag.TCP_SOCKET, string.Format("Connect to {0} : SUCCESS (Already connected before)", GetAddress()));
                    TriggerConnectionEvent(ConnectionState.CONNECTED);
                    StartReceiverThread();
                }
                else
                {
                    HandleConnectError(connectArgs.SocketError);
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Exception(Tag.TCP_SOCKET, e);
            }
        }

        private void OnSend(IAsyncResult asyncResult)
        {
            try
            {
                int bytesSent = tcpSocket.EndSend(asyncResult);

                UnifiedLogger.Trace(Tag.TCP_SOCKET, string.Format("Sent {0} bytes to {1}", bytesSent, GetAddress()));
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error sending data to {0} : {1}", GetAddress(), e));
            }
        }
        #endregion

        #region Handle error
        private void HandleConnectError(SocketError socketError)
        {
            if (SocketUtils.IsCriticalError(socketError))
            {
                UnifiedLogger.Error(Tag.TCP_SOCKET, string.Format("Error connecting to {0} : {1}", GetAddress(), connectArgs.SocketError));
            }
            else
            {
                UnifiedLogger.Warn(Tag.TCP_SOCKET, string.Format("Connect operation to {0} is pending: {1}", GetAddress(), socketError));
            }

            if (addressList != null)
            {
                currentAddressIndex++;
                if (currentAddressIndex < addressList.Length)
                {
                    UnifiedLogger.Info(Tag.TCP_SOCKET, string.Format("Current address index is {0}: {1}",
                        currentAddressIndex, addressList[currentAddressIndex]));

                    // Instantiates the endpoint
                    ipEndPoint = new IPEndPoint(addressList[currentAddressIndex], options.Port);
                    UnifiedLogger.Info(Tag.TCP_SOCKET, string.Format("Address Family of {0} is {1}, IP Address is {2}",
                        GetAddress(), ipEndPoint.AddressFamily, ipEndPoint.Address));

                    PrepareTcpSocket();

                    state = ConnectionState.DISCONNECTED;
                    Connect();
                }
                else
                {
                    currentAddressIndex = -1;
                    TriggerConnectionEvent(ConnectionState.DISCONNECTED);
                }
            }
            else
            {
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }
        #endregion

        #region Manage receiver thread
        private void StartReceiverThread()
        {
            ChangeState(ConnectionState.CONNECTED);
            if (receiverThread.ThreadState != ThreadState.Running)
            {
                receiverThread.Start();
            }
        }
        #endregion
    }
}
#endif