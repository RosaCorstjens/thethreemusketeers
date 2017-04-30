using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DetailedItemInformation : MonoBehaviour
{
    // All the components for basic item information.
    GameObject basicInformation;
    UISprite icon;
    UISprite qualityColor;
    UILabel generatedName;
    UILabel mainStatNumber;
    UILabel mainStatType;
    GameObject equipped;

    // All the components for standard stats.
    GameObject standardStats;
    UILabel requiredLvl;
    UILabel itemType;

    // All the components for item related stats.
    GameObject shieldRelatedStats;
    UILabel blockChance;
    UILabel blockAmount;
    GameObject weaponRelatedStats;
    UILabel damage;
    UILabel attackSpeed;
    UILabel xHanded;

    List<ButtonBase> actionButtons;
    UILabel useLabel;

    // All the components for item related stats.
    GameObject extraStatParent;
    GameObject extraStat;
    List<UILabel> extraStatLabels = new List<UILabel>(6); // six possible extra stats. 
    List<GameObject> extraStatObjects = new List<GameObject>(6);
    UIGrid extraStatGrid;

    private Dictionary<StatTypes, string> statTypeStringsForUI;
    public Dictionary<StatTypes, string> StatTypeStringForUI { get { return statTypeStringsForUI; } }

    public void Initialize()
    {
        // Actions, basic and standard information are already part of this gameobject. 
        // The other components need to be added according to the item information. 
        basicInformation = transform.FindChild("BasicInformation").gameObject;
        GetElementsForBasicInformation();

        standardStats = transform.FindChild("StandardStats").gameObject;
        GetElementsForStandardStats();
        InitializeItemRelatedStats();
        InitializeExtraStats();
        SetUIStringsForUI();

        ButtonBase[] temp = transform.FindChild("Actions/Grid").GetComponentsInChildren<ButtonBase>();
        actionButtons = new List<ButtonBase>(temp);
        actionButtons.HandleAction(b => b.Initialize());

        useLabel = transform.FindChild("Actions/Grid/EquipButton/Label").GetComponent<UILabel>();
        useLabel.text = "Equip";

        gameObject.SetActive(false);
    }

    private void SetUIStringsForUI()
    {
        statTypeStringsForUI = new Dictionary<StatTypes, string>();

        statTypeStringsForUI.Add(StatTypes.AreaDamage, "Area Damage");
        statTypeStringsForUI.Add(StatTypes.Armor, "Armor");
        statTypeStringsForUI.Add(StatTypes.AttackSpeed, "Attackspeed");
        statTypeStringsForUI.Add(StatTypes.BlockAmount, "Block Amount");
        statTypeStringsForUI.Add(StatTypes.BlockChance, "Block Chance");
        statTypeStringsForUI.Add(StatTypes.CoolDownReduction, "Cooldown Reduction");
        statTypeStringsForUI.Add(StatTypes.CritDamage, "Crit Damage");
        statTypeStringsForUI.Add(StatTypes.CritRate, "Crit Rate");
        statTypeStringsForUI.Add(StatTypes.Damage, "Damage");
        statTypeStringsForUI.Add(StatTypes.Dexterity, "Dexterity");
        statTypeStringsForUI.Add(StatTypes.DodgeChance, "Dodge Chance");
        statTypeStringsForUI.Add(StatTypes.ExperienceBonus, "Experience Bonus");
        statTypeStringsForUI.Add(StatTypes.HealthPerHit, "Health Per Hit");
        statTypeStringsForUI.Add(StatTypes.HealthPerKill, "Heath Per Kill");
        statTypeStringsForUI.Add(StatTypes.HealthPerSec, "Health Per Second");
        statTypeStringsForUI.Add(StatTypes.Intelligence, "Intelligence");
        statTypeStringsForUI.Add(StatTypes.MaxHealth, "Maximum Health");
        statTypeStringsForUI.Add(StatTypes.MaxResource, "Maximum Resource");
        statTypeStringsForUI.Add(StatTypes.MovementSpeed, "Movement Speed");
        statTypeStringsForUI.Add(StatTypes.Resistance, "Resistance");
        statTypeStringsForUI.Add(StatTypes.ResourceRegen, "Resource Regeneration");
        statTypeStringsForUI.Add(StatTypes.Strength, "Strength");
        statTypeStringsForUI.Add(StatTypes.Thorns, "Thorns");
        statTypeStringsForUI.Add(StatTypes.Vitality, "Vitality");
        statTypeStringsForUI.Add(StatTypes.WeaponDamage, "Weapon Damage");
    }

    private void GetElementsForBasicInformation()
    {
        // Set icon. 
        icon = basicInformation.transform.FindChild("InventorySlot/Icon").GetComponent<UISprite>();
        qualityColor = basicInformation.transform.FindChild("InventorySlot/QualityColor").GetComponent<UISprite>();

        // Generated name.
        generatedName = basicInformation.transform.FindChild("Name").GetComponent<UILabel>();

        // Damage/Armor number. 
        mainStatNumber = basicInformation.transform.FindChild("Number").GetComponent<UILabel>();
        mainStatType = basicInformation.transform.FindChild("Number/Label").GetComponent<UILabel>();

        // Equipped label on/off.
        equipped = basicInformation.transform.FindChild("EquippedLabel").gameObject;
    }

    private void GetElementsForStandardStats()
    {
        requiredLvl = standardStats.transform.FindChild("RequiredLevel").GetComponent<UILabel>();

        itemType = standardStats.transform.FindChild("ItemType").GetComponent<UILabel>();
    }

    private void InitializeItemRelatedStats()
    { 
        shieldRelatedStats = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/DetailedItemInformation/ShieldRelatedStats")) as GameObject;
        shieldRelatedStats.transform.SetParent(gameObject.transform);
        shieldRelatedStats.transform.localScale = new Vector3(1,1,1);
        blockChance = shieldRelatedStats.transform.FindChild("ChanceToBlock").GetComponent<UILabel>();
        blockAmount = shieldRelatedStats.transform.FindChild("BlockAmount").GetComponent<UILabel>();

        shieldRelatedStats.SetActive(false);

        weaponRelatedStats = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/DetailedItemInformation/WeaponRelatedStats")) as GameObject;
        weaponRelatedStats.transform.SetParent(gameObject.transform);
        weaponRelatedStats.transform.localScale =  Vector3.one;
        damage = weaponRelatedStats.transform.FindChild("Damage").GetComponent<UILabel>();
        attackSpeed = weaponRelatedStats.transform.FindChild("AttackPerSecond").GetComponent<UILabel>();
        xHanded = weaponRelatedStats.transform.FindChild("xHanded").GetComponent<UILabel>();

        weaponRelatedStats.SetActive(false);
    }

    private void InitializeExtraStats()
    {
        extraStatParent = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/DetailedItemInformation/ExtraStats")) as GameObject;
        extraStatParent.transform.SetParent(gameObject.transform);
        extraStatParent.transform.localScale =  Vector3.one;

        extraStatGrid = extraStatParent.transform.FindChild("Grid").GetComponent<UIGrid>();

        extraStat = Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/DetailedItemInformation/ExtraStat") as GameObject;

        for (int i = 0; i < 6; i++)
        {
            GameObject toAdd = GameObject.Instantiate(extraStat) as GameObject;
            toAdd.transform.SetParent(extraStatParent.transform);
            toAdd.transform.localScale =  Vector3.one;
            extraStatObjects.Add(toAdd);
            extraStatLabels.Add(toAdd.GetComponentInChildren<UILabel>());

            extraStatObjects.Last().SetActive(false);
        }

        extraStatParent.SetActive(false);
    }

    public void Show(EquipmentInstance item) 
    {
        gameObject.SetActive(true);

        // Set basic information
        icon.spriteName = item.ItemInfo.SpriteName;
        qualityColor.color = GameManager.Instance.ItemManager.QualityColors[(int)item.Quality];

        generatedName.text = item.GetTitle();

        if (item.BaseEquipment.EquipmentType != EquipmentType.Jewelry)
        {
            mainStatType.gameObject.SetActive(true);
            mainStatNumber.gameObject.SetActive(true);
            mainStatType.text = item.BaseEquipment.MainStat.ToString();
            mainStatNumber.text = "" + item.MainStatValue;
        }
        else
        {
            mainStatType.gameObject.SetActive(false);
            mainStatNumber.gameObject.SetActive(false);
        }

        // Set standard stats.
        requiredLvl.text = "Required level: "+ item.Level; 
        itemType.text = "[" + GameManager.Instance.ItemManager.QualityHexColors[(int)item.Quality] + "]" + item.Quality.ToString() + " " + item.ItemInfo.Name + "[-]";

        equipped.SetActive(item.Equipped);
        useLabel.text = item.Equipped ? "Deequip" : "Equip";

        // Set extra stats if it has any. 
        if (item.AffixStats != null)
        {
            // foreach to enble looping over a dictionary.
            for(int i = 0; i < item.AffixStats.Count; i++)
            {
                extraStatLabels[i].text = "+ " + item.AffixStats[i].Modifier.value + (item.AffixStats[i].IsPercent ? "% " : " ") + statTypeStringsForUI[item.AffixStats[i].Modifier.affected];
                extraStatObjects[i].transform.SetParent(extraStatGrid.transform);
                extraStatObjects[i].SetActive(true);
            }

            extraStatGrid.Reposition();

            extraStatParent.GetComponent<UIWidget>().SetAnchor(standardStats, 0, -20 + (item.AffixStats.Count * -15), 0, -40);

            extraStatParent.gameObject.SetActive(true);
        }

        // Set the item related stats if it has any. 
        if (item.BaseEquipment.EquipmentType == EquipmentType.Shield)
        {
            blockChance.text = item.BaseStats.Find(b => b.StatType == StatTypes.BlockChance).Value + "% Chance to Block";
            blockAmount.text = item.BaseStats.Find(b => b.StatType == StatTypes.BlockAmount).Value + " Block Amount";

            shieldRelatedStats.GetComponent<UIWidget>().SetAnchor(item.AffixStats != null ? extraStatParent : standardStats, 0, -50, 0, item.AffixStats != null ? -20 + (item.AffixStats.Count * -15) : -50);
            shieldRelatedStats.gameObject.SetActive(true);
        }

        if (item.BaseEquipment.EquipmentType == EquipmentType.Weapon)
        {
            damage.text = item.BaseStats.Find(b => b.StatType == StatTypes.WeaponDamage).Value + " Damage";
            attackSpeed.text = item.BaseStats.Find(b => b.StatType == StatTypes.AttackSpeed).Value + " Attacks per Second";
            xHanded.text = item.GetComponent<WeaponInstance>().BaseWeapon.Handed.ToString();

            weaponRelatedStats.GetComponent<UIWidget>().SetAnchor(item.AffixStats != null ? extraStatParent : standardStats, 0, -50, 0, item.AffixStats != null ? -20 + (item.AffixStats.Count * -15) : -50);
            weaponRelatedStats.gameObject.SetActive(true);
        }
    }

    private void Reset()
    {
        extraStatObjects.HandleAction(e =>
            {
                e.transform.SetParent(extraStatParent.transform);
                e.gameObject.SetActive(false);
            });
        shieldRelatedStats.gameObject.SetActive(false);

        weaponRelatedStats.gameObject.SetActive(false);

        extraStatParent.gameObject.SetActive(false);
    }

    public void Hide()
    {
        Reset();

        actionButtons.HandleAction(b => b.DeHighlightButton());
        gameObject.SetActive(false);
    }

    public void Equip()
    {
        GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.Use();

        equipped.SetActive(GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.CurrentItem.Equipped);
        useLabel.text = GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.CurrentItem.Equipped ? "Deequip" : "Equip";
    }

    public void Drop()
    {
        GameManager.Instance.UIManager.InventoryManager.CurrentActiveSlot.Drop();
        GameManager.Instance.UIManager.InventoryManager.HideDetailedInformation();
    }

    public void GoBack()
    {
        GameManager.Instance.UIManager.InventoryManager.HideDetailedInformation();
    }
}
