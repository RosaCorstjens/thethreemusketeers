using UnityEngine;
using System.Collections;

public class InventoryMenu : MonoBehaviour
{
    Color normalColor = Color.black;
    Color selectedColor = Color.white;

    HooverButton inventory;
    HooverButton details;
    HooverButton skills;

    public void Initialize()
    {
        inventory = transform.FindChild("Grid/InventoryButton").GetComponent<HooverButton>();
        details = transform.FindChild("Grid/DetailsButton").GetComponent<HooverButton>();
        skills = transform.FindChild("Grid/SkillsButton").GetComponent<HooverButton>();

        inventory.Initlialize();
        details.Initlialize();
        skills.Initlialize();
        inventory.Selected = true;
    }

    public void ShowInventory()
    {
        inventory.Selected = true;
        details.Selected = false;
        skills.Selected = false;

        UIManager.Instance.InventoryManager.ChangeState(InventoryManager.InventoryStates.Inventory);
    }

    public void ShowCharacterDetails()
    {
        inventory.Selected = false;
        details.Selected = true;
        skills.Selected = false;

        UIManager.Instance.InventoryManager.ChangeState(InventoryManager.InventoryStates.Details);
    }

    public void ShowSkills()
    {

    }
}
