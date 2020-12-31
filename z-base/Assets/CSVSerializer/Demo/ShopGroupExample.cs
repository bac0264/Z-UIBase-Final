using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

public class ShopGroupExample : ScriptableObject
{
    [System.Serializable]
    public class Resource
    {
        public int res_type;
        public int res_id;
        public int res_number;
    }
    [System.Serializable]
    public class Reward
    {
        public int money_type;
        public int money_value;
    }

    [System.Serializable]
    public class RewardStock
    {
        public int id;
        public int rate;
        public int stock;
        public Resource[] resources;
        public Reward reward;
    }

    [System.Serializable]
    public class Shop
    {
        public int shop_type;
        public int group_rate;
        public RewardStock[] rewardStocks;
    }
    
    [System.Serializable]
    public class ShopGroup
    {
        public int group_id;
        public int[] stage_min;
        public int stage_max;
        public Shop[] shops;
    }

    public ShopGroup[] shopGroups;
    
}

#if UNITY_EDITOR
public class GroupPostprocessor : AssetPostprocessor
{
    static string path = "/shop_group.csv";
    static string subPath = "/shop";
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        string fullPath = subPath + path;
        
        Setup(path, fullPath, importedAssets);
    }

    static void Setup(string path, string fullPath, string[] importedAssets)
    {
        foreach (string str in importedAssets)
        {
            if (str.IndexOf(path) != -1)
            {
                var assetPath = str.Replace(path, fullPath);
                
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                
                string assetfile = assetPath.Replace(".csv", ".asset");
                
                ShopGroupExample gm = AssetDatabase.LoadAssetAtPath<ShopGroupExample>(assetfile);
                if (gm == null)
                {
                    gm = ScriptableObject.CreateInstance<ShopGroupExample>();
                    AssetDatabase.CreateAsset(gm, assetfile);
                }
                
                gm.shopGroups = CSVSerializer.Deserialize<ShopGroupExample.ShopGroup>(data.text);
                
                EditorUtility.SetDirty(gm);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + assetPath);
#endif
            }
        }
    }
}
#endif
