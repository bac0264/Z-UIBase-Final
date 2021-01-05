using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;

[Serializable]
public class ShowRewardProperties : WindowProperties
{
    public readonly Reward[] rewards;

    public ShowRewardProperties(Reward[] rewards)
    {
        this.rewards = rewards;
    }
}
public class UIShowReward : AWindowController <ShowRewardProperties>
{
    [SerializeField] private Transform rewardAnchor = null;

    private IconView prefab = null;
    private List<IconView> iconViews = null;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnPropertiesSet()
    {
        InitOrUpdateView();
    }

    private void InitOrUpdateView()
    {
        var rewards = Properties.rewards;
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