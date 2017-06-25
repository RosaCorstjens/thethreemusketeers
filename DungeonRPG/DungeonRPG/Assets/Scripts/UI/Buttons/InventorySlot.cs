using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : SlotBase
{
    public override void Initialize()
    {
        base.Initialize();

        slotType = SlotType.Inventory;
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        GameManager.Instance.UIManager.InventoryManager.EmptySlots++;

        Debug.Log("Item removed: " + GameManager.Instance.UIManager.InventoryManager.EmptySlots + " slots remaining");
    }

    public override void Use()
    {
        base.Use();
        EquipmentInstance temp = item;
        ClearSlot();

        // Take first item and use it. 
        if (!GameManager.Instance.UIManager.InventoryManager.EquipItem(temp))
        {
            AddItem(temp);
            return;
        }

        if (item == null)
        {
            GameManager.Instance.UIManager.InventoryManager.Reorder(this);
        }

        UpdateColorBasedOnPlayerLevel();
    }

    public void UpdateColorBasedOnPlayerLevel()
    {
        if (item == null) return;

        if (item.Level > GameManager.Instance.ActiveCharacterInformation.Level)
        {
            iconSprite.color = new Color(1, 1, 1, 0.3f);
            qualityColor.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            iconSprite.color = new Color(1, 1, 1, 1);
            qualityColor.color = new Color(1, 1, 1, 1);
        }
    }

    public override void AddItem(EquipmentInstance item)
    {
        base.AddItem(item);
        
        GameManager.Instance.UIManager.InventoryManager.EmptySlots--;
        Debug.Log("Item added: " + GameManager.Instance.UIManager.InventoryManager.EmptySlots + " slots remaining");
        UpdateColorBasedOnPlayerLevel();
    }
}
