using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterLevelCollection : ScriptableObject
{
    public CharacterLevelData[] dataGroups;

    public CharacterLevelData GetCharacterLevelData(int level)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (level == dataGroups[i].level)
                return dataGroups[i];
        }
        
         return null;
    }

    public int GetMaxLevel()
    {
        return dataGroups.Length;
    }
}

[System.Serializable]
public class CharacterLevelData
{
    public int level;
    public Reward[] rewardDatas;

    private Resource[] resources = null;
    public Resource[] GetResources()
    {
        var _resources = new List<Resource>();

        for (int i = 0; i < rewardDatas.Length; i++)
        {
            _resources.Add(rewardDatas[i].GetResource());
        }

        resources = _resources.ToArray();
        return resources;
    }
}
