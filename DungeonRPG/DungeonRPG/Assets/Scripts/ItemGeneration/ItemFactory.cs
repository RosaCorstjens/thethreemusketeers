using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ItemFactory
{
    public ItemFactory()
    {
        itemContainer = new ItemContainer();
        affixContainer = new AffixContainer();

        LoadAffixXML();
        SetQualityValues();

        LoadItemXML();
    }

    private Dictionary<Quality, int> qualityProbability;
    int summedQualityProbability = 0;

    private GameObject dropPrefab;
    public GameObject DropPrefab { get { if (dropPrefab == null) dropPrefab = Resources.Load<GameObject>("Prefabs/Items/DroppedItems/ItemDrop"); return dropPrefab; } }

    private ItemContainer itemContainer;
    private AffixContainer affixContainer;

    public AffixContainer AffixContainer
    {
        get { return affixContainer; }
        set { affixContainer = value; }
    }

    private void SetQualityValues()
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

    private void LoadAffixXML()
    {
        Type[] types = { typeof(AffixRange) };

        XmlSerializer serializer = new XmlSerializer(typeof(AffixContainer), types);
        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Affixes.xml");
        affixContainer = (AffixContainer)serializer.Deserialize(textReader);

        textReader.Close();
    }

    public int ItemAmount()
    {
        return itemContainer.ItemAmount();
    }

    private void LoadItemXML()
    {
        Type[] itemTypes = { typeof(BasePotion), typeof(BaseShield), typeof(BaseWeapon), typeof(BaseArmor), typeof(BaseJewelry) };
        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);
        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Items.xml");
        itemContainer = (ItemContainer)serializer.Deserialize(textReader);

        textReader.Close();
    }

    public List<AffixRange> ChooseAffixes(Quality quality, Equipment equipment, int level)
    {
        // Determine amount of affixes. 
        int amountAffixes = (int)(quality + 1) * 2;
        if (amountAffixes > 6) amountAffixes = 6;

        List<AffixRange> possibleAffixes = new List<AffixRange>();
        List<AffixRange> copyAffixContainer = affixContainer.Affixes.FindAll(a => a.EquipmentTypes.Contains(equipment) && a.MinLevel <= level && a.MaxLevel >= level);

        /*for (int j = 0; j < copyAffixContainer.Count; j++)
        {
            AffixRange tempAffixRange = new AffixRange(copyAffixContainer[j].ID, copyAffixContainer[j].Name, copyAffixContainer[j].IsPrefix, copyAffixContainer[j].Rare, copyAffixContainer[j].Tier, copyAffixContainer[j].MinLevel, copyAffixContainer[j].MaxLevel, copyAffixContainer[j].Probability, copyAffixContainer[j].Modifier, copyAffixContainer[j].PossibleValues, copyAffixContainer[j].EquipmentTypes);
            possibleAffixes.Add(tempAffixRange);
        }*/

        possibleAffixes.AddRange(copyAffixContainer.FindAll(a => a.EquipmentTypes.Contains(equipment) && a.MinLevel <= level && a.MaxLevel >= level));
        List<AffixRange> chosenAffixes = new List<AffixRange>();

        bool hasPre = false;
        bool hasSuff = false;

        if (amountAffixes > 1)
        {
            List<AffixRange> prefixPossibilities = possibleAffixes.FindAll(a => a.IsPrefix);
            List<AffixRange> suffixPosibilities = possibleAffixes.FindAll(a => !a.IsPrefix);

            AffixRange choosenPre = prefixPossibilities[UnityEngine.Random.Range(0, prefixPossibilities.Count)];
            chosenAffixes.Add(choosenPre);

            AffixRange choosenSuff = suffixPosibilities[UnityEngine.Random.Range(0, suffixPosibilities.Count)];
            chosenAffixes.Add(choosenSuff);

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

                if (roll < count)
                {
                    chosenAffixes.Add(possibleAffixes[k]);

                    if (possibleAffixes[k].IsPrefix) hasPre = true;
                    if (!possibleAffixes[k].IsPrefix) hasSuff = true;

                    possibleAffixes.RemoveAt(k);

                    break;
                }
            }
        }
         return chosenAffixes;
    }

    public List<Affix> DetermineAffixes(List<AffixRange> chosenAffixes)
    {
        // Initialize all chosen affixes with determined values. 
        List<Affix> affixStats = new List<Affix>();
        for (int j = 0; j < chosenAffixes.Count; j++)
        {
            float rawVal = UnityEngine.Random.Range(chosenAffixes[j].PossibleValues.min, chosenAffixes[j].PossibleValues.max);
            float roundedVal = chosenAffixes[j].Modifier.affected == StatTypes.AttackSpeed ? (float)Math.Round(rawVal, 2) : (float)Math.Round(rawVal);

            Modifier tempModifier = new Modifier(chosenAffixes[j].Modifier.affected, chosenAffixes[j].Modifier.modifierType, roundedVal);
            affixStats.Add(new Affix(chosenAffixes[j].ID, chosenAffixes[j].Name, chosenAffixes[j].Tier, tempModifier, chosenAffixes[j].IsPercent));
        }

        // Now all the values for the affixes are choosen, add the affixes that are the same from a different tier. 
        for (int j = 0; j < affixStats.Count; j++)
        {
            for (int k = 0; k < affixStats.Count; k++)
            {
                if (k == j) continue;
                if (affixStats[j].Modifier.affected == affixStats[k].Modifier.affected && affixStats[j].Modifier.modifierType == affixStats[k].Modifier.modifierType)
                {
                    affixStats[j].Modifier.value += affixStats[k].Modifier.value;
                    affixStats.RemoveAt(k);
                    k--;
                }
            }
        }

        return affixStats;
    }

    public BaseItem GetFlyWeightById(ItemType type, int id)
    {
        switch (type)
        {
            case ItemType.Armor:
                return itemContainer.Armor[id];

            case ItemType.Shield:
                return itemContainer.Shields[id];

            case ItemType.Jewerly:
                return itemContainer.Jewelry[id];

            case ItemType.Weapon:
                return itemContainer.Weapons[id];

            case ItemType.Potion:
                return itemContainer.Potions[id];

            case ItemType.Equipment:
                //TODO: remove equipment, for now selects one of the equipment types as random
                int randType = (int)UnityEngine.Random.Range(0, 4);
                return GetFlyWeightById((ItemType)1 + randType, id);

            default: return null;
        }
    }
        
    BaseItem GetFlyWeight(ItemType type, int minTier, int maxTier)
    {
        switch (type)
        {
            case ItemType.Armor:
                int rndArmor = UnityEngine.Random.Range(0, itemContainer.Armor.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier).Count);
                return itemContainer.Armor.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier)[rndArmor];
                
            case ItemType.Shield:
                int rndShield = UnityEngine.Random.Range(0, itemContainer.Shields.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier).Count);
                return itemContainer.Shields.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier)[rndShield];
                
            case ItemType.Jewerly:
                int rndJewel = UnityEngine.Random.Range(0, itemContainer.Jewelry.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier).Count);
                return itemContainer.Jewelry.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier)[rndJewel];
                
            case ItemType.Weapon:
                int rndWeapon = UnityEngine.Random.Range(0, itemContainer.Weapons.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier).Count);
                return itemContainer.Weapons.FindAll(w => w.Tier <= maxTier && w.Tier >= minTier)[rndWeapon];

            case ItemType.Potion:
                int randPotion = UnityEngine.Random.Range(0, itemContainer.Potions.Count);
                return itemContainer.Potions[randPotion];

            case ItemType.Equipment:
                //TODO: remove equipment, for now selects one of the equipment types as random
                int randType = (int)UnityEngine.Random.Range(0, 4);
                return GetFlyWeight((ItemType)1 + randType, minTier, maxTier);

            default: return null;
        }
    }

    public ItemInstance GetItemInstance(ItemType type)
    {
        if (type == ItemType.Equipment || type == ItemType.Random)
        {
            int randType = (int) UnityEngine.Random.Range(0, type == ItemType.Equipment ? 4 : 5);
            return GetItemInstance((ItemType) 1 + randType);
        }

        Quality quality = GetQuality();

        int level = (int)((int)(quality + 1) * UnityEngine.Random.Range(1, 2.5f));
        int maxTier = (Mathf.FloorToInt((float)(GameManager.Instance.ActiveCharacterInformation.Level + 3) / 10)) + 1;
        if (maxTier > 4) maxTier = 4;
        int minTier = ((GameManager.Instance.ActiveCharacterInformation.Level - 5) < ((maxTier == 1 ? maxTier : maxTier - 1) * 10)) ? maxTier - 1 : maxTier;

        GameObject toAdd = GameObject.Instantiate(DropPrefab);

        List<Stat> baseStats = new List<Stat>();
        Equipment equipment;

        string generatedName = "";

        ItemPrivateData itemData = new ItemPrivateData(quality);

        if (type == ItemType.Potion)
        {
            PotionInstance potionInstance = toAdd.AddComponent<PotionInstance>();
            potionInstance.Initialize(itemData, (BasePotion)GetFlyWeight(ItemType.Potion, minTier, maxTier));
            return potionInstance;
        }
        else
        {
            switch (type)
            {
                case ItemType.Armor:
                    ArmorInstance armorInstance = toAdd.AddComponent<ArmorInstance>();
                    BaseArmor armor = (BaseArmor) GetFlyWeight(ItemType.Armor, minTier, maxTier); // itemContainer.Armor.FindAll(a => a.Tier <= maxTier && a.Tier >= minTier)[randomItem];

                    equipment = (Equipment)armor.ArmorType;
                    level += (armor.Tier - 1) * 10;

                    List<AffixRange> chosenArmorAffixes = ChooseAffixes(quality, equipment, level);
                    List<Affix> armorAffixStats = DetermineAffixes(chosenArmorAffixes);

                    int armorValue = (int)armor.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();
                    Stat stat = new Stat(StatTypes.Armor, armorValue);
                    baseStats.Add(stat);

                    armorInstance.Initialize(itemData, new EquipmentPrivateData(armor, GetGeneratedName(chosenArmorAffixes, armor.Name), level, quality, baseStats, armorAffixStats), armor);
                    return armorInstance;
                    break;

                case ItemType.Shield:
                    ShieldInstance shieldInstance = toAdd.AddComponent<ShieldInstance>();
                    BaseShield shield = (BaseShield)GetFlyWeight(ItemType.Shield, minTier, maxTier);

                    equipment = Equipment.Shield;
                    level += (shield.Tier - 1) * 10;

                    List<AffixRange> chosenShieldAffixes = ChooseAffixes(quality, equipment, level);
                    List<Affix> shieldAffixStats = DetermineAffixes(chosenShieldAffixes);

                    int armorVal = (int)shield.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();
                    int blockChanceVal = (int)shield.BaseStats.Find(s => s.StatType == StatTypes.BlockChance).Range.GetRandomInRange();
                    int blockAmountVal = (int)shield.BaseStats.Find(s => s.StatType == StatTypes.BlockAmount).Range.GetRandomInRange();
                    Stat armorS = new Stat(StatTypes.Armor, armorVal);
                    Stat blockChance = new Stat(StatTypes.BlockChance, blockChanceVal);
                    Stat blockAmount = new Stat(StatTypes.BlockAmount, blockAmountVal);
                    baseStats.Add(armorS);
                    baseStats.Add(blockChance);
                    baseStats.Add(blockAmount);

                    shieldInstance.Initialize(itemData, new EquipmentPrivateData(shield, GetGeneratedName(chosenShieldAffixes, shield.Name), level, quality, baseStats, shieldAffixStats), shield);
                    return shieldInstance;

                case ItemType.Jewerly:
                    JewerlyInstance jewerlyInstance = toAdd.AddComponent<JewerlyInstance>();
                    BaseJewelry jewerly = (BaseJewelry)GetFlyWeight(ItemType.Jewerly, minTier, maxTier);

                    equipment = jewerly.JewelryType == JewelryType.Amulet ? Equipment.Amulet : Equipment.Ring;
                    level += (jewerly.Tier - 1) * 10;

                    List<AffixRange> chosenJewerlyAffixes = ChooseAffixes(quality, equipment, level);
                    List<Affix> jewerlyAffixStats = DetermineAffixes(chosenJewerlyAffixes);

                    jewerlyInstance.Initialize(itemData, new EquipmentPrivateData(jewerly, GetGeneratedName(chosenJewerlyAffixes, jewerly.Name), level, quality, new List<Stat>(), jewerlyAffixStats), jewerly);
                    return jewerlyInstance;

                case ItemType.Weapon:
                    WeaponInstance weaponInstance = toAdd.AddComponent<WeaponInstance>();
                    BaseWeapon weapon = (BaseWeapon)GetFlyWeight(ItemType.Weapon, minTier, maxTier);

                    equipment = Equipment.Weapon;
                    level += (weapon.Tier - 1) * 10;

                    // affixes
                    List<AffixRange> chosenWeaponAffixes = ChooseAffixes(quality, equipment, level);
                    List<Affix> weaponAffixStats = DetermineAffixes(chosenWeaponAffixes);

                    // base stats weapon
                    int damageValue = (int)weapon.BaseStats.Find(s => s.StatType == StatTypes.Damage).Range.GetRandomInRange();
                    float attackSpeedValue = (float)Math.Round(weapon.BaseStats.Find(s => s.StatType == StatTypes.AttackSpeed).Range.GetRandomInRange(), 2);
                    Stat damage = new Stat(StatTypes.WeaponDamage, damageValue);
                    Stat attackSpeed = new Stat(StatTypes.AttackSpeed, attackSpeedValue);
                    baseStats.Add(damage);
                    baseStats.Add(attackSpeed);

                    weaponInstance.Initialize(itemData, new EquipmentPrivateData(weapon, GetGeneratedName(chosenWeaponAffixes, weapon.Name), level, quality, baseStats, weaponAffixStats), weapon);
                    return weaponInstance;
            }
        }

        return null;
    }

    private string GetGeneratedName(List<AffixRange> chosenAffixes, string name)
    {
        string prefix = "";
        string suffix = "";

        if (chosenAffixes.Find(a => a.IsPrefix == true) != null)
        {
            List<AffixRange> prefixes = chosenAffixes.FindAll(a => a.IsPrefix);
            prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
        }
        if (chosenAffixes.Find(a => a.IsPrefix == false) != null)
        {
            List<AffixRange> suffixes = chosenAffixes.FindAll(a => !a.IsPrefix);
            suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
        }
        return (prefix + name + suffix);
    }

    public List<ItemInstance> GetItemInstances(int amount)
    {
        List<ItemInstance> returnInstances = new List<ItemInstance>();
        for (int i = 0; i < amount; i++)
        {
            returnInstances.Add(GetItemInstance(ItemType.Random));
        }
        return returnInstances;
    }

    Quality GetQuality()
    {
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
        return quality;
    }
}
