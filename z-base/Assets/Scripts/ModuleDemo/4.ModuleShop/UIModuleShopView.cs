using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;

public class UIModuleShopView : AWindowController
{
    [SerializeField] private Transform bundleContainer;
    [SerializeField] private Transform rawPackContainer;

    private RawPackView rawPrefabs = null;
    private BundleItemView bundleItemPrefabs = null;

    private ShopBundleCollection shopBundlePack = null;
    private ShopRawPackCollection shopRawPack = null;
    
    private List<RawPackView> rawPacks = new List<RawPackView>();
    private List<BundleItemView> bundleItems = new List<BundleItemView>();

    protected override void Awake()
    {
        base.Awake();
        shopBundlePack = LoadResourceController.GetShopWithType<ShopBundleCollection>();
        shopRawPack = LoadResourceController.GetShopWithType<ShopRawPackCollection>();
        
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