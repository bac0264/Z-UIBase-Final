using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModuleInventoryHandle : MonoBehaviour
{
    [SerializeField] private UIModuleEquipmentView equipmentView;
    [SerializeField] private UIModuleInventoryView inventoryView;
    [SerializeField] private UIModuleCharacterInfoView characterInfoView;

    private PlayerInventory playerInventory = null;
    private PlayerCharacter playerCharacter = null;
    void Awake()
    {
        equipmentView.OnRightClickEvent = Unequip;
        inventoryView.OnRightClickEvent = Equip;
        playerInventory = DataPlayer.GetModule<PlayerInventory>();
        playerCharacter = DataPlayer.GetModule<PlayerCharacter>();
    }

    private void OnValidate()
    {
        if (equipmentView == null) equipmentView = transform.GetComponentInChildren<UIModuleEquipmentView>();
        if (inventoryView == null) inventoryView = transform.GetComponentInChildren<UIModuleInventoryView>();
    }

    private void Unequip(ItemResource item)
    {
        if (equipmentView.RemoveToUnequip(item, ()=>inventoryView.ReloadData()))
        {
            characterInfoView.UpdateCharacterView();
            playerInventory.Save();
        }
    }

    private void Equip(ItemResource item)
    {
        var inventoryId = item.inventoryId;
        if (equipmentView.AddToEquip(item, () => inventoryView.ReloadDataWithInventoryId(inventoryId)))
        {
            characterInfoView.UpdateCharacterView();
            playerInventory.Save();
        }
    }

    public void RefreshUI()
    {
        var index = playerCharacter.GetCurrentCharacter().characterId + 1;
        if (index > 5) index = 0;
        playerCharacter.SetCurrentCharacter(index);
        equipmentView.RefreshUI();
        inventoryView.ReloadData();
        characterInfoView.UpdateCharacterView();
    }
}
