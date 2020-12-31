using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class GachaCollection : ScriptableObject
{
    public GachaData[] dataGroups;

    public GachaData GetGachaData(int id)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].id == id) return dataGroups[i];
        }

        return null;
    }
}

[System.Serializable]
public class GachaData
{
    public int id;
    public Reward[] consumeGachas;
    public long timeConfig;
    public int minId;
    public int maxId;
    public int rate1;
    public int rate2;
    public int rate3;

    public Reward[] GetGacha()
    {
        var rewards = new List<Reward>();
        rewards.Add(GetReward());

        return rewards.ToArray();
    }
    
    private Reward GetReward()
    {
        var rewards = new List<Reward>();
        var id = UnityEngine.Random.Range(minId, maxId) * GameConstant.ITEM_ID_CONSTANT;
        var temp = UnityEngine.Random.Range(1, rate3);
        if (temp > 0 && temp <= rate1)
        {
            id += UnityEngine.Random.Range(1, 3);
        }
        else if (rate1 < temp && temp <= rate2)
        {
            id += 3;
        }
        else
        {
            id += 4;
        }

        return Reward.CreateInstanceReward((int) ResourceType.ItemType, id, 1);
    }
    
    public Reward[] GetGacha10()
    {
        List<Reward> rewardList = new List<Reward>();
        for (int i = 0; i < 10; i++)
        {
            rewardList.Add(GetReward());
        }

        return rewardList.ToArray();
    }

    public Reward GetRequireGacha1()
    {
        return consumeGachas[0];
    }

    public Reward GetRequireGacha10()
    {
        return consumeGachas[1];
    }

    public long GetTimeConfig()
    {
        return timeConfig;
    }
}


