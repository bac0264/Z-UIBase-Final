using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignMapView : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    [SerializeField] private Text mapIdText;
    [SerializeField] private Text statusText;
    
    [SerializeField] private CampaignMapConfig map;

    public void SetupView(CampaignMapConfig map)
    {
        this.map = map;

        statusText.text =  map.GetState().ToString();

        icon.sprite = LoadResourceController.GetCampaignMapIcon(map.mapId);
        
        mapIdText.text = "Map: " + map.mapId;
    }
}
