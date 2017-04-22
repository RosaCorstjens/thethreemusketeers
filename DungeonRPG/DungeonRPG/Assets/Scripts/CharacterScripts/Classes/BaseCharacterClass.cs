using UnityEngine;
using System.Collections;
using System;

public enum CharacterClass { Warrior, Mage, Hunter }

[Serializable]
public class BaseCharacterClass
{
    protected string characterClassName;
    protected string characterClassDescription;

    protected StatTypes primaryStat;
    protected CharacterClass characterClassType;

    public string ClassName { get { return characterClassName; } }
    public string ClassDescription { get { return characterClassDescription; } }
    public StatTypes PrimaryStat { get { return primaryStat; } }
    public CharacterClass CharacterClassType { get { return characterClassType; } }

    public BaseCharacterClass() { }
}
