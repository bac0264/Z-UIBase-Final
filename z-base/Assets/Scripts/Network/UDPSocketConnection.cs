#if !UNITY_WEBGL
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UnifiedNetwork
{
    public class UDPSocketConnection : AbstractConnection
    {
        private Socket udpSocket;

        private IPEndPoint ipEndPoint;

        private byte[] receiveBuffer;
        private Thread receiverThread;
        private int numberReceiveZeroByteInARow;
        private EndPoint senderEndPoint;

        private AsyncCallback sendCallback;

        private byte[] pingData;

        public UDPSocketConnection(ConnectorOptions options)
            : base(options)
        {
            receiveBuffer = new byte[options.BufferSize];

            sendCallback = new AsyncCallback(OnSend);
            receiverThread = new Thread(Receive);
            senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

            pingData = UnifiedNetworkFactory.CreatePingData(options);
        }

        #region Init
        public override bool Initialize()
        {
            try
            {
                // Instantiates the endpoint
                ipEndPoint = ConnectionUtils.GetIPEndPoint(options.Host, options.Port, options.IsPreferIPv4);
                UnifiedLogger.Info(Tag.UDP_SOCKET, string.Format("Address Family of {0} is {1}, IP Address is {2}",
                    GetAddress(), ipEndPoint.AddressFamily, ipEndPoint.Address));

                PrepareUdpSocket();
                return true;
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Error initialize socket to {0} : {1}", GetAddress(), e));
                return false;
            }
        }

        void PrepareUdpSocket()
        {
            switch (ipEndPoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    break;

                case AddressFamily.InterNetworkV6:
                    udpSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                    break;

                default:
                    udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    UnifiedLogger.Warn(Tag.UDP_SOCKET, string.Format("Address Family of {0} is {1}: may not be supported. Auto fallback to IPv4 address family",
                        GetAddress(), ipEndPoint.AddressFamily));
                    break;
            }

            udpSocket.Bind(senderEndPoint);

            udpSocket.SendBufferSize = options.BufferSize;
            udpSocket.ReceiveBufferSize = options.BufferSize;

            UnifiedLogger.Info(Tag.UDP_SOCKET, string.Format("Initialize socket to {0} : SUCCESS", GetAddress()));
        }
        #endregion

        #region Connection Control
        public override void Connect()
        {
            TriggerConnectionEvent(ConnectionState.CONNECTING);

            try
            {
                TriggerConnectionEvent(ConnectionState.CONNECTED);
                StartReceiverThread();
            }
            catch (Exception e)
            {
                UnifiedLogger.Exception(Tag.UDP_SOCKET, e);
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }

        public override void Disconnect()
        {
            TriggerConnectionEvent(ConnectionState.DISCONNECTING);

            try
            {
                udpSocket.Close(Constants.DISCONNECT_TIME_OUT);
                StopReceiverThread();
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Error disconnect to {0} : {1}", GetAddress(), e));
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
                UnifiedLogger.Trace(Tag.UDP_SOCKET, string.Format("Sending {0} bytes to {1}", bytesNeedWrite, GetAddress()));
                udpSocket.BeginSendTo(data, 0, bytesNeedWrite, SocketFlags.None, ipEndPoint, sendCallback, null);
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Error sending data to {0} : {1}", GetAddress(), e));
            }
        }

        private void Receive()
        {
            try
            {
                while (IsConnected())
                {
                    UnifiedLogger.Trace(Tag.UDP_SOCKET, string.Format("Waiting data from {0}", GetAddress()));
                    int bytesReceived = udpSocket.ReceiveFrom(receiveBuffer, ref senderEndPoint);

                    if (bytesReceived > 0)
                    {
                        numberReceiveZeroByteInARow = 0;

                        UnifiedLogger.Trace(Tag.UDP_SOCKET, string.Format("Received {0} bytes from {1}", bytesReceived, GetAddress()));
                        TriggerDataReceivedEvent(receiveBuffer, bytesReceived);
                    }
                    else
                    {
                        numberReceiveZeroByteInARow++;

                        UnifiedLogger.Warn(Tag.UDP_SOCKET, string.Format("Received 0 bytes {0} times in a row", numberReceiveZeroByteInARow));
                        if (numberReceiveZeroByteInARow >= Constants.MAX_NUMBER_RECEIVE_ZERO_BYTE_IN_A_ROW)
                        {
                            UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Exceeded {0} times receive 0 byte in a row", Constants.MAX_NUMBER_RECEIVE_ZERO_BYTE_IN_A_ROW));
                            TriggerConnectionEvent(ConnectionState.DISCONNECTED);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Error receiving data from {0} : {1}" + e, GetAddress(), e));
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }
        #endregion

        #region Callback
        private void OnSend(IAsyncResult asyncResult)
        {
            try
            {
                int bytesSent = udpSocket.EndSendTo(asyncResult);

                UnifiedLogger.Trace(Tag.UDP_SOCKET, string.Format("Sent {0} bytes to {1}", bytesSent, GetAddress()));
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.UDP_SOCKET, string.Format("Error sending data to {0} : {1}", GetAddress(), e));
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

        private void StopReceiverThread()
        {
            if (receiverThread != null && receiverThread.ThreadState == ThreadState.Running)
            {
                receiverThread.Abort();
            }
        }
        #endregion
    }
}
#endif