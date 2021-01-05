using System.Collections.Generic;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

[System.Serializable]
public class RetensionGiftData
{
    public int id = 0;
    public DictionaryOfStringAndString conditions = new DictionaryOfStringAndString();

    /// <summary>
    /// Phần thưởng tương ứng với Notification này.
    /// </summary>
    public List<Resource> rewards = new List<Resource>();
}

public class DictionaryOfStringAndString : SerializableDictionary<string, string>
{
}