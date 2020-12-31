using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;
using Zitga.Localization;
using Zitga.Localization.Tutorials;

public class DailyQuestView : EnhancedScrollerCellView
{
    [SerializeField] private Text id;
    [SerializeField] private Text type;

    [SerializeField] private Button claimBtn;

    [SerializeField] private Image progress;
    [SerializeField] private Text progressTxt;

    [SerializeField] private Transform rewardAnchor;

    private DailyQuestData questData;
    private QuestProgress questProgress;

    private List<IconView> iconViews = new List<IconView>();
    private IconView prefab = null;

    private int index;
    
    private Action<int> reloadData = null;
    private void Awake()
    {
        prefab = LoadResourceController.GetIconView();
        claimBtn.onClick.AddListener(OnCickClaim);
    }

    private void OnCickClaim()
    {
        questProgress.Claim();
        reloadData?.Invoke(index);
        
        Reward.RecieveManyRewards(questData.rewards);
        WindowManager.Instance.ShowWindowWithData<Reward[]>(WindowType.UI_SHOW_REWARD, questData.rewards);
        
    }

    public void SetData(DailyQuestData questData, Action<int> reloadData, int index)
    {
        this.questData = questData;
        this.questProgress = questData.GetProgress();
        this.reloadData = reloadData;
        this.index = index;
        
        progressTxt.text = questProgress.progress + "/" + questData.required;
        type.text = ((QuestType) questData.type).ToString();
        id.text = "id: " + questData.id;
        
        var fill = (float) questProgress.progress / questData.required;
        progress.fillAmount = fill;
        
        claimBtn.interactable = questProgress.GetState() == QuestState.Done;
        
        InitOrUpdateView();
    }

    private void InitOrUpdateView()
    {
        int i = 0;
        for (; i < questData.rewards.Length; i++)
        {
            if (i < iconViews.Count)
            {
                iconViews[i].gameObject.SetActive(true);
                iconViews[i].SetData(questData.rewards[i].GetResource());
            }
            else
            {
                var view = Instantiate(prefab, rewardAnchor);
                view.SetData(questData.rewards[i].GetResource());
                iconViews.Add(view);
            }
        }

        for (; i < iconViews.Count; i++)
        {
            iconViews[i].gameObject.SetActive(false);
        }
    }
}