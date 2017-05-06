using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterStats
{
    [SerializeField]
    List<Stat> stats = new List<Stat>();
    public List<Stat> Stats { get { return stats; } set { stats = value; } }

    [SerializeField]
    List<Modifier> modifiers = new List<Modifier>();
    public List<Modifier> Modifiers { get { return modifiers; } set { modifiers = value; } }

    private StatTypes baseStat;
    public StatTypes BaseStat { get { return baseStat; } }

    public float MaxDeterminedHealth { get { return (int)(Get(StatTypes.MaxHealth) + (Get(StatTypes.Vitality) * 10)); } }
    public float DeterminedDamage { get { return Get(StatTypes.Damage) * (1 + (Get(baseStat) / 100)); } } // Damage with base stats taken into account. 
    public float DeterminedArmor { get { return Get(StatTypes.Armor) * (1 + (Get(StatTypes.Strength) / 100)); } }
    public float DeterminedResistance { get { return Get(StatTypes.Resistance) * (1 + (Get(StatTypes.Intelligence) / 100)); } }
    public float DeterminedDodgeChance { get { return Get(StatTypes.DodgeChance) * (1 + (Get(StatTypes.Dexterity) / 100)); } }

    public float CritFactor { get { return ((Get(StatTypes.CritRate) / 100) * (Get(StatTypes.CritDamage) / 100) + 1); } }

    public float ArmorToughnessFactor { get { return (Get(StatTypes.Armor) / (Get(StatTypes.Armor) + (50 * GameManager.Instance.ActiveCharacterInformation.Level))); } }
    public float ResistanceToughnessFactor { get { return (Get(StatTypes.Resistance) / (Get(StatTypes.Resistance) + (50 * GameManager.Instance.ActiveCharacterInformation.Level))); } }
    public float DogdeToughnessFactor { get { return 1 / (1 - Get(StatTypes.DodgeChance)); } }

    public float DeterminedSpeedMultiplier { get { return (100f + Get(StatTypes.MovementSpeed) * 10) / 100f; } }

    // TO DO: Replace the 'player health' var by the average monster health at player level. 
    public float PotentialMonstersKilledPerSecond { get { return (PotentialDamagePerSec / Get(StatTypes.MaxHealth)); } }

    public int PotentialDamagePerSec { get { return (int)(CritFactor * Get(StatTypes.AttackSpeed) * (DeterminedDamage + Get(StatTypes.WeaponDamage))); } }

    public int PotentialHealPerSec
    {
        get
        {
            return (int)(Get(StatTypes.HealthPerSec) + (Get(StatTypes.AttackSpeed) * Get(StatTypes.HealthPerHit)) + (PotentialMonstersKilledPerSecond * Get(StatTypes.HealthPerKill)));
        }
    } 
    public int PotentialProtectionPerSec
    {
        get
        {
            return (int)(MaxDeterminedHealth * (1 + ArmorToughnessFactor) * (1 + ResistanceToughnessFactor));// * (1 + (DogdeToughnessFactor/2)));
        }
    }

    public CharacterStats() { }

    public CharacterStats(List<Stat> stats, List<Modifier> modifiers, StatTypes baseStat)
    {
        foreach (Stat stat in stats)
        {
            this.stats.Add(stat);
        }

        foreach (Modifier mod in modifiers)
        {
            this.modifiers.Add(mod);
        }

        this.baseStat = baseStat;
    }

    public float GetBase(StatTypes id)
    {
        return stats.Find(s => s.StatType == id).Value;
    }

    public float Get(StatTypes id)
    {
        float total = stats.Find(s => s.StatType == id).Value;
        float multiplier = 0;

        foreach (Modifier mod in modifiers)
        {
            if (mod.modifierType == ModifierType.add) total += mod.affected == id ? mod.value : 0;
            else if (mod.modifierType == ModifierType.mult) multiplier += mod.affected == id ? mod.value : 0;
        }
        multiplier /= 100;

        return total + (total * multiplier);
    }

    public void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);       
    }

    public void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
    }

}



