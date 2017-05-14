using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldInstance : EquipmentInstance 
{
    private BaseShield baseShield;
    public BaseShield BaseShield { get { return baseShield; } }

    private GameObject shieldObject;
    public GameObject ShieldObject { get { return shieldObject; } }

    public void Initialize(ItemPrivateData itemData, EquipmentPrivateData equipmentData, BaseShield shieldData)
    {
        base.Initialize(itemData, equipmentData);
        baseShield = shieldData;

        //shieldObject = GameObject.Instantiate(Resources.Load<GameObject>(equipmentData.BaseEquipment.baseShield.PrefabPath));
        //shieldObject.SetActive(false);
    }

}

