#if UF_WEB_SOCKET_ENABLE
using BestHTTP.WebSocket;
#endif

using System;
using System.Net;
using System.Text;

namespace UnifiedNetwork
{
    /// <summary>
    /// Frame-based Web Socket implementation
    /// </summary>
    public class WebSocketConnection : AbstractConnection
    {
#if UF_WEB_SOCKET_ENABLE
        private WebSocket webSocket;
#endif

        private string url;

        private byte[] pingData;

        public WebSocketConnection(ConnectorOptions options)
            : base(options)
        {
            url = options.WebSocketUrlPrefix + "://" + GetAddress();
            if (string.IsNullOrEmpty(options.WebSocketUrlPostfix) == false)
            {
                url += ("/" + options.WebSocketUrlPostfix);
            }

            pingData = UnifiedNetworkFactory.CreatePingData(options);
        }

        public override bool Initialize()
        {
            bool isSuccess = true;

            try
            {
                UnifiedLogger.Info(Tag.WEB_SOCKET, string.Format("Initializing socket to {0}", url));

#if UF_WEB_SOCKET_ENABLE
                webSocket = new WebSocket(new Uri(url));

                webSocket.OnOpen = OnWebSocketOpened;
                webSocket.OnClosed = OnWebSocketClosed;

                webSocket.OnMessage = OnMessageReceived;
                webSocket.OnBinary = OnBinaryMessageReceived;

                webSocket.OnError = OnWebSocketError;
#endif
                UnifiedLogger.Info(Tag.WEB_SOCKET, string.Format("Initialize socket to {0} : SUCCESS", GetAddress()));
            }
            catch (Exception e)
            {
                isSuccess = false;
                UnifiedLogger.Error(Tag.WEB_SOCKET, string.Format("Error initialize socket to {0} : {1}", GetAddress(), e));
            }

            return isSuccess;
        }

        public override void Connect()
        {
            TriggerConnectionEvent(ConnectionState.CONNECTING);

            try
            {
                if (!IsConnected())
                {
#if UF_WEB_SOCKET_ENABLE
                    webSocket.Open();
#endif
                }
                else
                {
                    TriggerConnectionEvent(ConnectionState.CONNECTED);
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.WEB_SOCKET, string.Format("Error connecting to {0} : {1}", GetAddress(), e));
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }

        public override void Disconnect()
        {
            TriggerConnectionEvent(ConnectionState.DISCONNECTING);

            try
            {
#if UF_WEB_SOCKET_ENABLE
                webSocket.Close();
#endif
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.WEB_SOCKET, string.Format("Error disconnect to {0} : {1}", GetAddress(), e));
            }
        }

        public override bool IsConnected()
        {
#if UF_WEB_SOCKET_ENABLE
            if (webSocket != null)
            {
                return webSocket.IsOpen && state == ConnectionState.CONNECTED;
            }
            else
            {
                return false;
            }
#else
            return false;
#endif
        }

        public override void Ping(byte pingId)
        {
            pingData[pingData.Length - 1] = pingId;
#if UF_WEB_SOCKET_ENABLE
            webSocket.Send(pingData, 0, (ulong)pingData.Length);
#endif
        }

        public override IPEndPoint GetConnectedIPEndPoint()
        {
            return null;
        }

        public override void Send(byte[] data, int bytesNeedWrite)
        {
            try
            {
                UnifiedLogger.Trace(Tag.WEB_SOCKET, string.Format("Sending {0} bytes to {1}", bytesNeedWrite, GetAddress()));

#if UF_WEB_SOCKET_ENABLE
                webSocket.Send(data, 0, (ulong)bytesNeedWrite);
#endif
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.WEB_SOCKET, string.Format("Error sending data to {0} : {1}", GetAddress(), e));
            }
        }

        #region WebSocket events
#if UF_WEB_SOCKET_ENABLE
        private void OnWebSocketOpened(WebSocket webSocket)
        {
            TriggerConnectionEvent(ConnectionState.CONNECTED);
        }

        private void OnWebSocketClosed(WebSocket webSocket, UInt16 code, string message)
        {
            TriggerConnectionEvent(ConnectionState.DISCONNECTED);
        }

        private void OnMessageReceived(WebSocket webSocket, string message)
        {
            //Currently, we only use binary message. 
            //So we omit text message for now.
            UnifiedLogger.Error(Tag.WEB_SOCKET, string.Format("[Not supported] Received text from {0}: {1}", GetAddress(), message));
        }

        private void OnBinaryMessageReceived(WebSocket webSocket, byte[] message)
        {
            if (message != null && message.Length > 0)
            {
                IncreaseByteReceived(message.Length);

                UnifiedLogger.Trace(Tag.WEB_SOCKET, string.Format("Received {0} bytes from {1}", message.Length, GetAddress()));
                TriggerDataReceivedEvent(message, message.Length);
            }
        }

        private void OnWebSocketError(WebSocket ws, Exception ex)
        {
            UnifiedLogger.Error(Tag.WEB_SOCKET, "An error has occured: " + (ex != null ? ex.Message : "Unknown error"));

            if (webSocket.IsOpen == false)
            {
                TriggerConnectionEvent(ConnectionState.DISCONNECTED);
            }
        }
#endif
        #endregion
    }
}