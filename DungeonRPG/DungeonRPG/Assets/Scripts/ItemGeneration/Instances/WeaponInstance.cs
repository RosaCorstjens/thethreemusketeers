using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInstance : EquipmentInstance
{
    private BaseWeapon baseWeapon;
    public BaseWeapon BaseWeapon { get { return baseWeapon; } }

    private GameObject weaponObject;
    public GameObject WeaponObject { get { return weaponObject; } }

    public void Initialize(ItemPrivateData itemData, EquipmentPrivateData equipmentData, BaseWeapon weaponInfo)
    {
        base.Initialize(itemData, equipmentData);
        baseWeapon = weaponInfo;

        weaponObject = Instantiate(Resources.Load<GameObject>(baseWeapon.PrefabPath));
        weaponObject.SetActive(false);
    }
}
