using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyQuestCollection : ScriptableObject
{
    public DailyQuestData[] dataGroups;

    public DailyQuestData GetDailyQuestData(int id)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].id == id) return dataGroups[i];
        }

        return null;
    }
}

[System.Serializable]
public class DailyQuestData : BaseQuestData
{

    public QuestProgress GetProgress()
    {
        return DataPlayer.GetModule<PlayerDailyQuest>().GetProgressWithId(id);
    }
    
    public QuestProgress CreateInstance(long progress = 0, int state = 0)
    {
        return QuestProgress.CreateInstance(this, progress, state);
    }
    
    public QuestProgress CreateInstance(BaseQuestData data)
    {
        return QuestProgress.CreateInstance(data);
    }
}
