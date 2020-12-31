using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ShopRawPackCollection : ShopBase
{
    public RawPackInfo[] dataGroups;

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
