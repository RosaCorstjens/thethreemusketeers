using UnityEngine;
using System.Collections;

public class InventoryMenu : MonoBehaviour
{
    Color normalColor = Color.black;
    Color selectedColor = Color.white;

    HooverButton inventory;
    HooverButton details;

    public void Initialize()
    {
        inventory = transform.FindChild("Grid/InventoryButton").GetComponent<HooverButton>();
        details = transform.FindChild("Grid/DetailsButton").GetComponent<HooverButton>();

        inventory.Initlialize();
        details.Initlialize();
        inventory.Selected = true;
    }

    public void ShowInventory()
    {
        inventory.Selected = true;
        details.Selected = false;

        GameManager.Instance.UIManager.InventoryManager.ChangeState(InventoryManager.InventoryStates.Inventory);
    }

    public void ShowCharacterDetails()
    {
        inventory.Selected = false;
        details.Selected = true;

        GameManager.Instance.UIManager.InventoryManager.ChangeState(InventoryManager.InventoryStates.Details);
    }

    public void ShowSkills()
    {

    }
}
