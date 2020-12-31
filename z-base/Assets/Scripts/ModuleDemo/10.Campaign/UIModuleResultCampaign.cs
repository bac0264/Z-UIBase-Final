using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIModuleResultCampaign : BaseWindowGeneric<Reward[]>
{
    [SerializeField] private Text resultText;
    
    [SerializeField] private Button adsBtn;

    [SerializeField] private Transform rewardAnchor;

    private List<IconView> iconViews = new List<IconView>();
    private IconView prefab = null;

    private Reward[] rewards;
    private PlayerAds playerAds;
    private AdsConfigCollection adsConfig;
    public override void Awake()
    {
        base.Awake();
        
        adsConfig = LoadResourceController.GetAdsConfigCollection();
        playerAds = DataPlayer.GetModule<PlayerAds>();
        prefab = LoadResourceController.GetIconView();

        InitButtons();
        CheckWinLose();
    }

    public override void OnHide()
    {
        OnClickHome();
    }

    private void CheckWinLose()
    {
        bool isWin = UnityEngine.Random.Range(0, 2) % 2 == 1;
        var message = isWin ? "Victory" : "Lose";

        if (isWin)
        {
            var stage = DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass();
            {
                var dataNextLevel = LoadResourceController.GetCampaignConfigCollection()
                    .GetNextStage(stage);
                if (dataNextLevel != null)
                {
                    DataPlayer.GetModule<PlayerCampaign>().SetLastStagePass(dataNextLevel.stage);
                    SetupData(dataNextLevel.rewards, message);
                }
            }
        }
        else
        {
            SetupData(null, message);
        }
    }
    private void InitButtons()
    {
        adsBtn.onClick.AddListener(OnClickAds);
    }
    
    public override void SetupData(Reward[] _data1 = default, string message = null, Action noCallBack = null,
        Action yesCallBack = null)
    {
        rewards = _data1;
        
        resultText.text = message;
        rewardAnchor.gameObject.SetActive(resultText.text.Equals("Victory"));
        
        InitOrUpdateView();
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
    

    private void OnClickHome()
    {
        SceneManager.LoadScene("10.Stage");
    }

    private void OnClickAds()
    {
        void onSuccess()
        {
            var adsData = adsConfig.GetAdsConfigData(playerAds.GetAdsCount());
            WindowManager.Instance.ShowWindowWithData<Reward[]>(WindowType.UI_SHOW_REWARD, adsData.Rewards);
            playerAds.AddAds(1);
        }

        IronSourceManager.instance.ShowRewardedVideo(onSuccess);
    }
}