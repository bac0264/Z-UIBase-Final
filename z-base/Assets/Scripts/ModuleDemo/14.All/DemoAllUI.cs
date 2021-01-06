﻿using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Zitga.ContextSystem;
using Zitga.Localization;
using Zitga.Sound;
using Zitga.Update;

public class DemoAllUI : MonoBehaviour
{
    public static DemoAllUI instance;
    private Context context;

    private GlobalUpdateSystem updateSystem;

    private void Awake()
    {
        instance = this;
        context = Context.Current;
        PublisherService.Register();

        InitUpdateSystem();
        InitLocalization();
        InitSoundManager();
        InitUIFrame();
    }

    private void Start()
    {
        //UIFrame.Instance.OpenWindow(WindowIds.HomeWindow);
    }

    private void InitUIFrame()
    {
        var result = Resources.Load<UIFrame>("UIFrame");

        if (result)
        {
            var uiFrame = Instantiate(result);

            context.GetContainer().Register(uiFrame);
        }
        else
        {
            throw new Exception("UIFrame is not exist");
        }
    }

    private void InitUpdateSystem()
    {
        updateSystem = new GlobalUpdateSystem();

        context.GetContainer().Register(updateSystem);
    }

    private void InitLocalization()
    {
        var localization = Localization.Current;

        localization.localCultureInfo = Locale.GetCultureInfoByLanguage(SystemLanguage.English);

        context.GetContainer().Register(localization);
    }

    private void InitSoundManager()
    {
        var soundManager = new SoundManager();

        context.GetContainer().Register(soundManager);
    }

    private void Update()
    {
        updateSystem.OnUpdate(Time.deltaTime);
    }

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
        return;
        UIFrame.Instance.OpenWindow(WindowIds.GiftCode);
    }
}