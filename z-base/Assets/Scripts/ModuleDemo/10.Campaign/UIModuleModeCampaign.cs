using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedScrollerDemos.MultipleCellTypesDemo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIModuleModeCampaign : MonoBehaviour
{
    [SerializeField] private Transform campaignViewAnchor;
    [SerializeField] private Snap snap;
    
    [SerializeField] private Transform buttonGo;
    [SerializeField] private Transform buttonLock;
    
    private CampaignConfigCollection collection = null;
    
    private List<CampaignModeView> campaignViews = new List<CampaignModeView>();
    private CampaignModeView prefab = null;

    private void Awake()
    {
        collection = LoadResourceController.GetCampaignConfigCollection();
        prefab = LoadResourceController.GetCampaignModeView();

        
        InitOrUpdateView();
        
    }

    private void InitOrUpdateView()
    {
        int i = 0;
        for (; i < collection.worldConfig.modeConfigList.Count; i++)
        {
            if (i < campaignViews.Count)
            {
                campaignViews[i].SetupView(collection.worldConfig.modeConfigList[i]);
            }
            else
            {
                var view = Instantiate(prefab, campaignViewAnchor);
                view.SetupView(collection.worldConfig.modeConfigList[i]);
                campaignViews.Add(view);
                snap.AddRectTransform(view.GetComponent<RectTransform>());
            }
        }
        snap.SetupSnap(RefreshUI);
    }

    public void OnClickGo()
    {
        var modeIndex = snap.GetIndex() + 1;

        var mode = collection.GetModeCampaignWithId(modeIndex);
        DataPlayer.GetModule<PlayerCampaign>().SetModePick(modeIndex);
        SceneManager.LoadScene("10.Map");
        // uiModuleCampaign.InitOrUpdateView(mode);
        // uiModuleCampaign.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        var modeIndex = snap.GetIndex() + 1;

        var mode = collection.GetModeCampaignWithId(modeIndex);
        
        if (mode == null)
        {
            Debug.Log("comming soon");
            buttonGo.gameObject.SetActive(false);
            buttonLock.gameObject.SetActive(true);
            return;
        }
        
        var state = mode.GetStateWithModeId();

        if (state == ModeState.Completed || state == ModeState.Opening)
        {
            buttonGo.gameObject.SetActive(true);
            buttonLock.gameObject.SetActive(false);
        }
        else 
        {
            buttonGo.gameObject.SetActive(false);
            buttonLock.gameObject.SetActive(true);
        }
    }
}
