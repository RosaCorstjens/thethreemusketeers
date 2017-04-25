using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


[Serializable]
public class ItemManager
{
    [SerializeField]
    private ItemContainer itemContainer = new ItemContainer();
    public ItemContainer ItemContainer { get { return itemContainer; } }

    private ItemGenerator itemGenerator;
    public ItemGenerator ItemGenerator { get { return itemGenerator; } }

    public void Initialize()
    {
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
