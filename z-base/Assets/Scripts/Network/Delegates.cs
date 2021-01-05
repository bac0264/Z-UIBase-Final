namespace UnifiedNetwork
{
    // notify connected and disconnected
    public delegate void ConnectionEvent();

    // notify something is sent
    public delegate void SendEvent();

    // notify something is received
    public delegate void ReceiveEvent(int opCode);

    public delegate void OnDataReceived(byte[] data, int bytesNeedRead);

    public delegate void OnMessageReceived(Inbound inboundMessage);
}