using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModuleGacha : MonoBehaviour
{
    [SerializeField] private Transform gachaAnchor;
    [SerializeField] private UIGachaLayout layout;
    
    private List<GachaTab> gachaTabList = new List<GachaTab>();
    
    private GachaTab prefab = null;
    private GachaData currentGacha = null;

    private GachaCollection collection;
    private void Start()
    {
        collection = LoadResourceController.GetGachaConfigCollection();
        prefab = LoadResourceController.GetGachaTab();
        InitOrUpdateView();
    }

    private void InitOrUpdateView()
    {
        int i = 0;
        for (; i < collection.dataGroups.Length; i++)
        {
            var z = i;
            if (z == 0) currentGacha = collection.dataGroups[z];
            
            if (i < gachaTabList.Count)
            {
                gachaTabList[z].SetupAction(collection.dataGroups[z], ShowLayout);
            }
            else
            {
                var tab = Instantiate(prefab, gachaAnchor);
                tab.SetupAction(collection.dataGroups[z], ShowLayout);
                gachaTabList.Add(tab);
            }
        }
        layout.UpdateView(currentGacha);
    }

    private void ShowLayout(GachaData data)
    {
        layout.UpdateView(data);
    }
}
