using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

[System.Serializable]
public class ShopAllDataCollection : ScriptableObject
{
    public ShopBaseDictionary shopDict;


    public void AddConsumableIds(Action<string> addConsumableAction)
    {
        foreach (var data in shopDict)
        {
            data.Value.AddConsumableIds(addConsumableAction);
        }
    }
    public ShopBase GetShopWithType(Type type)
    {
        if (shopDict.ContainsKey(type.ToString()))
            return shopDict[type.ToString()];
        Debug.Log($"{type} null");
        return null;
    }
    
#if UNITY_EDITOR
    public void SetupAllData()
    {
        shopDict = GetAllVariables();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    // This method works only in Editor
    private ShopBaseDictionary GetAllVariables()
    {
        string[] guilds = AssetDatabase.FindAssets("t:ShopBase");
        var shopDict = new ShopBaseDictionary();
        
        ShopBase[] vars = new ShopBase[guilds.Length];
        for (int i = 0; i < guilds.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guilds[i]);
            vars[i] = AssetDatabase.LoadAssetAtPath<ShopBase>(path);
            shopDict.Add(vars[i].GetType().ToString(), vars[i]);
        }
        return shopDict;
    }
#endif
}

[System.Serializable]
public abstract class ShopBase : ScriptableObject
{
    public abstract void AddConsumableIds(Action<string> addConsumableAction);
}

[System.Serializable]
public class ShopBaseDictionary : SerializableDictionary<string, ShopBase> { }
