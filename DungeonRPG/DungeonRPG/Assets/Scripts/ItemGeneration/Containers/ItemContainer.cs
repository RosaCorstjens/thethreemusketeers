using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ItemContainer
{
    private List<BasePotion> potions = new List<BasePotion>();

    private List<BaseWeapon> weapons = new List<BaseWeapon>();

    private List<BaseArmor> armor = new List<BaseArmor>();

    private List<BaseShield> shields = new List<BaseShield>();

    private List<BaseJewelry> jewelry = new List<BaseJewelry>();

    public List<BasePotion> Potions { get { return potions; } set { potions = value; } }

    public List<BaseWeapon> Weapons { get { return weapons; } set { weapons = value; } }

    public List<BaseArmor> Armor { get { return armor; } set { armor = value; } }

    public List<BaseShield> Shields { get { return shields; } set { shields = value; } }

    public List<BaseJewelry> Jewelry { get { return jewelry; } set { jewelry = value; } }

    public int ItemAmount()
    {
        return (potions.Count + weapons.Count + armor.Count + shields.Count + jewelry.Count);
    }
}
