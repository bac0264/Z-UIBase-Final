using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class PlayerAds
{
    [JsonProperty("player_ads")]
    private PlayerAdsSaveLoad adsSaveLoad;

    public PlayerAds()
    {
        Load();
    }

    public void Load()
    {
        adsSaveLoad = JsonConvert.DeserializeObject<PlayerAdsSaveLoad>(PlayerPrefs.GetString(KeyUtils.ADS_DATA));
        if (adsSaveLoad == null)
        {
            adsSaveLoad = new PlayerAdsSaveLoad();
            adsSaveLoad.adsCount = 1;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.ADS_DATA, JsonConvert.SerializeObject(adsSaveLoad));
    }

    /// <summary>
    /// Milestone of player ads
    /// </summary>
    /// <param name="value"></param>
    public void AddAds(int value)
    {
        adsSaveLoad.adsCount += value;
        Save();
    }

    /// <summary>
    /// Get Ads Count.
    /// </summary>
    /// <returns></returns>
    public int GetAdsCount()
    {
        return adsSaveLoad.adsCount;
    }
}

[System.Serializable]
public class PlayerAdsSaveLoad
{
    [JsonProperty("0")] public int adsCount ;
}
