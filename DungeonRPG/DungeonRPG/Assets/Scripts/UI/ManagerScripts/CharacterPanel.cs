using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterPanel : MonoBehaviour
{
    private UISprite equipedOutline;
    private GameObject equipedPrefab;
    private int amountEquiped;

    private UISprite characterSprite;

    private Dictionary<EquipmentSlotType, EquipmentSlot> equippedItems;
    public Dictionary<EquipmentSlotType, EquipmentSlot> EquippedItems { get { return equippedItems; } }

    private bool offHandDissabled;
    public bool OffHandDissabled { get { return offHandDissabled; } }

    private UILabel damageLabel;
    private UILabel healLabel;
    private UILabel protectionLabel;

    private UILabel playerName;
    private UILabel level;

    private UILabel strengthLabel;
    private UILabel dexterityLabel;
    private UILabel intelligenceLabel;
    private UILabel vitalityLabel;

    public void Initialize()
    {
        SetupEquimentIcons();

        GetElements();

        CalculateEquippedStats();

        SetPlayerInformation();
    }

    // Positions the different equiment icons on the outline circle. 
    private void SetupEquimentIcons()
    {
        equipedOutline = GameManager.Instance.UIManager.InventoryManager.InventoryTransform.transform.FindChild("Anchor_MidLeft/Equipment/EquipmentOutline").GetComponent<UISprite>();
        equipedPrefab = Resources.Load<GameObject>("Prefabs/UI/InventoryPopup/EquipSlot");

        characterSprite = GameManager.Instance.UIManager.InventoryManager.InventoryTransform.transform.FindChild("Anchor_MidLeft/Equipment/CharacterOutline/Character").GetComponent<UISprite>();
        characterSprite.spriteName = GameManager.Instance.ActiveCharacterInformation.CharacterClass.CharacterClassType.ToString().ToLower();

        amountEquiped = System.Enum.GetValues(typeof(EquipmentSlotType)).Length;

        equippedItems = new Dictionary<EquipmentSlotType, EquipmentSlot>();

        float r = equipedOutline.localSize.x / 2;
        float angle = 360 / amountEquiped;

        Vector2 origin = equipedOutline.transform.position;

        Vector2 firstPoint = new Vector2(origin.x, origin.y + r);

        GameObject temp = GameObject.Instantiate(equipedPrefab, firstPoint, Quaternion.identity) as GameObject;
        temp.transform.SetParent(GameManager.Instance.UIManager.InventoryManager.InventoryTransform.transform.FindChild("Anchor_MidLeft/Equipment/EquipmentOutline"));
        temp.transform.localScale =  Vector3.one;
        temp.transform.localPosition = firstPoint;

        equippedItems.Add((EquipmentSlotType)0, temp.GetComponent<EquipmentSlot>());
        temp.GetComponent<EquipmentSlot>().Initialize((EquipmentSlotType)0);

        for (int i = 1; i < amountEquiped; i++)
        {
            Vector2 tempVector = RotateAroundPoint(firstPoint, origin, Quaternion.Euler(0, 0, angle * i));//inventoryGameObject.transform.RotateAround(firstPoint, origin, angle * i));
            GameObject tempObject = GameObject.Instantiate(equipedPrefab, tempVector, Quaternion.identity) as GameObject;
            tempObject.transform.SetParent(GameManager.Instance.UIManager.InventoryManager.InventoryTransform.transform.FindChild("Anchor_MidLeft/Equipment/EquipmentOutline"));
            tempObject.transform.localScale =  Vector3.one;
            tempObject.transform.localPosition = tempVector;

            equippedItems.Add((EquipmentSlotType)i, tempObject.GetComponent<EquipmentSlot>());

            tempObject.GetComponent<EquipmentSlot>().Initialize((EquipmentSlotType)i);
        }
    }

    private Vector2 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }

    private void GetElements()
    {
        damageLabel = gameObject.transform.FindChild("OverviewStats/Damage/Label").GetComponent<UILabel>();
        healLabel = gameObject.transform.FindChild("OverviewStats/Heal/Label").GetComponent<UILabel>();
        protectionLabel = gameObject.transform.FindChild("OverviewStats/Protection/Label").GetComponent<UILabel>();

        playerName = gameObject.transform.FindChild("Name").GetComponent<UILabel>();
        level = gameObject.transform.FindChild("Level").GetComponent<UILabel>();

        strengthLabel = gameObject.transform.FindChild("PrimaryStats/StatGrid/Strength/Amount").GetComponent<UILabel>();
        dexterityLabel = gameObject.transform.FindChild("PrimaryStats/StatGrid/Dexterity/Amount").GetComponent<UILabel>();
        intelligenceLabel = gameObject.transform.FindChild("PrimaryStats/StatGrid/Intelligence/Amount").GetComponent<UILabel>();
        vitalityLabel = gameObject.transform.FindChild("PrimaryStats/StatGrid/Vitality/Amount").GetComponent<UILabel>();

    }

    public bool Equip(EquipmentInstance item, EquipmentSlotType type)
    {
        offHandDissabled = equippedItems[EquipmentSlotType.MainHand].CurrentItem != null ? (equippedItems[EquipmentSlotType.MainHand].CurrentItem.GetComponent<WeaponInstance>().BaseWeapon.Handed == Handed.TwoHanded ? true : false) : false;
        if (type == EquipmentSlotType.OffHand)
        {
            if (offHandDissabled)
            {
                Debug.Log("Can't carry shield with twohanded weapon.");
                return false;
            }
        }

        if (type == EquipmentSlotType.MainHand && item.GetComponent<WeaponInstance>().BaseWeapon.Handed == Handed.TwoHanded && equippedItems[EquipmentSlotType.OffHand].CurrentItem != null) equippedItems[EquipmentSlotType.OffHand].Use();

        // Equip the item.
        equippedItems[type].AddItem(item);

        return true;
    }

    public void CalculateEquippedStats()
    {
        //Dictionary<PlayerStats, int> paramStats = new Dictionary<PlayerStats, int>();
        //paramStats.Add(PlayerStats.Attack, 0);
        //paramStats.Add(PlayerStats.Health, 0);
        //paramStats.Add(PlayerStats.Defense, 0);
        //paramStats.Add(PlayerStats.CritRate, 0);
        //paramStats.Add(PlayerStats.CritDamage, 0);

        //int armor = 0;
        //int damage = 0;
        //float attackspd = 0;
        //int blockchance = 0;
        //int blockamount = 0;

        //foreach (KeyValuePair<EquipmentSlotType, EquipmentSlot> entry in equippedItems)
        //{
        //    if (!entry.Value.IsEmpty)
        //    {
        //        EquipmentInstance item = entry.Value.CurrentItem;

        //        if (entry.Value.CurrentItem.ExtraStats != null)
        //        {
        //            if (item.ExtraStats.ContainsKey(PlayerStats.Attack)) paramStats[PlayerStats.Attack] += item.ExtraStats[PlayerStats.Attack];
        //            if (item.ExtraStats.ContainsKey(PlayerStats.Health)) paramStats[PlayerStats.Health] += item.ExtraStats[PlayerStats.Health];
        //            if (item.ExtraStats.ContainsKey(PlayerStats.Defense)) paramStats[PlayerStats.Defense] += item.ExtraStats[PlayerStats.Defense];
        //            if (item.ExtraStats.ContainsKey(PlayerStats.CritRate)) paramStats[PlayerStats.CritRate] += item.ExtraStats[PlayerStats.CritRate];
        //            if (item.ExtraStats.ContainsKey(PlayerStats.CritDamage)) paramStats[PlayerStats.CritDamage] += item.ExtraStats[PlayerStats.CritDamage];
        //        }

        //        if (item.BaseEquipment._EquipmentType == EquipmentType.Shield)
        //        {
        //            //blockchance = item.GetComponent<ShieldInstance>().BaseShield.BlockChance;
        //            //blockamount = item.GetComponent<ShieldInstance>().BaseShield.BlockAmount;
        //        }

        //        if (item.BaseEquipment._EquipmentType == EquipmentType.Weapon)
        //        {
        //            //attackspd = item.GetComponent<WeaponInstance>().BaseWeapon.AttackSpeed;
        //        }
        //    }
        //}

        //GameManager.Instance.Player.SetStats(paramStats, attackspd, blockchance, blockamount);
    }

    public void SetPlayerInformation()
    {
        damageLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.PotentialDamagePerSec; 
        healLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.PotentialHealPerSec;
        protectionLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.PotentialProtectionPerSec;

        strengthLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Strength); 
        dexterityLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Dexterity);
        intelligenceLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Intelligence);
        vitalityLabel.text = "" + GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Vitality);

        playerName.text = "" + GameManager.Instance.ActiveCharacterInformation.Name;
        level.text = "Level " + GameManager.Instance.ActiveCharacterInformation.Level;
    }

    public void UpdateLevelText()
    {
        level.text = "Lvl. " + GameManager.Instance.ActiveCharacterInformation.Level;
    }
}
