using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BaseGold : BaseItem
{
    [SerializeField]
    private IntRange value;
    public IntRange Value { get { return value; } set { this.value = value; } }
    public BaseGold() { }

    public BaseGold(string name, string spriteName, ItemType itemType, int tier, IntRange value): base(name, spriteName, itemType, tier)
    {
        this.value = value;
    }
}
