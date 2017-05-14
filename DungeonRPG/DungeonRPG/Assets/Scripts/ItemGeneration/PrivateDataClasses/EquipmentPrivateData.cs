using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPrivateData
{
    private string generatedName;
    public string GeneratedName { get { return generatedName; } }

    private string statsText;
    public string StatsText { get { return statsText; } }

    private bool equipped;
    public bool Equipped { get { return equipped; } set { equipped = value; } }

    private List<Stat> baseStats;
    public List<Stat> BaseStats { get { return baseStats; } set { baseStats = value; } }

    private List<Affix> affixStats;
    public List<Affix> AffixStats { get { return affixStats; } set { affixStats = value; } }

    private List<Modifier> modifiers;
    public List<Modifier> Modifiers { get { return modifiers; } }

    private int level;
    public int Level { get { return level; } }
}
