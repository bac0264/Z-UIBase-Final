using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdsConfigCollection : ScriptableObject
{
    public AdsConfigData[] dataGroups;

    public AdsConfigData GetAdsConfigData(int id)
    {
        if (id > dataGroups.Length) id = dataGroups.Length;
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].id == id) return dataGroups[i];
        }

        return null;
    }
}


[System.Serializable]
public class AdsConfigData
{
    public int id;
    public Reward[] Rewards;
}
