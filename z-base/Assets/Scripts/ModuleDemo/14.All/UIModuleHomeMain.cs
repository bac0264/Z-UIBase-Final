using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;

public class UIModuleHomeMain : AWindowController
{
    public void Inventory()
    {
        UIFrame.Instance.OpenWindow(WindowIds.Inventory);
    }

    public void ItemToolTip()
    {
        UIFrame.Instance.OpenWindow(WindowIds.ItemToolTip);
    }

    public void Ads()
    {
        UIFrame.Instance.OpenWindow(WindowIds.Ads);
    }

    public void Shop()
    {
        UIFrame.Instance.OpenWindow(WindowIds.Shop);
    }

    public void DailyReward()
    {
        UIFrame.Instance.OpenWindow(WindowIds.DailyReward);
    }

    public void DailyQuest()
    {
        UIFrame.Instance.OpenWindow(WindowIds.DailyQuest);
    }

    public void Campaign()
    {
        var mapConfig = LoadResourceController.GetCampaignConfigCollection()
            .GetMapCampaignConfigWithStageId(DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass());
        UIFrame.Instance.OpenWindow(WindowIds.StageCampaign, mapConfig);
    }
    
    public void Gacha()
    {
        UIFrame.Instance.OpenWindow(WindowIds.Gacha);
    }

    public void Setting()
    {
        UIFrame.Instance.OpenWindow(WindowIds.Setting);
    }
    
    public void GiftCode()
    {
        UIFrame.Instance.OpenWindow(WindowIds.GiftCode);
    }

    public void SendToServer()
    {
        DataPlayer.SendDataToServer();
    }
}
