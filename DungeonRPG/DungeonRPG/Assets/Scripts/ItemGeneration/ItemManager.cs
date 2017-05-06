using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public enum EquipmentSlotType { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, MainHand, OffHand }
public enum Equipment { Belt, Boots, Bracers, Chest, Helmet, Pants, Gloves, Shoulders, Ring, Amulet, Weapon, Shield }

public enum Quality { Common, Magic, Rare, Legendary }

public class ItemManager : Singleton<ItemManager>, ISingletonInstance
{
    private ItemContainer itemContainer = new ItemContainer();
    public ItemContainer ItemContainer { get { return itemContainer; } }

    private ItemGenerator itemGenerator;
    public ItemGenerator ItemGenerator { get { return itemGenerator; } }

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

        itemGenerator = new ItemGenerator();
        itemGenerator.Initialize();
        ReadItemXML();
        Debug.Log("Items: "+ itemContainer.ItemAmount());
    }

    public void ReadItemXML()
    {
        Type[] itemTypes = { typeof(BasePotion), typeof(BaseShield), typeof(BaseWeapon), typeof(BaseArmor), typeof(BaseJewelry) };

        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);

        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Items.xml");

        itemContainer = (ItemContainer) serializer.Deserialize(textReader);

        textReader.Close();
    }

    // COLOR STUFF
    private List<Color> qualityColors = new List<Color>();
    public List<Color> QualityColors { get { return qualityColors; } }

    private List<string> qualityHexColors = new List<string>();
    public List<string> QualityHexColors { get { return qualityHexColors; } }

    private List<Material> qualityMaterials = new List<Material>();
    public List<Material> QualityMaterials { get { return qualityMaterials; } }
}
