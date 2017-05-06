using UnityEngine;
using System.Collections;

public class PotionInstance : ItemInstance 
{
    private BasePotion basePotion;
    public BasePotion BasePotion { get { return basePotion; } }

    public void Initialize(BasePotion itemInfo, Quality quality)
    {
        base.Initialize(itemInfo, quality);

        gameObject.SetActive(false);

        basePotion = itemInfo;

        dropped = false;

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
