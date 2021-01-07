using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


public enum DataPlayerType
{
    PlayerMoney,
    PlayerInventory,
    PlayerCharacter,
    PlayerShop,
    PlayerDailyReward,
    PlayerGacha,
    PlayerAds,
}

public static class DataPlayer
{
    [JsonProperty("0")] private static readonly Dictionary<Type, object>
        ResgisteredModules = new Dictionary<Type, object>();
    
    public static T GetModule<T>()
    {
        return (T) _GetModule(typeof(T));
    }

    private static object _GetModule(Type moduleType)
    {
        if (ResgisteredModules.ContainsKey(moduleType))
        {
            return ResgisteredModules[moduleType];
        }

        var firstConstructor = moduleType.GetConstructors()[0];
        
        object module = null;
        
        if (!firstConstructor.GetParameters().Any())
        {
            module = firstConstructor.Invoke(null);
        }
        else
        {
            Debug.Log("!! Warning, Not support Constructor with params");
        }
        
        ResgisteredModules.Add(moduleType, module);
        return module;
    }

    public static void SendDataToServer()
    {
        // foreach (var suit in (DataPlayerType[]) Enum.GetValues(typeof(DataPlayerType)))
        // {
        //     SetupData(suit);
        // }
        //
        // PlayerPrefs.SetString(KeyUtils.ALL_DATA, JsonConvert.SerializeObject(ResgisteredModules));
        //
        // Debug.Log(PlayerPrefs.GetString(KeyUtils.ALL_DATA));

        //GetDataToServer();
    }
    
    public static void GetDataToServer()
    {
        //
        // var data = JsonConvert.DeserializeObject<Dictionary<Type, object>>(PlayerPrefs.GetString(KeyUtils.ALL_DATA));
        //
        // if (data == null) return;
        //
        // Debug.Log(PlayerPrefs.GetString(KeyUtils.ALL_DATA));
        //
        // foreach (var _data in data)
        // {
        //     Debug.Log("Type: "+_data.Key +", Value: "+_data.Value);
        //     var method = _data.GetType().GetMethod("Save");
        //     
        //     method?.Invoke(null, null);
        // }
    }
    private static void SetupData(DataPlayerType dataPlayerType)
    {
        Type moduleType = Type.GetType(dataPlayerType.ToString());
        if (moduleType == null) return;
        
        if (ResgisteredModules.ContainsKey(moduleType))
        {
            return;
        }
        
        var firstConstructor = moduleType.GetConstructors()[0];
        
        object module = null;
        
        if (!firstConstructor.GetParameters().Any())
        {
            module = firstConstructor.Invoke(null);
        }
        else
        {
            Debug.Log("!! Warning, Not support Constructor with params");
        }
        
        ResgisteredModules.Add(moduleType, module);
    }
}

[System.Serializable]
public class DataSave<T>
{
    [JsonProperty("0")] public List<T> dataList;

    public DataSave()
    {
        dataList = new List<T>();
    }

    public virtual void AddData(T t)
    {
        dataList.Add(t);
    }

    public virtual void RemoveData(T t)
    {
        dataList.Remove(t);
    }
}
