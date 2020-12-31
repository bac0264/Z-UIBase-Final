using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaTab : MonoBehaviour
{
    [SerializeField] private Button tabBtn = null;
    [SerializeField] private Text nameTab;
    private GachaData data;
    private Action<GachaData> showGacha;
    private void Awake()
    {
        tabBtn.onClick.AddListener(OnClickTab);
    }

    public void SetupAction(GachaData data, Action<GachaData> showGachaView)
    {
        this.data = data;

        nameTab.text = "Tab " + data.id;
        showGacha = showGachaView;
    }
    private void OnClickTab()
    {
        showGacha?.Invoke(data);
    }
}
