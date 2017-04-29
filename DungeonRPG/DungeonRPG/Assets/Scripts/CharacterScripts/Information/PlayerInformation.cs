using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;

public class PlayerInformation : BaseCharacterInformation
{
    public SaveData savedata; 

    private float xp;
    private float xpTillNextLvl;
    public float XP { get { return xp; } }

    //private GameObject playerPrefab;
    //public GameObject PlayerPrefab { get { return playerPrefab; } }

    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } set { playerController = value; } }

    public PlayerInformation() { }

    /// <summary>
    /// Call when creating a new player. 
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <param name="characterClass">The class of the character.</param>
    public PlayerInformation(string name, BaseCharacterClass characterClass) : base(name, characterClass)
    {
        this.level = 1;

        SetUpStats();
        //playerPrefab = Resources.Load<GameObject>("Prefabs/CharactersForSelection/" + characterClass.CharacterClassType.ToString() + "2");

        savedata = new SaveData(name, level, 0, characterClass.CharacterClassType, stats);
    }

    public PlayerInformation(SaveData savedata)
    {
        this.savedata = savedata;

        name = savedata.Name;
        level = savedata.Level;
        xp = savedata.XP;
        xpTillNextLvl = GetXPTillNextLevel(level);

        characterClass = new BaseWarriorClass();

        //stats = savedata.Stats;
        SetUpStats();
    }

    public void AddExperiencePoints(float progress)
    {
        Debug.Log("progress: " + progress);
        float amount = progress * xpTillNextLvl;

        xp += (int)amount;

        if (xp > xpTillNextLvl)
        {
            LevelUp();
        }
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        xpTillNextLvl = GetXPTillNextLevel(level);
        Debug.Log("xp till next: " + xpTillNextLvl);
        GameManager.Instance.UIManager.LevelUp();
    }

    private int GetXPTillNextLevel(int x)
    {
        if (x <= 7)
        {
            return ((150 * (int)Mathf.Pow(x, 2)) + (1050 * x));
        }
        else if (x <= 11)
        {
            return ((200 * (int)Mathf.Pow(x, 2)) + (1050 * x) - 2450);
        }
        else if (x <= 22)
        {
            return ((50 * (int)Mathf.Pow(x, 2)) + (1750 * x) + 9800);
        }
        else if (x <= 30)
        {
            return ((250 * (int)Mathf.Pow(x, 2)) - (1500 * x) - 22750);
        }
        else if (x <= 35)
        {
            return ((500 * (int)Mathf.Pow(x, 2)) - (13500 * x) + 88000);
        }
        else if (x <= 50)
        {
            return ((200 * (int)Mathf.Pow(x, 2)) + (1800 * x) - 80000);
        }
        else 
        {
            return ((2500 * (int)Mathf.Pow(x, 2)) - (145000 * x) + 2102500);
        }
    }
}

[Serializable]
public class SaveData
{
    [SerializeField]
    private int id;
    public int ID { get { return id; } set { id = value; } }

    [SerializeField]
    private string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]
    private int xp;
    public int XP { get { return xp; } set { xp = value; } }

    [SerializeField]
    private CharacterClass characterClass;
    public CharacterClass CharacterClass { get { return characterClass; } set { characterClass = value; } }

    [SerializeField]
    private CharacterStats stats = new CharacterStats();
    public CharacterStats Stats { get { return stats; } set { stats = value; } }

    public SaveData() { }

    public SaveData(string name, int level, int xp, CharacterClass characterClass, CharacterStats stats)
    {
        this.name = name;
        this.level = level;
        this.xp = xp;
        this.characterClass = characterClass;
        this.stats = stats;
    }
}
