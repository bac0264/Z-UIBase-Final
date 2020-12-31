using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModuleCharacterInfoView : MonoBehaviour
{
    [SerializeField] private Image iconCharacter = null;
    [SerializeField] private Text level = null;
    
    [SerializeField] private Transform statAnchor;
    [SerializeField] private Transform moneyBarAnchor;

    private List<MoneyBarView> moneyBarList = new List<MoneyBarView>();
    private MoneyBarView moneyBar;
    
    private List<StatView> statViews = new List<StatView>();
    private StatView prefab = null;
    
    private CharacterResource characterResource = null;
    private CharacterLevelCollection levelCollection = null;
    private void Awake()
    {
        DataPlayer.GetModule<PlayerAds>();
        InitData();
        UpdateCharacterView();
    }

    private void InitData()
    {
        levelCollection = LoadResourceController.GetCharacterLevelCollection();
        moneyBar = LoadResourceController.GetMoneyBarView();
        prefab = LoadResourceController.GetStatView();
    }
    
    public void UpdateCharacterView()
    {
        characterResource = DataPlayer.GetModule<PlayerCharacter>().GetCurrentCharacter();
        iconCharacter.sprite = LoadResourceController.GetCharacterItem(characterResource.characterId);
        level.text = "Lv. "+ characterResource.level;
        characterResource.ReloadCharacterStat();
        
        UpdateUpgradeView();
        UpdateStatView();

    }

    private void UpdateUpgradeView()
    {
        var data = levelCollection.GetCharacterLevelData(characterResource.level + 1);
        
        if (data == null) moneyBarAnchor.gameObject.SetActive(false);
        else
        {
            moneyBarAnchor.gameObject.SetActive(true);
            int k = 0;
            
            for (; k < data.rewardDatas.Length; k++)
            {
                if (k < moneyBarList.Count)
                {
                    moneyBarList[k].gameObject.SetActive(true);
                    moneyBarList[k].SetData(data.rewardDatas[k].GetResource());
                }
                else
                {
                    var moneyBarView = Instantiate(moneyBar, moneyBarAnchor);
                    moneyBarView.SetData(data.rewardDatas[k].GetResource());
                    moneyBarList.Add(moneyBarView);
                }
            }

            for (; k < moneyBarList.Count; k++)
            {
                moneyBarList[k].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateStatView()
    {
        int i = 0;
        
        for (;i < characterResource.characterStats.Count && i < statViews.Count; i++)
        {
            statViews[i].SetData(characterResource.characterStats[i]);
        }

        for (; i < characterResource.characterStats.Count; i++)
        {
            var view = Instantiate(prefab, statAnchor);
            view.SetData(characterResource.characterStats[i]);
            statViews.Add(view);
        }
    }

    public void OnClickUpgrade()
    { 
        var data = levelCollection.GetCharacterLevelData(characterResource.level + 1);
        var canUpgrade = false;
        
        if (data != null)
        {
            canUpgrade = DataPlayer.GetModule<PlayerMoney>().IsEnoughManyMoney(data.GetResources());
        }

        if (canUpgrade)
        {
            characterResource.AddLevel(1);
            UpdateCharacterView();
            DataPlayer.GetModule<PlayerCharacter>().Save();
            DataPlayer.GetModule<PlayerMoney>().SubManyMoney(data.GetResources());
        }
    }
}
