using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public enum ArmorType { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders }

[Serializable]
public class BaseArmor : BaseEquipment
{
    [SerializeField]
    private ArmorType armorType;
    public ArmorType ArmorType { get { return armorType; } set { armorType = value; } }

    public BaseArmor() { }

    public BaseArmor(string name, string spriteName, ItemType itemType, int tier, EquipmentType equipmentType, StatTypes mainStat, List<StatRange> baseStats, ArmorType armorType)
         : base(name, spriteName, itemType, tier, equipmentType, mainStat, baseStats)
    {
        this.ArmorType = armorType;
    }
}
