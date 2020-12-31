using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class PlayerAds
{
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

    public void AddAds(int value)
    {
        adsSaveLoad.adsCount += value;
        Save();
    }

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
