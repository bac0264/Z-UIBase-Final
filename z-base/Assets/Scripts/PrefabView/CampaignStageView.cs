using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class CampaignStageView : EnhancedScrollerCellView
{
    [SerializeField] private Text stageText;
    [SerializeField] private Text stateText;
    [SerializeField] private Button playBtn;

    [SerializeField] private Transform rewardAnchor;
    [SerializeField] private Transform moneyBarAnchor;

    private List<IconView> iconViews = new List<IconView>();
    private IconView prefab = null;

    private MoneyBarView moneyBar = null;

    private CampaignStageData data = null;
    private CampaignRequireConfigCollection requireCollection = null;

    private void Awake()
    {
        prefab = LoadResourceController.GetIconView();
        requireCollection = LoadResourceController.GetCampaignRequireConfigCollection();
        playBtn.onClick.AddListener(OnClickPlay);
    }

    public void SetData(CampaignStageData data)
    {
        this.data = data;

        stageText.text = "Stage: " + data.stage.ToString();
        stateText.text = "State: " + data.GetState();

        var canFight = data.GetState() == StageState.Opening || data.GetState() == StageState.Completed;

        playBtn.interactable = canFight;

        InitOrUpdateView();
    }

    private void InitOrUpdateView()
    {
        if (moneyBar == null)
        {
            moneyBar = Instantiate(LoadResourceController.GetMoneyBarView(), moneyBarAnchor);
            moneyBar.transform.localPosition = Vector3.zero;
        }

        moneyBar.SetData(requireCollection.GetCampaignRequireData(data.stage).require.GetResource());

        int i = 0;
        for (; i < data.rewards.Length; i++)
        {
            if (i < iconViews.Count)
            {
                iconViews[i].gameObject.SetActive(true);
                iconViews[i].SetData(data.rewards[i].GetResource());
            }
            else
            {
                var view = Instantiate(prefab, rewardAnchor);
                view.SetData(data.rewards[i].GetResource());
                iconViews.Add(view);
            }
        }

        for (; i < iconViews.Count; i++)
        {
            iconViews[i].gameObject.SetActive(false);
        }
    }

    private void OnClickPlay()
    {
        var playerMoney = DataPlayer.GetModule<PlayerMoney>();
        var require = requireCollection.GetCampaignRequireData(data.stage).require.GetResource();
        bool canPlay = playerMoney.IsEnoughMoney(require);
        if (canPlay)
        {
            playerMoney.SubOne((MoneyType) require.id, require.number);
            SceneManager.LoadScene("10.Result");
        }
        else Debug.Log("not enough money");
    }
}