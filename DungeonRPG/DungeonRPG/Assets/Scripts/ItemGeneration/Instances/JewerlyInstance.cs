using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JewerlyInstance : EquipmentInstance
{
    private BaseJewelry baseJewelry;
    public BaseJewelry BaseJewelry { get { return baseJewelry; } }

    public void Initialize(ItemPrivateData itemData, EquipmentPrivateData equipmentData, BaseJewelry jewelryData)
    {
        base.Initialize(itemData, equipmentData, jewelryData);
        baseJewelry = jewelryData;
    }
}
