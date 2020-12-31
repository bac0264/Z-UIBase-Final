using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModuleEquipmentItemView : UIModuleInventoryItemView
{
    public ItemType id;

    public override void SetupItem(ItemResource itemResource)
    {
        base.SetupItem(itemResource);
        gameObject.SetActive(itemResource != null);
    }
}
