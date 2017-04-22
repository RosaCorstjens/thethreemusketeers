using UnityEngine;
using System.Collections;

public class ItemActionMenu : MonoBehaviour
{
    UIWidget equipButton;
    UILabel equipLabel;
    UIWidget detailsButton;
    UIWidget dropButton;

    public void Initialize()
    {
        equipButton = transform.FindChild("Container/Grid/EquipButton").GetComponent<UIWidget>();
        equipLabel = transform.FindChild("Container/Grid/EquipButton/Label").GetComponent<UILabel>();
        detailsButton = transform.FindChild("Container/Grid/DetailsButton").GetComponent<UIWidget>();
        dropButton = transform.FindChild("Container/Grid/DropButton").GetComponent<UIWidget>();
    }

    public void Equip()
    {
        GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.Use();
    }

    public void ShowDetails()
    {
        GameManager.Instance.UIManager.InventoryManager.ShowDetailedInformation();
    }

    public void Drop()
    {
        GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.Drop();
    }

    public void Open(SlotBase slot)
    {
        equipLabel.text = slot.ThisSlotType == SlotBase.SlotType.Equipment ? "Deequip" : "Equip";
        SetPosition();

        gameObject.SetActive(true);
    }

    public void SetPosition()
    {
        Vector3 position;

        position = Camera.main.WorldToNormalizedViewportPoint(Input.mousePosition);
        position = GameManager.Instance.UIManager.UICamera.ScreenToViewportPoint(Input.mousePosition);
        position = GameManager.Instance.UIManager.UICamera.NormalizedViewportToWorldPoint(position);
        position.z = 0;
        transform.position = position;
    }

    public void Close()
    {
        GameManager.Instance.UIManager.InventoryManager.HideActionMenu();
    }
}
