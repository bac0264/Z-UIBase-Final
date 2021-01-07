using System.Collections.Generic;
using Newtonsoft.Json;

namespace GiftCode
{
    public class GiftCodeReward
    {
        [JsonProperty("item")]
        public List<GiftCodeItem> items { get; private set; }
    }
    
    [System.Serializable]
    public class GiftCodeItem
    {
        [JsonProperty("id")]
        public int id { get; private set; }

        [JsonProperty("number")]
        public int number { get; private set; }

        [JsonProperty("type")]
        public int type { get; private set; }

        public Reward GetReward()
        {
            return Reward.CreateInstanceReward(type,id,number);
        }
    }
}