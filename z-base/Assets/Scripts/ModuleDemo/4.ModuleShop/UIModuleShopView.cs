using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModuleShopView : MonoBehaviour
{
    [SerializeField] private Transform bundleContainer;
    [SerializeField] private Transform rawPackContainer;

    private UIShopRawPackView rawPrefabs = null;
    private UIShopBundleItemView bundleItemPrefabs = null;

    private List<UIShopRawPackView> rawPacks = new List<UIShopRawPackView>();
    private List<UIShopBundleItemView> bundleItems = new List<UIShopBundleItemView>();

    private ShopBundleCollection shopBundlePack = null;
    private ShopRawPackCollection shopRawPack = null;

    private void Awake()
    {
        shopBundlePack = LoadResourceController.GetShopBundleCollection();
        shopRawPack = LoadResourceController.GetShopRawPackCollection();
        
        rawPrefabs = LoadResourceController.GetRawPackView();
        bundleItemPrefabs = LoadResourceController.GetBundleItemView();
        
        InitBundleItems();
        InitRawPacks();
    }

    private void InitRawPacks()
    {

        for (int i = 0; i < shopRawPack.dataGroups.Length; i++)
        {
            var raw = Instantiate(rawPrefabs, rawPackContainer);
            raw.InitView(shopRawPack.dataGroups[i], i);
            rawPacks.Add(raw);
        }
    }

    private void InitBundleItems()
    {

        for (int i = 0; i < shopBundlePack.dataGroups.Length; i++)
        {
            var bundleItem = Instantiate(bundleItemPrefabs, bundleContainer);
            bundleItem.InitView(shopBundlePack.dataGroups[i], i);
            bundleItems.Add(bundleItem);
        }
    }
}