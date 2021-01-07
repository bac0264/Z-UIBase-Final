using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BattleResultProperties : WindowProperties
{
    public bool isWin;
    public int stagePlay;

    public BattleResultProperties(bool isWin, int stagePlay) 
    {
        this.isWin = isWin;
        this.stagePlay = stagePlay;
    }
}

public class UIModuleResultCampaign : AWindowController<BattleResultProperties>
{
    [SerializeField] private Text resultText;

    [SerializeField] private Button adsBtn;

    [SerializeField] private Transform rewardAnchor;

    private List<IconView> iconViews = new List<IconView>();
    private IconView prefab = null;

    private Reward[] rewards;
    private PlayerAds playerAds;
    private AdsConfigCollection adsConfig;
    private CampaignConfigCollection campaignConfig;

    protected override void Awake()
    {
        base.Awake();

        adsConfig = LoadResourceController.GetAdsConfigCollection();
        campaignConfig = LoadResourceController.GetCampaignConfigCollection();
        playerAds = DataPlayer.GetModule<PlayerAds>();
        prefab = LoadResourceController.GetIconView();

        InitButtons();
    }

    private void UpdateData()
    {
        if (Properties.isWin)
        {
            var stage = DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass();
            if (Properties.stagePlay == stage)
            {
                var dataNextLevel = campaignConfig.GetNextStage(stage);
                if (dataNextLevel != null)
                    DataPlayer.GetModule<PlayerCampaign>().SetLastStagePass(dataNextLevel.stage);
            }
        }
    }

    private void InitButtons()
    {
        adsBtn.onClick.AddListener(OnClickAds);
    }

    protected override void OnPropertiesSet()
    {
        rewards = campaignConfig.GetStageCampaignWithStageId(Properties.stagePlay).rewards;

        var message = Properties.isWin ? "Victory" : "Lose";

        resultText.text = message;
        rewardAnchor.gameObject.SetActive(resultText.text.Equals("Victory"));

        InitOrUpdateView();

        UpdateData();
    }
    
    private void InitOrUpdateView()
    {
        int i = 0;

        if (rewards == null) return;

        for (; i < rewards.Length; i++)
        {
            if (i < iconViews.Count)
            {
                iconViews[i].SetData(rewards[i].GetResource());
                iconViews[i].gameObject.SetActive(true);
            }
            else
            {
                var view = Instantiate(prefab, rewardAnchor);
                view.SetData(rewards[i].GetResource());
                iconViews.Add(view);
            }
        }

        for (; i < iconViews.Count; i++)
        {
            iconViews[i].gameObject.SetActive(false);
        }
    }


    private void OnClickAds()
    {
        void onSuccess()
        {
            var adsData = adsConfig.GetAdsConfigData(playerAds.GetAdsCount());
            UIFrame.Instance.OpenWindow(WindowIds.UIShowReward, new ShowRewardProperties(adsData.Rewards));
            playerAds.AddAds(1);
        }

        IronSourceManager.instance.ShowRewardedVideo(onSuccess);
    }
}