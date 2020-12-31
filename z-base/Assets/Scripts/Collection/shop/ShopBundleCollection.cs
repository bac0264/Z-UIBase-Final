using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class BundlePackInfo : RawPackInfo
{
}

[System.Serializable]
public class ShopBundleCollection : ShopBase
{
    public BundlePackInfo[] dataGroups;
    public override void AddConsumableIds(Action<string> addConsumableAction)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            var packNameId = dataGroups[i].packnameIap;
            if (!packNameId.Equals("") && !packNameId.Equals(" "))
            {
                addConsumableAction?.Invoke(packNameId);
            }
        }
    }
}
