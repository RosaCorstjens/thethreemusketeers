using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum WeaponType { Sword, Bow, Staff }
public enum Handed { OneHanded, TwoHanded }

[Serializable]
public class BaseWeapon : BaseEquipment
{
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; } }

    [SerializeField]
    private Handed handed;
    public Handed Handed { get { return handed; } set { handed = value; } }

    [SerializeField]
    private string prefabPath;
    public string PrefabPath { get { return prefabPath; } set { prefabPath = value; } }

    public BaseWeapon() { }

    public BaseWeapon(string name, string spriteName, ItemType itemType, int tier, EquipmentType equipmentType, StatTypes mainStat, List<StatRange> baseStats, WeaponType weaponType, Handed handed, string prefabPath) 
         : base(name, spriteName, itemType, tier, equipmentType, mainStat, baseStats)
    {
        this.weaponType = weaponType;
        this.handed = handed;
        this.prefabPath = prefabPath;
    }
}
