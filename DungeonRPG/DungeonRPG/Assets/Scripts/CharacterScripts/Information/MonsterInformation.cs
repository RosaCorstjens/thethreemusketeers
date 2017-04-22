using UnityEngine;
using System.Collections;

public class MonsterInformation : BaseCharacterInformation
{

    public MonsterInformation() { }

    /// <summary>
    /// Call when creating a new player. 
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <param name="characterClass">The class of the character.</param>
    /// <param name="level">The level of the character.</param>
    public MonsterInformation(string name, BaseCharacterClass characterClass, int level) : base(name, characterClass)
    {
        this.level = level;

        SetUpStats();
    }
}
