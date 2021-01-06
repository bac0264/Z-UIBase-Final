using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIModuleMapCampaign : AWindowController<CampaignModeConfig>
{
    [SerializeField] private Transform mapViewAnchor;
    [SerializeField] private Snap snap;
    [SerializeField] private Transform buttonGo;
    [SerializeField] private Transform buttonLock;

    private CampaignModeConfig mode;

    private List<CampaignMapView> mapViews = new List<CampaignMapView>();
    private CampaignMapView prefab = null;

    protected override void Awake()
    {
        base.Awake();
        // mode = LoadResourceController.GetCampaignConfigCollection()
        //     .GetModeCampaignWithId(DataPlayer.GetModule<PlayerCampaign>().GetModePick());
        // InitOrUpdateView(mode);
        prefab = LoadResourceController.GetCampaignMapView();
    }

    protected override void OnPropertiesSet()
    {
        mode = Properties;
        InitOrUpdateView();
    }

    public void InitOrUpdateView()
    {
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
        
        CloseWindow();
        
        UIFrame.Instance.OpenWindow(WindowIds.StageCampaign, map);
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