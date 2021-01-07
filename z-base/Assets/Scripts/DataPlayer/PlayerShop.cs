using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerShop
{
    [JsonProperty("player_shop")]
    private DataSaveShop dataShop;

    public PlayerShop()
    {
        Load();
    }
    
    public void Save()
    {
        Debug.Log("shop");
        PlayerPrefs.SetString(KeyUtils.SHOP_DATA, JsonConvert.SerializeObject(dataShop));
    }

    public void Load()
    {
        dataShop = JsonConvert.DeserializeObject<DataSaveShop>(PlayerPrefs.GetString(KeyUtils.SHOP_DATA));
        if (dataShop == null)
        {
            dataShop = new DataSaveShop();
            Save();
        }
    }
    
    /// <summary>
    /// Add quantity of pack bought.
    /// </summary>
    /// <param name="type"> Shop Type</param>
    /// <param name="idPack"></param>
    public void AddBought(ShopEnum type, int idPack)
    {
        dataShop.AddBought(type, idPack);
        Save();
    }

    /// <summary>
    /// Get quantity of bought count.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="idPack"></param>
    /// <returns></returns>
    public int GetBoughtCount(ShopEnum type, int idPack)
    {
        return dataShop.GetBoughtCount(type, idPack);
;    }

    /// <summary>
    /// Can purchase if true and else.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="idPack"></param>
    /// <param name="stock"></param>
    /// <returns></returns>
    public bool IsAvailableForBuying(ShopEnum type, int idPack, int stock)
    {
        return dataShop.IsAvailableForBuying(type, idPack, stock);
    }
}

public enum ShopEnum
{
    RAW_PACK = 0,
    RAW_PACK_FREE = 1,
    BUNDLE = 2,
}

[Serializable]
public class DataSaveShop
{
    [JsonIgnore]
    private Dictionary<ShopEnum, Dictionary<int, int>> shopDic = new Dictionary<ShopEnum, Dictionary<int, int>>();
 
    [JsonProperty("0")] public Dictionary<int, int> rawPackFreeCount = new Dictionary<int, int>();
    [JsonProperty("1")] public Dictionary<int, int> bundlePackBoughtCount= new Dictionary<int, int>();
    [JsonProperty("2")] public Dictionary<int, int> rawPackBoughtCount = new Dictionary<int, int>();

    public void UpdateShopDict()
    {
        if (shopDic.Count == 0)
        {
            shopDic.Add(ShopEnum.RAW_PACK, rawPackBoughtCount);
            shopDic.Add(ShopEnum.BUNDLE, bundlePackBoughtCount);
            shopDic.Add(ShopEnum.RAW_PACK_FREE, rawPackFreeCount);
        }
    }
    
    public void AddBought(ShopEnum type, int id)
    {
        UpdateShopDict();
        var data = shopDic[type];
        if (data.ContainsKey(id))
        {
            data[id] += 1;
        }
        else
        {
            data.Add(id, 1);
        }
    }

    public int GetBoughtCount(ShopEnum type, int id)
    {
        UpdateShopDict();
        var data = shopDic[type];
        if (data.ContainsKey(id))
        {
            return data[id];
        }
        else
        {
            data.Add(id, 0);
            return data[id];
        }
    }

    public bool IsAvailableForBuying(ShopEnum type, int id, int stock)
    {
        UpdateShopDict();
        if (stock == -1) return true;
        var data = shopDic[type];
        return GetBoughtCount(type, id) < stock;
    }
    
}
