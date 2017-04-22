using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum JewelryType { Ring, Amulet }

[Serializable]
public class BaseJewelry : BaseEquipment
{
    private JewelryType jewelryType;
    public JewelryType JewelryType { get { return jewelryType; } }

    public BaseJewelry() { }

    public BaseJewelry(string name, string spriteName, ItemType itemType, int tier, EquipmentType equipmentType, JewelryType jewelryType, StatTypes mainStat = StatTypes.Armor, List<StatRange> baseStats = null)
         : base(name, spriteName, itemType, tier, equipmentType, mainStat, baseStats)
    {
        this.jewelryType = jewelryType;
    }
}
