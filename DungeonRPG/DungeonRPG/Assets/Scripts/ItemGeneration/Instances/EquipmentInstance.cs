using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentInstance: ItemInstance
{
    protected EquipmentPrivateData equipmentData;
    public EquipmentPrivateData EquipmentData { get { return equipmentData; } }

    protected BaseEquipment baseEquipment;
    public BaseEquipment BaseEquipment { get { return baseEquipment; } }

    public bool Equipped
    {
        get { return equipmentData.Equipped; }
        set
        {
            equipmentData.Equipped = value;
            if (equipmentData.Equipped) GameManager.Instance.ActiveCharacterInformation.AddStats(this);
            else GameManager.Instance.ActiveCharacterInformation.RemoveStats(this);
            UIManager.Instance.InventoryManager.CharacterPanel.SetPlayerInformation();
        }
    }

    public float MainStatValue { get { return equipmentData.BaseStats.Find(b => b.StatType == BaseEquipment.MainStat).Value; } }

    public void Initialize(ItemPrivateData itemData, EquipmentPrivateData equipmentData, BaseEquipment baseEquipment)
    {
        base.Initialize(itemData, baseEquipment);

        this.equipmentData = equipmentData;
        this.baseEquipment = baseEquipment;
    }

    public override void Use()
    {
        Debug.Log(BaseItem.Name + " equipped.");
    }

    public override void Drop()
    {
        base.Drop();

        float angle = UnityEngine.Random.Range(-45, 45);

        Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        v *= 3; 

        gameObject.transform.position = GameManager.Instance.ActiveCharacterInformation.PlayerController.transform.position - v;

        itemData.Dropped = true;

        gameObject.SetActive(true);
    }

    protected override bool AddToInventory()
    {
        return UIManager.Instance.InventoryManager.AddItem(this);
    }
}
