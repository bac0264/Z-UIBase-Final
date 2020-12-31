using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zitga.CSVSerializer.Dictionary;

public class SpawnEnemyExample : ScriptableObject
{
    [System.Serializable]
    public class Bonus
    {
        public int type_id, number, interval, bonus_hp, bonus_move_speed, bonus_atk;
    }

    [System.Serializable]
    public class SpawnEnemy
    {
        public int time_start;
        public int time_end;
        public int[] zone_id;
        public Bonus[] bonuses;
    }

    public SpawnEnemy[] spawnEnemies;
}

#if UNITY_EDITOR
public class SpawnEnemyPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            if (str.IndexOf("/SpawnEnemy.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                string assetfile = str.Replace(".csv", ".asset");
                SpawnEnemyExample gm = AssetDatabase.LoadAssetAtPath<SpawnEnemyExample>(assetfile);
                if (gm == null)
                {
                    gm = ScriptableObject.CreateInstance<SpawnEnemyExample>();
                    AssetDatabase.CreateAsset(gm, assetfile);
                }

                gm.spawnEnemies = CSVSerializer.Deserialize<SpawnEnemyExample.SpawnEnemy>(data.text);

                EditorUtility.SetDirty(gm);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }
        }
    }
}
#endif