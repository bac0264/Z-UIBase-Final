using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuestData
{
    [SerializeField] public int group;
    [SerializeField] public int id;
    [SerializeField] public int type;
    [SerializeField] public int idTarget;
    [SerializeField] public long required;
    [SerializeField] public Reward[] rewards;
    [SerializeField] public string title;
    [SerializeField] public string iconName;
    [SerializeField] private string description;
    

    public string Description
    {
        get { return GetDescription(); }
    }

    public bool CanGoto()
    {
        return false; //QuestAchievementHelper.CanGoToQuest((QuestType) type);
    }

    public string GetDescription()
    {
        return ""; // QuestAchievementHelper.GetDescription(type, description, required, idTarget);
    }
}

public enum QuestType
{
    PLAY_CAMPAIGN = 0,
    LOGIN = 1,
    WATCH_ADS = 2,
    GACHA = 3,
}