using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerDailyQuest
{
    private PlayerDailyQuestData playerQuestData;

    public PlayerDailyQuest()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.QUEST_DATA, JsonConvert.SerializeObject(playerQuestData));
    }
    
    private void Load()
    {
        Debug.Log(PlayerPrefs.GetString(KeyUtils.QUEST_DATA));
        playerQuestData =
            JsonConvert.DeserializeObject<PlayerDailyQuestData>(PlayerPrefs.GetString(KeyUtils.QUEST_DATA));
        
        if (playerQuestData == null)
        {
            playerQuestData = new PlayerDailyQuestData();
        }

        SyncQuest();
    }

    private void SyncQuest()
    {
        var questCollection = LoadResourceController.GetDailyQuestCollection();
        
        
        for (int i = 0; i < questCollection.dataGroups.Length; i++)
        {
            playerQuestData.SyncQuest(questCollection.dataGroups[i]);
        }
        Save();
    }

    public QuestProgress GetProgressWithId(int id)
    {
        return playerQuestData.GetQuestProgress(id);
    }

    public void AddQuestProgress(BaseListenerData data)
    {
        if (playerQuestData.OnQuestNotify(data))
            Save();
    }

    public void UpdateDailyQuest(long oldLastTimeOnline, long lastTimeOnline)
    {
        var isNextDay = TimeUtils.GetDayCount(oldLastTimeOnline, lastTimeOnline) > 0 ;
        
        if (isNextDay)
        {
            playerQuestData.ResetAllQuest();
            Save();
        }
    }
}

[System.Serializable]
public class PlayerDailyQuestData
{
    [JsonProperty("0")] public Dictionary<int, QuestProgress> dailyQuest = new Dictionary<int, QuestProgress>();

    public QuestProgress GetQuestProgress(int id)
    {
        if (dailyQuest.ContainsKey(id))
        {
            return dailyQuest[id];
        }

        return null;
    }

    public void SyncQuest(DailyQuestData data)
    {
        QuestProgress baseProgress;
        if (!dailyQuest.ContainsKey(data.id))
        {
            baseProgress = data.CreateInstance();
            dailyQuest.Add(data.id, baseProgress);
        }
        else
        {
            baseProgress = data.CreateInstance(dailyQuest[data.id].progress, dailyQuest[data.id].state);
            dailyQuest[data.id] = baseProgress;
        }

        baseProgress.SetState(data.required);
    }

    public bool OnQuestNotify(BaseListenerData data)
    {
        bool isChanged = false;
        
        foreach (var questProgress in dailyQuest)
        {
            if (questProgress.Value.IsShouldNotify(data))
            {
                isChanged = true;

                var required = LoadResourceController.GetDailyQuestCollection().GetDailyQuestData(questProgress.Key)
                    .required;
                questProgress.Value.AddProgress(data);
                questProgress.Value.SetState(required);
            }
        }

        return isChanged;
    }

    public void ResetAllQuest()
    {
        foreach (var questProgress in dailyQuest)
        {
            questProgress.Value.ResetQuest();
        }
    }
}