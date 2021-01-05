using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyRewardCollection : ScriptableObject
{
    public DailyRewardData[] dataGroups;
}


[System.Serializable]
public class DailyRewardData
{
    public int id;
    public Reward[] rewards;
    
    public bool IsReceived()
    {
        return DataPlayer.GetModule<PlayerDailyReward>().IsReceived(id);
    }
    
    public bool IsReceivable()
    {
        return id <= DataPlayer.GetModule<PlayerDailyReward>().GetCurrentDay();
    }
    
    public bool IsNextDay()
    {
        return DataPlayer.GetModule<PlayerDailyReward>().IsNextDay(id - 1);
    }

    public void Claim()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].RecieveReward();
        }
        DataPlayer.GetModule<PlayerDailyReward>().AddDayReceived(id);
    }
}

