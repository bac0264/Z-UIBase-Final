using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIModuleDailyRewardView : MonoBehaviour
{
    [SerializeField] private Transform viewAnchor;

    private List<DailyRewardView> dailyRewardViews = new List<DailyRewardView>();
    private DailyRewardCollection dailyRewardCollection = null;

    private DailyRewardView prefab = null;
    private TimeBarView timeBarView = null;

    private List<Reward> rewardList = new List<Reward>();

    private PlayerDailyReward playerDailyReward = null;
    private void Start()
    {
        playerDailyReward = DataPlayer.GetModule<PlayerDailyReward>();
        prefab = LoadResourceController.GetDailyRewardView();
        dailyRewardCollection = LoadResourceController.GetDailyReward();
        TimeManager.Ins.UpdateTime(UpdateView);
    }

    public void UpdateView()
    {
        if (timeBarView == null)
        {
            timeBarView = Instantiate(LoadResourceController.GetTimeBarView(), transform);
        }

        int i = 0;
        for (; i < dailyRewardCollection.dataGroups.Length && i < dailyRewardViews.Count; i++)
        {
            var view = dailyRewardViews[i];
            view.SetupView(dailyRewardCollection.dataGroups[i], UpdateTimeView,
                dailyRewardCollection.dataGroups.Length);
        }

        for (; i < dailyRewardCollection.dataGroups.Length; i++)
        {
            var view = Instantiate(prefab, viewAnchor);
            view.SetupView(dailyRewardCollection.dataGroups[i], UpdateTimeView,
                dailyRewardCollection.dataGroups.Length);
            dailyRewardViews.Add(view);
        }
    }

    private void UpdateTimeView(int nextDay, long currentTime, bool completed)
    {
        StartCoroutine(_updateTimeView(nextDay, currentTime, completed));
    }

    IEnumerator _updateTimeView(int nextDay, long currentTime, bool completed)
    {
        yield return new WaitForEndOfFrame();
        if (completed)
        {
            timeBarView.StopCountDown();
        }
        else
        {
            if (nextDay >= dailyRewardViews.Count)
            {
                timeBarView.gameObject.SetActive(false);
            }
            else
            {
                timeBarView.transform.SetParent(dailyRewardViews[nextDay].transform);
                timeBarView.SetData(currentTime, SetupFinishDay);
                timeBarView.transform.localPosition = Vector3.zero;
                timeBarView.gameObject.SetActive(true);
            }
        }
    }

    private void SetupFinishDay()
    {
        playerDailyReward.SetNextDay();
        UpdateView();
    }

    public void OnClickClaim()
    {
        rewardList.Clear();
        for (int i = 0; i < playerDailyReward.GetCurrentDay(); i++)
        {
            if (!dailyRewardViews[i].IsReceived())
            {
                dailyRewardViews[i].ClaimReward();
                rewardList.AddRange(dailyRewardViews[i].GetRewards());
            }
        }

        if (rewardList.Count == 0) return;
        
        WindowManager.Instance.ShowWindowWithData(WindowType.UI_SHOW_REWARD, Reward.FixDuplicateRewards(rewardList));
    }
}