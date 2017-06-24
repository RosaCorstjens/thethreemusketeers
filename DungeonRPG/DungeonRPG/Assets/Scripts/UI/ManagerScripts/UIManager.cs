using UnityEngine;
using System.Collections;

public class UIManager
{
    // Reference to root of all UI elements to parent them.
    private UIRoot uiRoot;                               
    public UIRoot UIRoot { get { return uiRoot; } }

    // Reference to top anchor.
    private UIAnchor anchorTop;
    public UIAnchor AnchorTop { get { return anchorTop; } }

    // Reference to UI camera.
    private Camera uiCamera;
    public Camera UICamera { get { return uiCamera; } }

    // The inventory manager. 
    private InventoryManager inventoryManager;
    public InventoryManager InventoryManager { get { return inventoryManager; } }

    // The worldUI manager.
    private WorldUIManager worldUIManager;
    public WorldUIManager WorldUIManager { get { return worldUIManager; } }

    private HUDManager hudManager;
    public HUDManager HudManager { get { return hudManager; } }

    private MegaMiniMapManager megaMiniMapManager;
    public MegaMiniMapManager MegaMiniMapManager { get { return megaMiniMapManager; } }

    // The label that appears when the inventory reached max capicity. 
    private UILabel warningMessage;
    private const string INVENTORY_FULL_WARNING = "Your inventory is full.";
    private const string EQUIP_WEAPON_WARNING = "Equip a weapon to attack.";
    private const string YOU_DIED = "You died...";
    private const string LEVEL_UP = "Level up!";
    private const string NO_ENGERGY_POTIONS = "You have no energy potions.";
    private const string NO_HEALTH_POTIONS = "You have no health potions.";
    private const string NEXT_DUNGEON = "Welcome in the next dungeon ...";
    private const string NO_KEY = "You have no corresponding keys";
    private const string YOU_REACHED_CHECKPOINT = "You have reached a checkpoint!";

    public void Initialize()
    {
        // Get the root.
        uiRoot = GameObject.FindObjectOfType<UIRoot>();

        anchorTop = uiRoot.transform.FindChild("Anchor_Top").GetComponent<UIAnchor>();

        // Get the camera. 
        uiCamera = uiRoot.transform.FindChild("Camera").GetComponent<Camera>();

        warningMessage = GameObject.Instantiate(Resources.Load<UILabel>("Prefabs/UI/InGameUI/WarningMessage"));
        warningMessage.transform.SetParent(GameManager.Instance.UIManager.AnchorTop.transform);
        warningMessage.transform.localPosition = Vector3.zero;
        warningMessage.transform.localScale =  Vector3.one;
        warningMessage.gameObject.SetActive(false);
    }

    // Call to initialize the UIManager. 
    public void InitializeGameUI()
    {
        // Create and initialize inventory manager. 
        inventoryManager = new InventoryManager();
        inventoryManager.Initialize();

        // Create and initialize inventory manager. 
        worldUIManager = new WorldUIManager();
        worldUIManager.Initialize();

        hudManager = new HUDManager();
        hudManager.Initialize();

        megaMiniMapManager = new MegaMiniMapManager();
        megaMiniMapManager.Initialize();
    }

    public void InventoryFullWarning()
    {
        warningMessage.text = INVENTORY_FULL_WARNING;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(1));
    }

    public void EquipWeaponWarning()
    {
        warningMessage.text = EQUIP_WEAPON_WARNING;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(1));
    }

    public void YouDiedWarning()
    {
        warningMessage.text = YOU_DIED;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(3));
    }

    public void YouReachedCheckpoint()
    {
        warningMessage.text = YOU_REACHED_CHECKPOINT;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(3));
    }

    public void YouDoNotHaveAKeyWarning()
    {
        warningMessage.text = NO_KEY;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(1));
    }

    public void LevelUp()
    {
        warningMessage.text = LEVEL_UP;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(2));

        GameManager.Instance.UIManager.HudManager.UpdateLevelText();
        GameManager.Instance.UIManager.InventoryManager.CharacterPanel.UpdateLevelText();
        GameManager.Instance.UIManager.InventoryManager.UpdateSlotColorsBasedOnPlayerLevel();
    }

    public void NoHealthPotion()
    {
        warningMessage.text = NO_HEALTH_POTIONS;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(1));
    }

    public void NoEnergyPotion()
    {
        warningMessage.text = NO_ENGERGY_POTIONS;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(1));
    }

    public void NextDungeon()
    {
        warningMessage.text = NEXT_DUNGEON;
        warningMessage.gameObject.SetActive(true);
        GameManager.Instance.StartCoroutine(ShowWarning(3));

        GameManager.Instance.UIManager.HudManager.UpdateFloorText();
    }

    private IEnumerator ShowWarning(float showWarningTime)
    {
        float start = Time.time;
        float t = start;

        while (t < start + showWarningTime)
        {
            t += Time.deltaTime;

            yield return null;
        }

        warningMessage.gameObject.SetActive(false);

        yield break;
    }
}
