using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JewerlyInstance : EquipmentInstance
{
    private BaseJewelry baseJewelry;
    public BaseJewelry BaseJewelry { get { return baseJewelry; } }

    public void Initialize(BaseJewelry itemInfo, Quality quality, int level, string generatedName, List<Stat> baseStats = null, List<Affix> affixes = null)
    {
        base.Initialize(itemInfo, quality, level, generatedName, baseStats, affixes);
        baseJewelry = itemInfo;
    }
}
