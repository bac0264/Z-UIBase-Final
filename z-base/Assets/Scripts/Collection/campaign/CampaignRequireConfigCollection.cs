using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

[System.Serializable]
public class CampaignRequireConfigCollection : ScriptableObject
{
    private CampaignRequireConfigData[] dataGroups;
    
    public CampaignRequireDictionary requireDict;

    public void Convert()
    {
        requireDict.Clear();
        foreach (var data in dataGroups)
        {
            requireDict.Add(data.stage, data);
        }
    }

    public CampaignRequireConfigData GetCampaignRequireData(int stage)
    {
        if (requireDict.ContainsKey(stage))
            return requireDict[stage];
        return null;
    }
}

[System.Serializable]
public class CampaignRequireConfigData
{
    public int stage;
    public Reward require;
}

[System.Serializable]
public class CampaignRequireDictionary : SerializableDictionary<int, CampaignRequireConfigData> { }


