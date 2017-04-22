using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInstance : EquipmentInstance
{
    private BaseWeapon baseWeapon;
    public BaseWeapon BaseWeapon { get { return baseWeapon; } }

    private GameObject weaponObject;
    public GameObject WeaponObject { get { return weaponObject; } }

    public void Initialize(BaseWeapon itemInfo, Quality quality, int level, string generatedName, List<Stat> baseStats, List<Affix> affixes = null)
    {
        base.Initialize(itemInfo, quality, level, generatedName, baseStats, affixes);
        baseWeapon = itemInfo;

        weaponObject = GameObject.Instantiate(Resources.Load<GameObject>(baseWeapon.PrefabPath));
        weaponObject.SetActive(false);
    }

}
