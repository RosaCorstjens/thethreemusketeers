using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorInstance : EquipmentInstance
{
    private BaseArmor baseArmor;
    public BaseArmor BaseArmor { get { return baseArmor; } }

    public void Initialize(ItemPrivateData itemData, EquipmentPrivateData equipmentData, BaseArmor armorData)
    {
        base.Initialize(itemData, equipmentData);
        baseArmor = armorData;
    }
}
