using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public enum WindowType
{
    UI_SHOW_REWARD = 0,
    UI_RESULT_CAMPAIGN = 1,
}

public class BaseWindow : MonoBehaviour
{
    public WindowType type;
    
    public Transform popupContainer;

    public Button closeBtn;
    private void OnValidate()
    {
        //name = type.ToString();
    }

    public virtual void Awake()
    {
        if(closeBtn != null) closeBtn.onClick.AddListener(OnHide);
    }

    public virtual void SetupData(string message = null, Action noCallBack = null, Action yesCallBack = null)
    {
    }

    public virtual void OnShow()
    {
        //StartCoroutine(_OnHideShow(true));
        _OnHideShow(true);
    }

    public virtual void OnHide()
    {
        //StartCoroutine(_OnHideShow(false));
        _OnHideShow(false);
    }
    

    public void _OnHideShow(bool isShow)
    {
        gameObject.SetActive(isShow);
       // yield return new WaitForEndOfFrame();
    }
}

public class BaseWindowGeneric<T> : BaseWindow
{
    public virtual void SetupData(T _data1 = default, T data2 = default, string message = null, Action noCallBack = null, Action yesCallBack = null)
    {
    }
    public virtual void SetupData(T _data = default, string message = null, Action noCallBack = null, Action yesCallBack = null)
    {
    }
    public override void SetupData(string message = null, Action noCallBack = null, Action yesCallBack = null)
    {
        base.SetupData(message, noCallBack, yesCallBack);
    }
}
