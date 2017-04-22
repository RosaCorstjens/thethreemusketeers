using UnityEngine;
using System.Collections;

public class OldSnippets
{

    public void CreateItemXML()
    {
        //ItemContainer itemContainer = new ItemContainer();

        //Type[] itemTypes = { typeof(BasePotion), typeof(BaseShield), typeof(BaseWeapon), typeof(BaseArmor) };

        //FileStream fs = new FileStream(Application.streamingAssetsPath + "/Items.xml", FileMode.Open);

        //XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);

        //itemContainer = (ItemContainer)serializer.Deserialize(fs);

        //serializer.Serialize(fs, itemContainer);

        //fs.Close();

        //// loop through every list of ItemManager. 
        //for (int i = 0; i < itemManager.Potions.Count; i++)
        //{
        //    itemContainer.Potions.Add(itemManager.Potions[i]);
        //}

        //for (int i = 0; i < itemManager.Armor.Count; i++)
        //{
        //    itemContainer.Armor.Add(itemManager.Armor[i]);
        //}

        //for (int i = 0; i < itemManager.Weapons.Count; i++)
        //{
        //    itemContainer.Weapons.Add(itemManager.Weapons[i]);
        //}

        //for (int i = 0; i < itemManager.Shields.Count; i++)
        //{
        //    itemContainer.Shields.Add(itemManager.Shields[i]);
        //}

        //fs = new FileStream(Application.streamingAssetsPath + "/Items.xml", FileMode.Create);

        //serializer.Serialize(fs, itemContainer);

        //fs.Close();
    }

    private void CreateAffixXML()
    {
        //AffixContainer tempAffixContainer = new AffixContainer();

        //Type[] itemTypes = { typeof(Affix) };

        //FileStream fs = new FileStream(Application.streamingAssetsPath + "/Affixes.xml", FileMode.Open);

        //XmlSerializer serializer = new XmlSerializer(typeof(AffixContainer), itemTypes);

        //tempAffixContainer = (AffixContainer)serializer.Deserialize(fs);

        //serializer.Serialize(fs, affixContainer);

        //fs.Close();

        //for (int i = 0; i < affixContainer.Affixes.Count; i++)
        //{
        //    tempAffixContainer.Affixes.Add(affixContainer.Affixes[i]);
        //}

        //fs = new FileStream(Application.streamingAssetsPath + "/Affixes.xml", FileMode.Create);

        //serializer.Serialize(fs, tempAffixContainer);

        //fs.Close();
    }
}
