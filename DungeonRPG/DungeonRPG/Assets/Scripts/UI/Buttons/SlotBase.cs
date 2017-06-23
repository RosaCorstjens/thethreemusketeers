using UnityEngine;
using System.Collections;

// Currently not in use. 
public class SlotBase : ButtonBase
{
    protected EquipmentInstance item;
    public EquipmentInstance CurrentItem { get { return item; } }

    public bool IsEmpty { get { return item == null; } }

    protected UISprite qualityColor;
    protected UISprite iconSprite;

    public enum SlotType { Inventory, Equipment }
    protected SlotType slotType;
    public SlotType ThisSlotType { get { return slotType; } }

    public override void Initialize()
    {
        base.Initialize();

        qualityColor = transform.FindChild("QualityColor").GetComponent<UISprite>();
        iconSprite = transform.FindChild("Icon").GetComponent<UISprite>();
    }

    public override void HighlightButton()
    {
        if (IsEmpty) return;

        GameManager.Instance.UIManager.InventoryManager.ShowTooltip(item);

        base.HighlightButton();
    }

    public override void DeHighlightButton()
    {
        GameManager.Instance.UIManager.InventoryManager.HideTooltip();

        base.DeHighlightButton();
    }

    //Left click
    public virtual void ShowDetailedInformation()
    {
        if (IsEmpty) return;

        DeHighlightButton();

        GameManager.Instance.UIManager.InventoryManager.ShowDetailedInformation(this);
    }

    // Right click
    public virtual void ShowActions()
    {
        if (IsEmpty) return;

        GameManager.Instance.UIManager.InventoryManager.ShowActionMenu(this);
    }

    public virtual void AddItem(EquipmentInstance item)
    {
        iconSprite.gameObject.SetActive(true);
        qualityColor.gameObject.SetActive(true);

        this.item = item;

        iconSprite.spriteName = item.ItemInfo.SpriteName;
        qualityColor.color = GameManager.Instance.ItemManager.QualityColors[(int)item.Quality];
    }

    // Double click
    public virtual void Use()
    {
        if (IsEmpty) return;

    }

    public virtual void Drop()
    {
        item.Drop();
        ClearSlot();

        GameManager.Instance.UIManager.InventoryManager.AlertItemDropped(this);
    }

    public virtual void ClearSlot()
    {
        item = null;

        iconSprite.gameObject.SetActive(false);
        qualityColor.gameObject.SetActive(false);

        DeHighlightButton();
    }
}
