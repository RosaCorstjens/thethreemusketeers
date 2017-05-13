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

    List<AffixRange> ChooseAffixes(Quality quality, Equipment equipment, int level)
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

    List<Affix> DetermineAffixes(List<AffixRange> chosenAffixes)
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
        Quality quality = getQuality();

        int level = (int)((int)(quality + 1) * UnityEngine.Random.Range(1, 2.5f));
        int maxTier = (Mathf.FloorToInt((float)(GameManager.Instance.ActiveCharacterInformation.Level + 3) / 10)) + 1;
        if (maxTier > 4) maxTier = 4;
        int minTier = ((GameManager.Instance.ActiveCharacterInformation.Level - 5) < ((maxTier == 1 ? maxTier : maxTier - 1) * 10)) ? maxTier - 1 : maxTier;

        GameObject toAdd = GameObject.Instantiate(DropPrefab);

        List<Stat> baseStats = new List<Stat>();
        Equipment equipment;
        string generatedName = "";
        string prefix = "";
        string suffix = "";


        switch (type)
        {
            case ItemType.Armor:
                int randomArmor = UnityEngine.Random.Range(0, itemContainer.Armor.FindAll(a => a.Tier <= maxTier && a.Tier >= minTier).Count);
                BaseArmor armor = itemContainer.Armor.FindAll(a => a.Tier <= maxTier && a.Tier >= minTier)[randomArmor];

                equipment = (Equipment)armor.ArmorType;
                generatedName = armor.Name;
                level += (armor.Tier - 1) * 10;
                int armorValue = (int)armor.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();

                List<AffixRange> chosenArmorAffixes = ChooseAffixes(quality, equipment, level);

                //determine new name
                prefix = "";
                suffix = "";
                if (chosenArmorAffixes.Find(a => a.IsPrefix == true) != null)
                {
                    List<AffixRange> prefixes = chosenArmorAffixes.FindAll(a => a.IsPrefix);
                    prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
                }
                if (chosenArmorAffixes.Find(a => a.IsPrefix == false) != null)
                {
                    List<AffixRange> suffixes = chosenArmorAffixes.FindAll(a => !a.IsPrefix);
                    suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
                }
                generatedName = prefix + generatedName + suffix;

                List<Affix> armorAffixStats = DetermineAffixes(chosenArmorAffixes);

                Stat stat = new Stat(StatTypes.Armor, armorValue);
                baseStats.Add(stat);

                ArmorInstance armorInstance = toAdd.AddComponent<ArmorInstance>();
                armorInstance.Initialize((BaseArmor)GetFlyWeight(ItemType.Armor, minTier, maxTier), quality, level, generatedName, baseStats, armorAffixStats);
                return armorInstance;

                break;
            case ItemType.Shield:
                int randomShield = UnityEngine.Random.Range(0, itemContainer.Shields.FindAll(s => s.Tier <= maxTier && s.Tier >= minTier).Count);
                BaseShield baseShield = itemContainer.Shields.FindAll(s => s.Tier <= maxTier && s.Tier >= minTier)[randomShield];

                equipment = Equipment.Shield;
                generatedName = baseShield.Name;
                level += (baseShield.Tier - 1) * 10;

                List<AffixRange> chosenShieldAffixes = ChooseAffixes(quality, equipment, level);
                //determine new name
                prefix = "";
                suffix = "";
                if (chosenShieldAffixes.Find(a => a.IsPrefix == true) != null)
                {
                    List<AffixRange> prefixes = chosenShieldAffixes.FindAll(a => a.IsPrefix);
                    prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
                }
                if (chosenShieldAffixes.Find(a => a.IsPrefix == false) != null)
                {
                    List<AffixRange> suffixes = chosenShieldAffixes.FindAll(a => !a.IsPrefix);
                    suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
                }
                generatedName = prefix + generatedName + suffix;
                List<Affix> shieldAffixStats = DetermineAffixes(chosenShieldAffixes);

                int armorVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.Armor).Range.GetRandomInRange();
                int blockChanceVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.BlockChance).Range.GetRandomInRange();
                int blockAmountVal = (int)baseShield.BaseStats.Find(s => s.StatType == StatTypes.BlockAmount).Range.GetRandomInRange();
                Stat armorS = new Stat(StatTypes.Armor, armorVal);
                Stat blockChance = new Stat(StatTypes.BlockChance, blockChanceVal);
                Stat blockAmount = new Stat(StatTypes.BlockAmount, blockAmountVal);
                baseStats.Add(armorS);
                baseStats.Add(blockChance);
                baseStats.Add(blockAmount);

                ShieldInstance shieldInstance = toAdd.AddComponent<ShieldInstance>();
                shieldInstance.Initialize((BaseShield)GetFlyWeight(ItemType.Shield, minTier, maxTier), quality, level, generatedName, baseStats, shieldAffixStats);
                return shieldInstance;

            case ItemType.Jewerly:
                int randomJewel = UnityEngine.Random.Range(0, itemContainer.Jewelry.FindAll(j => j.Tier <= maxTier && j.Tier >= minTier).Count);
                BaseJewelry baseJewerly = itemContainer.Jewelry.FindAll(j => j.Tier <= maxTier && j.Tier >= minTier)[randomJewel];

                equipment = baseJewerly.JewelryType == JewelryType.Amulet ? Equipment.Amulet : Equipment.Ring;
                generatedName = baseJewerly.Name;
                level += (baseJewerly.Tier - 1) * 10;

                List<AffixRange> chosenJewerlyAffixes = ChooseAffixes(quality, equipment, level);
                //determine new name
                prefix = "";
                suffix = "";
                if (chosenJewerlyAffixes.Find(a => a.IsPrefix == true) != null)
                {
                    List<AffixRange> prefixes = chosenJewerlyAffixes.FindAll(a => a.IsPrefix);
                    prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
                }
                if (chosenJewerlyAffixes.Find(a => a.IsPrefix == false) != null)
                {
                    List<AffixRange> suffixes = chosenJewerlyAffixes.FindAll(a => !a.IsPrefix);
                    suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
                }
                generatedName = prefix + generatedName + suffix;
                List<Affix> jewerlyAffixStats = DetermineAffixes(chosenJewerlyAffixes);

                JewerlyInstance jewerlyInstance = toAdd.AddComponent<JewerlyInstance>();
                jewerlyInstance.Initialize(baseJewerly, quality, level, generatedName, new List<Stat>(), jewerlyAffixStats);
                return jewerlyInstance;

            case ItemType.Weapon:
                equipment = Equipment.Weapon;
                BaseWeapon weapon = (BaseWeapon)GetFlyWeight(ItemType.Weapon, minTier, maxTier);
                generatedName = weapon.Name;
                level += (weapon.Tier - 1) * 10;

                List<AffixRange> chosenWeaponAffixes = ChooseAffixes(quality, equipment, level);
                //determine new name
                prefix = "";
                suffix = "";
                if (chosenWeaponAffixes.Find(a => a.IsPrefix == true) != null)
                {
                    List<AffixRange> prefixes = chosenWeaponAffixes.FindAll(a => a.IsPrefix);
                    prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Count)].Name + " ";
                }
                if (chosenWeaponAffixes.Find(a => a.IsPrefix == false) != null)
                {
                    List<AffixRange> suffixes = chosenWeaponAffixes.FindAll(a => !a.IsPrefix);
                    suffix = " " + suffixes[UnityEngine.Random.Range(0, suffixes.Count)].Name;
                }
                generatedName = prefix + generatedName + suffix;
                List<Affix> weaponAffixStats = DetermineAffixes(chosenWeaponAffixes);

                int damageValue = (int)weapon.BaseStats.Find(s => s.StatType == StatTypes.Damage).Range.GetRandomInRange();
                float attackSpeedValue = (float)Math.Round(weapon.BaseStats.Find(s => s.StatType == StatTypes.AttackSpeed).Range.GetRandomInRange(), 2);
                Stat damage = new Stat(StatTypes.WeaponDamage, damageValue);
                Stat attackSpeed = new Stat(StatTypes.AttackSpeed, attackSpeedValue);
                baseStats.Add(damage);
                baseStats.Add(attackSpeed);

                WeaponInstance weaponInstance = toAdd.AddComponent<WeaponInstance>();
                weaponInstance.Initialize((BaseWeapon)GetFlyWeight(ItemType.Weapon, minTier, maxTier), quality, level, generatedName, baseStats, weaponAffixStats);
                return weaponInstance;

            case ItemType.Potion:
                PotionInstance potionInstance = toAdd.AddComponent<PotionInstance>();
                potionInstance.Initialize((BasePotion)GetFlyWeight(ItemType.Potion, minTier, maxTier), quality);
                return potionInstance;

            case ItemType.Equipment:
                //TODO: remove equipment, for now selects one of the equipment types as random
                int randEquipment = (int)UnityEngine.Random.Range(0, 4);
                Debug.Log("picked as random type" + (ItemType)1 + randEquipment);
                return GetItemInstance((ItemType) 1 + randEquipment);

            case ItemType.Random:
                int randType = (int)UnityEngine.Random.Range(0, 5);
                return GetItemInstance((ItemType)1 + randType);
        }
        return null;
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

    Quality getQuality()
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

    // COLOR STUFF
    [SerializeField]
    private List<Color> qualityColors = new List<Color>();
    public List<Color> QualityColors { get { return qualityColors; } }

    [SerializeField]
    private List<string> qualityHexColors = new List<string>();
    public List<string> QualityHexColors { get { return qualityHexColors; } }

    [SerializeField]
    private List<Material> qualityMaterials = new List<Material>();
    public List<Material> QualityMaterials { get { return qualityMaterials; } }
}
