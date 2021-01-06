using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;

public class UIModuleSetting : AWindowController
{
    public SoundAndMusicView soundAndMusicView;
    public LanguageView languageView;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnPropertiesSet()
    {
        soundAndMusicView.InitOrUpdateView();
        languageView.InitOrUpdateView();
    }
}
