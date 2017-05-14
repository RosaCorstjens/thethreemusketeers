using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseCharacterInformation
{
    protected string name;
    public string Name { get { return name; } }

    protected int level;
    public int Level { get { return level; } }

    protected BaseCharacterClass characterClass;
    public BaseCharacterClass CharacterClass { get { return characterClass; } }

    protected CharacterStats stats;
    public CharacterStats Stats { get { return stats; } }

    public BaseCharacterInformation() { }

    public BaseCharacterInformation(string name, BaseCharacterClass characterClass)
    {
        this.name = name;
        this.characterClass = characterClass;
    }

    protected void SetUpStats()
    {
        List<Stat> baseStats = new List<Stat>();

        int numberOfStats = (System.Enum.GetValues(typeof(StatTypes))).Length;

        for (int i = 0; i < numberOfStats; i++)
        {
            baseStats.Add(new Stat((StatTypes)i, 0));
        }

        // Set all key stats. 
        baseStats.Find(s => s.StatType == StatTypes.Strength).Value = 8;
        baseStats.Find(s => s.StatType == StatTypes.Dexterity).Value = 8;
        baseStats.Find(s => s.StatType == StatTypes.Intelligence).Value = 8;
        baseStats.Find(s => s.StatType == StatTypes.Vitality).Value = 8;

        // Add 2 to the prim stat of player class. 
        baseStats.Find(s => s.StatType == characterClass.PrimaryStat).Value += 2;

        baseStats.Find(s => s.StatType == StatTypes.Armor).Value = 18;
        baseStats.Find(s => s.StatType == StatTypes.Damage).Value = 2;
        baseStats.Find(s => s.StatType == StatTypes.WeaponDamage).Value = 0;
        baseStats.Find(s => s.StatType == StatTypes.Resistance).Value = 1;

        baseStats.Find(s => s.StatType == StatTypes.MaxHealth).Value = 40;
        baseStats.Find(s => s.StatType == StatTypes.HealthPerSec).Value = 0.5f;
        baseStats.Find(s => s.StatType == StatTypes.HealthPerHit).Value = 0f;
        baseStats.Find(s => s.StatType == StatTypes.HealthPerKill).Value = 0f;

        baseStats.Find(s => s.StatType == StatTypes.MaxResource).Value = 125;
        baseStats.Find(s => s.StatType == StatTypes.ResourceRegen).Value = 4;

        baseStats.Find(s => s.StatType == StatTypes.ExperienceBonus).Value = 0f;
        baseStats.Find(s => s.StatType == StatTypes.MovementSpeed).Value = 1f;

        baseStats.Find(s => s.StatType == StatTypes.AttackSpeed).Value = 0;
        baseStats.Find(s => s.StatType == StatTypes.CritRate).Value = 50;
        baseStats.Find(s => s.StatType == StatTypes.CritDamage).Value = 100;

        baseStats.Find(s => s.StatType == StatTypes.BlockAmount).Value = 0;
        baseStats.Find(s => s.StatType == StatTypes.BlockChance).Value = 0;
        baseStats.Find(s => s.StatType == StatTypes.Thorns).Value = 0;

        stats = new CharacterStats(baseStats, new List<Modifier>(), characterClass.PrimaryStat);
    }

    public void AddStats(EquipmentInstance item)
    {
        for (int i = 0; i < item.EquipmentData.Modifiers.Count; i++)
        {
            if (item.EquipmentData.Modifiers[i].affected == StatTypes.HealthPerHit)
            {
                Debug.Log("item hph: "+ item.EquipmentData.Modifiers[i].value);
            }
            stats.AddModifier(item.EquipmentData.Modifiers[i]);
            if (item.EquipmentData.Modifiers[i].affected == StatTypes.HealthPerHit)
            {
                Debug.Log("stat hph: " + stats.Get(StatTypes.HealthPerHit));
            }
        }
    }

    public void RemoveStats(EquipmentInstance item)
    {
        for (int i = 0; i < item.EquipmentData.Modifiers.Count; i++)
        {
            stats.RemoveModifier(item.EquipmentData.Modifiers[i]);
        }
    }

    // Although enemies can't really level up, this is usefull to generate their stats based on base stats and there level. 
    protected virtual void LevelUp()
    {
        level++;

        // Some increase of stats stuff. 
    }
}
