using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ShopRawPackCollection : ScriptableObject
{
    public RawPackInfo[] dataGroups;

    public List<string> GetShopRawPackProductIds()
    {
        var list = new List<string>();
        for (int i = 0; i < dataGroups.Length; i++)
        {
            list.Add(dataGroups[i].packnameIap);
        }

        return list;
    }
}

// #if UNITY_EDITOR
// public class ShopRawPackProcessor : BasePostProcessor
// {
//     // public ShopRawPackProcessor()
//     // {
//     //     if (isRun == false)
//     //     {
//     //         assetfile = PathUtils.shopRawPack;
//     //         classScriptObject = "ShopRawPackCollection";
//     //         classData = "RawPackInfo";
//     //         isRun = true;
//     //     }
//     // }
// }
// #endif
