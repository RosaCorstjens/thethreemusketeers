using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(BoxCollider))]
public class ItemInstance : MonoBehaviour
{
    protected ItemPrivateData itemData;
    public ItemPrivateData ItemData { get { return itemData; } }

    protected BaseItem baseItem;
    public BaseItem BaseItem { get { return baseItem; } }

    public virtual void Initialize(ItemPrivateData itemData, BaseItem baseItem)
    {
        this.itemData = itemData;
        this.baseItem = baseItem;

        gameObject.GetComponent<Renderer>().material = ItemManager.Instance.QualityMaterials[(int)itemData.Quality];
        gameObject.SetActive(false);
        itemData.Dropped = false;
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
            itemData.Dropped = false;
        }
    }

    protected virtual bool AddToInventory()
    {
        return true;
    }

    public virtual void Drop()
    {
        itemData.Dropped = true;
        gameObject.SetActive(true);
    }
}

