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
}
