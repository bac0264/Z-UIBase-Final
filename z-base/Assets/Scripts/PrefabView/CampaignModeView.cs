using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignModeView : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    [SerializeField] private Text lengthMapTxt;
    [SerializeField] private Text describeMapTxt;
    [SerializeField] private CampaignModeConfig mode;

    public void SetupView(CampaignModeConfig mode)
    {
        this.mode = mode;

        icon.sprite = LoadResourceController.GetCampaignModeIcon(mode.modeId);

        describeMapTxt.text = "Describe mode: " + mode.modeId;
        lengthMapTxt.text = "Length: " + mode.mapList.Count;
    }
}
