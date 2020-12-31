using System;
using System.Collections.Generic;

[System.Serializable]
public class Reward : IRecieveReward
{

    public int resType;

    public int resId;

    public long resNumber;

    public Reward()
    {
    }
    
    public Reward(int resType, int resId, long resNumer)
    {
        this.resType = resType;
        this.resId = resId;
        this.resNumber = resNumer;
    }

    public static Reward CreateInstanceReward(int type, int id, long number)
    {
        return new Reward(type, id, number);
    }

    public Resource GetResource()
    {
        return Resource.CreateInstance(resType, resId, resNumber);
    }

    public void RecieveReward()
    {
        if (resType == (int) ResourceType.ItemType)
        {
            DataPlayer.GetModule<PlayerInventory>().AddNewItem(ItemResource.CreateInstance(resType, resId, resNumber, 0, 0));
        }
        else if (resType == (int) ResourceType.MoneyType)
        {
            DataPlayer.GetModule<PlayerMoney>().AddOne((MoneyType) resId, resNumber);
        }
        else if (resType == (int) ResourceType.CharacterType)
        {
            DataPlayer.GetModule<PlayerCharacter>().AddCharacter(CharacterResource.CreateInstance(resType, resId, resNumber));
        }
        else
        {

        }
    }

    public static void RecieveManyRewards(Reward[] rewards)
    {
        foreach (var reward in rewards)
        {
            reward.RecieveReward();
        }
    }
    
    public static Reward[] FixDuplicateRewards(List<Reward> rewardList)
    {
        List<Reward> rewards = new List<Reward>();
        for (int i = 0; i < rewardList.Count; i++)
        {
            for (int j = rewardList.Count - 1; j >= 0 && i != j; j--)
            {
                if (rewardList[i].resType == rewardList[j].resType && rewardList[i].resId == rewardList[j].resId)
                {
                    rewardList[i].resNumber += rewardList[j].resNumber;
                    rewardList.Remove(rewardList[j]);
                }
            }
            rewards.Add(rewardList[i]);
        }

        return rewards.ToArray();
    }
    
    public static Resource[] GetResources(Reward[] rewardList)
    {
        List<Resource> rewards = new List<Resource>();
        for (int i = 0; i < rewardList.Length; i++)
        {
            rewards.Add(rewardList[i].GetResource());
        }

        return rewards.ToArray();
    }
}