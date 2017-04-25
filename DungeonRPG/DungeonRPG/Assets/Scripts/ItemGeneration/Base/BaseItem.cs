using UnityEngine;
using System.Collections;
using System;

public enum ItemType { Equipment, Potion }

[Serializable]
public class BaseItem
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    private string spriteName;
    public string SpriteName { get { return spriteName; } set { spriteName = value; } }

    [SerializeField]
    private ItemType itemType;
    public ItemType ItemType { get { return itemType; } set { itemType = value; } }

    [SerializeField]
    private int tier;
    public int Tier { get { return tier; } set { tier = value; } }

    public BaseItem() { }

    public BaseItem(string name, string spriteName, ItemType itemType, int tier)
    {
        this.name = name;
        this.spriteName = spriteName;
        this.itemType = itemType;
        this.tier = tier;
    }
}