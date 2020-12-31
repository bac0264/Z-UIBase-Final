using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardView : MonoBehaviour
{
    [SerializeField] private Transform rewardAnchor = null;

    [SerializeField] private Text dayText = null;
    [SerializeField] private Text nameText = null;

    [SerializeField] private GameObject highLight = null;
    [SerializeField] private GameObject receivedCard = null;
    [SerializeField] private GameObject openedCard = null;

    private List<IconView> iconViews = new List<IconView>();
    private DailyRewardData dailyRewardData;
    private IconView prefab = null;

    private Action<int, long, bool> timeCountDownAction = null;

    private int maxDay;

    private void Awake()
    {
        prefab = LoadResourceController.GetIconView();
    }

    public void SetupView(DailyRewardData dailyRewardData, Action<int, long, bool> timeCountDownAction = null,
        int maxDay = 0)
    {
        this.dailyRewardData = dailyRewardData;
        this.timeCountDownAction = timeCountDownAction;
        this.maxDay = maxDay;

        SetupIconViews();
        SetupUI();
    }

    private void SetupIconViews()
    {
        int i = 0;
        for (; i < dailyRewardData.rewards.Length && i < iconViews.Count; i++)
        {
            iconViews[i].SetData(dailyRewardData.rewards[i].GetResource());
        }

        for (; i < dailyRewardData.rewards.Length; i++)
        {
            var iconView = Instantiate(prefab, rewardAnchor);
            iconView.SetData(dailyRewardData.rewards[i].GetResource());
            iconViews.Add(iconView);
        }
    }

    private void SetupUI()
    {
        nameText.text = "Day: " + dailyRewardData.id;
        dayText.text = dailyRewardData.id.ToString();

        var isReceived = dailyRewardData.IsReceived();
        var isReceivable = dailyRewardData.IsReceivable();
        var isNextDay = dailyRewardData.IsNextDay();
        var isCompleted = dailyRewardData.id == maxDay && isReceived;

        receivedCard.SetActive(isReceived);
        openedCard.SetActive(!isReceived);
        highLight.SetActive(!isReceived && isReceivable);

        if (isNextDay)
        {
            var lastTimeOnline = DataPlayer.GetModule<PlayerTime>().GetLastTimeOnline();
            timeCountDownAction?.Invoke(dailyRewardData.id - 1, lastTimeOnline, isCompleted);
        }
    }

    public Reward[] GetRewards()
    {
        return dailyRewardData.rewards;
    }

    public bool IsReceived()
    {
        return dailyRewardData.IsReceived();
    }
    
    public void ClaimReward()
    {
        dailyRewardData.Claim();
        SetupUI();
    }
}