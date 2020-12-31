using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModuleItemToolTipView : MonoBehaviour
{
    [SerializeField] private UIModuleInventoryItemView itemView = null;

    [SerializeField] private Text localizeDescribeItem = null;
    [SerializeField] private Text levelTxt = null;
    [SerializeField] private Text nextLevelTxt = null;
    [SerializeField] private Text statsTxt = null;
    [SerializeField] private Text nextStatsTxt = null;

    [SerializeField] private Button upgradeBtn = null;
    [SerializeField] private Button sellItemBtn = null;
    [SerializeField] private Button nextItemBtn = null;

    [SerializeField] private MoneyBarView sellMoneyBar = null;
    [SerializeField] private MoneyBarView upgradeMoneyBar = null;
    
    [SerializeField] private GameObject nextStatsContainer;
    
    [SerializeField] private UpgradeItemCollection upgradeCollection;
    [SerializeField] private SellItemCollection sellCollection;
    
    [SerializeField] private ItemResource itemPick;

    private PlayerMoney playerMoney;
    private PlayerInventory playerInventory;

    private int index = 0;
    private void Awake()
    {
        playerMoney = DataPlayer.GetModule<PlayerMoney>();
        playerInventory = DataPlayer.GetModule<PlayerInventory>();
        
        upgradeCollection = LoadResourceController.GetUpgradeItemCollection();
        sellCollection = LoadResourceController.GetSellItemCollection();
        
        InitButtons();
        InitLocalize();

        NextItem();
    }

    private void InitButtons()
    {
        upgradeBtn.onClick.AddListener(OnClickUpgrade);
        sellItemBtn.onClick.AddListener(OnClickSellItem);
        nextItemBtn.onClick.AddListener(NextItem);
    }

    private void InitLocalize()
    {
    }

    public void UpdateView(ItemResource itemPick)
    {
        if (itemPick == null) return;

        this.itemPick = itemPick;

        var isMaxLevel = itemPick.IsMaxLevel();
        var stats = "";
        var nextStats = "";

        itemPick.ReloadItemStats();

        stats = itemPick.GetStatLocalize();
        nextStats = itemPick.GetStatLocalizeNextLevel();

        SetupView(stats, nextStats, isMaxLevel);
    }

    private void SetupView(string stats, string nextStats, bool isMaxLevel)
    {
        itemView.SetupItem(itemPick);
        
        statsTxt.text = stats;
        nextStatsTxt.text = nextStats;
        levelTxt.text = "level: " + (itemView.itemResource.level + 1);
        localizeDescribeItem.text = "localize item: " + itemPick.id;
        nextLevelTxt.text = "next level: " + (itemView.itemResource.level + 2);

        upgradeMoneyBar.SetData(upgradeCollection.dataGroups.GetPrice(itemPick.level));
        sellMoneyBar.SetData(sellCollection.GetSellData(itemPick.rarity).GetPrice());

        nextStatsContainer.SetActive(!isMaxLevel);
    }

    private void OnClickUpgrade()
    {
        // 
        var goldUpgrade = upgradeCollection.dataGroups.GetPrice(itemPick.level).number;

        if (playerMoney.IsEnoughMoney(Resource.CreateInstance((int) ResourceType.MoneyType,
            (int) MoneyType.Gold,
            goldUpgrade)) && itemPick != null && itemPick.level < upgradeCollection.dataGroups.maxLevel)
        {
            itemPick.level += 1;

            playerMoney.SubOne(MoneyType.Gold, goldUpgrade);
            playerInventory.Save();

            UpdateView(itemPick);
        }
    }

    private void OnClickSellItem()
    {
        var goldSell  = sellCollection.GetSellData(itemPick.rarity).GetPrice().number;

        if (itemPick != null)
        {
            playerInventory.RemoveItem(itemPick);
            playerInventory.Save();
            playerMoney.AddOne(MoneyType.Gold, goldSell);

            NextItem();
        }
    }

    private void NextItem()
    {
        UpdateView(playerInventory.GetItem(index));
        index++;
    }
}