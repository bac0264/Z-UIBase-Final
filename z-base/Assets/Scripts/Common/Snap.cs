using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Snap : MonoBehaviour
{
    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private RectTransform center;

    public bool pureSnap = false;
    public bool scaleSnap = false;
    
    private List<RectTransform> posList = new List<RectTransform>();

    private int minIndex;

    private float[] distance;
    private float bttnDistance;
    private float MaxDistance = 0;

    private bool startSnap;
    private bool dragging = false;

    private Action callback = null;
    public void SetupSnap(Action callback = null)
    {
        this.callback = callback;
        Invoke("_setupSnap", 0.1f);
    }

    private void _setupSnap()
    {
        bttnDistance = (int) Mathf.Abs(posList[1].anchoredPosition.x - posList[0].anchoredPosition.x);
        MaxDistance = bttnDistance;

        minIndex = 0;
        distance = new float[posList.Count];

        _initPos((minIndex) * (-bttnDistance));
    }

    public void AddRectTransform(RectTransform rect)
    {
        posList.Add(rect);
    }

    private void OnDisable()
    {
        startSnap = false;
    }

    private void Update()
    {
        if (startSnap)
        {
            for (int i = 0; i < posList.Count; i++)
            {
                var pos = Mathf.Abs(center.transform.position.x - posList[i].transform.position.x);
                distance[i] = (int) pos;
                if (scaleSnap)
                {
                    float x = Mathf.Abs(pos - MaxDistance) / MaxDistance;
                    Vector3 scale = new Vector3(x, x, 0);
                    posList[i].transform.DOScale(scale, 0f);
                }
            }

            float minDistance = Mathf.Min(distance);


            for (int a = 0; a < posList.Count; a++)
            {
                if (minDistance == distance[a])
                {
                    minIndex = a;
                    callback?.Invoke();
                    break;
                }
            }

            if (!dragging)
            {
                LerpToImage(minIndex * (-bttnDistance));
            }
        }
    }

    private void LerpToImage(float position)
    {
        float newX = Mathf.Lerp(scrollContent.anchoredPosition.x, position, 0.2f);
        Vector2 newPosition = new Vector2(newX, scrollContent.anchoredPosition.y);
        scrollContent.anchoredPosition = newPosition;
    }

    private void _initPos(float position)
    {
        float newX = position;
        Vector2 newPosition = new Vector2(newX, scrollContent.anchoredPosition.y);
        scrollContent.anchoredPosition = newPosition;
    }

    public void OnPointerDown()
    {
        if (pureSnap || scaleSnap)
            startSnap = true;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public int GetIndex()
    {
        return minIndex;
    }
}