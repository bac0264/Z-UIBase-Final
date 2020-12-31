using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

public class RankingData : ScriptableObject
{
    public enum Country
    {
        gb=1,
        de=2,
        fi,
        be
    }
    
    [Serializable]
    public class Item
    {
        public int ranking;
        public string driver;
        public string constructor;
        public int score;
        public int podium;

        public Country country;
        public string[] win;
    }
    
    [Serializable]
    public class ItemDictionary : SerializableDictionary<int, Item> { }

    public ItemDictionary itemDict;
}
