using UnityEditor;
using UnityEngine;

public class LoadDataHelper : MonoBehaviour
{
#if UNITY_EDITOR
    public void LoadAllData()
    {
        ReimportAsset();
    }

    [MenuItem("AssetDatabase/loadAllAssetsAtPath")]
    private void ReimportAsset()
    {
        var shopAsset = OnCreateOrUpdateShopCollection();
        if (shopAsset != null)
        {
            var textAssets = AssetDatabase.FindAssets("t: TextAsset", new[] {"Assets/Csv/Collection"});

            foreach (var asset in textAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                if (path.Equals("Assets/Csv/" + PathUtils.defineCollection + ".csv"))
                {
                    var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(textAsset));
                    break;
                }
            }

            foreach (var asset in textAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(textAsset));
            }

            var _shopAsset = shopAsset as ShopAllDataCollection;
            _shopAsset.SetupAllData();
            
        }
    }

    private Object OnCreateOrUpdateShopCollection()
    {
        var type = typeof(ShopAllDataCollection);
        var assetFile = "Assets/Resources/" + PathUtils.allShopDataCollection + ".asset";
        var gm = AssetDatabase.LoadAssetAtPath(assetFile, type);

        if (gm == null)
        {
            gm = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(gm, assetFile);
            Debug.Log("Creating Shop Collection, please reload all data.");
            return gm;
        }
        Debug.Log("Created Shop Collection");
        return gm;
    }
    #endif

}