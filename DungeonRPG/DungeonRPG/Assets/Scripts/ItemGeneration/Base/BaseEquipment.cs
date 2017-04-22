using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum EquipmentType { Shield, Weapon, Armor, Jewelry }

[Serializable]
public class BaseEquipment : BaseItem
{
    [SerializeField]
    private EquipmentType equipmentType;
    public EquipmentType EquipmentType { get { return equipmentType; } set { equipmentType = value; } }

    [SerializeField]
    private StatTypes mainStat;
    public StatTypes MainStat { get { return mainStat; }set { mainStat = value; } }

    [SerializeField]
    private List<StatRange> baseStats = new List<StatRange>();
    public List<StatRange> BaseStats { get { return baseStats; } set { baseStats = value; } }

    public BaseEquipment() { }

    public BaseEquipment(string name, string spriteName, ItemType itemType, int tier, EquipmentType equipmentType, StatTypes mainStat, List<StatRange> baseStats) : base(name, spriteName, itemType, tier)
    {
        this.EquipmentType = equipmentType;
        this.mainStat = mainStat;
        this.baseStats = baseStats;
    }

}
