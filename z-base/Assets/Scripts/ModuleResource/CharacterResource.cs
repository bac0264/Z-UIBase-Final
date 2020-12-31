using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class CharacterResource : Resource
{
    public int characterId;

    public int level;


    [NonSerialized] public Dictionary<int, ItemStat> characterStats;

    [NonSerialized] private CharacterStatCollection characterStatCollection = null;

    public CharacterResource(int type, int id, long number, int characterId, int level) : base(type, id, number)
    {
        this.characterId = characterId;
        this.level = level;

        if (characterStatCollection == null)
            characterStatCollection = LoadResourceController.GetCharacterStat();

        ReloadCharacterStat();
    }

    public void AddLevel(int value)
    {
        level += value;
    }

    public void ReloadCharacterStat()
    {
        this.characterStats = characterStatCollection.GetItemStatDataWithItemId(id).GetItemStats(level);

        var itemList = DataPlayer.GetModule<PlayerInventory>().GetEquipmentItemWithIdCharacter(characterId);

        for (int j = 0; j < itemList.Count; j++)
        {
            for (int k = 0; k < itemList[j].itemStats.Length; k++)
            {
                var itemStat = itemList[j].itemStats[k];
                var statModifier = new StatModifier(itemStat.baseStat.Value, StatModType.Flat);

                characterStats[itemStat.statType].baseStat.AddModifier(statModifier);
            }
        }
    }

    public static CharacterResource CreateInstance(int type, int id, long number, int characterId = -1, int level = 1)
    {
        return new CharacterResource(type, id, number, characterId, level);
    }
}