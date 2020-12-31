using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerInventory
{
    private PlayerInventorySaveLoad resourceList = new PlayerInventorySaveLoad();
    private Dictionary<int, ItemResource> inventoryDic = new Dictionary<int, ItemResource>();

    public PlayerInventory()
    {
        Load();
    }

    public void Load()
    {
        resourceList =
            JsonConvert.DeserializeObject<PlayerInventorySaveLoad>(PlayerPrefs.GetString(KeyUtils.INVENTORY_DATA));

        if (resourceList == null)
        {
            resourceList = new PlayerInventorySaveLoad();
            resourceList.inventoryIdMax = 0;
            for (int i = 0; i < 6; i++)
            {
                int id = UnityEngine.Random.Range(1, 5) * GameConstant.ITEM_ID_CONSTANT + UnityEngine.Random.Range(1,5);
                resourceList.AddNewData(ItemResource.CreateInstance((int) ResourceType.ItemType, id, 1, resourceList.inventoryIdMax, 0));
            }

            Save();
        }

        for (int i = 0; i < resourceList.dataList.Count; i++)
        {
            inventoryDic.Add(resourceList.dataList[i].inventoryId, resourceList.dataList[i]);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.INVENTORY_DATA, JsonConvert.SerializeObject(resourceList));
    }

    public bool AddItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        ItemResource resource = itemResource;
        
        inventoryDic.Add(resource.inventoryId, resource);
        resourceList.AddData(resource);

        return true;
    }

    public bool AddNewItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        ItemResource resource = itemResource;
        resource.inventoryId = resourceList.inventoryIdMax + 1;

        inventoryDic.Add(resource.inventoryId, resource);
        resourceList.AddNewData(resource);
        Save();
        return true;
    }

    public bool RemoveItem(ItemResource itemResource)
    {
        if (itemResource == null) return false;

        if (inventoryDic.ContainsKey(itemResource.inventoryId))
        {
            inventoryDic.Remove(itemResource.inventoryId);
            resourceList.RemoveData(itemResource);

            return true;
        }

        return false;
    }

    public bool AddEquipmentItem(int characterId, ItemResource itemResource)
    {
        var isSuccess = resourceList.AddEquipmentItem(characterId, itemResource);
        return isSuccess;
    }

    public bool RemoveEquipmentItem(int characterId, ItemResource itemResource)
    {
        var isSuccess = resourceList.RemoveEquipmentItem(characterId, itemResource);
        return isSuccess;
    }

    public ItemResource GetItem(int index)
    {
        if (resourceList.dataList.Count == 0) return null;
        if (index < resourceList.dataList.Count)
        {
            return resourceList.dataList[index];
        }

        index = index % resourceList.dataList.Count;
        return resourceList.dataList[index];
    }

    public List<ItemResource> GetItemResources()
    {
        return resourceList.dataList;
    }

    public List<ItemResource> GetEquipmentItemWithIdCharacter(int characterId)
    {
        return resourceList.GetEquipmentItemWithCharacterId(characterId);
    }
}

[System.Serializable]
public class PlayerInventorySaveLoad : DataSave<ItemResource>
{
    [JsonProperty("1")] public int inventoryIdMax = 0;

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