using UnityEngine;
using System.Collections;

public class CharacterDetailsPanel : MonoBehaviour
{
    UIGrid offenseGrid;
    UIGrid defenseGrid;
    UIGrid lifeGrid;
    UIGrid resourceGrid;

    UILabel dmgIncrByBaseStat;
    UILabel attacksPerSec;
    UILabel critRate;
    UILabel critDmg;
    UILabel areaDmg;
    UILabel cooldownReduc;
    UILabel armor;
    UILabel blockAmount;
    UILabel blockChance;
    UILabel dodgeChance;
    UILabel resistance;
    UILabel thorns;
    UILabel maxHealth;
    UILabel lps;
    UILabel lph;
    UILabel lpk;
    UILabel maxResource;
    UILabel resourceRegen;

    public void Initialize()
    {
        offenseGrid = transform.FindChild("Details/Offense/StatContainer/OffenseGrid").GetComponent<UIGrid>();
        defenseGrid = transform.FindChild("Details/Defense/StatContainer/DefenseGrid").GetComponent<UIGrid>();
        lifeGrid = transform.FindChild("Details/Life/StatContainer/LifeGrid").GetComponent<UIGrid>();
        resourceGrid = transform.FindChild("Details/Resource/StatContainer/ResourceGrid").GetComponent<UIGrid>();

        dmgIncrByBaseStat = offenseGrid.transform.FindChild("dmgIncrByBaseStat/Value").GetComponent<UILabel>();
        attacksPerSec = offenseGrid.transform.FindChild("attacksPerSec/Value").GetComponent<UILabel>();
        critRate = offenseGrid.transform.FindChild("critRate/Value").GetComponent<UILabel>();
        critDmg = offenseGrid.transform.FindChild("critDmg/Value").GetComponent<UILabel>();
        areaDmg = offenseGrid.transform.FindChild("areaDmg/Value").GetComponent<UILabel>();
        cooldownReduc = offenseGrid.transform.FindChild("cooldownReduc/Value").GetComponent<UILabel>();
        armor = defenseGrid.transform.FindChild("armor/Value").GetComponent<UILabel>();
        blockAmount = defenseGrid.transform.FindChild("blockAmount/Value").GetComponent<UILabel>();
        blockChance = defenseGrid.transform.FindChild("blockChance/Value").GetComponent<UILabel>();
        dodgeChance = defenseGrid.transform.FindChild("dodgeChance/Value").GetComponent<UILabel>();
        resistance = defenseGrid.transform.FindChild("resistance/Value").GetComponent<UILabel>();
        thorns = defenseGrid.transform.FindChild("thorns/Value").GetComponent<UILabel>();
        maxHealth = lifeGrid.transform.FindChild("maxHealth/Value").GetComponent<UILabel>();
        lps = lifeGrid.transform.FindChild("lps/Value").GetComponent<UILabel>();
        lph = lifeGrid.transform.FindChild("lph/Value").GetComponent<UILabel>();
        lpk = lifeGrid.transform.FindChild("lpk/Value").GetComponent<UILabel>();
        maxResource = resourceGrid.transform.FindChild("maxResource/Value").GetComponent<UILabel>();
        resourceRegen = resourceGrid.transform.FindChild("resourceRegen/Value").GetComponent<UILabel>();

        offenseGrid.transform.FindChild("dmgIncrByBaseStat/Name").GetComponent<UILabel>().text = "Damge increased by " + GameManager.Instance.ActiveCharacterInformation.Stats.BaseStat.ToString();

    }

    public void SetStats()
    {
        dmgIncrByBaseStat.text = (GameManager.Instance.ActiveCharacterInformation.Stats.Get(GameManager.Instance.ActiveCharacterInformation.Stats.BaseStat) / 100) + "%";
        attacksPerSec.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.AttackSpeed) + "";
        critRate.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.CritRate) + "%";
        critDmg.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.CritDamage) + "%";
        areaDmg.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.AreaDamage) + "%";
        cooldownReduc.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.CoolDownReduction) + "%";
        armor.text = (int)GameManager.Instance.ActiveCharacterInformation.Stats.DeterminedArmor + "";
        blockAmount.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.BlockAmount) + "";
        blockChance.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.BlockChance) + "%";
        dodgeChance.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.DodgeChance) + "%";
        resistance.text = GameManager.Instance.ActiveCharacterInformation.Stats.DeterminedResistance + "";
        thorns.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.Thorns) + "";
        maxHealth.text = GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth + "";
        lph.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerHit) + "";
        lps.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerSec) + "";
        lpk.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.HealthPerKill) + "";
        maxResource.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.MaxResource) + "";
        resourceRegen.text = GameManager.Instance.ActiveCharacterInformation.Stats.Get(StatTypes.ResourceRegen) + "";
    }

    public void Show()
    {
        SetStats();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
