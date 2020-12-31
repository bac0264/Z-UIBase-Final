using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TrackingType
{
    None = -1,
    Click_Button = 0,
}

public class FirebaseHelper : MonoBehaviour
{
    public TrackingType type = TrackingType.Click_Button;

    public Button buttonClick;

    private void OnValidate()
    {
        if (buttonClick == null) buttonClick = GetComponent<Button>();
    }

    private void Start()
    {
        if (type != TrackingType.None)
            buttonClick.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        FirebaseManager.instance.SetLogEvent(type.ToString().ToLower());
    }
}