using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class DefineCollection : ScriptableObject
{

    public DefineData[] dataGroups;

    public DefineData GetDefineCollectionData(string csvReimport)
    {
        for (int i = 0; i < dataGroups.Length; i++)
        {
            if (dataGroups[i].IsInDefineCollection(csvReimport)) return dataGroups[i];
        }
        Debug.LogError("Warning! Not exist DefineDataCollection, check out csvReimport");
        return null;
    }
}

[System.Serializable]
public class DefineData
{
    public int id;
    public string classCollection;
    public string classData;
    public string assetPath;

    public bool IsInDefineCollection(string csvReimport)
    {
        var data = assetPath.Split('/');
        if (data.Length == 0)
        {
            return false;
        }

        var csv = data[data.Length - 1] +".csv";

        if (csv.Equals(csvReimport))
        {
            return true;
        }

        return false;
    }
}
