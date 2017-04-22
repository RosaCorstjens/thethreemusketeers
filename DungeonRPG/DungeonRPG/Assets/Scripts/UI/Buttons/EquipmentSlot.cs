using UnityEngine;
using System.Collections;

public class EquipmentSlot : SlotBase
{
    private EquipmentSlotType equipmentType;

    private UISprite emptyIcon;

    public void Initialize(EquipmentSlotType _equipmentType)
    {
        base.Initialize();

        slotType = SlotType.Equipment;

        equipmentType = _equipmentType;

        emptyIcon = transform.FindChild("EmptyIcon").GetComponent<UISprite>();

        emptyIcon.spriteName = "empty_" + equipmentType.ToString(); 
    }

    public override void AddItem(EquipmentInstance item)
    {
        if (!IsEmpty) Use();

        item.Equipped = true;

        if (item.BaseEquipment.EquipmentType == EquipmentType.Weapon) GameManager.Instance.ActiveCharacterInformation.PlayerController.SetHand(item.GetComponent<WeaponInstance>());
        if (item.BaseEquipment.EquipmentType == EquipmentType.Shield) GameManager.Instance.ActiveCharacterInformation.PlayerController.SetOffHand(item.GetComponent<ShieldInstance>());

        emptyIcon.gameObject.SetActive(false);

        base.AddItem(item);
    }

    public override void Use()
    {
        base.Use();

        iconSprite.spriteName = "empty_" + equipmentType.ToString().ToLower();

        item.Equipped = false;

        GameManager.Instance.UIManager.InventoryManager.AddItem(item);

        ClearSlot();
    }

    public override void ClearSlot()
    {
        if (item.BaseEquipment.EquipmentType == EquipmentType.Weapon) GameManager.Instance.ActiveCharacterInformation.PlayerController.RemoveFromHand();
        if (item.BaseEquipment.EquipmentType == EquipmentType.Shield) GameManager.Instance.ActiveCharacterInformation.PlayerController.RemoveFromOffHand();

        base.ClearSlot();

        emptyIcon.gameObject.SetActive(true);

        GameManager.Instance.UIManager.InventoryManager.CharacterPanel.CalculateEquippedStats();
    }

    public override void Drop()
    {
        item.Equipped = false;

        base.Drop();
    }
}
