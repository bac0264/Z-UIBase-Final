namespace UnifiedNetwork
{
    /// <summary>
    /// Message that will be received from Server
    /// </summary>
    public interface Inbound
    {
        void Deserialize(ByteBuf buffer);
    }
}