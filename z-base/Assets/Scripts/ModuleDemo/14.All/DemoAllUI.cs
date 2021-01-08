using System;
using deVoid.UIFramework;
using UnityEngine;
using Zitga.ContextSystem;
using Zitga.Localization;
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
        DataPlayer.GetDataToServer();
        
        InitUpdateSystem();
        InitLocalization();
        InitSoundManager();
        InitUIFrame();
    }

    private void Start()
    {
        UIFrame.Instance.OpenWindow(WindowIds.HomeMain);
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
}