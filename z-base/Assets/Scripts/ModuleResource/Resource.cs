using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    MoneyType = 0,
    ItemType = 1,
    CharacterType = 2,
}

public enum ItemType
{
    Weapon = 1,
    Armor = 2,
    Armulet = 3,
    Ring = 4,
}

public enum MoneyType
{
    None = -1,
    Gold = 0,
    Gem = 1,
    VipPoint = 2,
    Potion = 3,
    
    KeyBasic = 10,
    KeyPremium = 11,

}

[System.Serializable]
public class Resource
{
    /// <summary>
    /// A type in ResourceType.
    /// </summary>
    public int type;

    /// <summary>
    /// Unique identify of a type.
    /// </summary>
    public int id;

    /// <summary>
    /// Number of resource.
    /// </summary>
    public long number;

    // Constructor
    public Resource()
    {

    }

    /// <summary>
    /// Resource constructor with params
    /// </summary>
    /// <param name="type"> A type in ResourceType. </param>
    /// <param name="id"> Unique identify of a type. </param>
    /// <param name="number"> Number of resource.. </param>
    /// <returns></returns>
    public Resource(int type, int id, long number)
    {
        this.type = type;

        this.id = id;

        this.number = number;
    }

    public static Resource CreateInstance(int type, int id, long number)
    {
        return new Resource(type, id, number);
    }

    public virtual bool Add(long value)
    {
        if (value < 0) return false;
        number += value;
        return true;
    }
    public virtual bool Sub(long value)
    {
        if (value < 0) return false;
        if (number - value < 0) return false;
        number -= value;
        return true;
    }
    public virtual void Set(long value)
    {
        if (value < 0) value = 0;
        number = value;
    }

    public virtual Reward GetReward()
    {
        return Reward.CreateInstanceReward(type, id, number);
    }
}
