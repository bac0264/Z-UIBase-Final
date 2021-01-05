namespace UnifiedNetwork
{
    /// <summary>
    /// Message that will be sent to Server
    /// </summary>
    public interface Outbound
    {
        void Serialize(ByteBuf buffer);
    }
}