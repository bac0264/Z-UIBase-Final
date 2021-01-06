using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zitga.Localization;

public class LanguageSubView : EnhancedScrollerCellView
{
    [SerializeField] private Button languageBtn;
    [SerializeField] private Text languageTxt;
    [SerializeField] private int id;

    private Action updateCurrentLanguage;
    private void Awake()
    {
        languageBtn.onClick.AddListener(OnClickLanguage);
    }

    public void SetData(int id, Action updateCurrentLanguage)
    {
        this.id = id;
        this.updateCurrentLanguage = updateCurrentLanguage;
        
        languageTxt.text = ((SystemLanguage) id).ToString();
    }

    private void OnClickLanguage()
    { 
        Localization.Current.localCultureInfo = Locale.GetCultureInfoByLanguage((SystemLanguage)id);
        DataPlayer.GetModule<PlayerSetting>().SetLanguage(id);
        
        updateCurrentLanguage?.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
