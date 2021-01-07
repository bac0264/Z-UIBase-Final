using Newtonsoft.Json;
using UnityEngine;
using Zitga.Localization;

public class PlayerSetting
{
    [JsonProperty("player_setting")]
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
        Debug.Log("setting");
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
        Localization.Current.localCultureInfo = Locale.GetCultureInfoByLanguage((SystemLanguage) settingData.language);
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