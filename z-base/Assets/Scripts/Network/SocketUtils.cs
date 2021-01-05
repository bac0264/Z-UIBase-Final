#if !UNITY_WEBGL
using System.Net.Sockets;

public enum SocketStatusCode
{
    UNKNOWN_ERROR = -1,

    SUCCESS = 0,

    OPERATION_PENDING = 1,
    OPERATION_IN_PROGRESS = 2,
    OPERATION_CANCELLED = 3,

    INVALID_PARAMS = 4,

    ACCESS_DENIED = 5,
    NETWORK_ERROR = 6,
    RESOURCES_UNAVAILABLE = 7,
    NOT_SUPPORTED = 8
}

namespace UnifiedNetwork
{
    public class SocketUtils
    {
        public static bool IsCriticalError(SocketError socketError)
        {
            SocketStatusCode code = ConvertToStatusCode(socketError);
            if (code != SocketStatusCode.SUCCESS && code != SocketStatusCode.OPERATION_PENDING)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static SocketStatusCode ConvertToStatusCode(SocketError socketError)
        {
            SocketStatusCode code = SocketStatusCode.UNKNOWN_ERROR;

            switch (socketError)
            {
                case SocketError.SocketError:
                    code = SocketStatusCode.UNKNOWN_ERROR;
                    break;

                case SocketError.Success:
                case SocketError.IsConnected:
                    code = SocketStatusCode.SUCCESS;
                    break;

                case SocketError.IOPending:
                case SocketError.WouldBlock:
                case SocketError.NoBufferSpaceAvailable:
                    code = SocketStatusCode.OPERATION_PENDING;
                    break;

                case SocketError.InProgress:
                case SocketError.AlreadyInProgress:
                    code = SocketStatusCode.OPERATION_IN_PROGRESS;
                    break;

                case SocketError.Interrupted:
                    code = SocketStatusCode.OPERATION_CANCELLED;
                    break;

                case SocketError.AccessDenied:
                    code = SocketStatusCode.ACCESS_DENIED;
                    break;

                case SocketError.AddressAlreadyInUse:
                case SocketError.AddressNotAvailable:
                case SocketError.NetworkDown:
                case SocketError.NetworkUnreachable:
                case SocketError.NetworkReset:
                case SocketError.HostUnreachable:
                case SocketError.HostNotFound:
                case SocketError.HostDown:
                case SocketError.ConnectionRefused:
                case SocketError.ConnectionAborted:
                case SocketError.ConnectionReset:
                case SocketError.Disconnecting:
                case SocketError.NotConnected:
                case SocketError.Shutdown:
                case SocketError.OperationAborted:
                case SocketError.Fault:
                case SocketError.TryAgain:
                case SocketError.TimedOut:
                case SocketError.NoRecovery:
                case SocketError.NoData:
                    code = SocketStatusCode.NETWORK_ERROR;
                    break;

                case SocketError.TooManyOpenSockets:
                case SocketError.ProcessLimit:
                case SocketError.SystemNotReady:
                    code = SocketStatusCode.RESOURCES_UNAVAILABLE;
                    break;

                case SocketError.ProtocolNotSupported:
                case SocketError.SocketNotSupported:
                case SocketError.OperationNotSupported:
                case SocketError.ProtocolFamilyNotSupported:
                case SocketError.AddressFamilyNotSupported:
                case SocketError.VersionNotSupported:
                    code = SocketStatusCode.NOT_SUPPORTED;
                    break;

                case SocketError.InvalidArgument:
                case SocketError.NotSocket:
                case SocketError.DestinationAddressRequired:
                case SocketError.MessageSize:
                case SocketError.ProtocolType:
                case SocketError.ProtocolOption:
                case SocketError.TypeNotFound:
                case SocketError.NotInitialized:
                    code = SocketStatusCode.INVALID_PARAMS;
                    break;
            }

            return code;
        }
    }
}
#endif