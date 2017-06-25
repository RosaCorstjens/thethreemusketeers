using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ItemInstance : MonoBehaviour 
{
    protected BaseItem itemInfo;
    public BaseItem ItemInfo { get { return itemInfo; } }

    protected bool dropped;                               
    public bool Dropped { get { return dropped; } set { dropped = value; } }

    protected Quality quality;
    public Quality Quality { get { return quality; } }

    private float attrationSpeed = 3.0f;

    private Coroutine fallingRoutine;

    public virtual void Initialize(BaseItem itemInfo, Quality quality)
    {
        // TO DO: give potions and gold their own material
        gameObject.GetComponent<Renderer>().material = GameManager.Instance.ItemManager.QualityMaterials[(int)quality];

        gameObject.SetActive(false);

        this.itemInfo = itemInfo;
        this.quality = quality;

        dropped = false;

        fallingRoutine = null;
    }

    public virtual void Use()
    {
        Debug.Log("Used item.");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.Instance.UIManager.InventoryManager.EmptySlots <= 0)
            {
                GameManager.Instance.UIManager.InventoryFullWarning();
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.Instance.UIManager.InventoryManager.EmptySlots <= 0)
            {
                if (fallingRoutine == null)
                {
                    fallingRoutine = StartCoroutine(FallingDown());
                }
                return;
            }

            transform.position += (other.transform.position - new Vector3(transform.position.x, transform.position.y - 1, transform.position.z)).normalized * attrationSpeed * Time.deltaTime;

            if ((other.transform.position - new Vector3(transform.position.x, transform.position.y - 1, transform.position.z)).magnitude < 1.0f)
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

    private IEnumerator FallingDown()
    {
        while (transform.position.y > 0.3f)
        {
            transform.position -= new Vector3(0, attrationSpeed / 2 * Time.deltaTime, 0);
            yield return null;
        }

        if (transform.position.y <= 0.3f)
        {
            transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        }

        fallingRoutine = null;

        yield break;
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

