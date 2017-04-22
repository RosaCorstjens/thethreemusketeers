using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseShield : BaseEquipment
{
    [SerializeField]
    private string prefabPath;
    public string PrefabPath { get { return prefabPath; } set { prefabPath = value; } }

    public BaseShield() { }

    public BaseShield(string name, string spriteName, ItemType itemType, int tier, EquipmentType equipmentType, StatTypes mainStat, List<StatRange> baseStats, string prefabPath) 
         : base(name, spriteName, itemType, tier, equipmentType, mainStat, baseStats)
    {
        this.PrefabPath = prefabPath;
    }
}
