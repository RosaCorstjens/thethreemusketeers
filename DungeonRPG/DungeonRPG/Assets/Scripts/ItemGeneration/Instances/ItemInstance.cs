using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(BoxCollider))]
public class ItemInstance : MonoBehaviour 
{
    protected BaseItem itemInfo;
    public BaseItem ItemInfo { get { return itemInfo; } }

    protected bool dropped;                               
    public bool Dropped { get { return dropped; } set { dropped = value; } }

    protected Quality quality;
    public Quality Quality { get { return quality; } }

    public virtual void Initialize(BaseItem itemInfo, Quality quality)
    {
        // TO DO: give potions and gold their own material
        gameObject.GetComponent<Renderer>().material = ItemManager.Instance.QualityMaterials[(int)quality];

        gameObject.SetActive(false);

        //BaseItem x = new BaseArmor("Cool armor", "nope", ItemType.Equipment, 1, EquipmentType.Armor, StatTypes.Armor, null, ArmorType.Chest);

        //BaseArmor y = (BaseArmor)x;

        //Debug.Log(y.ArmorType);

        this.itemInfo = itemInfo;
        this.quality = quality;

        dropped = false;
    }

    public virtual void Use()
    {
        Debug.Log("Used item.");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!AddToInventory())
            {
                UIManager.Instance.InventoryFullWarning();
                return;
            }
            gameObject.SetActive(false);
            dropped = false;
        }
    }

    protected virtual bool AddToInventory()
    {
        return true;
    }

    public virtual void Drop()
    {
        dropped = true;

        gameObject.SetActive(true);
    }
}

