using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorInstance : EquipmentInstance
{
    private BaseArmor baseArmor;
    public BaseArmor BaseArmor { get { return baseArmor; } }

    public void Initialize(BaseArmor itemInfo, Quality quality, int level, string generatedName, List<Stat> baseStats, List<Affix> affixes = null)
    {
        base.Initialize(itemInfo, quality, level, generatedName, baseStats, affixes);
        baseArmor = itemInfo;
    }
}
