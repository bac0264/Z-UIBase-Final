using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerCharacterSaveLoad : DataSave<CharacterResource>
{
    [JsonProperty("1")] public int currentCharacter;
}
public class PlayerCharacter
{
    private PlayerCharacterSaveLoad resourceList = new PlayerCharacterSaveLoad();
    private Dictionary<int, CharacterResource> inventoryDic = new Dictionary<int, CharacterResource>();

    public PlayerCharacter()
    {
        Load();
    }

    public void Load()
    {
        resourceList =
            JsonConvert.DeserializeObject<PlayerCharacterSaveLoad>(PlayerPrefs.GetString(KeyUtils.CHARACTER_DATA));
            //JsonUtility.FromJson<PlayerCharacterSaveLoad>(PlayerPrefs.GetString(KeyUtils.CHARACTER_DATA));

        if (resourceList == null)
        {
            resourceList = new PlayerCharacterSaveLoad {currentCharacter = 0};

            for (int i = 0; i < 6; i++)
            {
                int id = UnityEngine.Random.Range(0, 4);
                resourceList.AddData(CharacterResource.CreateInstance((int) ResourceType.CharacterType, id, 1, i));
            }
            
            Save();
        }

        for (int i = 0; i < resourceList.dataList.Count; i++)
        {
            inventoryDic.Add(resourceList.dataList[i].characterId, resourceList.dataList[i]);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.CHARACTER_DATA, JsonConvert.SerializeObject(resourceList));
    }

    public bool AddCharacter(CharacterResource resource)
    {
        if (resource == null || inventoryDic.ContainsKey(resource.characterId)) return false;
        
        resourceList.AddData(resource);
        inventoryDic.Add(resource.characterId, resource);
        Save();
        
        return true;
    }

    public bool RemoveCharacter(CharacterResource resource)
    {
        if (resource == null) return false;
        
        resourceList.RemoveData(resource);
        inventoryDic.Remove(resource.characterId);
        Save();
        
        return true;
    }

    // public void AddHeroExpWithId(int characterId, long value)
    // {
    //     if (inventoryDic.ContainsKey(characterId))
    //     {
    //         inventoryDic[characterId].AddExp(value);
    //     }
    // }
    
    public void SetCurrentCharacter(int characterId)
    {
        resourceList.currentCharacter = characterId;
        Save();
    }
    
    public CharacterResource GetCurrentCharacter()
    {
        if (inventoryDic.ContainsKey(resourceList.currentCharacter))
        {
            var characterResource = inventoryDic[resourceList.currentCharacter];
            return characterResource;
        }
        
        return null;
    }
}