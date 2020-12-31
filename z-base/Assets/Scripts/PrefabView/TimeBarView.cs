using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarView : MonoBehaviour
{
    [SerializeField] private Text timeCountDown = null;

    public Color color;

    private Coroutine countDown = null;

    private void Awake()
    {
        timeCountDown.color = color;
    }

    public void SetData(long currentTime, Action callback = null)
    {
        if (countDown == null)
            countDown = StartCoroutine(TimeUtils.TimeCountDown(timeCountDown, currentTime, callback));
    }

    public void StopCountDown()
    {
        if (countDown != null)
        {
            StopCoroutine(countDown);
            countDown = null;
        }
        gameObject.SetActive(false);
    }
}