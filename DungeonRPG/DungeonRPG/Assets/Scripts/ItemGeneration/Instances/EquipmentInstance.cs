using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentInstance: ItemInstance
{
    private string generatedName;
    public string GeneratedName { get { return generatedName; } }

    protected string statsText;
    protected string levelText;

    private bool equipped;

    public bool Equipped
    {
        get
        {
            return equipped;
        }
        set
        {
            equipped = value;
            if (equipped) GameManager.Instance.ActiveCharacterInformation.AddStats(this);
            else GameManager.Instance.ActiveCharacterInformation.RemoveStats(this);
            GameManager.Instance.UIManager.InventoryManager.CharacterPanel.SetPlayerInformation();
        }
    }

    private BaseEquipment baseEquipment;
    public BaseEquipment BaseEquipment { get { return baseEquipment; } }

    public float MainStatValue { get { return baseStats.Find(b => b.StatType == baseEquipment.MainStat).Value; } }

    [SerializeField]
    private List<Stat> baseStats;
    public List<Stat> BaseStats { get { return baseStats; } set { baseStats = value; } }

    [SerializeField]
    private List<Affix> affixStats;
    public List<Affix> AffixStats { get { return affixStats; } set { affixStats = value; } }

    private List<Modifier> modifiers;
    public List<Modifier> Modifiers { get { return modifiers; } }

    protected int level;
    public int Level { get { return level; } }

    public void Initialize(BaseEquipment itemInfo, Quality quality, int level, string generatedName, List<Stat> baseStats, List<Affix> affixStats = null)
    {
        base.Initialize(itemInfo, quality);

        baseEquipment = itemInfo;

        this.baseStats = baseStats;
        this.affixStats = affixStats;
        this.level = level;
        this.generatedName = generatedName;

        statsText = string.Empty;
        levelText = string.Empty;

        DetermineModifiers();
    }

    private void DetermineModifiers()
    {
        modifiers = new List<Modifier>();

        for (int i = 0; i < baseStats.Count; i++)
        {
            ModifierType modifierType = ModifierType.add;

            modifiers.Add(new Modifier(baseStats[i].StatType, modifierType, baseStats[i].Value));
        }

        for (int i = 0; i < affixStats.Count; i++)
        {
            modifiers.Add(affixStats[i].Modifier);
        }
    }

    public string GetTitle()
    {
        return string.Format("[" + GameManager.Instance.ItemManager.QualityHexColors[(int)quality] + "]{0}[-]", generatedName);
    }

    public  string GetTooltipText()
    {
        if (statsText == string.Empty)
        {
            if (baseEquipment.EquipmentType == EquipmentType.Jewelry) return "";

            statsText += baseStats.Find(b => b.StatType == baseEquipment.MainStat).Value + " " + (baseEquipment.MainStat == StatTypes.Armor ? "Armor" : "Damage");
        }

        return string.Format("{0}", statsText);
    }

    public string GetTooltipLevelText()
    {
        if (levelText == string.Empty)
        {
            levelText += "Level " + level;
        }

        return string.Format("{0}", levelText);
    }

    public override void Use()
    {
        Debug.Log(itemInfo.Name + " equipped.");
    }

    public override void Drop()
    {
        base.Drop();

        float angle = UnityEngine.Random.Range(-45, 45);

        Vector3 pos = GameManager.Instance.ActiveCharacterInformation.PlayerController.transform.position + (3f * GameManager.Instance.ActiveCharacterInformation.PlayerController.transform.forward);

        Vector3 v = new Vector3(Random.Range(pos.x - 1f, pos.x + 1f), 0.3f, Random.Range(pos.z - 1f, pos.z + 1f));

        gameObject.transform.position = v;

        dropped = true;

        gameObject.SetActive(true);
    }

    protected override bool AddToInventory()
    {
        return GameManager.Instance.UIManager.InventoryManager.AddItem(this);
    }
}
