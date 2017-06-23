using UnityEngine;
using System.Collections;

public class HUDManager
{
    private GameObject hud;
    public GameObject HUD { get { return hud; } }

    private HUDBar healthBar;
    public HUDBar HealthBar { get { return healthBar; } }

    private UILabel keysLabel;
    private UILabel multiKeysLabel;
    private UILabel finalKeysLabel;
    private UILabel healthPotionsLabel;

    private UILabel levelLabel;
    private UILabel floorLabel;

    public void Initialize()
    {
        hud = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InGameUI/HUD"));
        hud.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        hud.transform.localPosition = Vector3.zero;
        hud.transform.localScale = Vector3.one;

        healthBar = hud.transform.FindChild("Anchor_BottomLeft/HealthBarContainer").GetComponent<HUDBar>();
        healthBar.Initialize();

        healthPotionsLabel = hud.transform.FindChild("Anchor_BottomRight/StandardItems/HealthPotions/Label").GetComponent<UILabel>();
        keysLabel = hud.transform.FindChild("Anchor_BottomRight/StandardItems/Keys/Label").GetComponent<UILabel>();
        multiKeysLabel = hud.transform.FindChild("Anchor_BottomRight/StandardItems/MultiKeys/Label").GetComponent<UILabel>();
        finalKeysLabel = hud.transform.FindChild("Anchor_BottomRight/StandardItems/FinalKeys/Label").GetComponent<UILabel>();

        levelLabel = hud.transform.FindChild("Anchor_TopLeft/Level/Label").GetComponent<UILabel>();
        floorLabel = hud.transform.FindChild("Anchor_TopLeft/Floor/Label").GetComponent<UILabel>();

        healthPotionsLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.HealthPotionAmount + " (F)";
        keysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.KeyAmount;
        multiKeysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.MultiKeyAmount;
        finalKeysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.FinalKeyAmount;

        levelLabel.text = "Lvl. " + 1;
        floorLabel.text = "Floor: " + 1;
    }

    public void UpdatePotionValue()
    {
        healthPotionsLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.HealthPotionAmount + " (F)";
    }

    public void UpdateKeyValue(KeyType type)
    {
        switch (type)
        {
            case KeyType.Normal:
                keysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.KeyAmount;
                break;
            case KeyType.Multi:
                multiKeysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.MultiKeyAmount;
                break;
            case KeyType.Final:
                finalKeysLabel.text = "" + GameManager.Instance.UIManager.InventoryManager.FinalKeyAmount;
                break;
        }
    }

    public void UpdateLevelText()
    {
        levelLabel.text = "Lvl. " + GameManager.Instance.ActiveCharacterInformation.Level;
    }

    public void UpdateFloorText()
    {
        floorLabel.text = "Floor: " + GameManager.Instance.DungeonManager.CurrentLevel;
    }
}