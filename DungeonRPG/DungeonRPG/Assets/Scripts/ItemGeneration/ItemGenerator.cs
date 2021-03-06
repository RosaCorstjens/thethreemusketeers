﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

public class ItemGenerator
{
    private GameObject dropPrefab;
    public GameObject DropPrefab { get { if (dropPrefab == null) dropPrefab = Resources.Load<GameObject>("Prefabs/Items/DroppedItems/ItemDrop"); return dropPrefab; } }

    private AffixContainer affixContainer = new AffixContainer();
    public AffixContainer AffixContainer { get { return affixContainer; } }

    private Dictionary<Quality, int> qualityProbability;
    int summedQualityProbability = 0;

    public void Initialize()
    {
        ReadAffixXML();
        Debug.Log("Affixes:" + affixContainer.Affixes.Count);
        SetUpQualityProbability();
    }

    private void SetUpQualityProbability()
    {
        qualityProbability = new Dictionary<Quality, int>();
        qualityProbability.Add(Quality.Common, 20);
        qualityProbability.Add(Quality.Magic, 15);
        qualityProbability.Add(Quality.Rare, 10);
        qualityProbability.Add(Quality.Legendary, 5);

        foreach (KeyValuePair<Quality, int> entry in qualityProbability)
        {
            summedQualityProbability += entry.Value;
        }
    }

    private void ReadAffixXML()
    {
        Type[] types = { typeof(AffixRange) };

        XmlSerializer serializer = new XmlSerializer(typeof(AffixContainer), types);

        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Affixes.xml");

        affixContainer = (AffixContainer)serializer.Deserialize(textReader);

        textReader.Close();
    }

    // TO DO: implement loottable system. 
    public List<ItemInstance> GenerateRandomItem(int amount = 1)
    {
        // Will be filled with all items and be returned. 
        List<ItemInstance> returnList = new List<ItemInstance>(amount);

        // Create as many items as 'amount'.
        for (int i = 0; i < amount; i++)
        {
            // Get a random item type. (Either: equipment, potion, gold)
            int itemType = 0;//UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemType)).Length);

            int randomObject;
            GameObject toAdd = GameObject.Instantiate(DropPrefab);

            // Determine quality by rolling and checking the probabilty.
            Quality quality = Quality.Common;

            int randomRoll = (int)UnityEngine.Random.Range(0, summedQualityProbability);
            int counter = 0;
            foreach (KeyValuePair<Quality, int> entry in qualityProbability)
            {
                counter += entry.Value;

                // Found the choosen one. 
                if (randomRoll < counter)
                {
                    quality = entry.Key;
                    break;
                }
            }

            int level = (int)((int)(quality+1) * UnityEngine.Random.Range(1, 2.5f));

            switch (itemType)
            {
                // Equipment
                case 0:
                    List<Stat> baseStats = new List<Stat>();
                    Equipment equipment = Equipment.Belt;
                    string generatedName = "";

                    BaseWeapon baseWeapon = new BaseWeapon() ;
                    BaseArmor baseArmor = new BaseArmor();
                    BaseShield baseShield = new BaseShield();
                    BaseJewelry baseJewerly = new BaseJewelry();

                    // First choose random: weapon, armor, shield, jewerly.
                    int r = 0;//UnityEngine.Random.Range(0, 4);
                    switch (r)
                    {
                        case 0:
                            randomObject = UnityEngine.Random.Range(0, GameManager.Instance.ItemManager.ItemContainer.Weapons.Count);
                            baseWeapon = GameManager.Instance.ItemManager.ItemContainer.Weapons[randomObject];

                            equipment = Equipment.Weapon;
                            generatedName = baseWeapon.Name;

                            level *= baseWeapon.Tier;

                            int damageValue = (int)baseWeapon.BaseStats.Find(s => s.StatType == StatTypes.Damage).Range.GetRandomInRange();
                            float attackSpeedValue = (float)Math.Round(baseWeapon.BaseStats.Find(s => s.StatType == StatTypes.AttackSpeed).Range.GetRandomInRange(), 2);
                            Stat damage = new Stat(StatTypes.WeaponDamage, damageValue);
                            Stat attackSpeed = new Stat(StatTypes.AttackSpeed, attackSpeedValue);
                            baseStats.Add(damage);
                            baseStats.Add(attackSpeed);

                            break;
                        case 1:
                            randomObject = UnityEngine.Random.Range(0, GameManager.Instance.ItemManager.ItemContainer.Armor.Count);
                            baseArmor = GameManager.Instance.ItemManager.ItemContainer.Armor[randomObject];

                            equipment = (Equipment)baseArmor.ArmorType;
                            generatedName = baseArmor.Name;

                            level *= baseArmor.Tier;

                            int armorValue = (int)baseArmor.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();
                            Stat armor = new Stat(StatTypes.Armor, armorValue);
                            baseStats.Add(armor);

                            break;
                        case 2:
                            randomObject = UnityEngine.Random.Range(0, GameManager.Instance.ItemManager.ItemContainer.Shields.Count);
                            baseShield = GameManager.Instance.ItemManager.ItemContainer.Shields[randomObject];

                            equipment = Equipment.Shield;
                            generatedName = baseShield.Name;

                            level *= baseShield.Tier;

                            int armorVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();
                            int blockChanceVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.BlockChance).Range.GetRandomInRange();
                            int blockAmountVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.BlockAmount).Range.GetRandomInRange();
                            Stat armorS = new Stat(StatTypes.Armor, armorVal);
                            Stat blockChance = new Stat(StatTypes.BlockChance, blockChanceVal);
                            Stat blockAmount = new Stat(StatTypes.BlockAmount, blockAmountVal);
                            baseStats.Add(armorS);
                            baseStats.Add(blockChance);
                            baseStats.Add(blockAmount);

                            break;
                        case 3:
                            randomObject = UnityEngine.Random.Range(0, GameManager.Instance.ItemManager.ItemContainer.Jewelry.Count);
                            baseJewerly = GameManager.Instance.ItemManager.ItemContainer.Jewelry[randomObject];

                            int rand = UnityEngine.Random.Range(0, 2);

                            equipment = rand == 0 ? Equipment.Amulet : Equipment.Ring;
                            generatedName = baseJewerly.Name;

                            level *= baseJewerly.Tier;

                            break;
                    }

                    // Determine amount of affixes. 
                    int amountAffixes = (int)(quality+1) * 2;
                    if (amountAffixes > 6) amountAffixes = 6;
                    List<AffixRange> possibleAffixes = new List<AffixRange>();

                    possibleAffixes.AddRange(affixContainer.Affixes.FindAll(a => a.EquipmentTypes.Contains(equipment) && a.MinLevel <= level && a.MaxLevel >= level));
                    List<AffixRange> choosenAffixes = new List<AffixRange>();

                    bool hasPre = false;
                    bool hasSuff = false;

                    if (amountAffixes > 1)
                    {
                        List<AffixRange> prefixPossibilities = possibleAffixes.FindAll(a => a.IsPrefix);
                        List<AffixRange> suffixPosibilities = possibleAffixes.FindAll(a => !a.IsPrefix);

                        AffixRange choosenPre = prefixPossibilities[UnityEngine.Random.Range(0, prefixPossibilities.Count)];
                        choosenAffixes.Add(choosenPre);

                        AffixRange choosenSuff = suffixPosibilities[UnityEngine.Random.Range(0, suffixPosibilities.Count)];
                        choosenAffixes.Add(choosenSuff);

                        possibleAffixes.Remove(choosenPre);
                        possibleAffixes.Remove(choosenSuff);
                        hasPre = true;
                        hasSuff = true;
                        amountAffixes -= 2;
                    }

                    for (int j = 0; j < amountAffixes; j++)
                    {
                        int total = 0;
                        possibleAffixes.HandleAction(a => total += a.Probability);
                        int roll = UnityEngine.Random.Range(0, total);

                        int count = 0;
                        for (int k = 0; k < possibleAffixes.Count; k++)
                        {
                            count += possibleAffixes[k].Probability;

                            if(roll < count)
                            {
                                choosenAffixes.Add(possibleAffixes[k]);

                                if (possibleAffixes[k].IsPrefix) hasPre = true;
                                if (!possibleAffixes[k].IsPrefix) hasSuff = true;

                                possibleAffixes.RemoveAt(k);

                                break;
                            }
                        }
                    }

                    string prefix = "";
                    string suffix = "";
                    if (hasPre)
                    {
                        List<AffixRange> prefixes = choosenAffixes.FindAll(a => a.IsPrefix);
                        prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
                    }
                    if (hasSuff)
                    {
                        List<AffixRange> suffixes = choosenAffixes.FindAll(a => !a.IsPrefix);
                        suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
                    }

                    generatedName = prefix + generatedName + suffix;

                    // Initialize all chosen affixes with determined values. 
                    List<Affix> affixStats = new List<Affix>();
                    for (int j = 0; j < choosenAffixes.Count; j++)
                    {
                        float rawVal = UnityEngine.Random.Range(choosenAffixes[j].PossibleValues.min, choosenAffixes[j].PossibleValues.max);
                        float roundedVal = choosenAffixes[j].Modifier.affected == StatTypes.AttackSpeed ? (float)Math.Round(rawVal,2) : (int)rawVal;
                        choosenAffixes[j].Modifier.value = roundedVal;
                        affixStats.Add(new Affix(choosenAffixes[j].ID, choosenAffixes[j].Name, choosenAffixes[j].Tier, choosenAffixes[j].Modifier, roundedVal, choosenAffixes[j].IsPercent));
                    }

                    // Now all the values for the affixes are choosen, add the affixes that are the same from a different tier. 
                    for (int j = 0; j < affixStats.Count; j++)
                    {
                        for (int k = 0; k < affixStats.Count; k++)
                        {
                            if (k == j) continue;
                            if(affixStats[j].Modifier.affected == affixStats[k].Modifier.affected && affixStats[j].Modifier.modifierType == affixStats[k].Modifier.modifierType)
                            {
                                affixStats[j].Value += affixStats[k].Value;
                                affixStats[j].Modifier.value = affixStats[j].Value;
                                affixStats.RemoveAt(k);
                                k--;
                            }
                        }
                    }


                    switch (r)
                    {
                        case 0:
                            toAdd.AddComponent<WeaponInstance>().Initialize(baseWeapon, quality, level, generatedName, baseStats, affixStats);
                            break;

                        case 1:
                            toAdd.AddComponent<ArmorInstance>().Initialize(baseArmor, quality, level, generatedName, baseStats, affixStats);
                            break;

                        case 2:
                            toAdd.AddComponent<ShieldInstance>().Initialize(baseShield, quality, level, generatedName, baseStats, affixStats);
                            break;

                        case 3:
                            toAdd.AddComponent<JewerlyInstance>().Initialize(baseJewerly, quality, level, generatedName, new List<Stat>(), affixStats);
                            break;
                    }

                    break;

                // Potions
                case 1:
                    randomObject = UnityEngine.Random.Range(0, GameManager.Instance.ItemManager.ItemContainer.Potions.Count);

                    BasePotion basePotion = GameManager.Instance.ItemManager.ItemContainer.Potions[randomObject];

                    toAdd.AddComponent<PotionInstance>().Initialize(basePotion, quality);

                    break;

                // Gold
                case 2:
                    // No random object for gold.
                    BaseGold baseGold = new BaseGold();

                    int value = UnityEngine.Random.Range(0, 100);

                    toAdd.AddComponent<GoldInstance>().Initialize(baseGold, quality, value);

                    break;
            }

            returnList.Add(toAdd.GetComponent<ItemInstance>());

        }

        return returnList;
    }
}
