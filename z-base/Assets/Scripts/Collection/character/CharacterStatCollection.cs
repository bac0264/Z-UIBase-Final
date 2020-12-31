using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatCollection : ScriptableObject
{
    public CharacterStatData[] dataGroups;
    
    public CharacterStatData GetItemStatDataWithItemId(int id)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (id == dataGroups[i].id)
            {
                return dataGroups[i];
            }
        }

        return null;
    }
}
[System.Serializable]
public class CharacterStatData
{
    public int id;
    public int[] statTypes;

    private CharacterStatConfigCollection statConfigCollection = null;
    private Dictionary<int, ItemStat> itemDict = null;
    public void SetStatConfigCollection(CharacterStatConfigCollection statConfigCollection)
    {
        //if (statConfigCollection == null)
        this.statConfigCollection = statConfigCollection;
    }

    public Dictionary<int, ItemStat> GetItemStats(int level)
    {
        SetStatConfigCollection(LoadResourceController.GetCharacterConfigCollection());
        
        if(itemDict == null) 
            itemDict = new Dictionary<int, ItemStat>();
        
        for (int i = 0; i < statTypes.Length; i++)
        {
            var baseValue = statConfigCollection.GetStatConfigData(statTypes[i]).GetBaseValue(level);

            if (!itemDict.ContainsKey(statTypes[i]))
            {
                ItemStat itemStat = new ItemStat(baseValue, statTypes[i]);
                itemDict.Add(statTypes[i], itemStat);
            }
            else
            {
                itemDict[statTypes[i]].baseStat.RemoveAllModifiers();
                itemDict[statTypes[i]].baseStat.baseValue = baseValue;
            }
        }

        return itemDict;
    }
}
