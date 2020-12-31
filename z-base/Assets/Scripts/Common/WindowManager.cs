using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;

    //private Transform container;
    readonly Dictionary<WindowType, BaseWindow> windowList = new Dictionary<WindowType, BaseWindow>();

    public Transform uiMainCanvas = null;

    private void Awake()
    {
        // if (Instance == null) Instance = this;
        // else
        // {
        //     Destroy(this);
            Instance = this;
        //}
    }
    
    #region Show Notification

    public bool ShowWindowWithNoData(WindowType type, string _message = null, Action noCallBack = null,
        Action yesCallBack = null)
    {
        if (windowList.ContainsKey(type))
        {
            BaseWindow window = windowList[type];
            if (window != null)
            {
                window.SetupData(_message, noCallBack, yesCallBack);
                window.transform.SetAsLastSibling();
                window.OnShow();
                return true;
            }

            return false;
        }

        bool check = InitWindow(type, _message, noCallBack, yesCallBack);
        return check;
    }
    
    private bool InitWindow(WindowType type, string message = null, Action noCallBack = null, Action yesCallBack = null)
    {
        // UpdateContainer();
        BaseWindow windowPrefab =
            Resources.Load<BaseWindow>(string.Format(PathUtils.windowPath, type.ToString().ToLower()));
        if (windowPrefab == null) return false;

        BaseWindow window = Instantiate(windowPrefab, uiMainCanvas);
        windowList.Add(window.type, window);

        if (window != null)
        {
            window.SetupData(message, noCallBack, yesCallBack);
            window.transform.SetAsLastSibling();
            window.OnShow();
            return true;
        }

        return false;
    }

    #endregion


    #region Show window with data

    public bool ShowWindowWithData<T>(WindowType type, T data = default, string message = null,
        Action noCallBack = null, Action yesCallBack = null)
    {
        if (windowList.ContainsKey(type))
        {
            BaseWindow window = windowList[type];
            if (window != null)
            {
                BaseWindowGeneric<T> _window = window as BaseWindowGeneric<T>;
                _window.SetupData(data, message, noCallBack, yesCallBack);
                _window.transform.SetAsLastSibling();
                _window.OnShow();
                return true;
            }

            return false;
        }

        bool check = InitWindow(type, data, message, noCallBack, yesCallBack);
        Debug.Log("init success: " + check);
        return check;
    }

    private bool InitWindow<T>(WindowType type, T data = default, string message = null, Action noCallBack = null,
        Action yesCallBack = null)
    {
        // UpdateContainer();
        var path = string.Format(PathUtils.windowPath, type.ToString().ToLower());
        BaseWindow windowPrefab = Resources.Load<BaseWindow>(path);
        BaseWindow window = Instantiate(windowPrefab, uiMainCanvas);

        windowList.Add(window.type, window);
        if (window != null)
        {
            BaseWindowGeneric<T> _window = window as BaseWindowGeneric<T>;
            _window.SetupData(data, message, noCallBack, yesCallBack);
            _window.transform.SetAsLastSibling();
            _window.OnShow();
            return true;
        }

        return false;
    }

    #endregion

    #region Show window with data 2

    public bool ShowWindowWithManyData<T>(WindowType type, T data1 = default, T data2 = default, string message = null,
        Action noCallBack = null, Action yesCallBack = null)
    {
        if (windowList.ContainsKey(type))
        {
            BaseWindow window = windowList[type];
            if (window != null)
            {
                BaseWindowGeneric<T> _window = window as BaseWindowGeneric<T>;
                _window.SetupData(data1, data2, message, noCallBack, yesCallBack);
                _window.transform.SetAsLastSibling();
                _window.OnShow();
                return true;
            }

            return false;
        }

        bool check = InitWindow(type, data1, data2, message, noCallBack, yesCallBack);
        return check;
    }

    private bool InitWindow<T>(WindowType type, T data1, T data2 = default, string message = null,
        Action noCallBack = null, Action yesCallBack = null)
    {
        // UpdateContainer();
        BaseWindow windowPrefab =
            Resources.Load<BaseWindow>(string.Format(PathUtils.windowPath, type.ToString()).ToLower());
        if (windowPrefab == null) return false;
        BaseWindow window = Instantiate(windowPrefab, uiMainCanvas);

        windowList.Add(window.type, window);
        if (window != null)
        {
            BaseWindowGeneric<T> _window = window as BaseWindowGeneric<T>;
            _window.SetupData(data1, data2, message, noCallBack, yesCallBack);
            _window.transform.SetAsLastSibling();
            _window.OnShow();
            return true;
        }

        return false;
    }

    #endregion

    public void HideAllWindow()
    {
        foreach (var window in windowList.Values)
        {
            window.gameObject.SetActive(false);
        }
    }

    public void HideWindow(WindowType type)
    {
        if (windowList.ContainsKey(type))
        {
            windowList[type].OnHide();
        }
    }
}