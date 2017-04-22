using UnityEngine;
using System.Collections;
using System;

public enum PotionType { Health, Mana }

[Serializable]
public class BasePotion : BaseItem
{
    [SerializeField]
    private PotionType potionType;
    public PotionType PotionType { get { return potionType; } set { potionType = value; } }

    [SerializeField]
    private IntRange restoreAmount;
    public IntRange RestoreAmount { get { return restoreAmount; } set { restoreAmount = value; } } 

    public BasePotion() { }

    public BasePotion(string name, string spriteName, ItemType itemType, int tier, PotionType potionType, IntRange restoreAmount) : base(name, spriteName, itemType, tier)
    {
        this.PotionType = potionType;
        this.RestoreAmount = restoreAmount;
    }
}
