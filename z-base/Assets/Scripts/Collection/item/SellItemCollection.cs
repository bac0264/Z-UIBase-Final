using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SellItemCollection : ScriptableObject
{
    public SellItemData[] dataGroups;

    public SellItemData GetSellData(int rarity)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].rarity == rarity)
            {
                return dataGroups[i];
            }
        }

        return null;
    }
}

[System.Serializable]
public class SellItemData
{
    public int rarity;
    public Reward sellRequire;
    public float option1;

    public Resource GetPrice()
    {
        var resource = sellRequire.GetResource();
        resource.number = sellRequire.resNumber + rarity * (long) option1;
        return resource;
    }
}