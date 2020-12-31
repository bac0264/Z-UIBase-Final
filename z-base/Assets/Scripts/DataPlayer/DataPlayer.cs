using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


// public enum DataPlayerType
// {
//     PlayerMoney,
//     PlayerInventory,
//     PlayerCharacter,
//     PlayerShop,
//     PlayerDailyReward,
//     PlayerGacha,
//     PlayerAds,
// }

public static class DataPlayer
{
    #region Option1
    private static readonly Dictionary<Type, object>
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
    #endregion
    
    #region Option2
    //
    // private static readonly Dictionary<DataPlayerType, object>
    //     ResgisteredModules2 = new Dictionary<DataPlayerType, object>();
    // public static T GetModule2<T>(DataPlayerType dataPlayerType)
    // {
    //     if (typeof(T).ToString().Equals(dataPlayerType.ToString()))
    //         return (T) _GetModule2(dataPlayerType);
    //     return default;
    // }
    //
    // private static object _GetModule2(DataPlayerType dataPlayerType)
    // {
    //     if (ResgisteredModules2.ContainsKey(dataPlayerType))
    //     {
    //         return ResgisteredModules2[dataPlayerType];
    //     }
    //
    //     Type moduleType = Type.GetType(dataPlayerType.ToString());
    //     
    //     var firstConstructor = moduleType.GetConstructors()[0];
    //     
    //     object module = null;
    //     
    //     if (!firstConstructor.GetParameters().Any())
    //     {
    //         module = firstConstructor.Invoke(null);
    //     }
    //     else
    //     {
    //         Debug.Log("!! Warning, Not support Constructor with params");
    //     }
    //     
    //     ResgisteredModules2.Add(dataPlayerType, module);
    //     return module;
    // }
     #endregion
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
