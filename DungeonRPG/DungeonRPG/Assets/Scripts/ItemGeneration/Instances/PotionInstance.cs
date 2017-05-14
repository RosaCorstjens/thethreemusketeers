using UnityEngine;
using System.Collections;

public class PotionInstance : ItemInstance 
{
    private BasePotion basePotion;
    public BasePotion BasePotion { get { return basePotion; } }

    public void Initialize(ItemPrivateData itemData, BasePotion potionData)
    {
        base.Initialize(itemData);
        basePotion = potionData;
        gameObject.GetComponent<Renderer>().material = ItemManager.Instance.QualityMaterials[0];
    }

    public override void Use()
    {
        Debug.Log("Potion used.");
    }

    protected override bool AddToInventory()
    {
        return UIManager.Instance.InventoryManager.AddItem(this);
    }

}
