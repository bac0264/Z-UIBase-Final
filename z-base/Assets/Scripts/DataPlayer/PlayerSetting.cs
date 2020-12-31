using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerSetting
{
    private PlayerSettingData settingData;

    public PlayerSetting()
    {
        Load();
    }

    private void Load()
    {
        settingData = JsonConvert.DeserializeObject<PlayerSettingData>(PlayerPrefs.GetString(KeyUtils.SETTING_DATA));
        if (settingData == null)
        {
            settingData = new PlayerSettingData()
            {
                onMusic = true,
                onSound = true,
            };
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.SETTING_DATA, JsonConvert.SerializeObject(settingData));
    }
    
    public bool IsOpenMusic()
    {
        return settingData.onMusic;
    }

    public bool IsOpenSound()
    {
        return settingData.onSound;
    }
    
    public int GetCurrentLanguage()
    {
        return settingData.language;
    }
    
    public void SetOpenMusic()
    {
        settingData.onMusic = !settingData.onMusic;
        Save();
    }
    
    public void SetOpenSound()
    {
        settingData.onSound = !settingData.onSound;
        Save();
    }
    
    public void SetLanguage(int language)
    {
        settingData.language = language;
        Save();
    }
}

[System.Serializable]
public class PlayerSettingData
{
    [JsonProperty("0")] public bool onMusic;
    [JsonProperty("1")] public bool onSound;
    [JsonProperty("2")] public int language;
}