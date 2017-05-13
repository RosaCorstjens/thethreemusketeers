using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public enum EquipmentSlotType { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, MainHand, OffHand }
public enum Equipment { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, Weapon, Shield }

public enum Quality { Common, Magic, Rare, Legendary }

public class ItemManager 
{
    private ItemManager() { }

    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemManager();
                instance.Initialize();
            }
            return instance;
        }
    }

    private ItemFactory factory;
    public ItemFactory Factory
    {
        get
        {
            return factory;
        }

        set
        {
            factory = value;
        }
    }
    public void Initialize()
    {
        // quality colors
        qualityColors.Add(new Color(211f / 255f, 211f / 255f, 211f / 255f));
        qualityColors.Add(new Color(0, 117f / 255f, 1f));
        qualityColors.Add(new Color(1f, 237f / 255f, 0));
        qualityColors.Add(new Color(1f, 89f / 255f, 0));

        // hex colors
        qualityHexColors.Add("D3D3D3");
        qualityHexColors.Add("0075FF");
        qualityHexColors.Add("FFED00");
        qualityHexColors.Add("FF5900");

        // materials
        qualityMaterials.Add(Resources.Load<Material>("Materials/Items/DroppedItems/emissive_white"));
        qualityMaterials.Add(Resources.Load<Material>("Materials/Items/DroppedItems/emissive_blue"));
        qualityMaterials.Add(Resources.Load<Material>("Materials/Items/DroppedItems/emissive_yellow"));
        qualityMaterials.Add(Resources.Load<Material>("Materials/Items/DroppedItems/emissive_orange"));

        Factory = new ItemFactory();
        Debug.Log("Items: "+ Factory.ItemAmount());
    }

    // COLOR STUFF
    private List<Color> qualityColors = new List<Color>();
    public List<Color> QualityColors { get { return qualityColors; } }

    private List<string> qualityHexColors = new List<string>();
    public List<string> QualityHexColors { get { return qualityHexColors; } }

    private List<Material> qualityMaterials = new List<Material>();
    public List<Material> QualityMaterials { get { return qualityMaterials; } }
}
