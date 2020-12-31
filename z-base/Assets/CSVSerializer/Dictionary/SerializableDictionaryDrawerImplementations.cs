#if UNITY_EDITOR
using UnityEngine;

namespace Zitga.CSVSerializer.Dictionary
{
    // ---------------
//  Int => ShopGroup
// ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(ShopGroupExample))]
    public class IntShopGroupDictionaryDrawer : SerializableDictionaryDrawer<int, ShopGroupExample.ShopGroup>
    {
        protected override SerializableKeyValueTemplate<int, ShopGroupExample.ShopGroup> GetTemplate()
        {
            return GetGenericTemplate<SerializableIntShopGroupTemplate>();
        }
    }

    internal class SerializableIntShopGroupTemplate : SerializableKeyValueTemplate<int, ShopGroupExample.ShopGroup>
    {
    }
    
    
    // ---------------
//  Int => Item
// ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(RankingData.ItemDictionary))]
    public class IntItemDictionaryDrawer : SerializableDictionaryDrawer<int, RankingData.Item>
    {
        protected override SerializableKeyValueTemplate<int, RankingData.Item> GetTemplate()
        {
            return GetGenericTemplate<SerializableIntItemTemplate>();
        }
    }

    internal class SerializableIntItemTemplate : SerializableKeyValueTemplate<int, RankingData.Item>
    {
    }

// ---------------
//  String => Int
// ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(StringIntDictionary))]
    public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int>
    {
        protected override SerializableKeyValueTemplate<string, int> GetTemplate()
        {
            return GetGenericTemplate<SerializableStringIntTemplate>();
        }
    }

    internal class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int>
    {
    }

// ---------------
//  GameObject => Float
// ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
    public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float>
    {
        protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate()
        {
            return GetGenericTemplate<SerializableGameObjectFloatTemplate>();
        }
    }

    internal class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float>
    {
    }
}
#endif