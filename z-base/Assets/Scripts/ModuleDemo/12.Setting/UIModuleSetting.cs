using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModuleSetting : MonoBehaviour
{
    public SoundAndMusicView soundAndMusicView;
    public LanguageView languageView;
    public void Awake()
    {
        soundAndMusicView.InitOrUpdateView();
        languageView.InitOrUpdateView();
    }
}
