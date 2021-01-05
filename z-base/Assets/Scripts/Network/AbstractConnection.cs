using System;
using System.Collections.Generic;
using System.Net;

namespace UnifiedNetwork
{
    public abstract class AbstractConnection : IDisposable
    {
        #region Connection properties
        public ConnectorOptions options { get; private set; }
        public ConnectionState state { get; protected set; }
        #endregion

        #region Connection event
        public ConnectionEvent OnConnected;
        public ConnectionEvent OnDisconnected;

        public OnDataReceived OnDataReceived;
        #endregion

        public AbstractConnection(ConnectorOptions options)
        {
            this.options = options;

            state = ConnectionState.DISCONNECTED;
        }

        #region Connection Control
        /// <summary>
        /// Initialize connection properties, must be called before any other action
        /// </summary>
        /// <returns>Indicate initialize operation success or not</returns>
        public abstract bool Initialize();

        /// <summary>
        /// Attempt connect to specified host
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Force connection to be closed. You can not reuse connection after invoking Disconnect
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Check connection status. If isUsePing == true, the underlying connection will try pinging to host, which is time-consuming action.
        /// </summary>
        public abstract bool IsConnected();
        #endregion

        #region Transport Control
        /// <summary>
        /// Ping to check connection status
        /// </summary>
        public abstract void Ping(byte pingId);

        /// <summary>
        /// Send a byte array to host
        /// </summary>
        /// <param name="data">Data to send</param>
        public abstract void Send(byte[] data, int bytesNeedWrite);
        #endregion

        #region Connection Event
        protected void TriggerConnectionEvent(ConnectionState state)
        {
            if (ChangeState(state))
            {
                ConnectionEvent ConnectionEvent = null;
                switch (state)
                {
                    case ConnectionState.CONNECTED:
                        ConnectionEvent = OnConnected;
                        break;

                    case ConnectionState.DISCONNECTED:
                        ConnectionEvent = OnDisconnected;
                        break;
                }

                if (ConnectionEvent != null)
                {
                    try
                    {
                        ConnectionEvent();
                    }
                    catch (Exception e)
                    {
                        UnifiedLogger.Error(Tag.BASE_CONNECTION,
                            string.Format("{0} callback success, but listener has an error: {1}", state, e));
                    }
                }
            }
        }

        protected void TriggerDataReceivedEvent(byte[] data, int bytesNeedRead)
        {
            if (OnDataReceived != null)
            {
                try
                {
                    OnDataReceived(data, bytesNeedRead);
                }
                catch (Exception e)
                {
                    UnifiedLogger.Error(Tag.BASE_CONNECTION,
                        string.Format("OnDataReceived callback success, but listener has an error: {0}", e));
                }
            }
        }
        #endregion

        #region Util
        /// <summary>
        /// Get server address in format of host:port
        /// </summary>
        /// <returns>Address in format of host:port</returns>
        public string GetAddress()
        {
            return options.Address;
        }

        /// <summary>
        /// Get currently connected IP of server (Socket only)
        /// </summary>
        public abstract IPEndPoint GetConnectedIPEndPoint();

        /// <summary>
        /// Get server address in format of host:port
        /// </summary>
        /// <returns>Address in format of host:port</returns>

        protected bool ChangeState(ConnectionState newState)
        {
            if (state != newState)
            {
                UnifiedLogger.Info(Tag.BASE_CONNECTION, string.Format("{0} => {1} to {2}", state, newState, GetAddress()));
                state = newState;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Disconnect();
        }
        #endregion
    }
}