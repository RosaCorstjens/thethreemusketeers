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

    private float attrationSpeed = 5.0f;

    public virtual void Initialize(BaseItem itemInfo, Quality quality)
    {
        // TO DO: give potions and gold their own material
        gameObject.GetComponent<Renderer>().material = GameManager.Instance.ItemManager.QualityMaterials[(int)quality];

        gameObject.SetActive(false);

        this.itemInfo = itemInfo;
        this.quality = quality;

        dropped = false;
    }

    public virtual void Use()
    {
        Debug.Log("Used item.");
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.position += (other.transform.position - new Vector3(transform.position.x, transform.position.y - 1, transform.position.z)).normalized * attrationSpeed * Time.deltaTime;

            if ((other.transform.position - transform.position).magnitude < 1.0f)
            {
                if (!AddToInventory())
                {
                    GameManager.Instance.UIManager.InventoryFullWarning();
                    return;
                }
                gameObject.SetActive(false);
                dropped = false;
            }
        }
    }

    protected virtual bool AddToInventory()
    {
        return true;
    }

    public virtual void Drop()
    {
        GameManager.Instance.DungeonManager.CurrentDungeon.AddItem(this);

        dropped = true;

        gameObject.SetActive(true);
    }
}

