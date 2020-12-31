using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIModuleStageCampaign : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    
    private CampaignMapConfig mapConfig;
    private List<CampaignStageData> _data;
    
    private CampaignStageView stageViewPrefab;

    private CampaignConfigCollection campaignCollection = null;
    private PlayerCampaign playerCampaign;
    private void Awake()
    {
        stageViewPrefab = LoadResourceController.GetCampaignStageView();
        playerCampaign = DataPlayer.GetModule<PlayerCampaign>();
        campaignCollection = LoadResourceController.GetCampaignConfigCollection();
        mapConfig = campaignCollection.GetMapCampaignConfigWithStageId(playerCampaign.GetLastStagePass());
    }

    private void Start()
    {
        UpdateView(mapConfig);
    }

    public void UpdateView(CampaignMapConfig mapConfig)
    {
        this.mapConfig = mapConfig;
        
        LoadData();
    }
    
    private void LoadData()
    {
        _data = mapConfig.stageList;
        scroller.Delegate = this;
        scroller.ReloadData();
        scroller.JumpToDataIndex(CampaignStageData.GetStageIndex(playerCampaign.GetLastStagePass()) - 1);
    }
    
    
    public void SetNextLevel()
    {
        var dataNextLevel = campaignCollection.GetNextStage(playerCampaign.GetLastStagePass());
        if (dataNextLevel != null)
        {
            DataPlayer.GetModule<PlayerCampaign>().SetLastStagePass(dataNextLevel.stage);
        }

        SceneManager.LoadScene("10.Stage");
    }
    
    public void OnClickMap()
    {
        SceneManager.LoadScene("10.Mode");
    }
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
        return 300;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        CampaignStageView cellView = scroller.GetCellView(stageViewPrefab) as CampaignStageView;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.name = "Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
        cellView.SetData(_data[dataIndex]);

        // return the cell to the scroller
        return cellView;
    }
}