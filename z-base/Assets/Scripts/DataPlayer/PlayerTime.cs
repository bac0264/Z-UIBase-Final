using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerTime
{
    public PlayerTimeData timeData;

    public PlayerTime()
    {
        Load();
    }

    public void Load()
    {
        timeData = JsonConvert.DeserializeObject<PlayerTimeData>(PlayerPrefs.GetString(KeyUtils.TIME_DATA));

        if (timeData == null)
        {
            timeData = new PlayerTimeData();
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.TIME_DATA, JsonConvert.SerializeObject(timeData));
    }

    private void UpdateFeatureReference(long oldTime, long lastTime)
    {
        DataPlayer.GetModule<PlayerDailyReward>().UpdateDailyReward(oldTime, lastTime);
        DataPlayer.GetModule<PlayerDailyQuest>().UpdateDailyQuest(oldTime, lastTime);
    }
    
    public void SetLastTimeOnline(long lastTimeOnline)
    {
        UpdateFeatureReference(timeData.GetLastTimeOnline(), lastTimeOnline);
       
        timeData.SetLastTimeOnline(lastTimeOnline);
        Save();
    }

    public long GetLastTimeOnline()
    {
        return timeData.GetLastTimeOnline();
    }
    
    public void Add1Day()
    {
        timeData.lastTimeOnline -= TimeUtils.GetTimeADay();
        Save();
    }
    
}

[System.Serializable]
public class PlayerTimeData
{
    [JsonProperty("0")] public long lastTimeOnline;
    
    public void SetLastTimeOnline(long lastTimeOnline)
    {
        this.lastTimeOnline = lastTimeOnline;
    }

    public long GetLastTimeOnline()
    {
        return lastTimeOnline;
    }
}