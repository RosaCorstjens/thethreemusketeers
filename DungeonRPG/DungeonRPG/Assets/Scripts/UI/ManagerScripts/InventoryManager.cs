using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager
{
    public enum InventoryStates { Inventory, Details }
    private InventoryStates state = InventoryStates.Inventory; 

    // Anchor references.
    private UIAnchor anchorMidRight;
    private UIAnchor anchorMidLeft;

    private GameObject inventoryGameObject;                             // Go of the inventory panel. 
    private UIPanel inventoryPanel;
    public Transform InventoryTransform { get { return inventoryGameObject.transform; } }

    private Coroutine fadeIn;
    private Coroutine fadeOut;
    private bool fadingIn;
    private bool fadingOut;
    private float fadeTime = 0.5f;

    private GameObject slotPrefab;                                      // Prefab of an empty slot in the inventory.
    private int amountOfSlots = 30;                                     // The amount of slots.                         
    private List<InventorySlot> slots;                                  // List of all slots. 

    private UILabel keysLabel;
    private UILabel multiKeysLabel;
    private UILabel finalKeysLabel;

    private UILabel healthPotionsLabel;
    private List<PotionInstance> healthPotions;

    public int KeyAmount { get; private set; }
    public int MultiKeyAmount { get; private set; }
    public int FinalKeyAmount { get; private set; }
    public int HealthPotionAmount { get { return healthPotions.Count; } }

    private CharacterPanel characterPanel;
    public CharacterPanel CharacterPanel { get { return characterPanel; } }

    // ----------------- BEGIN INVENTORY ------------------------- // 

    // Actions with slots. 
    private GameObject tooltip;
    private UILabel tooltipText;
    private UILabel tooltipTitle;
    private IEnumerator handlePositionToolTip;
    private bool hooverDissabled;

    private ItemActionMenu actionMenu;
    private bool actionMenuOn;

    // References to parts of the UIPanel. 
    private UIGrid grid;
    private UIScrollBar scrollBar;
    private UIScrollView scrollView;

    private DetailedItemInformation detailedItemInformation;
    private bool showingDetailedInformation;

    // ----------------- END INVENTORY ------------------------- // 

    // ----------------- BEGIN CHARACTER DETAILS ------------------------- // 

    private CharacterDetailsPanel characterDetailsPanel;
    private bool showingCharacterDetailsPanel;

    // ----------------- END CHARACTER DETAILS ------------------------- // 

    // ----------------- BEGIN SKILLS ------------------------- // 
    // ----------------- END SKILLS ------------------------- // 

    private SlotBase currentActiveSlot;
    public SlotBase CurrentActiveSlot { get { return currentActiveSlot; } }

    private int emptySlots;
    public int EmptySlots
    {
        get { return emptySlots; }

        set { emptySlots = value; }
    }

    public WeaponInstance StartItem { get; private set; }

    // Called from UI manager to initialize the inventory manager. 
    public void Initialize()
    {
        InstantiateInventory();

        SetupInventoryGrid();

        SetupToolTip();

        SetupActionMenu();

        SetupStandardItems();

        characterPanel = inventoryGameObject.transform.FindChild("Anchor_MidLeft/CharacterStats").GetComponent<CharacterPanel>();
        characterPanel.Initialize();

        detailedItemInformation = inventoryGameObject.transform.FindChild("Anchor_MidRight/ItemInformationPopup").GetComponent<DetailedItemInformation>();
        detailedItemInformation.Initialize();

        characterDetailsPanel = inventoryGameObject.transform.FindChild("Anchor_MidRight/CharacterDetails").GetComponent<CharacterDetailsPanel>();
        characterDetailsPanel.Initialize();

        inventoryGameObject.transform.FindChild("SelectTabMenu/MenuContainer").GetComponent<InventoryMenu>().Initialize();

        // Deactivate the inventory gameobject. 
        inventoryGameObject.SetActive(false);

        // give the player his first item
        List<Stat> baseStats = new List<Stat>();
        Equipment equipment = Equipment.Weapon;
        string generatedName = "";
        GameObject toAdd = GameObject.Instantiate(GameManager.Instance.ItemManager.ItemGenerator.DropPrefab);

        BaseWeapon baseItem = GameManager.Instance.ItemManager.ItemContainer.Weapons[0];

        generatedName = baseItem.Name;

        int damageValue = (int)baseItem.BaseStats.Find(s => s.StatType == StatTypes.Damage).Range.GetRandomInRange();
        float attackSpeedValue = (float)Math.Round(baseItem.BaseStats.Find(s => s.StatType == StatTypes.AttackSpeed).Range.GetRandomInRange(), 2);
        Stat damage = new Stat(StatTypes.WeaponDamage, damageValue);
        Stat attackSpeed = new Stat(StatTypes.AttackSpeed, attackSpeedValue);
        baseStats.Add(damage);
        baseStats.Add(attackSpeed);
        toAdd.AddComponent<WeaponInstance>().Initialize(baseItem, 0, 1, generatedName, baseStats, new List<Affix>());

        StartItem = toAdd.GetComponent<WeaponInstance>();
        //AddItem(StartItem);
    }

    private void InstantiateInventory()
    {
        // Get the inventory prefab and instantiate it. 
        inventoryGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/InventoryPanel")) as GameObject;
        inventoryGameObject.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        inventoryGameObject.transform.localScale = Vector3.one;

        inventoryPanel = inventoryGameObject.GetComponent<UIPanel>();

        // Get the anchors. 
        anchorMidRight = inventoryGameObject.transform.FindChild("Anchor_MidRight").GetComponent<UIAnchor>();
        anchorMidLeft = inventoryGameObject.transform.FindChild("Anchor_MidRight").GetComponent<UIAnchor>();
    }

    private void SetupInventoryGrid()
    {
        // Get the inventory slot prefab. 
        slotPrefab = Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/InventorySlot");

        // Find the grid for the inventory slots to get a reference. 
        grid = inventoryGameObject.transform.FindChild("Anchor_MidRight/Scroll View/UIGrid").GetComponent<UIGrid>();

        // Find the scrollbar for the inventory slots to get a reference. 
        scrollBar = inventoryGameObject.transform.FindChild("Anchor_MidRight/Scroll Bar").GetComponent<UIScrollBar>();

        scrollView = inventoryGameObject.transform.FindChild("Anchor_MidRight/Scroll View").GetComponent<UIScrollView>();

        // Initialize the list with inventory slots. 
        slots = new List<InventorySlot>();

        // Instantiate every slot. 
        for (int i = 0; i < amountOfSlots; i++)
        {
            slots.Add(GameObject.Instantiate(slotPrefab).GetComponent<InventorySlot>());
            slots[i].gameObject.transform.SetParent(grid.transform);
            slots[i].gameObject.transform.localScale =  Vector3.one;
        }

        // Initialize every slot. 
        slots.HandleAction(i => i.Initialize());

        emptySlots = slots.Count;

        // Reposition the grid. 
        grid.Reposition();
    }

    private void SetupToolTip()
    {
        tooltip = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/Tooltip")) as GameObject;
        tooltip.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        tooltip.transform.localScale =  Vector3.one;
        tooltip.transform.localPosition = Vector3.zero;

        tooltipText = tooltip.transform.FindChild("ToolTipTitle/Text").GetComponent<UILabel>();
        tooltipTitle = tooltip.transform.FindChild("ToolTipTitle").GetComponent<UILabel>();
        tooltip.SetActive(false);

        handlePositionToolTip = HandlePositionToolTip();
    }

    private void SetupActionMenu()
    {
        actionMenu = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/InventoryActions/ItemActionMenu")).GetComponent<ItemActionMenu>();
        actionMenu.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        actionMenu.transform.localScale =  Vector3.one;
        actionMenu.transform.localPosition = Vector3.zero;

        ButtonBase[] buttons = actionMenu.GetComponentsInChildren<ButtonBase>();
        List<ButtonBase> buttonList = new List<ButtonBase>(buttons);
        buttonList.HandleAction(b => b.Initialize());

        actionMenu.Initialize();
        actionMenu.gameObject.SetActive(false);
    }

    private void SetupStandardItems()
    {
        // Find the labels for coins and potions to get a reference. 
        healthPotionsLabel = inventoryGameObject.transform.FindChild("Anchor_MidLeft/StandardItems/HealthPotions/Label").GetComponent<UILabel>();
        keysLabel = inventoryGameObject.transform.FindChild("Anchor_MidLeft/StandardItems/Keys/Label").GetComponent<UILabel>();
        multiKeysLabel = inventoryGameObject.transform.FindChild("Anchor_MidLeft/StandardItems/MultiKeys/Label").GetComponent<UILabel>();
        finalKeysLabel = inventoryGameObject.transform.FindChild("Anchor_MidLeft/StandardItems/FinalKeys/Label").GetComponent<UILabel>();

        healthPotions = new List<PotionInstance>();

        // Set the text labels to the correct amount (0). 
        healthPotionsLabel.text = healthPotions.Count + "";

        KeyAmount = 0;
        MultiKeyAmount = 0;
        FinalKeyAmount = 0;

        keysLabel.text = "" + KeyAmount;
        multiKeysLabel.text = "" + MultiKeyAmount;
        finalKeysLabel.text = "" + FinalKeyAmount;
    }

    public void ToggleMenu(bool on)
    {
        if (on) fadeIn = GameManager.Instance.StartCoroutine(FadeIn());
        else fadeOut = GameManager.Instance.StartCoroutine(FadeOut());

        grid.Reposition();
    }

    public IEnumerator FadeOut()
    {
        if (tooltip.activeInHierarchy) HideTooltip();
        if (actionMenuOn) HideActionMenu();

        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            if(fadeIn != null) GameManager.Instance.StopCoroutine(fadeIn);

            float startAlpha = inventoryPanel.alpha;
            float rate = 1 / fadeTime;
            float progress = 0.0f;

            while (progress < 1)
            {
                inventoryPanel.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            inventoryPanel.alpha = 0;
            fadingOut = false;
            inventoryGameObject.SetActive(false);
        }
    }

    public IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingIn = true;
            fadingOut = false;
            if(fadeOut != null) GameManager.Instance.StopCoroutine(fadeOut);

            float startAlpha = inventoryPanel.alpha;
            float rate = 1 / fadeTime;
            float progress = 0.0f;

            inventoryGameObject.SetActive(true);

            while (progress < 1)
            {
                inventoryPanel.alpha = Mathf.Lerp(startAlpha, 1, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            inventoryPanel.alpha = 1;
            fadingIn = false;
        }
    }

    public void AddKey(KeyType type)
    {
        switch (type)
        {
            case KeyType.Normal:
                KeyAmount++;
                keysLabel.text = "" + KeyAmount;

                break;
            case KeyType.Multi:
                MultiKeyAmount++;
                multiKeysLabel.text = "" + MultiKeyAmount;

                break;
            case KeyType.Final:
                FinalKeyAmount++;
                finalKeysLabel.text = "" + FinalKeyAmount;

                break;
        }

        GameManager.Instance.UIManager.HudManager.UpdateKeyValue(type);
    }

    public void RemoveKey(KeyType type)
    {
        switch (type)
        {
            case KeyType.Normal:
                KeyAmount--;
                keysLabel.text = "" + KeyAmount;

                break;
            case KeyType.Multi:
                MultiKeyAmount--;
                multiKeysLabel.text = "" + MultiKeyAmount;

                break;
            case KeyType.Final:
                FinalKeyAmount--;
                finalKeysLabel.text = "" + FinalKeyAmount;

                break;
        }

        GameManager.Instance.UIManager.HudManager.UpdateKeyValue(type);
    }

    // Returns true if item is succesfully added to the inventory. 
    public bool AddItem(EquipmentInstance item)
    {
        if (actionMenuOn) HideActionMenu();

        return AddToEmptySlot(item);
  
        // Add check for stackable items. 
        // return AddToStackableSlot(item);
    }

    public bool AddItem(PotionInstance item)
    {
        switch (item.BasePotion.PotionType)
        {
            case PotionType.Health:
                healthPotions.Add(item);
                healthPotionsLabel.text = healthPotions.Count + "";

                GameManager.Instance.UIManager.HudManager.UpdatePotionValue();

                return true;

            case PotionType.Mana:
                return true;
        }
        return false;
    }

    public void UsePotion(PotionType type)
    {
        if (type == PotionType.Health)
        {
            if (healthPotions.Count != 0)
            {
                float heal = (float)(GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.MaxHealth) / 100) * healthPotions[0].BasePotion.RestoreAmount.GetRandomInRange() ;
                GameManager.Instance.ActiveCharacterInformation.PlayerController.AdjustCurrentHealth(heal);

                healthPotions.Remove(healthPotions[0]);
                healthPotionsLabel.text = healthPotions.Count + "";

                GameManager.Instance.UIManager.HudManager.UpdatePotionValue();

            }
            else
            {
                GameManager.Instance.UIManager.NoHealthPotion();
            }
        }
        else
        {
            //TODO: mana potion.
        }
    }

    // Adds the item at current action slot. 
    public bool EquipItem(EquipmentInstance item = null)
    {
        // Get item at action slot. 
        if (item == null) item = currentActiveSlot.CurrentItem;

        // not a sufficient level
        if (item.Level > GameManager.Instance.ActiveCharacterInformation.Level) return false;

        if (actionMenuOn) HideActionMenu();

        // Get it's equipmentSlotType.
        EquipmentSlotType itemsType;

        // Check for shield
        if (item.BaseEquipment.EquipmentType == EquipmentType.Shield)
        {
            itemsType = EquipmentSlotType.OffHand;
        }
        else if (item.BaseEquipment.EquipmentType == EquipmentType.Weapon)
        {
            itemsType = EquipmentSlotType.MainHand;
        }
        else if(item.BaseEquipment.EquipmentType == EquipmentType.Armor)
        {
            itemsType = (EquipmentSlotType)(int)item.GetComponent<ArmorInstance>().BaseArmor.ArmorType;
        }
        else 
        {
            itemsType = (EquipmentSlotType)(int)(item.GetComponent<JewerlyInstance>().BaseJewelry.JewelryType) + Enum.GetValues(typeof(ArmorType)).Length;
        }

        if(!characterPanel.Equip(item, itemsType)) return false;

        if (detailedItemInformation) currentActiveSlot = characterPanel.EquippedItems[itemsType];

        emptySlots++;

        return true;
    }

    private bool AddToEmptySlot(EquipmentInstance item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                if (showingDetailedInformation) currentActiveSlot = slots[i];

                slots[i].AddItem(item);

                emptySlots--;

                return true;
            }
        }


        return false;
    }

    //// Reorders after an item is removed. 

    public void Reorder(InventorySlot emptySlot)
    {
        // Find index of given slot. 
        int index = slots.FindIndex(e => e == emptySlot);

        // For every slot after that index, assign it to the one before it. 
        // Exclude all slots that arn't filled. 
        for (int i = index + 1; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i - 1].ClearSlot();
                break;
            }
            // Assign to the slot before i.
            slots[i - 1].AddItem(slots[i].CurrentItem);

            // Clear i. 
            slots[i].ClearSlot();
        }
    }

    public void ShowTooltip(EquipmentInstance item)
    {
        if (actionMenuOn || state != InventoryStates.Inventory) return;

        tooltipTitle.text = item.GetTitle();
        tooltipText.text = item.GetTooltipText();

        tooltip.SetActive(true);

        if (handlePositionToolTip != null) GameManager.Instance.StopCoroutine(handlePositionToolTip);

        GameManager.Instance.StartCoroutine(handlePositionToolTip);
    }

    public void HideTooltip()
    {
        if (actionMenuOn) return;

        tooltip.SetActive(false);

        GameManager.Instance.StopCoroutine(handlePositionToolTip);
    }

    private IEnumerator HandlePositionToolTip()
    {
        Vector3 position;

        while (true)
        {
            position = Camera.main.WorldToNormalizedViewportPoint(Input.mousePosition);
            position = GameManager.Instance.UIManager.UICamera.ScreenToViewportPoint(Input.mousePosition);
            position = GameManager.Instance.UIManager.UICamera.NormalizedViewportToWorldPoint(position);
            position.z = 0;
            tooltip.transform.position = position;
            yield return null;
        }

        yield break;
    }

    public void ShowActionMenu(SlotBase slot)
    {
        if (state != InventoryStates.Inventory) return;

        // Check for the tooltip being active. 
        if (tooltip.activeInHierarchy) HideTooltip();

        currentActiveSlot = slot;

        actionMenuOn = true;

        actionMenu.Open(slot);
    }

    public void HideActionMenu()
    {
        if (!showingDetailedInformation) currentActiveSlot = null;

        actionMenu.gameObject.SetActive(false);
        actionMenuOn = false;
    }

    public void ShowDetailedInformation(SlotBase slot = null)
    {
        if (state != InventoryStates.Inventory) return;

        if (slot == null) slot = currentActiveSlot;

        if (actionMenuOn) HideActionMenu();

        showingDetailedInformation = true;

        HideTooltip();

        currentActiveSlot = slot;

        detailedItemInformation.Show(slot.CurrentItem);

        scrollView.gameObject.SetActive(false);
        scrollBar.gameObject.SetActive(false);
    }

    public void HideDetailedInformation()
    {
        showingDetailedInformation = false;

        currentActiveSlot = null;

        detailedItemInformation.Hide();

        scrollView.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(true);
    }

    public void ChangeState(InventoryStates newState)
    {
        switch (state)
        {
            case InventoryStates.Inventory:
                HideInventory();
                break;
            case InventoryStates.Details:
                HideCharacterDetails();
                break;
        }

        state = newState;

        switch (state)
        {
            case InventoryStates.Inventory:
                ShowInventory();
                break;
            case InventoryStates.Details:
                ShowCharacterDetails();
                break;
        }
    }

    public void ShowInventory()
    {
        scrollView.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        if (showingDetailedInformation) HideDetailedInformation();
        if (actionMenuOn) HideActionMenu();
        if (tooltip.activeInHierarchy) HideTooltip();
        scrollView.gameObject.SetActive(false);
        scrollBar.gameObject.SetActive(false);
    }

    public void ShowCharacterDetails()
    {
        if (showingDetailedInformation) HideDetailedInformation();

        HideTooltip();

        characterDetailsPanel.Show();

        scrollView.gameObject.SetActive(false);
        scrollBar.gameObject.SetActive(false);
    }

    public void HideCharacterDetails()
    {
        characterDetailsPanel.Hide();

        scrollView.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(true);
    }

    public void AlertItemDropped(SlotBase alertOrigin)
    {
        if (actionMenuOn) HideActionMenu();

        if (showingDetailedInformation) HideDetailedInformation();

        if(alertOrigin.ThisSlotType == SlotBase.SlotType.Inventory) Reorder(alertOrigin.GetComponent<InventorySlot>());
    }
    
}

