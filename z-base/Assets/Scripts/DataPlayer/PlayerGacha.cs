﻿using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class PlayerGachaSaveLoad
{
    [JsonProperty("0")] public Dictionary<long, long> lastTimeGacha1Dict = new Dictionary<long, long>();
}
public class PlayerGacha
{
    [JsonProperty("player_gacha")]
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
    
    /// <summary>
    /// Set last time gacha free
    /// </summary>
    /// <param name="id"> gacha id</param>
    /// <param name="currentTime"></param>
    public void SetLastTimeGacha1Free(int id, long currentTime)
    {
        gachaData.lastTimeGacha1Dict[id] = currentTime;
        Save();
    }

    /// <summary>
    ///  Take time left of Gacha
    /// </summary>
    /// <param name="id"> gacha id</param>
    /// <param name="timeConfig"> duration to get gacha</param>
    /// <param name="currentTime"> </param>
    /// <returns></returns>
    public long GetRangeTime(int id, long timeConfig, long currentTime)
    {
        if (gachaData.lastTimeGacha1Dict[id] < 1000) return 0;
        var rangeTime = TimeUtils.GetGachaTime(gachaData.lastTimeGacha1Dict[id], timeConfig, currentTime);

        return rangeTime;
    }
}
