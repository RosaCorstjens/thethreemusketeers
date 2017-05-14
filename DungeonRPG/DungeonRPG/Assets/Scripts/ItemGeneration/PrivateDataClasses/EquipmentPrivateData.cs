using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPrivateData
{
    public EquipmentPrivateData(BaseEquipment baseEquipment, string generatedName, int level, Quality quality,
        List<Stat> baseStats, List<Affix> affixStats = null)
    {
        this.generatedName = generatedName;
        this.level = level;

        this.baseStats = baseStats;
        this.affixStats = affixStats;

        statsText = string.Empty;
        if (baseEquipment.EquipmentType != EquipmentType.Jewelry)
        {
            statsText += baseStats.Find(b => b.StatType == baseEquipment.MainStat).Value + " " +
                (baseEquipment.MainStat == StatTypes.Armor ? "Armor" : "Damage");
        }

        titleText = string.Format("[" + ItemManager.Instance.QualityHexColors[(int)quality] + "]{0}[-]", generatedName);

        modifiers = new List<Modifier>();

        for (int i = 0; i < baseStats.Count; i++)
        {
            ModifierType modifierType = ModifierType.add;

            modifiers.Add(new Modifier(baseStats[i].StatType, modifierType, baseStats[i].Value));
        }

        for (int i = 0; i < affixStats.Count; i++)
        {
            modifiers.Add(affixStats[i].Modifier);
        }
    }

    private string generatedName;

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

    private string statsText;
    public string StatsText { get { return statsText; } }

    private string titleText;
    public string TitleText { get { return titleText; } }

}
