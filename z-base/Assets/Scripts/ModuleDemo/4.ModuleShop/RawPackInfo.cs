using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RawPackInfo
{
    public int id;
    public Reward[] rewards;
    public int stock;
    public float cost;
    public string packnameIap;
    public int stockFree;
    public bool free;

    public RawPackInfo()
    {
        
    }
}