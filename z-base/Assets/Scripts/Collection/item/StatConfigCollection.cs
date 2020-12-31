using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatConfigCollection : ScriptableObject
{
    public StatConfigData[] dataGroups;

    public StatConfigData GetStatConfigData(int statType)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].statType == statType)
            {
                return dataGroups[i];
            }
        }

        return null;
    }
}
[System.Serializable]
public class StatConfigData
{
    public int statType;
    public float baseValue;
    public float option1;
    
    public virtual float GetBaseValue(int level)
    {
        return baseValue + level * option1;
    }
}
