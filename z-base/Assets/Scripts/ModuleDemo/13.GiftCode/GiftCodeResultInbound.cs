using Newtonsoft.Json;
using UnifiedNetwork;

public class GiftCodeResultInbound : AbstractLogicCode, Inbound
{
    public int giftCodeLogicCode = 0;
    public Reward[] fullReward;

    public void Deserialize(ByteBuf buffer)
    {
        logicCode = buffer.GetShort();
        giftCodeLogicCode = buffer.GetShort();
        if (logicCode == LogicCode.SUCCESS)
        {
            string compressString = buffer.GetString(false);
            string jsonDataString = CompressString.StringCompressor.DecompressString(compressString);
            fullReward = JsonConvert.DeserializeObject<Reward[]>(jsonDataString);
        }
    }
}