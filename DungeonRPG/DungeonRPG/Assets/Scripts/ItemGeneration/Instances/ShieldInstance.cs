using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldInstance : EquipmentInstance 
{
    private BaseShield baseShield;
    public BaseShield BaseShield { get { return baseShield; } }

    private GameObject shieldObject;
    public GameObject ShieldObject { get { return shieldObject; } }

    public void Initialize(BaseShield itemInfo, Quality quality, int level, string generatedName, List<Stat> baseStats, List<Affix> affixes = null)
    {
        base.Initialize(itemInfo, quality, level, generatedName, baseStats, affixes);
        baseShield = itemInfo;

        shieldObject = GameObject.Instantiate(Resources.Load<GameObject>(baseShield.PrefabPath));
        shieldObject.SetActive(false);
    }

}

