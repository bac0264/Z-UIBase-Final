using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerInventory
{
    [JsonProperty("player_inventory")]
    private PlayerInventorySaveLoad playerData = new PlayerInventorySaveLoad();
    
    private Dictionary<int, ItemResource> inventoryDic = new Dictionary<int, ItemResource>();

    public PlayerInventory()
    {
        Load();
    }

    public void Load()
    {
        playerData =
            JsonConvert.DeserializeObject<PlayerInventorySaveLoad>(PlayerPrefs.GetString(KeyUtils.INVENTORY_DATA));

        if (playerData == null)
        {
            playerData = new PlayerInventorySaveLoad();
            playerData.inventoryIdMax = 0;
            for (int i = 0; i < 6; i++)
            {
                int id = UnityEngine.Random.Range(1, 5) * GameConstant.ITEM_ID_CONSTANT + UnityEngine.Random.Range(1,5);
                playerData.AddNewData(ItemResource.CreateInstance((int) ResourceType.ItemType, id, 1, playerData.inventoryIdMax, 0));
            }

            Save();
        }

        for (int i = 0; i < playerData.dataList.Count; i++)
        {
            inventoryDic.Add(playerData.dataList[i].inventoryId, playerData.dataList[i]);
        }
    }

    public void Save()
    {
        Debug.Log("inventory");
        PlayerPrefs.SetString(KeyUtils.INVENTORY_DATA, JsonConvert.SerializeObject(playerData));
    }

    public void DebugLog()
    {
        Debug.Log(JsonConvert.SerializeObject(playerData));
    }
    
    /// <summary>
    /// Re-add unequipment items to inventory and not generate new inventory id
    /// </summary>
    /// <param name="itemResource"> Equipment Item </param>
    /// <returns></returns>
    public bool AddItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        ItemResource resource = itemResource;
        
        inventoryDic.Add(resource.inventoryId, resource);
        playerData.AddData(resource);

        return true;
    }

    /// <summary>
    /// Add new item with new inventory id
    /// </summary>
    /// <param name="itemResource"></param>
    /// <returns></returns>
    public bool AddNewItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        ItemResource resource = itemResource;
        resource.inventoryId = playerData.inventoryIdMax + 1;

        inventoryDic.Add(resource.inventoryId, resource);
        playerData.AddNewData(resource);
        Save();
        return true;
    }

    /// <summary>
    /// Remove to sell or remove to equip
    /// </summary>
    /// <param name="itemResource"></param>
    /// <returns></returns>
    public bool RemoveItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        if (inventoryDic.ContainsKey(itemResource.inventoryId))
        {
            inventoryDic.Remove(itemResource.inventoryId);
            playerData.RemoveData(itemResource);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Add equipment item to character
    /// </summary>
    /// <param name="characterId"> current character id</param>
    /// <param name="itemResource"></param>
    /// <returns></returns>
    public bool AddEquipmentItem(int characterId, ItemResource itemResource)
    {
        var isSuccess = playerData.AddEquipmentItem(characterId, itemResource);
        return isSuccess;
    }

    /// <summary>
    /// Unequip item
    /// </summary>
    /// <param name="characterId"> Character id need</param>
    /// <param name="itemResource"></param>
    /// <returns></returns>
    public bool RemoveEquipmentItem(int characterId, ItemResource itemResource)
    {
        var isSuccess = playerData.RemoveEquipmentItem(characterId, itemResource);
        return isSuccess;
    }

    public ItemResource GetItem(int index)
    {
        if (playerData.dataList.Count == 0) return null;
        if (index < playerData.dataList.Count)
        {
            return playerData.dataList[index];
        }

        index = index % playerData.dataList.Count;
        return playerData.dataList[index];
    }

    /// <summary>
    ///  Get all item
    /// </summary>
    /// <returns></returns>
    public List<ItemResource> GetItemResources()
    {
        return playerData.dataList;
    }

    /// <summary>
    /// Get equipment item list of character.
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public List<ItemResource> GetEquipmentItemWithIdCharacter(int characterId)
    {
        return playerData.GetEquipmentItemWithCharacterId(characterId);
    }
}

[System.Serializable]
public class PlayerInventorySaveLoad : DataSave<ItemResource>
{
    [JsonProperty("1")] public int inventoryIdMax = 0;

    // List equipment item of each character
    [JsonProperty("2")] public List<PlayerEquipment> playerEquipments = new List<PlayerEquipment>();

    public void AddNewData(ItemResource t)
    {
        base.AddData(t);
        inventoryIdMax++;
    }

    public bool AddEquipmentItem(int characterId, ItemResource equipItem)
    {
        if (equipItem == null) return false;
        for (int i = 0; i < playerEquipments.Count; i++)
        {
            if (playerEquipments[i].characterId == characterId)
            {
                playerEquipments[i].AddData(equipItem);
                return true;
            }
        }

        PlayerEquipment playerEquip = new PlayerEquipment();
        playerEquip.characterId = characterId;
        playerEquip.AddData(equipItem);
        playerEquipments.Add(playerEquip);
        return true;
    }

    public bool RemoveEquipmentItem(int characterId, ItemResource equipItem)
    {
        if (equipItem == null) return false;
        for (int i = 0; i < playerEquipments.Count; i++)
        {
            if (playerEquipments[i].characterId == characterId)
            {
                playerEquipments[i].itemResources.Remove(equipItem);
                return true;
            }
        }

        return false;
    }

    public List<ItemResource> GetEquipmentItemWithCharacterId(int characterId)
    {
        for (int i = 0; i < playerEquipments.Count; i++)
        {
            if (playerEquipments[i].characterId == characterId)
            {
                return playerEquipments[i].itemResources;
            }
        }

        return new List<ItemResource>();
    }
}

[System.Serializable]
public class PlayerEquipment
{
    public int characterId;

    public List<ItemResource> itemResources;

    public PlayerEquipment()
    {
        characterId = 0;
        itemResources = new List<ItemResource>();
    }

    public void AddData(ItemResource equipItem)
    {
        itemResources.Add(equipItem);
    }
}