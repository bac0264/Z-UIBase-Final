using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

[System.Serializable]
public class PlayerGachaSaveLoad
{
    [JsonProperty("0")] public Dictionary<long, long> lastTimeGacha1Dict = new Dictionary<long, long>();
}
public class PlayerGacha
{
    private PlayerGachaSaveLoad gachaData;

    public PlayerGacha()
    {
        Load();
    }

    public void Load()
    {
        gachaData = JsonConvert.DeserializeObject<PlayerGachaSaveLoad>(PlayerPrefs.GetString(KeyUtils.GACHA_DATA));
        if (gachaData == null)
        {
            gachaData = new PlayerGachaSaveLoad();
            for (int i = 0; i < LoadResourceController.GetGachaConfigCollection().dataGroups.Length; i++)
            {
                gachaData.lastTimeGacha1Dict.Add(i + 1, 0);
            }
        }
    }
    
    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.GACHA_DATA, JsonConvert.SerializeObject(gachaData));
    }

    public void SetLastTimeGacha1Free(int id, long currentTime)
    {
        gachaData.lastTimeGacha1Dict[id] = currentTime;
        Save();
    }

    public long GetRangeTime(int id, long timeConfig, long currentTime)
    {
        if (gachaData.lastTimeGacha1Dict[id] < 1000) return 0;
        var rangeTime = TimeUtils.GetGachaTime(gachaData.lastTimeGacha1Dict[id], timeConfig, currentTime);

        return rangeTime;
    }
}
