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
    }

    public override void Use()
    {
        base.Use();
        
        // Take first item and use it. 
        if(!GameManager.Instance.UIManager.InventoryManager.EquipItem(item)) return;

        ClearSlot();
        GameManager.Instance.UIManager.InventoryManager.Reorder(this);
    }

    public void UpdateColorBasedOnPlayerLevel()
    {
        if (item == null) return;

        if (item.Level > GameManager.Instance.ActiveCharacterInformation.Level)
        {
            iconSprite.color = new Color(0.2f, 0.1f, 0.1f, 1.0f);
            qualityColor.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            iconSprite.color = Color.white;
            qualityColor.color = new Color(1, 1, 1, 1);
        }
    }
}
