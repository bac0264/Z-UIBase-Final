using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowReward : BaseWindowGeneric<Reward[]>
{
    [SerializeField] private Transform rewardAnchor = null;
    private Reward[] rewards = null;

    private IconView prefab = null;
    private List<IconView> iconViews = null;
    
    // Set Data into start coroutin to optimize animation
    public override void SetupData(Reward[] _data1 = default, string message = null,
        Action noCallBack = null,
        Action yesCallBack = null)
    {
        rewards = _data1;

        InitOrUpdateView();
    }

    private void InitOrUpdateView()
    {
        if (prefab == null)
            prefab = LoadResourceController.GetIconView();
        
        if (iconViews == null)
            iconViews = new List<IconView>();

        int i = 0;
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
}