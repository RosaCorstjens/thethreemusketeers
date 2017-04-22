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

        BaseCharacterClass newCharacterClass;
        switch (savedata.CharacterClass)
        {
            case global::CharacterClass.Warrior:
                newCharacterClass = new BaseWarriorClass();
                break;

            case global::CharacterClass.Hunter:
                newCharacterClass = new BaseHunterClass();
                break;

            case global::CharacterClass.Mage:
                newCharacterClass = new BaseMageClass();
                break;
            default:
                newCharacterClass = new BaseWarriorClass();
                break;
        }

        characterClass = newCharacterClass;
        stats = savedata.Stats;
    }

    private void AddExperiencePoints(float amount)
    {
        xp += amount;
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
