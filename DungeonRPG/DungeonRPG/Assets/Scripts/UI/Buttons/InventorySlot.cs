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
        if (!GameManager.Instance.UIManager.InventoryManager.EquipItem(item)) return;

        ClearSlot();
        GameManager.Instance.UIManager.InventoryManager.Reorder(this);

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

        UpdateColorBasedOnPlayerLevel();
    }
}
