using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIModuleMapCampaign : MonoBehaviour
{
    [SerializeField] private Transform mapViewAnchor;
    [SerializeField] private Snap snap;
    [SerializeField] private Transform buttonGo;
    [SerializeField] private Transform buttonLock;

    private CampaignModeConfig mode;
    
    private List<CampaignMapView> mapViews = new List<CampaignMapView>();
    private CampaignMapView prefab = null;

    private void Awake()
    {
        mode = LoadResourceController.GetCampaignConfigCollection()
            .GetModeCampaignWithId(DataPlayer.GetModule<PlayerCampaign>().GetModePick());
        InitOrUpdateView(mode);
    }

    public void InitOrUpdateView(CampaignModeConfig mode)
    {
        this.mode = mode;
        if (prefab == null) prefab = LoadResourceController.GetCampaignMapView();
        
        int i = 0;
        for (; i < mode.mapList.Count; i++)
        {
            if (i < mapViews.Count)
            {
                mapViews[i].SetupView(mode.mapList[i]);
            }
            else
            {
                var view = Instantiate(prefab, mapViewAnchor);
                view.SetupView(mode.mapList[i]);
                mapViews.Add(view);
                snap.AddRectTransform(view.GetComponent<RectTransform>());
            }
        }
        snap.SetupSnap(RefreshUI);
    }

    public void OnClickGo()
    {
        var mapId = snap.GetIndex() + 1;

        var map = mode.GetMapWithId(mapId);
        
        // stageCampaign.gameObject.SetActive(true);
        // stageCampaign.UpdateView(map);
        SceneManager.LoadScene("10.Stage");
        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        var mapIndex = snap.GetIndex() + 1;

        var map = mode.GetMapWithId(mapIndex);
        if (map == null)
        {
            buttonGo.gameObject.SetActive(false);
            buttonLock.gameObject.SetActive(true);
            return;
        }
        
        var state = map.GetState();

        if (state == MapState.Completed || state == MapState.Opening)
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