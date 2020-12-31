using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EnhancedScrollerDemos.MultipleCellTypesDemo;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class BasePostProcessor : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)

    {
        string[] asset;
        if (importedAssets.Length > 0)
        {
            asset = importedAssets[0].Split('/');
        }
        else
            return;

        var data = asset[asset.Length - 1];
        if (data.Contains(".csv"))
            Setup(data, importedAssets);
    }

    static void Setup(string path, string[] importedAssets)
    {
        Action<string, TextAsset, Type, Type> genAsset =
            delegate(string assetfile, TextAsset data, Type classCollection, Type classData)
            {
                var gm = AssetDatabase.LoadAssetAtPath(assetfile, classCollection);

                if (gm == null)
                {
                    gm = ScriptableObject.CreateInstance(classCollection);
                    AssetDatabase.CreateAsset(gm, assetfile);
                }

                Type type = classData;
                var field = gm.GetType().GetField("dataGroups",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var method = gm.GetType().GetMethod("Convert");
                
                field.SetValue(gm, CSVSerializer.Deserialize(data.text, type));
                
                method?.Invoke(gm, null);
                
                if(field.IsPrivate) field.SetValue(gm, null);
                
                EditorUtility.SetDirty(gm);
                AssetDatabase.SaveAssets();
            };

        foreach (string str in importedAssets)
        {
            if (str.IndexOf(path) != -1)
            {
                Debug.Log("str: " + str);
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);

                var isDefineCollection = path.Equals("define_collection.csv");

                var assetfile = "";
                Type classCollection;
                Type classData;

                if (isDefineCollection)
                {
                    assetfile = "Assets/Resources/Collection/define_collection.asset";
                    classCollection = Type.GetType("DefineCollection");
                    classData = Type.GetType("DefineData");
                    genAsset(assetfile, data, classCollection, classData);
                }
                else
                {
                    var defineCollection = LoadResourceController.GetDefineCollection();

                    if (defineCollection == null)
                    {
                        TextAsset textAsset =
                            AssetDatabase.LoadAssetAtPath<TextAsset>(
                                "Assets/Csv/Collection/define_collection.csv");

                        Debug.Log("textAsset:" + textAsset);
                        assetfile = "Assets/Resources/Collection/define_collection.asset";
                        classCollection = Type.GetType("DefineCollection");
                        classData = Type.GetType("DefineData");
                        genAsset(assetfile, textAsset, classCollection, classData);
                    }

                    defineCollection = LoadResourceController.GetDefineCollection();
                    var defineData = defineCollection.GetDefineCollectionData(path);
                    if (defineData == null) return;

                    assetfile = "Assets/Resources/" + defineData.assetPath + ".asset";
                    classCollection = Type.GetType(defineData.classCollection);
                    classData = Type.GetType(defineData.classData);
                    genAsset(assetfile, data, classCollection, classData);
                }

#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + assetfile);
#endif
            }
        }
    }
}
#endif