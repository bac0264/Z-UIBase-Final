using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeItemCollection : ScriptableObject
{
    public UpgradeItemData dataGroups;
}

[System.Serializable]
public class UpgradeItemData
{
    public Reward reward;
    public float option1;
    public int maxLevel;

    public Resource GetPrice(int level)
    {
        var resource = reward.GetResource();
        resource.number = reward.resNumber + (long) option1 * level;
        return resource;
    }
}
