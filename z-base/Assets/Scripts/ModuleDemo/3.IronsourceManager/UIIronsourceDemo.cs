﻿using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIIronsourceDemo : AWindowController
{
    [SerializeField] private Text rewardTxt = null;
    
    [SerializeField] private Button rewardBtn = null;
    [SerializeField] private Button interBtn = null;

    private AdsConfigCollection adsConfigCollection;
    private PlayerAds playerAds;
    
    private void Awake()
    {
        adsConfigCollection = LoadResourceController.GetAdsConfigCollection();
        playerAds = DataPlayer.GetModule<PlayerAds>();
        InitButtons();
    }

    private void InitButtons()
    {
        rewardBtn.onClick.AddListener(OnClickRewardVideo);
        interBtn.onClick.AddListener(OnClickInterVideo);
    }
    
    private void OnClickRewardVideo()
    {
        void onSuccess()
        {
            var adsData = adsConfigCollection.GetAdsConfigData(playerAds.GetAdsCount());
            UIFrame.Instance.OpenWindow(WindowIds.UIShowReward, new ShowRewardProperties(adsData.Rewards));
            playerAds.AddAds(1);
        }
        
        IronSourceManager.instance.ShowRewardedVideo(onSuccess);
    }
    
    private void OnClickInterVideo()
    {
        IronSourceManager.instance.ShowInterstitial();
    }
}
