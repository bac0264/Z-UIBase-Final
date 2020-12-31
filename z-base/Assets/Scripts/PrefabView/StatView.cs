using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatView : IconView
{
    public void SetData(ItemStat itemStat)
    {
        icon.sprite = LoadResourceController.GetStatIcon(itemStat.statType);
        value.text = itemStat.GetLocalize();
    }
}
