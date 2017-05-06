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

        UIManager.Instance.InventoryManager.EmptySlots++;
    }

    public override void Use()
    {
        base.Use();
        
        // Take first item and use it. 
        if(!UIManager.Instance.InventoryManager.EquipItem(item)) return;

        ClearSlot();
        UIManager.Instance.InventoryManager.Reorder(this);
    }

}
