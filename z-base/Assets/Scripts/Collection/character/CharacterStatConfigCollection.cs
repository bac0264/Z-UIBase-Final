using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStatConfigCollection : ScriptableObject
{
    public CharacterStatConfigData[] dataGroups;

    public CharacterStatConfigData GetStatConfigData(int statType)
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
public class CharacterStatConfigData : StatConfigData
{
    public override float GetBaseValue(int level)
    {
        return baseValue + level * option1;
    }
}