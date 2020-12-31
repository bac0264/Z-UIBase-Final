using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerDailyReward
{
    private DailyRewardSaveLoad dailyReward;

    private int DAY_MAX;

    public PlayerDailyReward()
    {
        Load();
    }

    private void Save()
    {
        PlayerPrefs.SetString(KeyUtils.DAILY_REWARD_DATA, JsonConvert.SerializeObject(dailyReward));
    }

    private void Load()
    {
        dailyReward =
            JsonConvert.DeserializeObject<DailyRewardSaveLoad>(PlayerPrefs.GetString(KeyUtils.DAILY_REWARD_DATA));
        if (dailyReward == null)
        {
            dailyReward = new DailyRewardSaveLoad();
        }

        DAY_MAX = LoadResourceController.GetDailyReward().dataGroups.Length;
    }

    public int GetCurrentDay()
    {
        return dailyReward.currentDay;
    }

    public void SetNextDay()
    {
        if (dailyReward.currentDay >= DAY_MAX)
        {
            Reset();
            return;
        }

        dailyReward.currentDay++;
    }

    public void AddDayReceived(int day)
    {
        dailyReward.AddDay(day);
    }

    public void Reset()
    {
        if (dailyReward.dayReceivedDic.Count >= DAY_MAX)
            dailyReward.Reset();
    }

    public bool IsReceived(int day)
    {
        return dailyReward.IsReceived(day);
    }

    public bool IsNextDay(int day)
    {
        return dailyReward.currentDay == day;
    }

    public void UpdateDailyReward(long oldLastTimeOnline, long lastTimeOnline)
    {
        // Set new current day
        if (oldLastTimeOnline > 1000)
        {
            var dayBonus = TimeUtils.GetDayCount(oldLastTimeOnline, lastTimeOnline);
            
            if (dayBonus > 0 && dailyReward.currentDay < DAY_MAX)
            {
                dailyReward.currentDay += (int) dayBonus;
                if (dailyReward.currentDay > DAY_MAX) dailyReward.currentDay = DAY_MAX;
            }
            else if (dailyReward.currentDay >= DAY_MAX)
            {
                Reset();
            }
            Save();
        }
    }
}

[System.Serializable]
public class DailyRewardSaveLoad
{
    [JsonProperty("0")] public int currentDay;
    [JsonProperty("1")] public Dictionary<int, int> dayReceivedDic = new Dictionary<int, int>();

    public DailyRewardSaveLoad()
    {
        currentDay = 1;
    }

    public void AddDay(int day)
    {
        if (dayReceivedDic.ContainsKey(day)) return;
        dayReceivedDic.Add(day, 1);
    }

    public void Reset()
    {
        currentDay = 1;
        dayReceivedDic.Clear();
    }

    public bool IsReceived(int day)
    {
        return dayReceivedDic.ContainsKey(day);
    }
}