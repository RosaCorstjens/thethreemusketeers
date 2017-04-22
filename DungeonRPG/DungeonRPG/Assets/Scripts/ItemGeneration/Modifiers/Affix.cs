using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Affix contains of a modifier but is unique. 
[Serializable]
public class AffixRange
{
    [SerializeField]
    private int id;
    public int ID { get { return id; } set { id = value; } }

    [SerializeField]
    private string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    private bool isPrefix;
    public bool IsPrefix { get { return isPrefix; } set { isPrefix = value; } }

    [SerializeField]
    private bool isPercent;
    public bool IsPercent { get { return isPercent; } set { isPercent = value; } }

    [SerializeField]
    private bool rare;
    public bool Rare { get { return rare; } set { rare = value; } }

    [SerializeField]
    private int tier;
    public int Tier { get { return tier; } set { tier = value; } }

    [SerializeField]
    private int minLevel;
    public int MinLevel { get { return minLevel; } set { minLevel = value; } }

    [SerializeField]
    private int maxLevel;
    public int MaxLevel { get { return maxLevel; } set { maxLevel = value; } }

    [SerializeField]
    private int probability;
    public int Probability { get { return probability; } set { probability = value; } }

    [SerializeField]
    private Modifier modifier;
    public Modifier Modifier { get { return modifier; } set { modifier = value; } }

    [SerializeField]
    private FloatRange possibleValues;
    public FloatRange PossibleValues { get { return possibleValues; } set { possibleValues = value; } }

    [SerializeField]
    private List<Equipment> equipmentTypes;
    public List<Equipment> EquipmentTypes { get { return equipmentTypes; } set { equipmentTypes = value; } }

    public bool CanBeAppliedTo(Equipment equipmentType)
    {
        return equipmentTypes.Contains(equipmentType);
    }

    public AffixRange() { }

    public AffixRange(int id, string name, bool isPrefix, bool rare, int tier, int minLevel, int maxLevel, int probability, Modifier modifier, FloatRange possibleValues, List<Equipment> equipmentTypes)
    {
        this.id = id;
        this.name = name;
        this.isPrefix = isPrefix;
        this.tier = tier;
        this.minLevel = minLevel;
        this.maxLevel = maxLevel;
        this.probability = probability;
        this.modifier = modifier;
        this.possibleValues = possibleValues;
        this.equipmentTypes = equipmentTypes;
    }

}

[Serializable]
public class Affix
{
    private int id;
    public int ID { get { return id; } set { id = value; } }

    private string name;
    public string Name { get { return name; } set { name = value; } }

    private int tier;
    public int Tier { get { return tier; } set { tier = value; } }

    private Modifier modifier;
    public Modifier Modifier { get { return modifier; } set { modifier = value; } }

    private float value;
    public float Value { get { return value; } set { this.value = value; } }

    private bool isPercent;
    public bool IsPercent { get { return isPercent; } set { isPercent = value; } }

    public Affix() { }

    public Affix(int id, string name, int tier, Modifier modifier, float value, bool isPercent)
    {
        this.id = id;
        this.name = name;
        this.tier = tier;
        this.modifier = modifier;
        this.value = value;
        this.isPercent = isPercent;
    }
}