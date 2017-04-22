using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AffixContainer
{
    [SerializeField]
    private List<AffixRange> affixes = new List<AffixRange>();

    public List<AffixRange> Affixes { get { return affixes; } set { affixes = value; } }

    public List<AffixRange> GetPossibleAffixes(Equipment equipmentType, int level)
    {
        List<AffixRange> returnList = new List<AffixRange>();

        affixes.HandleAction(a => { if (a.CanBeAppliedTo(equipmentType) && a.MinLevel <= level && a.MaxLevel >= level) returnList.Add(a); });

        return returnList;
    }
}
