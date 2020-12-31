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

    private PlayerDailyReward playerDailyReward = null;

    private void SetupController()
    {
        if (playerDailyReward == null) playerDailyReward = DataPlayer.GetModule<PlayerDailyReward>();
    }

    public bool IsReceived()
    {
        SetupController();
        
        return playerDailyReward.IsReceived(id);
    }
    
    public bool IsReceivable()
    {
        SetupController();
        return id <= playerDailyReward.GetCurrentDay();
    }
    
    public bool IsNextDay()
    {
        SetupController();
        return playerDailyReward.IsNextDay(id - 1);
    }

    public void Claim()
    {
        SetupController();
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].RecieveReward();
        }
        playerDailyReward.AddDayReceived(id);
    }
}

