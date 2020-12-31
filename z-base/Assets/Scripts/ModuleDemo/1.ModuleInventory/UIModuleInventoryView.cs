using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EnhancedScrollerDemos.NestedScrollers;
using EnhancedUI.EnhancedScroller;

public class UIModuleInventoryView : MonoBehaviour, IEnhancedScrollerDelegate
{
    private const int COLUMN = 5;

    private List<InventoryItemContainer> _data;

    private List<ItemResource> itemFilterDatas;

    private List<ItemResource> itemDatas;

    public EnhancedScroller masterScroller;

    public EnhancedScrollerCellView masterCellViewPrefab;

    public Action<ItemResource> OnRightClickEvent;

    void Start()
    {
        masterScroller.Delegate = this;
        itemDatas = DataPlayer.GetModule<PlayerInventory>().GetItemResources();
        LoadData();
    }

    /// <summary>
    /// Populates the data with a lot of records
    /// </summary>
    private void LoadData()
    {
        int RowCount = COLUMN;
        _data = new List<InventoryItemContainer>();

        int j = 0;
        int m = itemDatas.Count % RowCount;
        int n = itemDatas.Count / RowCount;
        if (m > 0)
            n += 1;

        for (var i = 0; i < n + 1; i++)
        {
            var masterData = new InventoryItemContainer()
            {
                normalizedScrollPosition = 0,
                childData = new List<ItemResource>()
            };
            _data.Add(masterData);

            for (; j < (RowCount * (i + 1)) && j < itemDatas.Count; j++)
            {
                masterData.childData.Add(itemDatas[j]);
            }
        }
        // tell the scroller to reload now that we have the data
        masterScroller.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        ReloadData();
    }

    private void UpdateData()
    {
        _data.Clear();
        
        int RowCount = COLUMN;
        int m = itemDatas.Count % RowCount;
        int n = itemDatas.Count / RowCount;
        if (m > 0)
            n += 1;
        
        int j = 0;
        for (var i = 0; i < n + 1; i++)
        {
            var masterData = new InventoryItemContainer()
            {
                normalizedScrollPosition = 0,
                childData = new List<ItemResource>()
            };
            _data.Add(masterData);

            for (; j < (RowCount * (i + 1)) && j < itemDatas.Count; j++)
            {
                masterData.childData.Add(itemDatas[j]);
            }
        }
    }
    public void ReloadData()
    {
        StartCoroutine(_ReloadData());
    }

    IEnumerator _ReloadData()
    {
        yield return new WaitForEndOfFrame();     
        yield return new WaitForEndOfFrame();   
        yield return new WaitForEndOfFrame();   
        yield return new WaitForEndOfFrame();   
        UpdateData();
        masterScroller.ReloadData();
    }

    public void ReloadDataWithInventoryId(int inventoryId)
    {
        StartCoroutine(_ReloadDataAndJumping(inventoryId));
    }

    IEnumerator _ReloadDataAndJumping(int inventoryId)
    {
        UpdateData();
        var index = 0;
        for(int i = 0; i < itemDatas.Count; i++)
        {
            if(itemDatas[i].inventoryId == inventoryId)
            {
                index = i;
                break;
            }
        }

        yield return new WaitForEndOfFrame();
        
        masterScroller.ReloadData();

        var jumpIndex = index / COLUMN;
        jumpIndex = jumpIndex == 0 ? jumpIndex : jumpIndex - 1;

        masterScroller.JumpToDataIndex(jumpIndex);
    }
    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 150;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {

        UIModuleInventoryMasterCellView masterCellView = scroller.GetCellView(masterCellViewPrefab) as UIModuleInventoryMasterCellView;
        masterCellView.SetData(_data[dataIndex], OnRightClickEvent);
        return masterCellView;
    }
    #endregion
}

public class InventoryItemContainer
{
    public float normalizedScrollPosition;

    public List<ItemResource> childData;
}
