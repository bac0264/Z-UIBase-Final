using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum StatType
{
    Atk = 0,
    Hp = 1,
    Def = 2,
    Speed = 3,
}

[System.Serializable]

public class ItemResource : Resource
{
    public int inventoryId;

    public int level;

    [NonSerialized] public int rarity;
    
    [NonSerialized] private ItemStatCollection itemStatCollection = null;
    [NonSerialized] private UpgradeItemCollection upgradeItemCollection = null;
    
    [NonSerialized] public ItemStat[] itemStats;
    
    [NonSerialized] private StringBuilder localize = null;
    [NonSerialized] private StringBuilder localizeNextLevel = null;
    /// <summary>
    /// Item constructor with params
    /// </summary>
    /// <param name="type"> A type in ResourceType. </param>
    /// <param name="id"> Unique identify of a type. </param>
    /// <param name="number"> Number of resource.. </param>
    /// <param name="itemStats"> Data of item including BaseStat and StatType. </param>
    /// <returns></returns>
    public ItemResource(int type, int id, long number, int inventoryId, int level, ItemStat[] itemStats = null) : base(type, id, number)
    {
        this.level = level;

        this.inventoryId = inventoryId;
        
        if (upgradeItemCollection == null)
            upgradeItemCollection = LoadResourceController.GetUpgradeItemCollection();

        if (itemStatCollection == null)
            itemStatCollection = LoadResourceController.GetItemStat();

        ReloadItemStats();
    }

    public ItemResource GetCopy()
    {
        return new ItemResource(type, id, number, inventoryId, level, itemStats);
    }

    public void ReloadItemStats()
    {
        var data = itemStatCollection.GetItemStatDataWithItemId(id);
        rarity = data.rarity;
        itemStats = data.GetItemStats(level);
    }

    public bool IsMaxLevel()
    {
        return level >= upgradeItemCollection.dataGroups.maxLevel;
    }
    public int GetPriority()
    {
        return id % GameConstant.ITEM_ID_CONSTANT;
    }

    public string GetStatLocalize(string option = "\n")
    {
        if(localize == null)
            localize = new StringBuilder();
        
        if(localizeNextLevel == null)
            localizeNextLevel = new StringBuilder();
        
        localize.Clear();
        localizeNextLevel.Clear();

        var isMaxLevel = level >= upgradeItemCollection.dataGroups.maxLevel;
            
        for (int i = 0; i < itemStats.Length; i++)
        {
            localize.Append(itemStats[i].GetLocalize() + option);
            if (!isMaxLevel)
            {
                var nextLevel = itemStats[i].GetStatNextLevel(level);
                localizeNextLevel.Append(nextLevel.GetLocalize() + "\n");
            }
        }

        return localize.ToString();
    }
    
    public string GetStatLocalizeNextLevel(string option = "\n")
    {
        if (localizeNextLevel == null)
        {
            GetStatLocalize(option);
        }

        return localizeNextLevel.ToString();
    }
    public static ItemResource CreateInstance(int type, int id, long number,int inventoryId, int level, ItemStat[] itemStats = null)
    {
        return new ItemResource(type, id, number, inventoryId, level, itemStats);
    }
}

[System.Serializable]
public class ItemStat
{
    public BaseStat baseStat;

    /// <summary>
    /// (StatType) Stat type of Item to get key localize.
    /// </summary>
    public int statType;

    private StatConfigCollection statConfigCollection = null;
    public ItemStat()
    {
        
    }
    public ItemStat(float _baseStat, StatType _statType)
    {
        this.baseStat = BaseStat.CreateInstance(_baseStat);
        this.statType = (int)_statType;
    }
    
    public ItemStat(float _baseStat, int _statType)
    {
        this.baseStat = BaseStat.CreateInstance(_baseStat);
        this.statType = (int)_statType;
    }

    public virtual ItemStat GetStatNextLevel(int level)
    {
        if (statConfigCollection == null)
        {
            statConfigCollection = LoadResourceController.GetStatConfigCollection();
        }
        
        // Get base data from scriptable object
        var baseValue = statConfigCollection.GetStatConfigData(statType).GetBaseValue(level + 1);
        ItemStat itemStat = new ItemStat(baseValue, statType);
        
        return itemStat;    
    }
    
    public string GetLocalize(string optionPlus = ": ")
    {
        var localize = (StatType) statType + optionPlus + baseStat.Value;
        return localize;
    }
}
