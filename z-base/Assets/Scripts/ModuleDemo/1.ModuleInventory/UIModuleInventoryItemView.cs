using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIModuleInventoryItemView : EnhancedScrollerCellView, IPointerClickHandler
{
    // Data
    public ItemResource itemResource;

    // View
    public Image _icon;
    public Image _typeIcon;
    public Image _frame;

    public Text _level;
    public Text _amount;

    public GameObject hide;
    // Action
    public Action<ItemResource> OnRightClickEvent;

    // Setup View
    public virtual void SetupItem(ItemResource itemResource)
    {
        this.itemResource = itemResource;
        if (itemResource != null)
        {
            if (_level != null)
                _level.text = (itemResource.level + 1).ToString();

            if (_icon != null)
            {
                _icon.sprite = LoadResourceController.GetItemIcon(itemResource.id);
            }

            if (_typeIcon != null)
            {
                // _icon.sprite = LoadResourceController.GetItemIcon(itemResource.type, itemResource.id);
            }

            if (_frame != null)
            {
                _frame.sprite = LoadResourceController.GetFrameWithPriority(itemResource.GetPriority());
            }

            if (hide != null)
            {
                // if (itemResource.isEquip)
                //     hide.SetActive(true);
                // else hide.SetActive(false);
            }
        }
        else
        {

        }
    }

    public void SetData(ItemResource data, Action<ItemResource> OnRightClickEvent = null)
    {
        SetupItem(data);
        this.OnRightClickEvent = OnRightClickEvent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && (eventData.button == PointerEventData.InputButton.Right || eventData.clickCount > 0))
        {
            if (OnRightClickEvent != null && itemResource != null)
            {
                OnRightClickEvent(itemResource);
            }
        }

    }
}
